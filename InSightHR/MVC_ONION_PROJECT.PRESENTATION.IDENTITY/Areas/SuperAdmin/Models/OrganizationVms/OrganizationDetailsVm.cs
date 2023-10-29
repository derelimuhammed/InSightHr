using MVC_ONION_PROJECT.APPLICATION.DTo_s.PackageDtos;
using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationVms
{
    public class OrganizationDetailsVm
    {

        [DisplayName("Organizasyon Adı")]
        public string OrganizationName { get; set; }

        [DisplayName("Organizasyon E-Posta")]
        public string OrganizationEmail { get; set; }

        [DisplayName("Organizasyon Telefon")]
        public string? OrganizationPhone { get; set; }

        [DisplayName("Organizasyon Adres")]
        public string? OrganizationAddress { get; set; }

        [DisplayName("Çalışan Sayısı")]
        public int EmployeeOfNumber { get; set; }

        [DisplayName("Vergi Numarası")]
        public string TaxNumber { get; set; }

        [DisplayName("Logo")]
        public byte[]? Logo { get; set; }

        [DisplayName("Sözleşme Başlangıç Tarihi")]
        public DateTime ContractStart { get; set; }

        [DisplayName("Sözleşme Bitiş Tarihi")]
        public DateTime ContractEnd { get; set; }

        [DisplayName("Paket Bilgisi")]
        public PackageDto Package { get; set; }

    }
}
