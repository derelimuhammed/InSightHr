using MVC_ONION_PROJECT.DOMAIN.ENUMS;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms
{
    public class TimeOffListVm
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfDays { get; set; }
        public string Name { get; set; }
        public Guid TimeOffTypeId { get; set; }
        public string CreatedBy { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
