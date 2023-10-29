using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms
{
    public class EmployeeDebitListVm
    {
        [DisplayName("Kimlik")]
        public Guid Id { get; set; }

        [DisplayName("Zimmet Adı")]
        public Guid OrgAssetId { get; set; }

        [DisplayName("Zimmet Adı")]
        public string DebitName { get; set; }

        [DisplayName("Çalışan Kimliği")]
        public Guid EmployeeId { get; set; }

        [DisplayName("Çalışan Ad Soyad")]
        public string EmployeeName { get; set; }

        [DisplayName("Zimmet Atama Tarihi")]
		[DataType(DataType.Date)]
		public DateTime AssignmentDate { get; set; }

        [DisplayName("Zimmet Durumu")]
        public ReturnStatus ReturnStatus { get; set; }
    }
}
