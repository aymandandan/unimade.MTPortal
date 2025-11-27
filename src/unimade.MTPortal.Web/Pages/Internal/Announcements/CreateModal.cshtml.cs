using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using unimade.MTPortal.Announcements;

namespace unimade.MTPortal.Web.Pages.Internal.Announcements
{
    public class CreateModalModel : MTPortalPageModel
    {
        [BindProperty]
        public CreateUpdateAnnounementDto Announcement { get; set; }

        private readonly IAnnouncementAppService _announcementAppService;

        public CreateModalModel(IAnnouncementAppService announcementAppService)
        {
            _announcementAppService = announcementAppService;
        }

        public void OnGet()
        {
            Announcement = new CreateUpdateAnnounementDto();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _announcementAppService.CreateAsync(Announcement);
            return NoContent();
        }
    }
}
