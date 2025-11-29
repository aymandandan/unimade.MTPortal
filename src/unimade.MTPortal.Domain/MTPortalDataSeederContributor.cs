using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using unimade.MTPortal.Roles;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace unimade.MTPortal
{
    public class MTPortalDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IdentityRoleManager _identityRoleManager;
        private readonly ICurrentTenant _currenttenant;
        private readonly IPermissionDataSeeder _permissionDataSeeder;

        public MTPortalDataSeederContributor(
            IGuidGenerator guidGenerator,
            IdentityRoleManager identityRoleManager,
            ICurrentTenant currenttenant,
            IPermissionDataSeeder permissionDataSeeder)
        {
            _guidGenerator = guidGenerator;
            _identityRoleManager = identityRoleManager;
            _currenttenant = currenttenant;
            _permissionDataSeeder = permissionDataSeeder;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currenttenant.Change(context?.TenantId))
            {
                await CreateRoleWithPermissionsAsync(
                    roleName: StaffRole.Name,
                    permissions: new List<string>
                    {
                        StaffRole.Permissions.AnnouncementManagement,
                        StaffRole.Permissions.PublicUsersManagement,
                        StaffRole.Permissions.ViewAnnouncements
                    },
                    tenantId: context?.TenantId);

                await CreateRoleWithPermissionsAsync(
                    roleName: PublicRole.Name,
                    permissions: new List<string>
                    {
                        PublicRole.Permissions.ViewAnnouncements
                    },
                    tenantId: context?.TenantId,
                    true);
            }
        }

        private async Task CreateRoleWithPermissionsAsync(
            string roleName,
            List<string> permissions,
            Guid? tenantId,
            bool isDefault = false)
        {
            // Check if role exists
            var existingRole = await _identityRoleManager.FindByNameAsync(roleName);
            if (existingRole != null)
            {
                // Update permissions for existing role
                await _permissionDataSeeder.SeedAsync(
                    RolePermissionValueProvider.ProviderName,
                    roleName,
                    permissions,
                    tenantId
                );
                return;
            }

            // Create new role
            var newRole = new IdentityRole(
                _guidGenerator.Create(),
                roleName,
                tenantId
            )
            {
                IsPublic = true,
                IsDefault = isDefault
            };

            var result = await _identityRoleManager.CreateAsync(newRole);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create role '{roleName}': {string.Join(", ", result.Errors)}");
            }

            // Assign permissions to the new role
            await _permissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                roleName,
                permissions,
                tenantId
            );
        }

    }
}
