using System.Threading.Tasks;
using unimade.MTPortal.Localization;
using unimade.MTPortal.Permissions;
using unimade.MTPortal.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using System.Security.Policy;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;
using unimade.MTPortal.Roles;

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
        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

        //Home
        if (!currentUser.IsAuthenticated || currentUser.IsInRole("admin"))
        {
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    MTPortalMenus.Home,
                    l["Menu:Home"],
                    "~/",
                    icon: "fa fa-home",
                    order: 1
                )
            );
        }


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

        if (currentTenant.Id != null)
        {
            // Internal Dashboard
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    MTPortalMenus.InternalDashboard,
                    l["Menu:Internal.Dashboard"],
                    url: "/Internal/Dashboard",
                    icon: "fa fa-dashboard",
                    order: 2
                )
                .RequirePermissions(MTPortalPermissions.Announcements.Manage, MTPortalPermissions.User.PublicManagement)
            );

            // Public User Management
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    MTPortalMenus.PublicUsers,
                    l["Menu:Internal.PublicUsers"],
                    url: "/Internal/Users",
                    icon: "fas fa-user-friends",
                    order: 3
                )
                .RequirePermissions(MTPortalPermissions.User.PublicManagement)
            );

            // Announcement
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    MTPortalMenus.Announcements,
                    l["Menu:Internal.Announcements"],
                    url: "/Internal/Announcements",
                    icon: "fa fa-bullhorn",
                    order: 4
                )
                .RequirePermissions(MTPortalPermissions.Announcements.Manage)
            );
        }

        return Task.CompletedTask;
    }
}
