namespace GameStart.CatalogService.Common.ViewModels
{
    public class LanguageViewModel
    {
        public string Name { get; set; }

        public bool AvailableForInterface { get; set; }

        public bool AvailableForAudio { get; set; }

        public bool AvailableForSubtitles { get; set; }
    }
}
