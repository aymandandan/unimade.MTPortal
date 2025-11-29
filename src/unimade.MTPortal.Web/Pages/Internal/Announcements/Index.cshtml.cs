using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using unimade.MTPortal.Roles;

namespace unimade.MTPortal.Web.Pages.Internal.Announcements
{
    [Authorize(Roles = StaffRole.Name)]
    public class IndexModel : MTPortalPageModel
    {
        public void OnGet()
        {
        }
    }
}
