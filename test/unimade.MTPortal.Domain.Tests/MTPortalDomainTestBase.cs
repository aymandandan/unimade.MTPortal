using Volo.Abp.Modularity;

namespace unimade.MTPortal;

/* Inherit from this class for your domain layer tests. */
public abstract class MTPortalDomainTestBase<TStartupModule> : MTPortalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
