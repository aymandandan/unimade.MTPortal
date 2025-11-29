using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using unimade.MTPortal.Announcements;

namespace unimade.MTPortal.Web.Pages.External.Announcements
{
    public class IndexModel : PageModel
    {
        private readonly IAnnouncementAppService _announcementAppService;

        public AnnouncementDto Announcement { get; set; }
        public PagedResultDto<AnnouncementDto> RelatedAnnouncements { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public IndexModel(IAnnouncementAppService announcementAppService)
        {
            _announcementAppService = announcementAppService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == Guid.Empty)
            {
                return RedirectToPage("/External/Index");
            }

            try
            {
                Announcement = await _announcementAppService.GetAsync(Id);

                // Only show published announcements to external users
                if (!Announcement.IsPublished)
                {
                    Announcement = null;
                    return Page();
                }

                // Load related announcements (same tenant, published, excluding current)
                var relatedInput = new AnnouncementGetListInput
                {
                    SkipCount = 0,
                    MaxResultCount = 4,
                    IsPublished = true,
                    ExcludeId = Id
                };

                RelatedAnnouncements = await _announcementAppService.GetListAsync(relatedInput);

                return Page();
            }
            catch (Exception)
            {
                // If announcement not found or user doesn't have permission
                Announcement = null;
                return Page();
            }
        }
    }
}