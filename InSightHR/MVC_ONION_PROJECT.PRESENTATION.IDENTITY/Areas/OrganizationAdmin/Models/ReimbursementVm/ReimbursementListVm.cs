using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.ReimbursementVm
{
    public class ReimbursementListVm
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; } // Tutar
        public Currency Currency { get; set; } // Para Birimi
        public PaymentStatus PaymentStatus { get; set; } // Ödeme Durumu
        public Guid EmployeeId { get; set; } // Kullanıcı ID
        public DateTime CreatedDate { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
