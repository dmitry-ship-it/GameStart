using GameStart.CatalogService.Common.Elasticsearch.Extensions;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Data.Models;
using Nest;

namespace GameStart.CatalogService.Common.Elasticsearch
{
    public class VideoGameSearchService : IElasticsearchService<VideoGame, VideoGameSearchRequest>
    {
        public string IndexName { get; } = nameof(VideoGame).ToLower();

        private readonly IElasticClient elasticClient;

        public VideoGameSearchService(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task CheckIndexAsync(CancellationToken cancellationToken = default)
        {
            var result = await elasticClient.Indices.ExistsAsync(IndexName, ct: cancellationToken);

            if (result.Exists)
            {
                return;
            }

            await elasticClient.Indices.CreateAsync(IndexName, config =>
                config.Map<VideoGame>(map => map.MapVideoGameGraph()), cancellationToken);
        }

        public async Task DeleteByIdAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            await CheckIndexAsync(cancellationToken);
            await elasticClient.DeleteAsync(DocumentPath<VideoGame>.Id(entity.Id), selector => selector.Index(IndexName), cancellationToken);
        }

        public async Task DeleteIndexAsync(CancellationToken cancellationToken = default)
        {
            await elasticClient.Indices.DeleteAsync(IndexName, ct: cancellationToken);
        }

        public async Task InsertAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            await CheckIndexAsync(cancellationToken);
            await elasticClient.IndexAsync(entity, config => config.Index(IndexName), cancellationToken);
        }

        public async Task InsertListAsync(IList<VideoGame> entities, CancellationToken cancellationToken = default)
        {
            await CheckIndexAsync(cancellationToken);
            await elasticClient.IndexManyAsync(entities, IndexName, cancellationToken);
        }

        public async Task UpdateAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            await elasticClient.UpdateAsync<VideoGame>(entity.Id, selector => selector.Index(IndexName).Doc(entity), cancellationToken);
        }

        public async Task<IEnumerable<VideoGame>> SearchAsync(VideoGameSearchRequest request, CancellationToken cancellationToken)
        {
            await CheckIndexAsync(cancellationToken);

            var response = await elasticClient.SearchAsync<VideoGame>(selector => selector
                .From(0)
                .Size(10)
                .Index(IndexName)
                .Query(query => query
                    .Bool(dismax => dismax
                        .Must(
                            query => query.Wildcard(wildcard => wildcard
                                .Field(field => field.Title)
                                .Value(request.Title)
                                .CaseInsensitive(true)
                            ),
                            query => query.DateRange(daterange => daterange
                                .Field(field => field.ReleaseDate)
                                .GreaterThanOrEquals(DateMath.Anchored(request.ReleasedFrom))
                                .LessThanOrEquals(DateMath.Anchored(request.ReleasedTo ?? DateTime.Now))
                            ),
                            query => query.Range(range => range
                                .Field(field => field.Price)
                                .GreaterThanOrEquals((double)request.PriceFrom)
                                .LessThanOrEquals(request.PriceTo is null ? ushort.MaxValue : (double)request.PriceTo)
                            ),
                            query => query.Wildcard(wildcard => wildcard
                                .Field(field => field.Publisher.Name)
                                .Value(request.Publisher)
                                .CaseInsensitive(true)
                            ),
                            query => query.Terms(terms => terms
                                .Field(field => field.Developers.First().Name)
                                .Terms(request.Developers)
                            ),
                            query => query.Terms(terms => terms
                                .Field(field => field.Ganres.First().Name)
                                .Terms(request.Ganres)
                            ),
                            query => query.Terms(terms => terms
                                .Field(field => field.LanguageAvailabilities.First().Language.Name)
                                .Terms(request.Languages)
                            ),
                            query => query.Terms(terms => terms
                                .Field(field => field.SystemRequirements.First().Platform.Name)
                                .Terms(request.Platforms)
                            )
                        )
                    )
                ), cancellationToken);

            return response.Documents;
        }
    }
}
