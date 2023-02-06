using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.Shared;

namespace GameStart.OrderingService.Application.Validators
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(dto => dto.UserId).NotEqual(Guid.Empty);

            RuleFor(dto => dto.DateTime).GreaterThanOrEqualTo(DateTime.Now.Subtract(TimeSpan.FromMinutes(10)));

            RuleFor(dto => dto).Must(ValidItemsAndAddress)
                .WithMessage(Constants.OrderingService.ValidationMessages.OrderedPhysicalCopyButAddressIsNull);
        }

        /// <summary>
        ///     If the user orders at least one physical copy, a delivery address must be provided.
        /// </summary>
        private bool ValidItemsAndAddress(OrderDto order)
        {
            return !order.Items.Any(item => item.IsPhysicalCopy) || order.Address is not null;
        }
    }
}
