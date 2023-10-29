using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class OrgAsset : AuditableEntity
    {
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
        public AssetStatus AssetStatus { get; set; }

        // Demirbaşın zimmetini alan kişi için ilişki
        public virtual ICollection<EmployeeDebit>  EmployeeDebit { get; set; }
        public virtual AssetCategory Category { get; set; }

    }
}
