using FluentValidation;
using KMH_ERP.Web.ViewModels;

namespace KMH_ERP.Application.Validators
{
    public class RoleViewModelValidator : AbstractValidator<RoleViewModel>
    {

        public RoleViewModelValidator()
        {
            RuleFor(x => x.Role).SetValidator(new RoleDtoValidator());
        }
    }
}
