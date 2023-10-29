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
    public class TimeOffConfiguration : AuditableEntityConfiguration<TimeOff>
    {
        public override void Configure(EntityTypeBuilder<TimeOff> builder)
        {
            builder.Property(x => x.StartTime).IsRequired();
            builder.Property(x => x.EndTime).IsRequired();


            base.Configure(builder);
        }
    }
}
