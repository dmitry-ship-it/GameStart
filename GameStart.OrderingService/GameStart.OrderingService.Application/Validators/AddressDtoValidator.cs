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

                RuleFor(dto => dto.State).Must(s => ValidateNullableStringLength(s, 3, 60));

                RuleFor(dto => dto.City).NotNull().Length(3, 100);

                RuleFor(dto => dto.Street).NotNull().Length(3, 60);

                RuleFor(dto => dto.House).NotNull().Length(1, 10);

                RuleFor(dto => dto.Flat).Must(s => ValidateNullableStringLength(s, 1, 10));

                // Some countries do not have postal codes,
                // in other countries it can be from 2 to 12 characters
                RuleFor(dto => dto.PostCode).Must(s => ValidateNullableStringLength(s, 2, 12));
            });
        }

        private static bool ValidateNullableStringLength(string s, int min, int max)
        {
            return s is null || (s.Length <= max && s.Length >= min);
        }
    }
}
