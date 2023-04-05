using AutoMapper;
using GameStart.IdentityService.Common.Mapping;
using GameStart.IdentityService.Data.Models;
using GameStart.Shared.MessageBus.Models.OrderModels;
using System.Security.Claims;

namespace GameStart.IdentityService.Common.Tests
{
    public class MapperTests
    {
        [Fact]
        public void Map_ShouldMapOrderItemCorrectly()
        {
            // Arrange
            var from = new OrderItem
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                GameKey = "Test key",
                Title = "Test title",
                PurchaseDateTime = DateTime.UtcNow,
                IsPhysicalCopy = true,
            };

            var to = new InventoryItem
            {
                Id = from.Id,
                GameId = from.GameId,
                GameKey = from.GameKey,
                Title = from.Title,
                PurchaseDateTime = from.PurchaseDateTime,
            };

            var config = new MapperConfiguration(options => options.AddProfile<InventoryItemProfile>());
            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<InventoryItem>(from);

            // Assert
            result.Should().BeEquivalentTo(to);
        }

        [Fact]
        public void Map_ShouldMapClaimsToUserCorrectly()
        {
            // Arrange
            const string email = "example@mail.com";
            const string nameIdentifier = "test identifier";

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Email, email),
                new(ClaimTypes.NameIdentifier, nameIdentifier)
            };

            var config = new MapperConfiguration(options => options.AddProfile<UserProfile>());
            var mapper = config.CreateMapper();

            // Act
            var result = mapper.Map<User>(claims);

            // Assert
            result.UserName.Should().Be(email);
            result.Email.Should().Be(email);
            result.ExternalProviderUserId.Should().Be(nameIdentifier);
            result.EmailConfirmed.Should().BeTrue();
        }
    }
}
