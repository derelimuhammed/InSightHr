using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.EmployeeFluentValidators.EmployeeTimeOffValidator
{
    public class EmployeeTimeOffCreateValidator : AbstractValidator<TimeOffCreateVm>
    {
        public EmployeeTimeOffCreateValidator()
        {
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("Lütfen İzin Balşangıç Tarihi Giriniz.").NotNull().WithMessage("Lütfen İzin Balşangıç Tarihi Giriniz.");
            RuleFor(x => x.EndTime).NotEmpty().WithMessage("Lütfen İzin Baitiş Tarihi Giriniz.").NotNull().WithMessage("Lütfen İzin Bitiş Tarihi Giriniz.");
            RuleFor(x => x.TimeOffTypeId)
                .Must(x => IsSelected(x))
                .WithMessage("Lütfen izin türünü seçiniz.");
        }

        private bool IsSelected(string secim)
        {
            if (secim == null)
                return false; 
            else
                return true;
        }
    }
}
