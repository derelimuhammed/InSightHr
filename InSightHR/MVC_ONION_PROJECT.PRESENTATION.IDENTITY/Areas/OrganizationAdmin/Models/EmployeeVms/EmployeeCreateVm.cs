using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms
{
    public class EmployeeCreateVm
    {

        [Display(Name = "Adı")]
        public string? Name { get; set; }

        [Display(Name = "Soyadı")]
        public string? Surname { get; set; }

        [Display(Name = "Adres")]
        public string? Address { get; set; }

        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "İşe Alım Tarihi")]
        public DateTime RecruitmentDate { get; set; }

        [Display(Name = "Departman Adı")]
        public Guid DepartmentId { get; set; }

        public SelectList? DepartmentList { get; set; }

        [Display(Name = "Fotoğraf Yolu")]
        public string? Photopath { get; set; }

        [Display(Name = "Özel E-Posta Kullan")]
        public bool IsCustomMail { get; set; }

        [Display(Name = "E-Posta")]
        public string? Email { get; set; }

        [Display(Name = "Resim")]
        public IFormFile? File { get; set; }
        [Display(Name = "Cinsiyeti")]
        public GenderStatus GenderStatus { get; set; }
        [Display(Name = "Rol")]
        public Role Role { get; set; }
    }
}
