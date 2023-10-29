using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AdvanceVms
{
    public class AdvancePaymentRejectedUpdateVm
    {
        [DisplayName ("Kimlik")]
        public Guid Id { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}

