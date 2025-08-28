using FluentValidation;
using KMH_ERP.Application.DTOs;

namespace KMH_ERP.Application.Validators
{
    public class RoleDtoValidator : AbstractValidator<RoleDto>
    {

        public RoleDtoValidator()
        {

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(20).WithMessage("Role name cannot exceed 20 characters.");

            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                .When(x => x.RoleId != 0);


        }

    }
}
