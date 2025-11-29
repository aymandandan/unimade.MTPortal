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
            // Check if admin role is being assigned
            var hasAdminRole = await HasAdminRoleInInputAsync(input);

            // If admin role is assigned, ensure UserType is set to Staff
            if (hasAdminRole)
            {
                input.SetProperty("UserType", UserType.Staff);
            }

            var userDto = await base.CreateAsync(input);
            var user = await UserRepository.GetAsync(userDto.Id);

            // Assign roles based on UserType and admin role assignment
            await AssignRoleBasedOnUserTypeAndAdminAsync(user, input, hasAdminRole);

            return userDto;
        }

        public override async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
        {
            // Get user before update to check current state
            var existingUser = await UserRepository.GetAsync(id);
            var previousUserType = existingUser.GetProperty<UserType>("UserType");
            var wasAdmin = await UserManager.IsInRoleAsync(existingUser, "admin");

            // Check if admin role is being assigned in this update
            var willBeAdmin = await HasAdminRoleInInputAsync(input);

            // If admin role is being assigned, ensure UserType is set to Staff
            if (willBeAdmin)
            {
                input.SetProperty("UserType", UserType.Staff);
            }

            // Let base implementation update the user
            var userDto = await base.UpdateAsync(id, input);

            // Get updated user entity
            var updatedUser = await UserRepository.GetAsync(id);
            var currentUserType = updatedUser.GetProperty<UserType>("UserType");

            // If UserType changed OR admin role assignment changed, reassign roles
            if (previousUserType != currentUserType || wasAdmin != willBeAdmin)
            {
                await ReassignRolesBasedOnUserTypeAndAdminAsync(updatedUser, previousUserType, currentUserType, wasAdmin, willBeAdmin);
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

        private async Task AssignRoleBasedOnUserTypeAndAdminAsync(IdentityUser user, IdentityUserCreateDto input, bool hasAdminRole)
        {
            var userType = input.GetProperty<UserType>("UserType");
            await SetUserRolesAsync(user, userType, hasAdminRole);
        }

        private async Task ReassignRolesBasedOnUserTypeAndAdminAsync(IdentityUser user, UserType previousUserType, UserType currentUserType, bool wasAdmin, bool willBeAdmin)
        {
            // Remove old roles based on previous user type and admin status
            await RemoveRolesByUserTypeAsync(user, previousUserType, wasAdmin);

            // Add new roles based on current user type and admin status
            await SetUserRolesAsync(user, currentUserType, willBeAdmin);
        }

        private async Task SetUserRolesAsync(IdentityUser user, UserType userType, bool hasAdminRole)
        {
            var roleNames = GetRoleNamesForUserType(userType, hasAdminRole);

            foreach (var roleName in roleNames)
            {
                var role = await RoleRepository.FindByNormalizedNameAsync(roleName.ToUpperInvariant());

                if (role != null && !await UserManager.IsInRoleAsync(user, roleName))
                {
                    await UserManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        private async Task RemoveRolesByUserTypeAsync(IdentityUser user, UserType userType, bool wasAdmin)
        {
            var rolesToRemove = GetRoleNamesForUserType(userType, wasAdmin);

            foreach (var roleName in rolesToRemove)
            {
                if (await UserManager.IsInRoleAsync(user, roleName))
                {
                    await UserManager.RemoveFromRoleAsync(user, roleName);
                }
            }
        }

        private static string[] GetRoleNamesForUserType(UserType userType, bool hasAdminRole)
        {
            // If user has admin role, assign both Public and Staff roles
            if (hasAdminRole)
            {
                return new[] { PublicRole.Name, StaffRole.Name, "admin" };
            }

            return userType switch
            {
                UserType.Staff => new[] { StaffRole.Name },
                UserType.Public => new[] { PublicRole.Name },
                _ => new[] { PublicRole.Name } // Default fallback
            };
        }

        private async Task<bool> HasAdminRoleInInputAsync(IdentityUserCreateDto input)
        {
            // Check if admin role is in the input role names
            var roleNames = input.RoleNames ?? Array.Empty<string>();
            return roleNames.Contains("admin", StringComparer.OrdinalIgnoreCase);
        }

        private async Task<bool> HasAdminRoleInInputAsync(IdentityUserUpdateDto input)
        {
            // Check if admin role is in the input role names
            var roleNames = input.RoleNames ?? Array.Empty<string>();
            return roleNames.Contains("admin", StringComparer.OrdinalIgnoreCase);
        }
    }
}