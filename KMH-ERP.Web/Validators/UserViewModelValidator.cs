using FluentValidation;
using KMH_ERP.Web.ViewModels;

namespace KMH_ERP.Application.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {        
            RuleFor(x => x.User).SetValidator(new UserDtoValidator());
         
            RuleFor(x => x.User.PasswordHash)
                .NotEmpty().WithMessage("Password is required.")
                .Equal(x => x.confirmPassword).WithMessage("Passwords do not match.")
                .When(x => x.User.UserId == 0);
        }
    }
}
