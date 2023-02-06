namespace GameStart.OrderingService.Application.DtoModels
{
    public class OrderDto
    {
        public Guid UserId { get; set; }

        public DateTime DateTime { get; set; }

        public IList<ItemDto> Items { get; set; }

        public AddressDto Address { get; set; }
    }
}
