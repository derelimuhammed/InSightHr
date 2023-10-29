using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class TimeOff : AuditableEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfDays { get; set; }
        public string? Reason { get; set; }
        public string? Description { get; set; }
        public Guid TimeOffTypeId { get; set; }
        public Guid? EmployeeId { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual TimeOffType TimeOffType { get; set; }
    }
}
