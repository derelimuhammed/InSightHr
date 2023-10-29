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

    public class AssetCategoryConfiguration : AuditableEntityConfiguration<AssetCategory>
    {
        public override void Configure(EntityTypeBuilder<AssetCategory> builder)
        {
            builder.Property(x => x.CategoryName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.OrganizationId).IsRequired();

            base.Configure(builder);
        }
    }
}
