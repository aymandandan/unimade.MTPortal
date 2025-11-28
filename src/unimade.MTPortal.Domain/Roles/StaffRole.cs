using System;
using System.Collections.Generic;
using System.Text;

namespace unimade.MTPortal.Roles
{
    public static class StaffRole
    {
        public const string Name = "Staff";
        public const string Deiscription = "Staff members with announcement and public users management permissions";

        public static class Permissions
        {
            public const string AnnouncementManagement = "MTPortal.Announcements.Manage";
            public const string ViewAnnouncements = "MTPortal.Announcements.View";
            public const string PublicUsersManagement = "MTPortal.Users.Public.Management";
        }
    }
}
