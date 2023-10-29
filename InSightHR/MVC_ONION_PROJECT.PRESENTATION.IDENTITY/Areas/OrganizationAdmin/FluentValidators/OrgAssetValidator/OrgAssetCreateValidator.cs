using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.OrgAssetVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.FluentValidators.OrgAssetValidator
{
    public class OrgAssetCreateValidator: AbstractValidator<OrgAssetCreateVm>
    {
        public OrgAssetCreateValidator()
        {
            RuleFor(x => x.SerialNumber).MinimumLength(5).WithMessage("Seri numarası en az 5 karakterli olmalıdır").NotNull().WithMessage("Seri numarası boş geçilemez.");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Demirbaş adı en az 3 karakterli olmalıdır").NotNull().WithMessage("Demirbaş adı boş geçilemez.");
           
            RuleFor(x => x.PurchaseDate).NotNull().WithMessage("Demirbaş alım tarihini giriniz.");
            RuleFor(x => x.CategoryId).NotNull().WithMessage("Demirbaş kategorisini seçiniz.");
            RuleFor(x => x.Price).NotNull().WithMessage("Demirbaş ücret bilgisi boş geçilemez.")
               .Must(ValueControl).WithMessage("Ücret bilgisi boş geçilemez");
        }

        private bool ValueControl(float price)
        {
            if (price > 0)
            {
                return true;
            }
            return false;
        }
    }
}
