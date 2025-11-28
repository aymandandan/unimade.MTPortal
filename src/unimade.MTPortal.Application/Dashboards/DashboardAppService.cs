using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using unimade.MTPortal.Accouncements;
using unimade.MTPortal.Announcements;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace unimade.MTPortal.Dashboards
{
    public class DashboardAppService : MTPortalAppService, IDashboardAppService
    {
        private readonly IRepository<Announcement, Guid> _announcementRepository;
        private readonly IIdentityUserRepository _userRepository;

        public DashboardAppService(
            IRepository<Announcement, Guid> announcementRepository,
            IIdentityUserRepository userRepository)
        {
            _announcementRepository = announcementRepository;
            _userRepository = userRepository;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            // Get tenant-specific data
            var tenantId = CurrentTenant.Id;

            var announcementQueryable = await _announcementRepository.GetQueryableAsync();

            // Use repository methods for users instead of GetQueryableAsync
            var totalUsers = await _userRepository.GetCountAsync();

            // Filter by current tenant
            announcementQueryable = announcementQueryable.Where(x => x.TenantId == tenantId);

            var totalAnnouncements = await AsyncExecuter.CountAsync(announcementQueryable);
            var publishedAnnouncements = await AsyncExecuter.CountAsync(
              announcementQueryable.Where(x => x.IsPublished));
            var lastAnnouncement = await AsyncExecuter.FirstOrDefaultAsync(
              announcementQueryable.OrderByDescending(x => x.CreationTime));

            return new DashboardStatsDto
            {
                TotalPublicUsers = (int)totalUsers,
                TotalAnnouncements = totalAnnouncements,
                PublishedAnnouncements = publishedAnnouncements,
                DraftAnnouncements = totalAnnouncements - publishedAnnouncements,
                LastAnnouncementDate = lastAnnouncement?.CreationTime
            };
        }

        public async Task<ListResultDto<AnnouncementDto>> GetRecentAnnouncementsAsync(int maxCount = 5)
        {
            var tenantId = CurrentTenant.Id;
            var queryable = await _announcementRepository.GetQueryableAsync();

            queryable = queryable
                .Where(x => x.TenantId == tenantId)
                .OrderByDescending(x => x.CreationTime)
                .Take(maxCount);

            var announcements = await AsyncExecuter.ToListAsync(queryable);
            return new ListResultDto<AnnouncementDto>(
                ObjectMapper.Map<List<Announcement>, List<AnnouncementDto>>(announcements)
            );
        }
    }
}
