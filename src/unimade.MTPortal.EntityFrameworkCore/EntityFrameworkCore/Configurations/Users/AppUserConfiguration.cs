using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using unimade.MTPortal.Users;

namespace unimade.MTPortal.EntityFrameworkCore.Configurations.Users
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AbpUsers"); // Extend the default Identity table name

            // Configure additional properties
            builder.Property(u => u.UserType)
                .IsRequired()
                .HasDefaultValue(UserType.Public);

            // Index on UserType for faster queries
            builder.HasIndex(u => u.UserType);
            builder.HasIndex(u => new {u.TenantId, u.UserType });

        }
    }
}
