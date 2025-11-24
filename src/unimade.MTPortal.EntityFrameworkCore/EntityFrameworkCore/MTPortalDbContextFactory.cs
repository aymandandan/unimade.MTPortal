using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace unimade.MTPortal.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class MTPortalDbContextFactory : IDesignTimeDbContextFactory<MTPortalDbContext>
{
    public MTPortalDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        MTPortalEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<MTPortalDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new MTPortalDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../unimade.MTPortal.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
