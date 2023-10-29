using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationAdminVms
{
    public class OrganizationAdminUpdateVm
    {
        [Display(Name = "Kimlik")]
        public Guid Id { get; set; }

        [Display(Name = "Adı")]
        public string? Name { get; set; }

        [Display(Name = "Soyadı")]
        public string? Surname { get; set; }

        [Display(Name = "Adres")]
        public string? Address { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "İşe Alım Tarihi")]
        public DateTime RecruitmentDate { get; set; }

        [Display(Name = "Departman")]
        public Guid DepartmentId { get; set; }

        public SelectList? DepartmentList { get; set; }

        [Display(Name = "Fotoğraf Yolu")]
        public string? Photopath { get; set; }

        [Display(Name = "Dosya")]
        public IFormFile? File { get; set; }
        [Display(Name = "Cinsiyeti")]
        public GenderStatus GenderStatus { get; set; }
    }
}
