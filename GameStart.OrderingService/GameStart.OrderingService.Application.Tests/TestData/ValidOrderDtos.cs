using GameStart.OrderingService.Application.DtoModels;
using System.Collections;

namespace GameStart.OrderingService.Application.Tests.TestData
{
    public class ValidOrderDtos : IEnumerable<object[]>
    {
        private static AddressDto ValidAddress => new()
        {
            Country = "Other Country",
            State = null,
            City = "Another city",
            Street = "Some street",
            House = "743",
            Flat = null,
            PostCode = "CE1234567890"
        };

        private readonly OrderDto[] orderDtos =
        {
            new()
            {
                Address = null,
                Items = new ItemDto[]
                {
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false },
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false },
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false },
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false }
                }
            },
            new()
            {
                Address = ValidAddress,
                Items = new ItemDto[]
                {
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false },
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false },
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false },
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false }
                }
            },
            new()
            {
                Address = ValidAddress,
                Items = new ItemDto[]
                {
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = true },
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = false },
                }
            },
            new()
            {
                Address = ValidAddress,
                Items = new ItemDto[]
                {
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = true },
                    new() { GameId = Guid.NewGuid(), IsPhysicalCopy = true },
                }
            },
            new()
            {
                Address = ValidAddress,
                Items = new ItemDto[]
                {
                    new() { GameId = Guid.Parse("42421489-4D76-4E13-BC2B-81BB8C2F4CE1"), IsPhysicalCopy = true },
                    new() { GameId = Guid.Parse("42421489-4D76-4E13-BC2B-81BB8C2F4CE1"), IsPhysicalCopy = true },
                }
            },
            new()
            {
                Address = ValidAddress,
                Items = new ItemDto[]
                {
                    new() { GameId = Guid.Parse("42421489-4D76-4E13-BC2B-81BB8C2F4CE1"), IsPhysicalCopy = true },
                    new() { GameId = Guid.Parse("42421489-4D76-4E13-BC2B-81BB8C2F4CE1"), IsPhysicalCopy = false },
                }
            },
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (var item in orderDtos)
            {
                yield return new[] { item };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
