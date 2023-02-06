namespace GameStart.OrderingService.Application.RequestModels
{
    public class ItemModel
    {
        public Guid GameId { get; set; }

        public bool IsPhysicalCopy { get; set; }
    }
}