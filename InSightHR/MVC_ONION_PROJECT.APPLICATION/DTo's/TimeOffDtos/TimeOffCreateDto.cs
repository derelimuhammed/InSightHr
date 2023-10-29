using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos
{
    public class TimeOffCreateDto
    {
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
        public int NumberOfDays { get; set; }
        public string Reason { get; set; }
        public Guid? TimeOffTypeId { get; set; }
        public string CreatedBy { get; set; }
        public Guid EmployeeId { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
