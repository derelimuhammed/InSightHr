using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.TimeOffDtos
{
    public class TimeOffRejectedUpdateDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
