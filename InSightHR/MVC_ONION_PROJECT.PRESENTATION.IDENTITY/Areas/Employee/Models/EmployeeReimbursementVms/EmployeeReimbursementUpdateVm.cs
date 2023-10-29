using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeReimbursementVms
{
    public class EmployeeReimbursementUpdateVm
    {
        public Guid Id { get; set; }
        [DisplayName("Tarih")]
        public DateTime Date { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [DisplayName("Tutar")]
        public decimal Amount { get; set; }

        [DisplayName("Para Birimi")]
        public Currency Currency { get; set; }

        [DisplayName("Ödeme Durumu")]
        public PaymentStatus PaymentStatus { get; set; }

        [DisplayName("Kullanıcı ID")]
        public Guid EmployeeId { get; set; }
        [DisplayName("Resim")]

        public IFormFile ExpenseAttachmentsfile { get; set; } // Masraf Ekleri (Bu sütunun veri türüne göre değişebilir)
        public string ExpenseAttachmentspath { get; set; } // Masraf Ekleri (Bu sütunun veri türüne göre değişebilir)
    }
}
