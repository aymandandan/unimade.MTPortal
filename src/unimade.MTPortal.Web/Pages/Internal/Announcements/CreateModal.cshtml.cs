using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using unimade.MTPortal.Announcements;
using unimade.MTPortal.Roles;

namespace unimade.MTPortal.Web.Pages.Internal.Announcements
{
    [Authorize(Roles = StaffRole.Name)]
    public class CreateModalModel : MTPortalPageModel
    {
        [BindProperty]
        public CreateUpdateAnnouncementDto Announcement { get; set; }

        private readonly IAnnouncementAppService _announcementAppService;

        public CreateModalModel(IAnnouncementAppService announcementAppService)
        {
            _announcementAppService = announcementAppService;
        }

        public void OnGet()
        {
            Announcement = new CreateUpdateAnnouncementDto();
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
