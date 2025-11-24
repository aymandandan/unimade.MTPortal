using Volo.Abp.Settings;

namespace unimade.MTPortal.Settings;

public class MTPortalSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MTPortalSettings.MySetting1));
    }
}
