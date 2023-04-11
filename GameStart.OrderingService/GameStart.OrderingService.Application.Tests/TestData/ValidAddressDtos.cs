using GameStart.OrderingService.Application.DtoModels;
using System.Collections;

namespace GameStart.OrderingService.Application.Tests.TestData
{
    public class ValidAddressDtos : IEnumerable<object[]>
    {
        private readonly AddressDto[] addressDtos =
        {
            new()
            {
                Country = "AAA",
                State = "Some state",
                City = "City",
                Street = "John Doe's",
                House = "1",
                Flat = "33",
                PostCode = "AB"
            },
            new()
            {
                Country = "Country",
                State = "Very very very long state name",
                City = "World's longest city name" + new string('a', 75),
                Street = "abc",
                House = "1234567890",
                Flat = "33b25c",
                PostCode = "CE1234567890"
            },
            new()
            {
                Country = "Other Country",
                State = null,
                City = "Another city",
                Street = "Some street",
                House = "743",
                Flat = null,
                PostCode = "CE1234567890"
            },
            new()
            {
                Country = "Another Other Country",
                State = "Ohio",
                City = "Lost City",
                Street = "Grove st.",
                House = "33",
                Flat = null,
                PostCode = "856463"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Lincoln ave.",
                House = "7343",
                Flat = "341",
                PostCode = "AA00"
            }
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var item in addressDtos)
            {
                yield return new[] { item };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
