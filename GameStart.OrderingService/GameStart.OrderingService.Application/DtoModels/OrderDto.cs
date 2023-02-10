namespace GameStart.OrderingService.Application.DtoModels
{
    public class OrderDto
    {
        public IList<ItemDto> Items { get; set; }

        public AddressDto Address { get; set; }
    }
}
