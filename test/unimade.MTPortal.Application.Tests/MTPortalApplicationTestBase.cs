using Volo.Abp.Modularity;

namespace unimade.MTPortal;

public abstract class MTPortalApplicationTestBase<TStartupModule> : MTPortalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
