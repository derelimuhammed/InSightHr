using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.OrgAssetVms
{
    public class OrgAssetListVm
    {
        [Display(Name = "Kimlik")]
        public Guid Id { get; set; }

        [DisplayName("Seri No")]
        public string SerialNumber { get; set; }

        [DisplayName("Demirbaş Adı")]
        public string Name { get; set; }

        [DisplayName("Alım Tarihi")]
        [DataType(DataType.Date)]
        [Display(Name = "Satın Alma Tarihi")]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Kategori")]
        public Guid CategoryId { get; set; }

        [DisplayName("Kategori")]
        public string CategoryName { get; set; }

        [DisplayName("Durumu")]
        public AssetStatus AssetStatus { get; set; }
    }
}
