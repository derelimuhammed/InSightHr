using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms
{
    public class EmployeeAdvanceCreateVm
    {
        public Guid Id { get; set; }
        public int AdvancePrice { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
        public Guid EmployeeId { get; set; }

        [DisplayName("Açıklama")]
        public string? Reason { get; set; }

    }
}
