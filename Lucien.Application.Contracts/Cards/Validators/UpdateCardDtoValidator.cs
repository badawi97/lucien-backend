using Lucien.Application.Contracts.Cards.Dto;
using FluentValidation;

namespace Lucien.Application.Contracts.Cards.Validators
{
    public class UpdateCardDtoValidator : AbstractValidator<UpdateCardDto>
    {
        public UpdateCardDtoValidator()
        {
            RuleFor(card => card.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");

            RuleFor(card => card.Gender)
                .InclusiveBetween(0, 2).WithMessage("Gender must be 0, 1, or 2.");

            RuleFor(card => card.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");

            RuleFor(card => card.Email)
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(card => card.Phone)
                .Matches(@"^\+\d{1,3}\d{1,14}$").WithMessage("Phone must be in a valid format.");

            RuleFor(card => card.Photo)
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Photo must be a valid URL.");

            RuleFor(card => card.Address)
                .NotEmpty().WithMessage("Address is required.");
        }
    }
}
