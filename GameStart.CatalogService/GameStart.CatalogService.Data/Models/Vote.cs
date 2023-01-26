namespace GameStart.CatalogService.Data.Models
{
    public class Vote
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public bool IsPositive { get; set; }
    }
}