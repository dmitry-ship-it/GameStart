namespace GameStart.CatalogService.Data.Models
{
    public class SystemRequirements
    {
        public int Id { get; set; }

        public virtual Platform Platform { get; set; }

        public string OS { get; set; }

        public string Processor { get; set; }

        public string Memory { get; set; }

        public string Graphics { get; set; }

        public string Network { get; set; }

        public string Storage { get; set; }
    }
}