using MVC_ONION_PROJECT.DOMAIN.ENUMS;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms
{
    public class EmployeeAdvanceListVm
    {
        public Guid Id { get; set; }
        public int AdvancePrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
