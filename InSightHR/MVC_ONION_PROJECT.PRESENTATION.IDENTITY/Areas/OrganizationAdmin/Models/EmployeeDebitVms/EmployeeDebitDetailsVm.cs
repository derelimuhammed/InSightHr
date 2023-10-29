using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms
{
	public class EmployeeDebitDetailsVm
	{
		[DisplayName("Çalışan Ad Soyad")]
		public string EmployeeName { get; set; }

		[DisplayName("Zimmet Adı")]
		public string DebitName { get; set; }

		[DisplayName("Zimmet Atama Tarihi")]
		[DataType(DataType.Date)]
		public DateTime AssignmentDate { get; set; }

		[DataType(DataType.Date)]

		[DisplayName("Zimmet Geri Teslim Tarihi")]
		public DateTime ReceiptDate { get; set; }

		[DisplayName("Dönüş Durumu")]
		public ReturnStatus ReturnStatus { get; set; }

		[DisplayName("Açıklama")]
		public string? Description { get; set; }
	}
}
