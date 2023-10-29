using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class AdvancePayment : AuditableEntity
    {
        public Guid Id { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
        public int AdvancePrice { get; set; }
        public Guid EmployeeId { get; set; }
        public string? Description { get; set; }
        public string? Reason { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
