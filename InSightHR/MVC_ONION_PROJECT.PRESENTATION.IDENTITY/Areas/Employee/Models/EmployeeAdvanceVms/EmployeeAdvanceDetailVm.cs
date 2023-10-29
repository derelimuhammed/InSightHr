using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms
{
    public class EmployeeAdvanceDetailVm
    {
        public Guid Id { get; set; }

        [DisplayName("Avans Miktarı")]
        public int AdvancePrice { get; set; }

        [DisplayName("Dönüş Durumu")]
        public ReturnStatus ReturnStatus { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Açıklama")]
        public string? Description { get; set; }

        [DisplayName("Neden")]
        public string? Reason { get; set; }

    }
}
