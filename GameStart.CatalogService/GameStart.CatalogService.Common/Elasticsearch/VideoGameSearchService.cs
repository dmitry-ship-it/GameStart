using GameStart.CatalogService.Common.Elasticsearch.Extensions;
using GameStart.CatalogService.Common.Elasticsearch.Search;
using GameStart.CatalogService.Data.Models;
using Microsoft.AspNetCore.Http;
using Nest;

namespace GameStart.CatalogService.Common.Elasticsearch
{
    public class VideoGameSearchService : IElasticsearchService<VideoGame, VideoGameSearchRequest>
    {
        public string IndexName { get; } = nameof(VideoGame);

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

            await elasticClient.Indices.CreateAsync(IndexName, index => index.MapVideoGame(), cancellationToken);
        }

        public async Task DeleteByIdAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            await elasticClient.DeleteAsync(DocumentPath<VideoGame>.Id(entity.Id), selector => selector.Index(IndexName), cancellationToken);
        }

        public async Task DeleteIndexAsync(CancellationToken cancellationToken = default)
        {
            await elasticClient.Indices.DeleteAsync(IndexName, ct: cancellationToken);
        }

        public async Task InsertAsync(VideoGame entity, CancellationToken cancellationToken = default)
        {
            var response = await elasticClient.CreateAsync(entity, selector => selector.Index(IndexName), cancellationToken);

            if (response.ApiCall?.HttpStatusCode == StatusCodes.Status409Conflict)
            {
                await elasticClient.UpdateAsync<VideoGame>(entity.Id, selector => selector.Index(IndexName).Doc(entity), cancellationToken);
            }
        }

        public async Task InsertListAsync(IList<VideoGame> entities, CancellationToken cancellationToken = default)
        {
            await elasticClient.IndexManyAsync(entities, IndexName, cancellationToken);
        }

        public async Task<IEnumerable<VideoGame>> SearchAsync(VideoGameSearchRequest request, CancellationToken cancellationToken)
        {
            await CheckIndexAsync(cancellationToken);

            var response = await elasticClient.SearchAsync<VideoGame>(selector => selector
                .From(0)
                .Size(10)
                .Index(IndexName)
                .Query(query => query
                    .DisMax(dismax => dismax.Queries(
                        query => query. MatchPhrase(match => match
                            .Field(field => field.Title)
                            .Query(request.Title)
                        ),
                        query => query.DateRange(date => date
                            .Field(field => field.ReleaseDate)
                            .GreaterThanOrEquals(request.ReleasedFrom.Value.ToDateTime(TimeOnly.MinValue))
                            .LessThanOrEquals(DateMath.Now)
                        ),
                        query => query.Range(price => price
                            .Field(field => field.Price)
                            .GreaterThanOrEquals((double)request.PriceFrom)
                            .LessThanOrEquals(request.PriceTo is null || request.PriceTo == decimal.Zero ? double.MaxValue : (double)request.PriceTo)
                        ),
                        query => query.MatchPhrase(match => match
                            .Field(field => field.Publisher.Name)
                            .Query(request.Publisher)
                        ),
                        query => query.Terms(terms => terms
                            .Field(field => field.Developers.Select(dev => dev.Name))
                            .Terms(request.Developers)
                        ),
                        query => query.Terms(terms => terms
                            .Field(field => field.Ganres.Select(ganre => ganre.Name))
                            .Terms(request.Ganres)
                        ),
                        query => query.Terms(terms => terms
                            .Field(field => field.LanguageAvailabilities.Select(language => language.Language.Name))
                            .Terms(request.Languages)
                        ),
                        query => query.Terms(terms => terms
                            .Field(field => field.SystemRequirements.Select(selector => selector.Platform.Name))
                            .Terms(request.Platforms)
                        )
                    )
                )
            ),
            cancellationToken);

            return response.Documents;
        }
    }
}
