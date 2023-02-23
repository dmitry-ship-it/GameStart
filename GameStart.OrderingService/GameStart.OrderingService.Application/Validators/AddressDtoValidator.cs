using FluentValidation;
using GameStart.OrderingService.Application.DtoModels;

namespace GameStart.OrderingService.Application.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            When(dto => dto is not null, () =>
            {
                RuleFor(dto => dto.Country).NotNull().Length(3, 60);

                RuleFor(dto => dto.State).Length(3, 60)
                    .When(dto => dto.State is not null, ApplyConditionTo.CurrentValidator);

                RuleFor(dto => dto.City).NotNull().Length(3, 100);

                RuleFor(dto => dto.Street).NotNull().Length(3, 60);

                RuleFor(dto => dto.House).NotNull().Length(1, 10);

                RuleFor(dto => dto.Flat).Length(1, 10)
                    .When(dto => dto.Flat is not null, ApplyConditionTo.CurrentValidator);

                // Some countries do not have postal codes,
                // in other countries it can be from 2 to 12 characters
                RuleFor(dto => dto.PostCode).Length(2, 12)
                    .When(postcode => postcode is not null, ApplyConditionTo.CurrentValidator);
            });
        }
    }
}
