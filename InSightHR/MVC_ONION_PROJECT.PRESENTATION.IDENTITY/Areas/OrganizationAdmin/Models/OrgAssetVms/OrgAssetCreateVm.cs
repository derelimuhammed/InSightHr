using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.OrgAssetVms
{
    public class OrgAssetCreateVm
    {
        [DisplayName("Seri No")]
        public string SerialNumber { get; set; }

        [DisplayName("Demirbaş Adı")]
        public string Name { get; set; }

        [DisplayName("Fiyatı")]
        public float Price { get; set; }

        [DisplayName("Alım Tarihi")]

        [DataType(DataType.Date)]
        [Display(Name = "Satın Alma Tarihi")]
        public DateTime PurchaseDate { get; set; }

        [DisplayName("Kategori")]
        public Guid CategoryId { get; set; }

        public SelectList? CategoryList { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }
    }
}
