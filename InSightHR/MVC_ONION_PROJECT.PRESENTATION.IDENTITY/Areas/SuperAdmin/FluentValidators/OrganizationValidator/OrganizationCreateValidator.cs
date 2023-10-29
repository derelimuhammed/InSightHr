using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationVms;
using System.Text.RegularExpressions;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.FluentValidators.OrganizationValidator
{
    public class OrganizationCreateValidator:AbstractValidator<OrganizationCreateVm>
    {
        public OrganizationCreateValidator()
        {
            RuleFor(x => x.OrganizationName)
           .NotEmpty().WithMessage("Organizasyon adı boş olamaz.")
           .Length(2, 100).WithMessage("Organizasyon adı 2 ile 100 karakter arasında olmalıdır.");

            RuleFor(x => x.OrganizationEmail)
                .NotEmpty().WithMessage("Organizasyon e-posta adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.OrganizationPhone)
          .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
          .Must(IsValidPhoneNumber).WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.OrganizationAddress)
                .NotNull().WithMessage("Organizasyon adresi gereklidir.")
                .Length(5, 200).When(x => string.IsNullOrWhiteSpace(x.OrganizationAddress)).WithMessage("Organizasyon adresi 5 ile 200 karakter arasında olmalıdır.");


            RuleFor(x => x.TaxNumber)
                .NotEmpty().WithMessage("Vergi numarası boş olamaz.").Length(10).WithMessage("Vergi numarası 10 karakterli olmalıdır.");

            RuleFor(x => x.Logopath)
           .Must(BeAValidFile).When(x => x.Logopath != null).WithMessage("Geçerli bir dosya seçiniz.")
           .When(x => x.Logopath != null && x.Logopath.Length > 0).WithMessage("Dosya boş olamaz.").Must(file => IsImage(file)) // Özel bir metot kullanarak dosyanın bir resim olup olmadığını kontrol ediyoruz
             .WithMessage("Geçerli bir resim dosyası yükleyin."); ;

            RuleFor(x => x.ContractStart)
   .NotEmpty().WithMessage("Sözleşme başlangıç tarihi boş olamaz.")
   .Must(date => date >= DateTime.Now.AddDays(-1)).WithMessage("Sözleşme başlangıç tarihi geçmiş bir tarih olamaz.");


          RuleFor(x => x.ContractEnd)
                .NotEmpty().WithMessage("Sözleşme bitiş tarihi boş olamaz.")
                .GreaterThanOrEqualTo(x => x.ContractStart).WithMessage("Sözleşme bitiş tarihi başlangıç tarihinden önce olamaz.");

            RuleFor(x => x.PackageId)
                .NotEmpty().WithMessage("Paket ID boş olamaz.").NotNull().WithMessage("Paket ID boş olamaz.");
        }
        private bool BeAValidFile(IFormFile file)
        {
            if (file == null)
                return true; // Dosya seçilmemişse valid kabul ediyoruz.

            

            
             return file.Length <= 10 * 1024 * 1024; // 10MB'dan küçük dosyaları kabul eder.
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string phonePattern = @"^\(\d{3}\) \d{3}-\d{4}$";
            if (phoneNumber is not null)
            {
                return Regex.IsMatch(phoneNumber, phonePattern);
            }
            phoneNumber = " ";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }
        private bool IsImage(IFormFile file)
        {
            if (file == null)
                return true;

            // Resim dosyası uzantılarını kontrol edebilirsiniz. Örneğin, JPEG, PNG, GIF, vb.
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(fileExtension);
        }
    }
}
