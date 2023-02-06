namespace GameStart.OrderingService.Application.RequestModels
{
    public class CreateOrderRequest
    {
        public Guid UserId { get; set; }

        public DateTime DateTime { get; set; }

        public IList<ItemModel> Items { get; set; }

        public AddressModel Address { get; set; }
    }
}
