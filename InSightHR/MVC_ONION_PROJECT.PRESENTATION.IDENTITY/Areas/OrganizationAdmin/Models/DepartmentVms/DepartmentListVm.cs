using AutoMapper;
using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.DepartmentVms
{
    public class DepartmentListVm 
    {
        public Guid Id { get; set; }
        [DisplayName("Departman Adı")]
        public string DepartmentName { get; set; }

    }
}
