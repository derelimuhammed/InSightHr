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
    public class TimeOffTypeConfiguration : AuditableEntityConfiguration<TimeOffType>
    {
        public override void Configure(EntityTypeBuilder<TimeOffType> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            


            base.Configure(builder);
        }
    }
}
