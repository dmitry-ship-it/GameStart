using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;
using GameStart.Shared;

namespace GameStart.OrderingService.Application.Validators
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(dto => dto).Must(ValidItemsAndAddress)
                .WithMessage(Constants.OrderingService.ValidationMessages.OrderedPhysicalCopyButAddressIsNull);

            RuleFor(dto => dto.Items).Must(ValidItemDigitalCopies);

            RuleFor(dto => dto.Address).SetValidator(new AddressDtoValidator());
            RuleFor(dto => dto.Items).ForEach(item => item.SetValidator(new ItemDtoValidator()));
        }

        /// <summary>
        ///     If the user orders at least one physical copy, a delivery address must be provided.
        /// </summary>
        private bool ValidItemsAndAddress(OrderDto order)
        {
            return !order.Items.Any(item => item.IsPhysicalCopy) || order.Address is not null;
        }

        /// <summary>
        ///     For each order there must be only one digital copy of each game
        /// </summary>
        private bool ValidItemDigitalCopies(IList<ItemDto> items)
        {
            var allDigitalCopies = items.Where(item => !item.IsPhysicalCopy);
            var uniqueDigitalCopies = allDigitalCopies.DistinctBy(item => item.GameId);

            return allDigitalCopies.Count() == uniqueDigitalCopies.Count();
        }
    }
}
