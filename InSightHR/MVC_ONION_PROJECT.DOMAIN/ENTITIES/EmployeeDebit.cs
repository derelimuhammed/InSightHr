using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class EmployeeDebit:AuditableEntity
    {
        [ForeignKey("OrgAsset")]
        public Guid OrgAssetId { get; set; }

        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
        public string? Description { get; set; }

        //Nav prop..
        public virtual Employee Employee { get; set; }
        public virtual OrgAsset OrgAsset { get; set; }
    }
}
