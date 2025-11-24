using System.Threading.Tasks;

namespace unimade.MTPortal.Data;

public interface IMTPortalDbSchemaMigrator
{
    Task MigrateAsync();
}
