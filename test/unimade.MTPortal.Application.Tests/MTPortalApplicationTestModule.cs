using Volo.Abp.Modularity;

namespace unimade.MTPortal;

[DependsOn(
    typeof(MTPortalApplicationModule),
    typeof(MTPortalDomainTestModule)
)]
public class MTPortalApplicationTestModule : AbpModule
{

}
