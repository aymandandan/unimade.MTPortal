using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unimade.MTPortal.Accouncements;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace unimade.MTPortal.Announcements
{
    public class AnnouncementAppService :
        CrudAppService<
            Announcement,
            AnnouncementDto,
            Guid,
            AnnouncementGetListInput,
            CreateUpdateAnnounementDto,
            CreateUpdateAnnounementDto>,
        IAnnouncementAppService
    {
        public AnnouncementAppService(IRepository<Announcement, Guid> repository) 
            : base(repository)
        {

        }

        // Override the GetListAsync method to apply custom filtering, sorting, and paging
        public override async Task<PagedResultDto<AnnouncementDto>> GetListAsync(AnnouncementGetListInput input)
        {
            // Create a queryable from the repository
            var queryable = await Repository.GetQueryableAsync();

            // Apply custom filters
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                x => x.Title.Contains(input.Filter) || x.Content.Contains(input.Filter))
            .WhereIf(input.IsPublished.HasValue,
                x => x.IsPublished == input.IsPublished.Value);
            }

            // Apply sorting and paging
            var announcements = await AsyncExecuter.ToListAsync(
                queryable.OrderBy(x => input.Sorting ?? "CreationTime DESC")
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            // Convert to DTOs and return
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            return new PagedResultDto<AnnouncementDto>(
                totalCount,
                ObjectMapper.Map<List<Announcement>, List<AnnouncementDto>>(announcements)
            );
        }

        // Override the CreateAsync method to set TenantId
        public override async Task<AnnouncementDto> CreateAsync(CreateUpdateAnnounementDto input)
        {
            // Create the entity using the base class
            var announcement = new Announcement(
                GuidGenerator.Create(),
                input.Title,
                input.Content,
                CurrentTenant.Id // Set the tenant ID from the current context
            );

            // You can add custom logic here, e.g., set PublishDate if IsPublished is true
            if (input.IsPublished)
            {
                announcement.Publish();
            }

            await Repository.InsertAsync(announcement);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Announcement, AnnouncementDto>(announcement);
        }
    }
}
