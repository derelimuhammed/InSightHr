using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.EmployeeFluentValidators.EmployeeAdvancePaymentValidator
{
    public class EmployeeAdvancePaymentCreateValidator : AbstractValidator<EmployeeAdvanceCreateVm>
    {
        public EmployeeAdvancePaymentCreateValidator()
        {
            RuleFor(x => x.AdvancePrice).NotEmpty().WithMessage("Lütfen Avans Miktarı Giriniz.").NotNull().WithMessage("Lütfen Avans Miktarı Giriniz.").Must(advancePrice => advancePrice <= 999999).WithMessage("Avans Miktarı Maksimum 6 rakam içermelidir.").GreaterThan(0).WithMessage("Tutar pozitif bir değer olmalıdır."); ;
        }
    }
}
