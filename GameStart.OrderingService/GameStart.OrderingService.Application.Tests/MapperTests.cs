using AutoMapper;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.OrderingService.Application.Mapping;
using GameStart.OrderingService.Core.Entities;
using GameStart.Shared;
using GameStart.Shared.MessageBus.Models.OrderModels;

namespace GameStart.OrderingService.Application.Tests
{
    public class MapperTests
    {
        [Fact]
        public void Map_ShouldMapAddressDtoToAddressEntity()
        {
            // Arrange
            var addressDto = new AddressDto
            {
                City = "Some City",
                Country = "Country",
                Flat = "3b",
                House = "33s5",
                PostCode = "BI653480",
                State = "State",
                Street = "John Doe's"
            };

            var address = new Address
            {
                City = addressDto.City,
                Country = addressDto.Country,
                Flat = addressDto.Flat,
                House = addressDto.House,
                PostCode = addressDto.PostCode,
                State = addressDto.State,
                Street = addressDto.Street
            };

            var config = new MapperConfiguration(options => options.AddProfile<AddressProfile>());
            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<Address>(addressDto);

            // Assert
            result.Should().BeEquivalentTo(address);
        }

        [Fact]
        public void Map_ShouldMapItemDtoToItem()
        {
            // Arrange
            var itemDto = new ItemDto
            {
                GameId = Guid.NewGuid(),
                IsPhysicalCopy = true
            };

            var item = new Item
            {
                GameId = itemDto.GameId,
                IsPhysicalCopy = itemDto.IsPhysicalCopy
            };

            var config = new MapperConfiguration(options => options.AddProfile<ItemProfile>());
            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<Item>(itemDto);

            // Assert
            result.Should().BeEquivalentTo(item);
        }

        [Fact]
        public void Map_ShouldMapItemToItemDto()
        {
            // Arrange
            var item = new Item
            {
                GameId = Guid.NewGuid(),
                Title = "Item A",
                IsPhysicalCopy = true
            };

            var itemDto = new ItemDto
            {
                GameId = item.GameId,
                IsPhysicalCopy = item.IsPhysicalCopy
            };

            var config = new MapperConfiguration(options => options.AddProfile<ItemProfile>());
            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<ItemDto>(item);

            // Assert
            result.Should().BeEquivalentTo(itemDto);
        }

        [Fact]
        public void Map_ShouldMapItemToOrderItem()
        {
            // Arrange
            var item = new Item
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Title = "Item A",
                IsPhysicalCopy = true
            };

            var orderItem = new OrderItem
            {
                Id = item.Id,
                GameId = item.GameId,
                Title = item.Title,
                IsPhysicalCopy = item.IsPhysicalCopy
            };

            var config = new MapperConfiguration(options => options.AddProfile<OrderItemProfile>());
            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<OrderItem>(item);

            // Assert
            result.Should().BeEquivalentTo(orderItem);
        }

        [Fact]
        public void Map_ShouldMapOrderItemToItem()
        {
            // Arrange
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Title = "Item A",
                IsPhysicalCopy = true
            };

            var item = new Item
            {
                Id = orderItem.Id,
                GameId = orderItem.GameId,
                Title = orderItem.Title,
                IsPhysicalCopy = orderItem.IsPhysicalCopy
            };

            var config = new MapperConfiguration(options => options.AddProfile<OrderItemProfile>());
            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<Item>(orderItem);

            // Assert
            result.Should().BeEquivalentTo(item);
        }

        [Fact]
        public void Map_ShouldMapOrderToOrderSubmitted()
        {
            // Arrange
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DateTime = DateTimeOffset.Now,
                TotalPrice = 109.99M,
                State = nameof(OrderStates.Submitted),
                Address = new() { Id = Guid.NewGuid() },
                Items = new Item[] { new() { Id = Guid.NewGuid() } }
            };

            var orderSubmitted = new OrderSubmitted
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderItems = order.Items.Select(item => new OrderItem
                {
                    Id = item.Id,
                    PurchaseDateTime = order.DateTime
                }).ToArray()
            };

            var config = new MapperConfiguration(options => options.AddProfile<OrderMessageProfile>());
            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<OrderSubmitted>(order);

            // Assert
            result.Should().BeEquivalentTo(orderSubmitted);
        }

        [Fact]
        public void Map_ShouldMapOrderDtoToOrder()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Address = new() { PostCode = "BF076432" },
                Items = new ItemDto[]
                {
                    new()
                    {
                        GameId = Guid.NewGuid(),
                        IsPhysicalCopy = true
                    }
                }
            };

            var order = new Order
            {
                Address = new() { PostCode = orderDto.Address.PostCode },
                Items = orderDto.Items.Select(item => new Item
                {
                    GameId = item.GameId,
                    IsPhysicalCopy = item.IsPhysicalCopy
                }).ToArray()
            };

            var config = new MapperConfiguration(options =>
            {
                options.AddProfile<OrderProfile>();
                options.AddProfile<AddressProfile>();
                options.AddProfile<ItemProfile>();
            });

            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<Order>(orderDto);

            // Assert
            result.Should().BeEquivalentTo(order);
        }
    }
}
