using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos
{
    public class AdvancePaymentUpdateDto
    {
        public Guid Id { get; set; }
        public int AdvancePrice { get; set; }
        public string Reason { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
    }
}
