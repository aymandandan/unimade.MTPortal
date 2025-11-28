namespace unimade.MTPortal.Permissions;

public static class MTPortalPermissions
{
    public const string GroupName = "MTPortal";

    public static class User
    {
        public const string Default = GroupName + ".User";

        public const string PublicManagement = Default + ".Public.Management";
    }

    public static class Announcements
    {
        public const string Default = GroupName + ".Announcement";
        public const string Manage = Default + ".Manage";
        public const string View = Default + ".View";
    }

}
