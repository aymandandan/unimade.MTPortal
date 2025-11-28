using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using unimade.MTPortal.Announcements;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace unimade.MTPortal.Dashboards
{
    public interface IDashboardAppService : IApplicationService
    {
        Task<DashboardStatsDto> GetStatsAsync();
        Task<ListResultDto<AnnouncementDto>> GetRecentAnnouncementsAsync(int maxCount = 5);
    }
}
