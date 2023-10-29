using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeReimbursementVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.EmployeeFluentValidators.EmployeeReimbursementValidator
{

    public class EmployeeReimbursementUpdateValidator : AbstractValidator<EmployeeReimbursementUpdateVm>
    {
        public EmployeeReimbursementUpdateValidator()
        {
            RuleFor(x => x.Date)
          .NotEmpty()
          .WithMessage("Tarih alanı gereklidir.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Açıklama alanı gereklidir.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Tutar pozitif bir değer olmalıdır.");

            RuleFor(x => x.Currency)
                .IsInEnum()
                .WithMessage("Geçerli bir para birimi seçin.");

            RuleFor(x => x.PaymentStatus)
                .IsInEnum()
                .WithMessage("Geçerli bir ödeme durumu seçin.");

            RuleFor(x => x.EmployeeId)
                .NotEmpty()
                .WithMessage("Kullanıcı ID gereklidir.");

            // ExpenseAttachmentsfile doğrulaması (sadece resim dosyalarını kabul eder)
            RuleFor(x => x.ExpenseAttachmentsfile)
                .Must(file => IsImage(file))
                .WithMessage("Sadece resim dosyası yükleyin.");
        }
        private bool IsImage(IFormFile file)
        {
            if (file == null)
                return true; // Dosya yüklenmemişse doğru kabul ediyoruz.

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension);
        }

    }
}
