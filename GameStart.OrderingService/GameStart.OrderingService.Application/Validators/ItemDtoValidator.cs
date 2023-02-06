using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;

namespace GameStart.OrderingService.Application.Validators
{
    public class ItemDtoValidator : AbstractValidator<ItemDto>
    {
        public ItemDtoValidator()
        {
            RuleFor(dto => dto.GameId).NotEqual(Guid.Empty);
        }
    }
}
