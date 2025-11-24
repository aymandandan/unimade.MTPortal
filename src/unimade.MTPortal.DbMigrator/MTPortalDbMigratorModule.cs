using unimade.MTPortal.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace unimade.MTPortal.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MTPortalEntityFrameworkCoreModule),
    typeof(MTPortalApplicationContractsModule)
)]
public class MTPortalDbMigratorModule : AbpModule
{
}
