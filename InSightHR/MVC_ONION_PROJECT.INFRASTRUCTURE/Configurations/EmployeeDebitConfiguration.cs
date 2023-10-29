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
    public class EmployeeDebitConfiguration : AuditableEntityConfiguration<EmployeeDebit>
    {
        public override void Configure(EntityTypeBuilder<EmployeeDebit> builder)
        {
            builder.Property(x => x.OrgAssetId).IsRequired();
            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.AssignmentDate).IsRequired();
            builder.Property(x => x.ReturnStatus).IsRequired();

            base.Configure(builder);
        }
    }
}
