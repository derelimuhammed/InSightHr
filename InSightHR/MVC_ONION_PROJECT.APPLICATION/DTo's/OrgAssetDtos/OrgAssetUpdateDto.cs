using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos
{
    public class OrgAssetUpdateDto
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid CategoryId { get; set; }
        public string Description { get; set; }
        public AssetStatus AssetStatus { get; set; }
    }
}
