﻿using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos
{
    public class ReimbursementListDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; } // Tutar
        public string Description { get; set; } // Açıklama
        public Currency Currency { get; set; } // Para Birimi
        public PaymentStatus PaymentStatus { get; set; } // Ödeme Durumu
        public Guid EmployeeId { get; set; } // Kullanıcı ID
        public DateTime CreatedDate { get; set; }
    }
}
