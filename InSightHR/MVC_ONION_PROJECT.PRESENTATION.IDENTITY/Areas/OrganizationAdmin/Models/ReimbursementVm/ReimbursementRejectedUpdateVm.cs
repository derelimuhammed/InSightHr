using MVC_ONION_PROJECT.DOMAIN.ENUMS;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.ReimbursementVm
{
    public class ReimbursementRejectedUpdateVm
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public PaymentStatus PaymentStatus { get; set; } // Ödeme Durumu
    }
}
