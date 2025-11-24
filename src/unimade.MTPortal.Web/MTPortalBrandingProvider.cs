using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using unimade.MTPortal.Localization;

namespace unimade.MTPortal.Web;

[Dependency(ReplaceServices = true)]
public class MTPortalBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<MTPortalResource> _localizer;

    public MTPortalBrandingProvider(IStringLocalizer<MTPortalResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
