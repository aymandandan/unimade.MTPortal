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
            public const string AnnouncementManagement = "MTPortal.Announcement.Manage";
            public const string ViewAnnouncements = "MTPortal.Announcement.View";
            public const string PublicUsersManagement = "MTPortal.User.Public.Management";
            public const string GetUser = "AbpIdentity.Users";
            public const string CreateUser = "AbpIdentity.Users.Create";
            public const string UpdateUser = "AbpIdentity.Users.Update";
            public const string DeleteUser = "AbpIdentity.Users.Delete";
        }
    }
}
