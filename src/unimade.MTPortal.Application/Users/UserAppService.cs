using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using unimade.MTPortal.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace unimade.MTPortal.Users
{
    public class UserAppService : MTPortalAppService, IUserAppService
    {
        private readonly IRepository<IdentityUser, Guid> _identityUserRepository;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IAuthorizationService _authorizationService;

        public UserAppService(
            IRepository<IdentityUser, Guid> identityUserRepository,
            IdentityUserManager identityUserManager,
            IAuthorizationService authorizationService)
        {
            _identityUserRepository = identityUserRepository;
            _identityUserManager = identityUserManager;
            _authorizationService = authorizationService;
        }

        public async Task<PagedResultDto<IdentityUserDto>> GetListAsync(UserGetListInput input)
        {
            var queryable = await _identityUserRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(user =>
                    user.UserName.Contains(input.Filter) ||
                    user.Name.Contains(input.Filter) ||
                    user.Surname.Contains(input.Filter) ||
                    user.Email.Contains(input.Filter));
            }

            var users = await AsyncExecuter.ToListAsync(
                queryable
                .OrderBy(x => input.Sorting ?? "CreationTime DESC")
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
            );

            var totalCount = await AsyncExecuter.CountAsync(queryable);
            return new PagedResultDto<IdentityUserDto>(
                totalCount,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(users)
            );
        }

        public async Task<IdentityUserDto> CreateUserAsync(IdentityUserCreateDto input)
        {
            var canUpdateUserType = await _authorizationService
                .IsGrantedAsync(MTPortalPermissions.User.UserType.Update);

            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                input.Email,
                CurrentTenant.Id
            )
            {
                Name = input.Name,
                Surname = input.Surname
            };

            if (!canUpdateUserType)
            {
                user.SetProperty("UserType", UserType.Public);
            }

            await _identityUserManager.CreateAsync(user, input.Password);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _identityUserRepository.GetAsync(id);
            await _identityUserManager.DeleteAsync(user);
        }

        public async Task<IdentityUserDto> GetAsync(Guid id)
        {
            var user = await _identityUserRepository.GetAsync(id);
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task<IdentityUserDto> UpdateUserAsync(Guid id, IdentityUserUpdateDto input)
        {
            var user = await _identityUserRepository.GetAsync(id);

            // Check if user has permission to update UserType
            var canUpdateUserType = await _authorizationService
                .IsGrantedAsync(MTPortalPermissions.User.UserType.Update);

            if (!canUpdateUserType)
            {
                // Preserve current UserType
                var currentUserType = user.GetProperty<string>("UserType");
                input.SetProperty("UserType", currentUserType);
            }

            // Update user properties
            user.Name = input.Name;
            user.Surname = input.Surname;
            user.SetPhoneNumber(input.PhoneNumber, false);

            // Handle extra properties
            if (input.ExtraProperties != null)
            {
                foreach (var property in input.ExtraProperties)
                {
                    user.SetProperty(property.Key, property.Value);
                }
            }

            await _identityUserManager.UpdateAsync(user);

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }
    }
}