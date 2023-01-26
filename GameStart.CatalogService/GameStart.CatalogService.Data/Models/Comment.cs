namespace GameStart.CatalogService.Data.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Content { get; set; }

        public DateTime PostedDateTime { get; set; }
    }
}