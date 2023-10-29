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
    public class OrganizationConfiguration : AuditableEntityConfiguration<Organization>
    {
        public override void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.Property(x => x.OrganizationName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.OrganizationAddress).HasMaxLength(160);
            builder.Property(x => x.logo).IsRequired(false);
            builder.Property(x => x.OrganizationEmail).IsRequired();
            builder.Property(x => x.OrganizationPhone).IsRequired();
            builder.Property(x => x.TaxNumber).IsRequired();
            builder.Property(x => x.PackageId).IsRequired(false);


            base.Configure(builder);
        }
    }
}
