using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.PasswordFluentValidator
{
    public class ForgotPasswordValidator:AbstractValidator<ResetPasswordViewModel>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.NewPassword)
           .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
           .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter uzunluğunda olmalıdır.")
           .Must(ContainUppercase).WithMessage("Yeni şifre en az bir büyük harf içermelidir.")
           .Must(ContainLowercase).WithMessage("Yeni şifre en az bir küçük harf içermelidir.")
           .Must(ContainSpecialCharacter).WithMessage("Yeni şifre en az bir özel karakter içermelidir.");

            RuleFor(x => x.CorfirmPassword)
                .NotEmpty().WithMessage("Şifre onayı boş olamaz.")
                .Equal(x => x.NewPassword).WithMessage("Şifreler uyuşmuyor.");
        }
        private bool ContainUppercase(string password)
        {
            return password.Any(char.IsUpper);
        }

        private bool ContainLowercase(string password)
        {
            return password.Any(char.IsLower);
        }

        private bool ContainSpecialCharacter(string password)
        {
            return password.Any(c => !char.IsLetterOrDigit(c));
        }
    }
}
