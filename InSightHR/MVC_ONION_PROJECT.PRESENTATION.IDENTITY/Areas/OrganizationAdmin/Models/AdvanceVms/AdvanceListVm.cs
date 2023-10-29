using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AdvanceVms
{
    public class AdvanceListVm
    {
        public Guid Id { get; set; }
        [DisplayName("Geri Dönüş Durumu")]
        public ReturnStatus ReturnStatus { get; set; }
        [DisplayName("Avans Miktarı")]
        public int AdvancePrice { get; set; }
        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; }
        public EmployeeDto Employee { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
