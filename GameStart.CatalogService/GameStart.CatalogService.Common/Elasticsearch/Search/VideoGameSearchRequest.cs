﻿namespace GameStart.CatalogService.Common.Elasticsearch.Search
{
    public class VideoGameSearchRequest : ISearchRequest
    {
        public string Title { get; set; }

        public DateTimeOffset ReleasedFrom { get; set; }

        public DateTimeOffset? ReleasedTo { get; set; }

        public decimal PriceFrom { get; set; }

        public decimal? PriceTo { get; set; }

        public string Publisher { get; set; }

        public IEnumerable<string> Developers { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<string> Languages { get; set; }

        public IEnumerable<string> Platforms { get; set; }
    }
}
