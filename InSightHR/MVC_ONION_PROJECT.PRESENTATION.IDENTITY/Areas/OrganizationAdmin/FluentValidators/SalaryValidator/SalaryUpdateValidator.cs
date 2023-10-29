using FluentValidation;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.SalaryVms;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.FluentValidators.SalaryValidator
{
    public class SalaryUpdateValidator : AbstractValidator<SalaryUpdateVm>
    {
        public SalaryUpdateValidator()
        {
            RuleFor(x => x.Salary).NotEmpty().WithMessage("Maaş bilgisi zorunludur.");
            RuleFor(x => x.SalaryDate).NotEmpty().WithMessage("Maaş başlangıç tarihi bilgisi zorunludur.").LessThan(x => x.SalaryEndDate).WithMessage("Başlangıç tarihi, bitiş tarihinden önce olmalıdır.");
            RuleFor(x => x.SalaryEndDate).NotEmpty().WithMessage("Maaş bitiş tarihi bilgisi zorunludur.").GreaterThanOrEqualTo(x => x.SalaryDate).WithMessage("Bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Çalışan seçimi zorunludur.");
        }
    }
}
