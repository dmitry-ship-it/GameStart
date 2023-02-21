namespace GameStart.CatalogService.Data.Models
{
    public class VideoGame : IEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Copyright { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal Price { get; set; }

        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<Developer> Developers { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }

        public virtual ICollection<LanguageAvailability> LanguageAvailabilities { get; set; }

        public virtual ICollection<SystemRequirements> SystemRequirements { get; set; }
    }
}
