using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.EmployeeFluentValidators.EmployeeTimeOffValidator
{
    public class EmployeeTimeOffUpdateValidator : AbstractValidator<TimeOffUpdateVm>
    {
        public EmployeeTimeOffUpdateValidator()
        {
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("Lütfen İzin Başlangıç Tarihi Giriniz.").NotNull().WithMessage("Lütfen İzin Başlangıç Tarihi Giriniz.");
            RuleFor(x => x.EndTime).NotEmpty().WithMessage("Lütfen İzin Bitiş Tarihi Giriniz.").NotNull().WithMessage("Lütfen İzin Bitiş Tarihi Giriniz.");
            
        }
    }
}
