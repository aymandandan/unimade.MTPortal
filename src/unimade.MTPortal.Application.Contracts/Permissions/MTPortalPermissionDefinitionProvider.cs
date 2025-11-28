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
        announcementPermissions.AddChild(MTPortalPermissions.Announcements.Create, L("Permissions:Announcement.Create"));
        announcementPermissions.AddChild(MTPortalPermissions.Announcements.Edit, L("Permissions:Announcement.Edit"));
        announcementPermissions.AddChild(MTPortalPermissions.Announcements.Delete, L("Permissions:Announcement.Delete"));
        announcementPermissions.AddChild(MTPortalPermissions.Announcements.View, L("Permissions:Announcement.View"));

        // User Type permissions - Add to Identity group
        var identityGroup = context.GetGroup(IdentityPermissions.GroupName);

        var userPermission = identityGroup.AddPermission(MTPortalPermissions.User.Default, L("Permission:User"));

        var userTypePermission = userPermission.AddChild(MTPortalPermissions.User.UserType.Default, L("Permission:User.UserType"));
        userTypePermission.AddChild(MTPortalPermissions.User.UserType.Update, L("Permission:User.UserType.Update"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MTPortalResource>(name);
    }
}
