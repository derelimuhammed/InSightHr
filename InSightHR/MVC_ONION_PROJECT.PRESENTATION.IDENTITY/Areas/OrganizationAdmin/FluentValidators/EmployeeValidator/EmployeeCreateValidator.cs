using FluentValidation;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using System.Text.RegularExpressions;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.FluentValidators.EmployeeValidator
{
    public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateVm>
    {
        public EmployeeCreateValidator()
        {
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Çalışan adı en az 3 karakterli olmalıdır").NotNull().WithMessage("İsim bölümü boş bırakılamaz.");
            RuleFor(x => x.Surname).MinimumLength(2).WithMessage("Çalışan soyadı en az 2 karakterli olmalıdır").NotNull().WithMessage("Soyisim bölümü boş bırakılamaz.");
            RuleFor(x => x.Address).MinimumLength(2).WithMessage("Address en az 2 karakterli olmalıdır").NotNull().WithMessage("Address bölümü boş bırakılamaz.").NotEmpty().WithMessage("Address bölümü boş bırakılamaz.");
            RuleFor(x => x.RecruitmentDate).NotNull().WithMessage("İşe giriş tarihi boş geçilemez.");
            RuleFor(x => x.DepartmentId).NotNull().WithMessage("Lütfen bir departman seçiniz.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email formatına uygun olmalıdır");
            RuleFor(kullanici => kullanici.PhoneNumber)
            .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
            .Must(IsValidPhoneNumber).WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.Email)
           .NotEmpty()
           .When(x => !x.IsCustomMail)
           .WithMessage("Email, Custom-Mail kapalı olduğunda zorunludur.").NotNull().When(x => !x.IsCustomMail).WithMessage("ZorunluAlan, Custom-Mail kapalı olduğunda zorunludur.");
            //RuleFor(x => x.File)
            // .NotEmpty() // Dosya boş olmamalıdır
            // .WithMessage("Resim dosyası gereklidir.")
            // .Must(file => IsImage(file)) // Özel bir metot kullanarak dosyanın bir resim olup olmadığını kontrol ediyoruz
            // .WithMessage("Geçerli bir resim dosyası yükleyin.");
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
