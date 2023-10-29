using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.SalaryVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.TimeOffVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.FluentValidators.TimeOffValidator
{
    public class TimeOffRejectedUpdateValidator: AbstractValidator<TimeOffRejectedUpdateVm>
    {
        public TimeOffRejectedUpdateValidator()
        { 
            RuleFor(x => x.Description).MinimumLength(5).WithMessage("5 harften fazla bir açıklama girin").NotEmpty().WithMessage("Açıklama bilgisi zorunludur.").NotEmpty().WithMessage("Açıklama bilgisi zorunludur.");
        }
           
            
        
    }
}
