using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class Reimbursement:AuditableEntity
    {
        public DateTime Date { get; set; } // Tarih
        public string Description { get; set; } // Açıklama
        public decimal Amount { get; set; } // Tutar
        public Currency Currency { get; set; } // Para Birimi
        public PaymentStatus PaymentStatus { get; set; } // Ödeme Durumu
        public Guid EmployeeId { get; set; } // Kullanıcı ID
        public Byte[] ExpenseAttachments { get; set; } // Masraf Ekleri (Bu sütunun veri türüne göre değişebilir)
        public virtual Employee Employee   { get; set; }
    }
}
