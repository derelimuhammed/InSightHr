using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos
{
    public class ReimbursementDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } // Tarih
        public string Description { get; set; } // Açıklama
        public decimal Amount { get; set; } // Tutar
        public Currency Currency { get; set; } // Para Birimi
        public PaymentStatus PaymentStatus { get; set; } // Ödeme Durumu
        public Guid EmployeeId { get; set; } // Kullanıcı ID
        public EmployeeDto Employee { get; set; } // Kullanıcı 
        public Byte[] ExpenseAttachments { get; set; } // Masraf Ekleri (Bu sütunun veri türüne göre değişebilir)
        public DateTime CreatedDate { get; set; }
    }
}
