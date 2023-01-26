namespace GameStart.CatalogService.Data.Models
{
    public class Review
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Content { get; set; }

        public DateTime PostedDateTime { get; set; }

        public bool IsRecommended { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
