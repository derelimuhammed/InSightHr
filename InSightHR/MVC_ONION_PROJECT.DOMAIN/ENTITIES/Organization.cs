using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class Organization: AuditableEntity
    {
        public string OrganizationName { get; set; }
        public string OrganizationEmail { get; set; }
        public string? OrganizationPhone { get; set; }
        public string? OrganizationAddress { get; set; }
        public string TaxNumber { get; set; }
        public Byte[]? logo { get; set; }
        public DateTime ContractStart { get; set; }
        public DateTime ContractEnd { get; set; }
        public Guid? PackageId { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<AssetCategory> AssetCategories { get; set; }
        public virtual Package Package { get; set; }
    }
}
