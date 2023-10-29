using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.Package;
using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationVms
{
    public class OrganizationUpdateVm
    {
        public Guid Id { get; set; }
        [DisplayName("Organizasyon Adı")]
        public string OrganizationName { get; set; }

        [DisplayName("Organizasyon E-Posta")]
        public string OrganizationEmail { get; set; }

        [DisplayName("Organizasyon Telefon")]
        public string? OrganizationPhone { get; set; }

        [DisplayName("Organizasyon Adres")]
        public string? OrganizationAddress { get; set; }

        [DisplayName("Vergi Numarası")]
        public string TaxNumber { get; set; }

        [DisplayName("Logo")]
        public IFormFile? Logopath { get; set; }

        [DisplayName("Sözleşme Başlangıç Tarihi")]
        public DateTime ContractStart { get; set; }

        [DisplayName("Sözleşme Bitiş Tarihi")]
        public DateTime ContractEnd { get; set; }

        [DisplayName("Paket ID")]
        public Guid? PackageId { get; set; }

        [DisplayName("Paket Listesi")]
        public List<PackageListVm> PackageList { get; set; }
    }
}
