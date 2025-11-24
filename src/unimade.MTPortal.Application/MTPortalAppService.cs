using unimade.MTPortal.Localization;
using Volo.Abp.Application.Services;

namespace unimade.MTPortal;

/* Inherit your application services from this class.
 */
public abstract class MTPortalAppService : ApplicationService
{
    protected MTPortalAppService()
    {
        LocalizationResource = typeof(MTPortalResource);
    }
}
