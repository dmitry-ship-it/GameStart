using GameStart.OrderingService.Application.DtoModels;
using System.Collections;

namespace GameStart.OrderingService.Application.Tests.TestData
{
    public class InvalidAddressDtos : IEnumerable<object[]>
    {
        private readonly AddressDto[] addressDtos =
        {
            new()
            {
                Country = "A",
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
                City = "More than World's longest city name" + new string('a', 100),
                Street = "abc",
                House = "1234567890",
                Flat = "33b25c",
                PostCode = "CE1234567890"
            },
            new()
            {
                Country = "Other Country",
                State = null,
                City = null,
                Street = "Some street",
                House = "743",
                Flat = null,
                PostCode = "CE1234567890"
            },
            new()
            {
                Country = "Another Other Country",
                State = "Oh",
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
                City = "Lo",
                Street = "Lincoln ave.",
                House = "7343",
                Flat = "341",
                PostCode = "AA00"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Li",
                House = "7343",
                Flat = "341",
                PostCode = "AA00"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Li" + new string('a', 59),
                House = "7343",
                Flat = "341",
                PostCode = "AA00"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Some Street",
                House = string.Empty,
                Flat = "341",
                PostCode = "AA00"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Some Street",
                House = "1234567890a",
                Flat = "341",
                PostCode = "AA00"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Some Street",
                House = "54",
                Flat = string.Empty,
                PostCode = "AA00"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Some Street",
                House = "54",
                Flat = new string('1', 11),
                PostCode = "AA00"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Some Street",
                House = "54",
                Flat = "53",
                PostCode = "A"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = "Lost City",
                Street = "Some Street",
                House = "54",
                Flat = "53",
                PostCode = "A123456789ABC"
            },
            new()
            {
                Country = "Another Other Country",
                State = null,
                City = null,
                Street = null,
                House = null,
                Flat = null,
                PostCode = null
            },
            new()
            {
                Country = "Another Other Country",
                State = "Some state",
                City = "Lost City",
                Street = "Some Street",
                House = "54",
                Flat = "53",
                PostCode = string.Empty
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
