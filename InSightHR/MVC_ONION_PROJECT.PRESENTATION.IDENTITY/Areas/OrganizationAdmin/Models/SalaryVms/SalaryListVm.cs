using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.SalaryVms
{
    public class SalaryListVm
    {
        [Display(Name = "Kimlik")]
        public Guid Id { get; set; }

        [Display(Name = "Çalışan Kimliği")]
        public Guid EmployeeId { get; set; }

        [Display(Name = "Çalışan Adı")]
        public string EmployeeName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Maaş Başlangıç Tarihi")]
        public DateTime SalaryDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Maaş Bitiş Tarihi")]
        public DateTime SalaryEndDate { get; set; }

        [Display(Name = "Maaş")]
        public double Salary { get; set; }

        [Display(Name = "Maaş Durumu")]
        public SalaryStatus SalaryStatus { get; set; }
    }
}
