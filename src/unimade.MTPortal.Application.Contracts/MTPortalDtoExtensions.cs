using unimade.MTPortal.Users;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace unimade.MTPortal;

public static class MTPortalDtoExtensions
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            ObjectExtensionManager.Instance
                .AddOrUpdateProperty<IdentityUserCreateDto, UserType>(
                    "UserType",
                    options =>
                    {
                        options.DefaultValue = UserType.Public;
                    }
                )
                .AddOrUpdateProperty<IdentityUserUpdateDto, UserType>(
                    "UserType",
                    options =>
                    {
                        options.DefaultValue = UserType.Public;
                    }
                )
                .AddOrUpdateProperty<IdentityUserDto, UserType>(
                    "UserType",
                    options =>
                    {
                        options.DefaultValue = UserType.Public;
                    }
                );
        });
    }
}
