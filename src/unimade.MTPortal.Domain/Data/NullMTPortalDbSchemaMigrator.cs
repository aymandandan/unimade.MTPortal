using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace unimade.MTPortal.Data;

/* This is used if database provider does't define
 * IMTPortalDbSchemaMigrator implementation.
 */
public class NullMTPortalDbSchemaMigrator : IMTPortalDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
