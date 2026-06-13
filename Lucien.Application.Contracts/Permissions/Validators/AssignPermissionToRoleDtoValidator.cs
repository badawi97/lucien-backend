using FluentValidation;
using Lucien.Application.Contracts.Permissions.Dtos;

namespace Lucien.Application.Contracts.Permissions.Validators
{
    public class AssignPermissionToRoleDtoValidator : AbstractValidator<AssignPermissionToRoleDto>
    {
        public AssignPermissionToRoleDtoValidator()
        {
            RuleFor(permission => permission.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 200).WithMessage("Name must be between 1 and 200 characters.");
        }
    }
}
