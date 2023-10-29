using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class Package : AuditableEntity
    {
        public string PackageName { get; set; }
        public double Price { get; set; }
        public int NumberOfUser { get; set; }
        public double PackageDurationMonth { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Organization> Organization { get; set; }
    }
}
