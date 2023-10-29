using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationVms
{
    public class OrganizationListVm
    {
        public Guid Id { get; set; }
        [DisplayName("Organizasyon Adı")]
        public string OrganizationName { get; set; }

        [DisplayName("Organizasyon Adresi")]
        public string? OrganizationAddress { get; set; }

        [DisplayName("Organizasyon E-Posta")]
        public string OrganizationEmail { get; set; }

        [DisplayName("Logo")]
        public byte[]? Logo { get; set; }

        [DisplayName("Sözleşme Başlangıç Tarihi")]
        public DateTime ContractStart { get; set; }

        [DisplayName("Sözleşme Bitiş Tarihi")]
        public DateTime ContractEnd { get; set; }

        [DisplayName("Aktif mi?")]
        public bool IsActive { get; set; }
    }
}
