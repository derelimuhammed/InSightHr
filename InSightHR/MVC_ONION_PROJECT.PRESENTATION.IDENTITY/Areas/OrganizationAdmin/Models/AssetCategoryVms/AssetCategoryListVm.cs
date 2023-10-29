using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AssetCategoryVms
{
    public class AssetCategoryListVm
    {
        public Guid Id { get; set; }
        [DisplayName("Kategori Adı")]
        public string CategoryName { get; set; }
        [DisplayName("Kategori Açıklaması")]
        public string? CategoryDescriptiom { get; set; }
    }
}
