using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using unimade.MTPortal.Announcements;
using unimade.MTPortal.Dashboards;

namespace unimade.MTPortal.Web.Pages.Internal.Dashboard
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : MTPortalPageModel
    {
        [BindProperty]
        public DashboardStatsDto DashboardStats { get; set; }

        [BindProperty]
        public List<AnnouncementDto> RecentAnnouncements { get; set; }

        private readonly IDashboardAppService _dashboardAppService;

        public IndexModel(IDashboardAppService dashboardAppService)
        {
            _dashboardAppService = dashboardAppService;
        }

        public async Task OnGetAsync()
        {
            DashboardStats = await _dashboardAppService.GetStatsAsync();
            var recentAnnouncementsResult = await _dashboardAppService.GetRecentAnnouncementsAsync();
            RecentAnnouncements = recentAnnouncementsResult.Items.ToList();
        }
    }
}
