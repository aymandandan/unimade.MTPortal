using System;
using System.Collections.Generic;
using System.Text;

namespace unimade.MTPortal.Dashboards
{
    public class DashboardStatsDto
    {
        public long TotalPublicUsers { get; set; }
        public long TotalAnnouncements { get; set; }
        public long PublishedAnnouncements { get; set; }
        public long DraftAnnouncements { get; set; }
        public DateTime? LastAnnouncementDate { get; set; }
    }
}
