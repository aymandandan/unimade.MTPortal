using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using unimade.MTPortal.Roles;
using unimade.MTPortal.Users;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using static Volo.Abp.Identity.Web.Pages.Identity.Users.EditModalModel;

namespace unimade.MTPortal.Web.Pages.Internal.Users
{
    [Authorize(Roles = StaffRole.Name)]
    public class EditModalModel : MTPortalPageModel
    {
        [BindProperty(SupportsGet = true)]
        [HiddenInput]
        public Guid Id { get; set; }

        [BindProperty]
        public IdentityUserUpdateDto UserInfo { get; set; }

        public bool IsEditCurrentUser { get; set; }

        public DetailViewModel Detail { get; set; }

        private readonly IIdentityUserAppService _userAppService;

        public EditModalModel(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task OnGetAsync()
        {
            var user = await _userAppService.GetAsync(Id);
            UserInfo = ObjectMapper.Map<IdentityUserDto, IdentityUserUpdateDto>(user);
            UserInfo.SetProperty("UserType", UserType.Public);

            IsEditCurrentUser = CurrentUser.Id == Id;

            Detail = ObjectMapper.Map<IdentityUserDto, DetailViewModel>(user);

            Detail.CreatedBy = await GetUserNameOrNullAsync(user.CreatorId);
            Detail.ModifiedBy = await GetUserNameOrNullAsync(user.LastModifierId);
        }

        private async Task<string> GetUserNameOrNullAsync(Guid? userId)
        {
            if (!userId.HasValue)
            {
                return null;
            }

            var user = await _userAppService.GetAsync(userId.Value);
            return user.UserName;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UserInfo.SetProperty("UserType", UserType.Public);
            await _userAppService.UpdateAsync(Id, UserInfo);
            return NoContent();
        }
    }
}
