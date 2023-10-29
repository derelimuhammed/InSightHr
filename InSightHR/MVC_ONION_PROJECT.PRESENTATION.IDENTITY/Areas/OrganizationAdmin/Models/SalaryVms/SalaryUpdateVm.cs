using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.SalaryVms
{
    public class SalaryUpdateVm
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        [Display(Name = "Çalışan Ad Soyad")]
        public string EmployeeName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Maaş Başlangıç Tarihi")]
        public DateTime SalaryDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Maaş Bitiş Tarihi")]
        public DateTime SalaryEndDate { get; set; }

        [Display(Name = "Maaş")]
        public double Salary { get; set; }
    }
}
