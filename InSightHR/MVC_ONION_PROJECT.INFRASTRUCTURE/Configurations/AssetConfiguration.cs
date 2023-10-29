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

    public class AssetConfiguration : AuditableEntityConfiguration<OrgAsset>
    {
        public override void Configure(EntityTypeBuilder<OrgAsset> builder)
        {
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.SerialNumber).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.PurchaseDate).IsRequired();

            base.Configure(builder);
        }
    }
}
