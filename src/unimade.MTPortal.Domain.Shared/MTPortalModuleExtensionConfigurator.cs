using System.ComponentModel.DataAnnotations;
using unimade.MTPortal.Users;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace unimade.MTPortal;

public static class MTPortalModuleExtensionConfigurator
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            ConfigureExistingProperties();
            ConfigureExtraProperties();
        });
    }

    private static void ConfigureExistingProperties()
    {
        /* You can change max lengths for properties of the
         * entities defined in the modules used by your application.
         *
         * Example: Change user and role name max lengths

           AbpUserConsts.MaxNameLength = 99;
           IdentityRoleConsts.MaxNameLength = 99;

         * Notice: It is not suggested to change property lengths
         * unless you really need it. Go with the standard values wherever possible.
         *
         * If you are using EF Core, you will need to run the add-migration command after your changes.
         */
    }

    private static void ConfigureExtraProperties()
    {
        ObjectExtensionManager.Instance.Modules()
            .ConfigureIdentity(identity =>
            {
                identity.ConfigureUser(user =>
                {
                    user.AddOrUpdateProperty<UserType>(
                        "UserType",
                        property =>
                        {
                            property.DefaultValue = UserType.Public;
                        }
                    );
                });
            })
            .ConfigureTenantManagement(tenant =>
            {
                tenant.ConfigureTenant(tenantEntity =>
                {
                    tenantEntity.AddOrUpdateProperty<string>(
                        "Country",
                        property =>
                        {
                            property.Attributes.Add(new MaxLengthAttribute(64));
                            property.DisplayName = new FixedLocalizableString("Country");
                        }
                    );
                    tenantEntity.AddOrUpdateProperty<string>(
                        "ContactEmail",
                        property =>
                        {
                            property.Attributes.Add(new MaxLengthAttribute(256));
                            property.Attributes.Add(new EmailAddressAttribute());
                            property.Attributes.Add(new DataTypeAttribute(DataType.EmailAddress));
                            property.DisplayName = new FixedLocalizableString("Contact Email");
                        }
                    );
                });
            });
    }
}
