using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeVms
{
    public class EmployeeEmployeeDetailsVm
    {
        public Guid Id { get; set; }

        [DisplayName("İsim")]
        public string? Name { get; set; }

        [DisplayName("Soyisim")]
        public string? Surname { get; set; }

        [Display(Name = "Cinsiyeti")]
        public GenderStatus GenderStatus { get; set; }

        [DisplayName("Fotoğraf")]
        public Byte[]? Photo { get; set; }

        [DisplayName("Adres")]
        public string? Address { get; set; }

        [DisplayName("Telefon Numarası")]
        public string? PhoneNumber { get; set; }

        [DisplayName("Departman Adı")]
        public string? DepartmentName { get; set; }

        [DisplayName("İşe Alım Tarihi")]
        public DateTime RecruitmentDate { get; set; }

    }
}
