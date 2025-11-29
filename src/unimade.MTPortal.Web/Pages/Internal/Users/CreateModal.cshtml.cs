using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using unimade.MTPortal.Roles;
using unimade.MTPortal.Users;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace unimade.MTPortal.Web.Pages.Internal.Users
{
    [Authorize(Roles = StaffRole.Name)]
    public class CreateModalModel : MTPortalPageModel
    {
        [BindProperty]
        public IdentityUserCreateDto UserInfo { get; set; }

        private readonly IIdentityUserAppService _userAppService;

        public CreateModalModel(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public void OnGet()
        {
            UserInfo = new IdentityUserCreateDto();
            UserInfo.SetProperty("UserType", UserType.Public);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UserInfo.SetProperty("UserType", UserType.Public);
            await _userAppService.CreateAsync(UserInfo);
            return NoContent();
        }
    }
}
