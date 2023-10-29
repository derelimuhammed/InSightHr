using MVC_ONION_PROJECT.DOMAIN.ENUMS;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms
{
    public class TimeOffUpdateVm
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfDays { get; set; }
        public string Reason { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
