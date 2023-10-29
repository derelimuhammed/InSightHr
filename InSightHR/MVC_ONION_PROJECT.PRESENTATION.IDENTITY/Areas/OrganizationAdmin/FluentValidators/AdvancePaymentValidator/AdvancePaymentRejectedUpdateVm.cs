using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AdvanceVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AssetCategoryVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.FluentValidators.AdvancePaymentValidator
{
    public class AdvancePaymentRejectedUpdateValidator : AbstractValidator<AdvancePaymentRejectedUpdateVm>
    {
        public AdvancePaymentRejectedUpdateValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama zorunludur.").MinimumLength(3).WithMessage("Açıklama ismi en az 3 karakterli olmalıdır.").NotNull().WithMessage("Açıklama  zorunludur.");
        }
    }
}
