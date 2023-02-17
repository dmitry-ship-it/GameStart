namespace GameStart.CatalogService.Common.Elasticsearch.Search
{
    public class VideoGameSearchRequest : ISearchRequest
    {
        public string Title { get; set; }

        public DateTime ReleasedFrom { get; set; }

        public DateTime? ReleasedTo { get; set; }

        public decimal PriceFrom { get; set; }

        public decimal? PriceTo { get; set; }

        public string Publisher { get; set; }

        public IEnumerable<string> Developers { get; set; }

        public IEnumerable<string> Ganres { get; set; }

        public IEnumerable<string> Languages { get; set; }

        public IEnumerable<string> Platforms { get; set; }
    }
}
