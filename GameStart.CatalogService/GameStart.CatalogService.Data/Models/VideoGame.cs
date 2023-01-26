namespace GameStart.CatalogService.Data.Models
{
    public class VideoGame
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Copyright { get; set; }

        public DateOnly ReleaseDate { get; set; }

        public decimal Price { get; set; }

        public int PhysicalCopiesAvailable { get; set; }

        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<Developer> Developers { get; set; }

        public virtual ICollection<SystemRequirements> MinimumSystemRequirements { get; set; }

        public virtual ICollection<SystemRequirements> RecommendedSystemRequirements { get; set; }

        public virtual ICollection<Ganre> Ganres { get; set; }

        public virtual ICollection<Language> InterfaceLanguages { get; set; }

        public virtual ICollection<Language> AudioLanguages { get; set; }

        public virtual ICollection<Language> SubtitlesLanguages { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
