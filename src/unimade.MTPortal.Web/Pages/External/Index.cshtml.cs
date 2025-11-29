using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using unimade.MTPortal.Announcements;

namespace unimade.MTPortal.Web.Pages.External
{
    public class IndexModel : MTPortalPageModel
    {
        private readonly IAnnouncementAppService _announcementAppService;

        public PagedResultDto<AnnouncementDto> Announcements { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SkipCount { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        public IndexModel(IAnnouncementAppService announcementAppService)
        {
            _announcementAppService = announcementAppService;
        }

        public async Task OnGetAsync()
        {
            var input = new AnnouncementGetListInput
            {
                SkipCount = SkipCount,
                MaxResultCount = 10,
                Filter = Filter,
                IsPublished = true // Only show published announcements for external users
            };

            Announcements = await _announcementAppService.GetListAsync(input);
        }
    }
}