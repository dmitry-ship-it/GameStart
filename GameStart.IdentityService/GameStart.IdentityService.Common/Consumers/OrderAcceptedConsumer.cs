using AutoMapper;
using GameStart.IdentityService.Data.Models;
using GameStart.IdentityService.Data.Repositories;
using GameStart.Shared.MessageBus.Models.OrderModels;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace GameStart.IdentityService.Common.Consumers
{
    public class OrderAcceptedConsumer : IConsumer<OrderAccepted>
    {
        private readonly UserManager<User> userManager;
        private readonly IRepository<InventoryItem> inventoryRepository;
        private readonly IMapper mapper;

        public OrderAcceptedConsumer(
            UserManager<User> userManager,
            IRepository<InventoryItem> inventoryRepository,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.inventoryRepository = inventoryRepository;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<OrderAccepted> context)
        {
            var message = context.Message;
            var digitalCopies = message.OrderItems.Where(item => !item.IsPhysicalCopy);

            var user = await userManager.FindByIdAsync(message.UserId.ToString());

            // if user does not exist or already own any of digital copies of order
            if (user is null || (await inventoryRepository
                .FindByConditionAsync(entity => entity.User.Id == user.Id))
                .Any(owned => digitalCopies
                    .Any(orderItem => orderItem.GameId == owned.GameId)))
            {
                await context.Publish<OrderFaulted>(new
                {
                    message.Id,
                    message.UserId
                }, context.CancellationToken);
            }
            else
            {
                var inventoryItems = mapper.Map<IEnumerable<InventoryItem>>(digitalCopies)
                    .DistinctBy(item => item.GameId)
                    .Select(item =>
                    {
                        item.User = user;
                        return item;
                    });

                await inventoryRepository.CreateRangeAsync(inventoryItems, context.CancellationToken);

                await context.Publish<OrderCompleted>(new
                {
                    message.Id,
                    message.UserId,
                    message.TotalPrice,
                    message.OrderItems
                });
            }
        }
    }
}
