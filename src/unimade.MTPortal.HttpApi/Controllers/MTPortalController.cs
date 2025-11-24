using unimade.MTPortal.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace unimade.MTPortal.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MTPortalController : AbpControllerBase
{
    protected MTPortalController()
    {
        LocalizationResource = typeof(MTPortalResource);
    }
}
