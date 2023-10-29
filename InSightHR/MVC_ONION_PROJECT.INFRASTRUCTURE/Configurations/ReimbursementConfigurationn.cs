using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC_ONION_PROJECT.DOMAIN.CORE.EntityTypeConfiguration;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.Configurations
{
    public class ReimbursementConfigurationn: AuditableEntityConfiguration<Reimbursement>
    {
        public override void Configure(EntityTypeBuilder<Reimbursement> builder)
        {
            // Tablo adını belirtin (varsayılan olarak sınıf adı kullanılır)
            builder.ToTable("Reimbursements");

            // Primary Key belirleme
            builder.HasKey(r => r.Id);

            // Date özelliği için yapılandırma
            builder.Property(r => r.Date)
                   .IsRequired();

            // Description özelliği için yapılandırma
            builder.Property(r => r.Description)
                   .HasMaxLength(255); // Açıklama sütununun maksimum uzunluğu

            // Amount özelliği için yapılandırma
            builder.Property(r => r.Amount)
                   .HasColumnType("decimal(18, 2)"); // Tutar sütununun veri türü

            // Currency özelliği için yapılandırma
            builder.Property(r => r.Currency)
                   .IsRequired()
                   .HasMaxLength(3); // Para Birimi sütununun maksimum uzunluğu ve gerekli olması

            // PaymentStatus özelliği için yapılandırma
            builder.Property(r => r.PaymentStatus)
                   .IsRequired()
                   .HasMaxLength(50); // Ödeme Durumu sütununun maksimum uzunluğu ve gerekli olması

            // UserID özelliği için yapılandırma
            builder.Property(r => r.EmployeeId)
                   .IsRequired();

            // ExpenseAttachments özelliği için yapılandırma
            // Masraf Ekleri sütununun maksimum uzunluğu
            base.Configure(builder);
        }
    }
}
