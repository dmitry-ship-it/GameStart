namespace GameStart.CatalogService.Common.ViewModels
{
    public class VideoGameViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Copyright { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public decimal? Price { get; set; }

        public string Publisher { get; set; }

        public IList<string> Developers { get; set; }

        public IList<string> Ganres { get; set; }

        public IList<LanguageViewModel> Languages { get; set; }

        public IList<SystemRequirementsViewModel> SystemRequirements { get; set; }
    }
}
