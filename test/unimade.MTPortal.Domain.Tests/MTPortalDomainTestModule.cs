using Volo.Abp.Modularity;

namespace unimade.MTPortal;

[DependsOn(
    typeof(MTPortalDomainModule),
    typeof(MTPortalTestBaseModule)
)]
public class MTPortalDomainTestModule : AbpModule
{

}
