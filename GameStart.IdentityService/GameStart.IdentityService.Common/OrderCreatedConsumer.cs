using AutoMapper;
using GameStart.IdentityService.Data.Models;
using GameStart.IdentityService.Data.Repositories;
using GameStart.Shared.MessageBus;
using GameStart.Shared.MessageBus.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace GameStart.IdentityService.Common
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly UserManager<User> userManager;
        private readonly IRepository<InventoryItem> inventoryRepository;
        private readonly IMapper mapper;

        public OrderCreatedConsumer(
            UserManager<User> userManager,
            IRepository<InventoryItem> inventoryRepository,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.inventoryRepository = inventoryRepository;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;

            var user = await userManager.FindByIdAsync(message.UserId.ToString());

            if (user is null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            var inventoryItems = mapper.Map<IEnumerable<InventoryItem>>(message.OrderItems)
                .Select(item =>
                {
                    item.User = user;
                    return item;
                });

            await inventoryRepository.CreateRangeAsync(inventoryItems, context.CancellationToken);
        }
    }
}
