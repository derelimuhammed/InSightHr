using FluentValidation;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeReimbursementVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.EmployeeFluentValidators.EmployeeReimbursementValidator
{
    public class EmployeeReimbursementCreateValidator:AbstractValidator<EmployeeReimbursementCreateVm>
    {
        public EmployeeReimbursementCreateValidator()
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
                .Must(x => IsSelected(x))
                .WithMessage("Lütfen para birimini seçiniz.");

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

        private bool IsSelected(Currency secim)
        {
            if (secim == 0 )
                return false; 
            else
                return true;
        }
    }

   
}

