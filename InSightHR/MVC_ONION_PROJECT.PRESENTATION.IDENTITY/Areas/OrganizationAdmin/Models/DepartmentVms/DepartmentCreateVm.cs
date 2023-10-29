using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms
{
    public class DepartmentCreateVm
    {
        [DisplayName("Departman Adı")]
        public string DepartmentName { get; set; }
        [DisplayName("Departman Açıklaması")]
        public string? Description { get; set; }
    }
}
