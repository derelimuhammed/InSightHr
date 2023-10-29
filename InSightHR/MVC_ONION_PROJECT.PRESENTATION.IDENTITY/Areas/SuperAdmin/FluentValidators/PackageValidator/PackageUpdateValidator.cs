using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.Package;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.FluentValidators.PackageValidator
{
    public class PackageUpdateValidator : AbstractValidator<PackageUpdateVm>
    {
        public PackageUpdateValidator()
        {
            RuleFor(x => x.PackageName)
           .NotEmpty()
           .WithMessage("Paket adı boş olamaz.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Fiyat pozitif bir değer olmalıdır.");

            RuleFor(x => x.NumberOfUser)
                .GreaterThan(0)
                .WithMessage("Kullanıcı sayısı pozitif bir tam sayı olmalıdır.");

            RuleFor(x => x.PackageDurationMonth)
                .GreaterThan(0)
                .WithMessage("Paket süresi pozitif bir değer olmalıdır.");
        }
    }
}
