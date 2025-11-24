using unimade.MTPortal.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace unimade.MTPortal.Web.Pages;

public abstract class MTPortalPageModel : AbpPageModel
{
    protected MTPortalPageModel()
    {
        LocalizationResourceType = typeof(MTPortalResource);
    }
}
