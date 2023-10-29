using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos
{
    public class TimeOffListDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumberOfDays { get; set; }
        public Guid TimeOffTypeId { get; set; }
        public string CreatedBy { get; set; }
        public string TimeOffTypeName { get; set; }
        public Guid EmployeeId { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
