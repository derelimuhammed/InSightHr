using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.TimeOffVms
{
    public class TimeOffRejectedUpdateVm
    {

        [Display(Name = "Kimlik")]
        public Guid Id { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}
