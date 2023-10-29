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
    public class PackageConfiguration: AuditableEntityConfiguration<Package>
    {
        public override void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.Property(x=>x.PackageName).IsRequired().HasMaxLength(256); ;
            builder.Property(x=>x.Price).IsRequired();
            builder.Property(x=>x.NumberOfUser).IsRequired();
            base.Configure(builder);
        }
    }
}
