using System.Threading.Tasks;
using unimade.MTPortal.Localization;
using unimade.MTPortal.Permissions;
using unimade.MTPortal.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;

namespace unimade.MTPortal.Web.Menus;

public class MTPortalMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<MTPortalResource>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                MTPortalMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );


        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);
    
        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }
        
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 7);

        // Internal Dashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                MTPortalMenus.InternalDashboard,
                l["Menu:Internal.Dashboard"],
                url: "/Internal/Dashboard",
                icon: "fa fa-dashboard"
            )
        );

        // Announcement
        context.Menu.AddItem(
            new ApplicationMenuItem(
                MTPortalMenus.Announcements,
                l["Menu:Internal.Announcement"],
                url: "/Internal/Announcements",
                icon: "fa fa-bullhorn",
                order: 2
            )
            //.RequirePermissions(MTPortalPermissions.Announcements.Default)
        );

        return Task.CompletedTask;
    }
}
