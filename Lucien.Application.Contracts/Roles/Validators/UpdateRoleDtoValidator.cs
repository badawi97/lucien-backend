using FluentValidation;
using Lucien.Application.Contracts.Roles.Dtos;

namespace Lucien.Application.Contracts.Roles.Validators
{
    public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator()
        {
            RuleFor(role => role.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 200).WithMessage("Name must be between 1 and 200 characters.");
        }
    }
}
