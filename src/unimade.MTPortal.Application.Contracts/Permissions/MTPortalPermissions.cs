namespace unimade.MTPortal.Permissions;

public static class MTPortalPermissions
{
    public const string GroupName = "MTPortal";

    public static class User
    {
        public const string Default = GroupName + ".User";

        public static class UserType
        {
            public const string Default = User.Default + ".UserType";
            public const string Update = Default + ".Update";
        }
    }

    public static class Announcements
    {
        public const string Default = GroupName + ".Announcement";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
    }

}
