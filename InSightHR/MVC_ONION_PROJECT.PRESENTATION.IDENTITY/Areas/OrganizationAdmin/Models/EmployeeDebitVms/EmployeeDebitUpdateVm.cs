using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms
{
    public class EmployeeDebitUpdateVm
    {
        [DisplayName("Kimlik")]
        public Guid Id { get; set; }

        [DisplayName("Çalışan Kimliği")]
        public Guid EmployeeId { get; set; }

        [DisplayName("Çalışan Ad Soyad")]
        public string EmployeeName { get; set; }

        [DisplayName("Zimmet Adı")]
        public Guid OrgAssetId { get; set; }

        [DisplayName("Zimmet Adı")]
        public string DebitName { get; set; }

        [DataType(DataType.Date)]

        [DisplayName("Zimmet Geri Teslim Tarihi")]
        public DateTime ReceiptDate { get; set; }


        [DisplayName("Açıklama")]
        public string? Description { get; set; }
    }
}
