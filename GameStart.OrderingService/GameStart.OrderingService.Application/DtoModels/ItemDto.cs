namespace GameStart.OrderingService.Application.DtoModels
{
    public class ItemDto
    {
        public Guid GameId { get; set; }

        public bool IsPhysicalCopy { get; set; }
    }
}
