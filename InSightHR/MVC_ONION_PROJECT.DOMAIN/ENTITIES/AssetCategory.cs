using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class AssetCategory : AuditableEntity
    {
        public string CategoryName { get; set; }
        public string? CategoryDescriptiom { get; set; }
        public Guid OrganizationId { get; set; }

        //Nav
        public virtual ICollection<OrgAsset> OrgAssets { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
