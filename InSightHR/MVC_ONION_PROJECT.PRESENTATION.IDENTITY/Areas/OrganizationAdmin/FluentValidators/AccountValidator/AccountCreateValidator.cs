using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.FluentValidators.AccountValidator
{
    public class AccountCreateValidator : AbstractValidator<LoginVM>
    {
        public AccountCreateValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen Email adresinizi giriniz");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Lütfen şifrenizi giriniz");
        }
    }
}
