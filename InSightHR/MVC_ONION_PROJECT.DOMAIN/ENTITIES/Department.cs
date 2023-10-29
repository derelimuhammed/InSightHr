using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class Department : AuditableEntity
    {
        public string DepartmentName { get; set; }
        public string? Description { get; set; }
        public Guid OrganizationId { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
