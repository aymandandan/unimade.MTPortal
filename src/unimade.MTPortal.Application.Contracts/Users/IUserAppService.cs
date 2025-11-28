using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace unimade.MTPortal.Users
{
    public interface IUserAppService : IApplicationService
    {
        public Task<PagedResultDto<IdentityUserDto>> GetListAsync(UserGetListInput input);
        public Task<IdentityUserDto> GetAsync(Guid id);
        public Task<IdentityUserDto> CreateUserAsync(IdentityUserCreateDto input);
        public Task<IdentityUserDto> UpdateUserAsync(Guid id, IdentityUserUpdateDto input);
        public Task DeleteUserAsync(Guid id);
    }
}
