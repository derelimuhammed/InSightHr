using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.EmployeeFluentValidators.EmployeeAdvancePaymentValidator
{
    public class EmployeeAdvancePaymentUpdateValidator : AbstractValidator<EmployeeAdvanceUpdateVm>
    {
        public EmployeeAdvancePaymentUpdateValidator()
        {
            RuleFor(x => x.AdvancePrice).NotEmpty().WithMessage("Lütfen Avans Miktarı Giriniz.").NotNull().WithMessage("Lütfen Avans Miktarı Giriniz.");
        }
    }
}
