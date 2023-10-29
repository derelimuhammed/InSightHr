using FluentValidation;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.FluentValidators.DepartmentValidator
{
    public class DepartmentCreateValidator : AbstractValidator<DepartmentCreateVm>
    {
        public DepartmentCreateValidator()
        {
            RuleFor(x => x.DepartmentName).NotEmpty().WithMessage("Departman ismi zorunludur.").MinimumLength(5).WithMessage("Departman ismi en az 5 karakterli olmalıdır.").NotNull().WithMessage("Departman ismi zorunludur.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description ismi zorunludur.").MinimumLength(5).WithMessage("Description ismi en az 5 karakterli olmalıdır.").NotNull().WithMessage("Description ismi zorunludur.");
        }
    }
}
