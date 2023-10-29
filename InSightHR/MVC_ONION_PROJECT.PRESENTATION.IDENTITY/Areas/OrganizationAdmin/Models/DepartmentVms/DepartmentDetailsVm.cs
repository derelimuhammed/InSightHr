using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms
{
    public class DepartmentDetailsVm
    {
        public Guid Id { get; set; }
        [DisplayName("Departman Adı")]
        public string DepartmentName { get; set; }
        [DisplayName("Departman Açıklaması")]
        public string? Description { get; set; }
        [DisplayName("Organizasyon Adı")]
        public string OrganizationName { get; set; }
    }
}
