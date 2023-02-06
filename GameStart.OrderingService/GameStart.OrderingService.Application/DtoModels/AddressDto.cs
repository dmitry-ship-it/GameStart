namespace GameStart.OrderingService.Application.DtoModels
{
    public class AddressDto
    {
        public Guid UserId { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Flat { get; set; }

        public string PostCode { get; set; }
    }
}
