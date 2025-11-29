using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using unimade.MTPortal.Roles;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace unimade.MTPortal.Web.Pages.Account
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ExLoginModel), typeof(LoginModel))]
    public class ExLoginModel : LoginModel
    {
        public ExLoginModel(
            IAuthenticationSchemeProvider schemeProvider, 
            IOptions<AbpAccountOptions> accountOptions,
            IOptions<IdentityOptions> identityOptions,
            IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache,
            IWebHostEnvironment webHostEnvironment) 
            : base(schemeProvider,
                   accountOptions,
                   identityOptions,
                   identityDynamicClaimsPrincipalContributorCache,
                   webHostEnvironment)
        {
        }

        public override async Task<IActionResult> OnPostAsync(string action)
        {
            await base.OnPostAsync(action);
            return RedirectBasedOnRole();
        }

        public override async Task<IActionResult> OnPostExternalLogin(string provider)
        {
            await base.OnPostExternalLogin(provider);
            return RedirectBasedOnRole();
        }

        private IActionResult RedirectBasedOnRole()
        {
            if (!CurrentUser.IsAuthenticated || !CurrentTenant.IsAvailable || CurrentUser.IsInRole("admin"))
            {
                return RedirectToPage("/Index");
            }

            if (CurrentUser.IsInRole(StaffRole.Name))
                return RedirectToPage("/Internal/Dashboard/Index");

            if (CurrentUser.IsInRole(PublicRole.Name))
                return RedirectToPage("/External/Index");

            return RedirectToPage("/Account/AccessDenied");
        }
    }
}
