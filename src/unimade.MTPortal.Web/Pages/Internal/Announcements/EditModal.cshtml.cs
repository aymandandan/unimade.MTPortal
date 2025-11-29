using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using unimade.MTPortal.Announcements;
using unimade.MTPortal.Roles;

namespace unimade.MTPortal.Web.Pages.Internal.Announcements
{
    [Authorize(Roles = StaffRole.Name)]
    public class EditModalModel : MTPortalPageModel
    {
        [BindProperty(SupportsGet = true)]
        [HiddenInput]
        public Guid Id { get; set; }
        
        [BindProperty]
        public CreateUpdateAnnouncementDto Announcement { get; set; }

        private readonly IAnnouncementAppService _announcementAppService;

        public EditModalModel(IAnnouncementAppService announcementAppService)
        {
            _announcementAppService = announcementAppService;
        }

        public async Task OnGetAsync()
        {
            var announcementDto = await _announcementAppService.GetAsync(Id);
            Announcement = ObjectMapper.Map<AnnouncementDto, CreateUpdateAnnouncementDto>(announcementDto);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _announcementAppService.UpdateAsync(Id, Announcement);
            return NoContent();
        }
    }
}
