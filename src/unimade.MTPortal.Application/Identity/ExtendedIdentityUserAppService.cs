using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using unimade.MTPortal.Roles;
using unimade.MTPortal.Users;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace unimade.MTPortal.Identity
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(IIdentityUserAppService),
        typeof(IdentityUserAppService),
        typeof(ExtendedIdentityUserAppService)
        )]
    public class ExtendedIdentityUserAppService : IdentityUserAppService
    {
        public ExtendedIdentityUserAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IOptions<IdentityOptions> identityOptions,
            IPermissionChecker permissionChecker) 
            : base(userManager, userRepository, roleRepository, identityOptions, permissionChecker)
        {
        }

        public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var userDto = await base.CreateAsync(input);
            var user = await UserRepository.GetAsync(userDto.Id);

            // Assign role based on UserType
            await AssignRoleBasedOnUserTypeAsync(user, input);

            return userDto;
        }

        public override async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            // Get user before update to check if UserType changed
            var existingUser = await UserRepository.GetAsync(id);
            var previousUserType = existingUser.GetProperty<UserType>("UserType");

            // Let base implementation update the user
            var userDto = await base.UpdateAsync(id, input);

            // Get updated user entity
            var updatedUser = await UserRepository.GetAsync(id);
            var currentUserType = updatedUser.GetProperty<UserType>("UserType");

            // If UserType changed, reassign roles
            if (previousUserType != currentUserType)
            {
                await ReassignRolesBasedOnUserTypeAsync(updatedUser, previousUserType, currentUserType);
            }

            return userDto;
        }

        public async Task<PagedResultDto<IdentityUserDto>> GetPublicUsersListAsync(GetIdentityUsersInput input)
        {
            var fullList = await base.GetListAsync(input);

            var filteredList = fullList.Items.Where(u => u.GetProperty<UserType>("UserType") != UserType.Staff).ToList();

            // apply pagination manually
            var pagedList = filteredList
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<IdentityUserDto>(filteredList.Count, pagedList);
        }

        private async Task AssignRoleBasedOnUserTypeAsync(IdentityUser user, IdentityUserCreateDto input)
        {
            var userType = input.GetProperty<UserType>("UserType");
            await SetUserRolesAsync(user, userType);
        }

        private async Task ReassignRolesBasedOnUserTypeAsync(IdentityUser user, UserType previousUserType, UserType currentUserType)
        {
            // Remove old roles based on previous user type
            await RemoveRolesByUserTypeAsync(user, previousUserType);

            // Add new roles based on current user type
            await SetUserRolesAsync(user, currentUserType);
        }

        private async Task SetUserRolesAsync(IdentityUser user, UserType userType)
        {
            var roleNames = GetRoleNamesForUserType(userType);

            foreach (var roleName in roleNames)
            {
                var role = await RoleRepository.FindByNormalizedNameAsync(roleName.ToUpperInvariant());

                if (role != null && !await UserManager.IsInRoleAsync(user, roleName))
                {
                    await UserManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        private async Task RemoveRolesByUserTypeAsync(IdentityUser user, UserType userType)
        {
            var rolesToRemove = GetRoleNamesForUserType(userType);

            foreach (var roleName in rolesToRemove)
            {
                if (await UserManager.IsInRoleAsync(user, roleName))
                {
                    await UserManager.RemoveFromRoleAsync(user, roleName);
                }
            }
        }
        private static string[] GetRoleNamesForUserType(UserType userType)
        {
            return userType switch
            {
                UserType.Staff => new[] { StaffRole.Name },
                UserType.Public => new[] { PublicRole.Name },
                _ => new[] { PublicRole.Name } // Default fallback
            };
        }

    }
}
