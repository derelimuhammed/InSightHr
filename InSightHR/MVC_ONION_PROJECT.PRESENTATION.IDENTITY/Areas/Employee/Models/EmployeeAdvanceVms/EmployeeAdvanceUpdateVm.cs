using MVC_ONION_PROJECT.DOMAIN.ENUMS;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms
{
    public class EmployeeAdvanceUpdateVm
    {
        public Guid Id { get; set; }
        public int AdvancePrice { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
        public string? Reason { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
