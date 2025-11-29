using System;
using System.Collections.Generic;
using System.Text;

namespace unimade.MTPortal.Roles
{
    public static class PublicRole
    {
        public const string Name = "Public";
        public const string Description = "Public members with announcements browsing";

        public static class Permissions
        {
            public const string ViewAnnouncements = "MTPortal.Announcement.View";
        }
    }
}
