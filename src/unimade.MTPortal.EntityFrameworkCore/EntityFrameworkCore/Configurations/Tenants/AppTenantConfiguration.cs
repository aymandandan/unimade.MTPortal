using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using unimade.MTPortal.Tenants;

namespace unimade.MTPortal.EntityFrameworkCore.Configurations.Tenants
{
    public class AppTenantConfiguration : IEntityTypeConfiguration<AppTenant>
    {
        public void Configure(EntityTypeBuilder<AppTenant> builder)
        {
            builder.ToTable("AbpTenants"); // Extend the default Tenant table name

            // Configure additional properties
            builder.Property(t => t.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.ContactEmail)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(t => t.DisplayName)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
