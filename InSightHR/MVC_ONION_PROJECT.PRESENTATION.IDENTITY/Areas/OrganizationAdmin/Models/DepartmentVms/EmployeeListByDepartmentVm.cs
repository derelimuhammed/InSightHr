using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms
{
    public class EmployeeListByDepartmentVm
    {
        public Guid Id { get; set; }

        [Display(Name = "Adı")]
        public string? Name { get; set; }

        [Display(Name = "Soyadı")]
        public string? Surname { get; set; }

        [Display(Name = "Fotoğraf")]
        public byte[]? Photo { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "İşe Alım Tarihi")]
        [DataType(DataType.Date)]
        public DateTime RecruitmentDate { get; set; }

        [Display(Name = "Departman")]
        public string Department { get; set; }

        public Guid DepartmentId { get; set; }
    }
}
