using unimade.MTPortal.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace unimade.MTPortal.Permissions;

public class MTPortalPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var mTPortalGroup = context.AddGroup(MTPortalPermissions.GroupName);

        // Announcement Permissions
        var announcementPermissions = mTPortalGroup.AddPermission(MTPortalPermissions.Announcements.Default, L("Permission:Announcement"));
        announcementPermissions.AddChild(MTPortalPermissions.Announcements.Manage, L("Permissions:Announcement.Manage"));
        announcementPermissions.AddChild(MTPortalPermissions.Announcements.View, L("Permissions:Announcement.View"));

        // User Type permissions - Add to Identity group
        var identityGroup = context.GetGroup(IdentityPermissions.GroupName);

        var userPermission = identityGroup.AddPermission(MTPortalPermissions.User.Default, L("Permission:User"));
        userPermission.AddChild(MTPortalPermissions.User.PublicManagement, L("Permission:User.Public.Management"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MTPortalResource>(name);
    }
}
