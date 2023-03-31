using GameStart.CatalogService.Common.Elasticsearch.Extensions;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Data.Models;
using GameStart.Shared.Services;
using Nest;

namespace GameStart.CatalogService.Common.Elasticsearch
{
    public class VideoGameSearchService : IElasticsearchService<VideoGame, VideoGameSearchRequest>
    {
        private readonly IElasticClient elasticClient;
        private readonly IDateTimeProvider clock;

        public VideoGameSearchService(IElasticClient elasticClient, IDateTimeProvider clock)
        {
            this.elasticClient = elasticClient;
            this.clock = clock;
        }

        public string IndexName { get; } = nameof(VideoGame).ToLower();

        public async Task CheckIndexAsync(CancellationToken cancellationToken = default)
        {
            var result = await elasticClient.Indices.ExistsAsync(IndexName, ct: cancellationToken);

            if (!result.Exists)
            {
                await elasticClient.Indices.CreateAsync(IndexName, config =>
                    config.Map<VideoGame>(map => map.MapVideoGameGraph()), cancellationToken);
            }
        }

        public async Task DeleteByIdAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            await CheckIndexAsync(cancellationToken);
            await elasticClient.DeleteAsync(DocumentPath<VideoGame>.Id(entity.Id),
                selector => selector.Index(IndexName), cancellationToken);
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
            await CheckIndexAsync(cancellationToken);
            await elasticClient.UpdateAsync<VideoGame>(entity.Id,
                selector => selector.Index(IndexName).Doc(entity), cancellationToken);
        }

        public async Task<IEnumerable<VideoGame>> SearchAsync(VideoGameSearchRequest request, CancellationToken cancellationToken)
        {
            await CheckIndexAsync(cancellationToken);

            var response = await elasticClient.SearchAsync<VideoGame>(selector => selector
                .From(0)
                .Size(10)
                .Index(IndexName)
                .Query(query => query
                    .Bool(boolean => boolean
                        .Must(
                            query => query.Wildcard(wildcard => wildcard
                                .Field(field => field.Title)
                                .Value(request.Title)
                                .CaseInsensitive(true)
                            ),
                            query => query.DateRange(dateRange => dateRange
                                .Field(field => field.ReleaseDate)
                                .GreaterThanOrEquals(DateMath.Anchored(request.ReleasedFrom.UtcDateTime))
                                .LessThanOrEquals(DateMath.Anchored(request.ReleasedTo?.UtcDateTime ?? clock.Now.UtcDateTime))
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
                            query => query.Nested(child => child
                                .Path(path => path.Developers)
                                .Query(query => query
                                    .Terms(terms => terms
                                        .Field(field => field.Developers.First().Name)
                                        .Terms(request.Developers)
                                    )
                                )
                            ),
                            query => query.Nested(child => child
                                .Path(path => path.Genres)
                                .Query(query => query
                                    .Terms(terms => terms
                                        .Field(field => field.Genres.First().Name)
                                        .Terms(request.Genres)
                                    )
                                )
                            ),
                            query => query.Nested(child => child
                                .Path(path => path.LanguageAvailabilities)
                                .Query(query => query
                                    .Terms(terms => terms
                                        .Field(field => field.LanguageAvailabilities.First().Language.Name)
                                        .Terms(request.Languages)
                                    )
                                )
                            ),
                            query => query.Nested(child => child
                                .Path(path => path.SystemRequirements)
                                .Query(query => query
                                    .Terms(terms => terms
                                        .Field(field => field.SystemRequirements.First().Platform.Name)
                                        .Terms(request.Platforms)
                                    )
                                )
                            )
                        )
                    )
                ), cancellationToken);

            return response.Documents;
        }
    }
}
