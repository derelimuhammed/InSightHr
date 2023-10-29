using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AssetCategoryVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.FluentValidators.AssetCategoryValidator
{
    public class AssetCategoryCreateValidator : AbstractValidator<AssetCategoryCreateVm>
    {
        public AssetCategoryCreateValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori ismi zorunludur.").MinimumLength(3).WithMessage("Kategori ismi en az 3 karakterli olmalıdır.").NotNull().WithMessage("Kategori ismi zorunludur.");
        }
    }

}
