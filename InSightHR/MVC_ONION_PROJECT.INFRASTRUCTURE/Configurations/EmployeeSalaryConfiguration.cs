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
    public class EmployeeSalaryConfiguration : AuditableEntityConfiguration<EmployeeSalary>
    {
        public override void Configure(EntityTypeBuilder<EmployeeSalary> builder)
        {
            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.SalaryDate).IsRequired();
            builder.Property(x => x.SalaryEndDate).IsRequired();
            builder.Property(x => x.Salary).IsRequired();

            base.Configure(builder);
        }
    }
}
