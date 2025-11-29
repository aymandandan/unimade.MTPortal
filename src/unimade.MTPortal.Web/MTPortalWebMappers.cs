using Riok.Mapperly.Abstractions;
using unimade.MTPortal.Announcements;
using unimade.MTPortal.Users;
using Volo.Abp.Identity;
using Volo.Abp.Mapperly;

namespace unimade.MTPortal.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MTPortalAnnouncementCreateUpdateMappers : MapperBase<AnnouncementDto, CreateUpdateAnnouncementDto>
{
    public override partial CreateUpdateAnnouncementDto Map(AnnouncementDto source);
    public override partial void Map(AnnouncementDto source, CreateUpdateAnnouncementDto destination);
}


[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MTPortalIdentityUserUpdateMappers : MapperBase<IdentityUserDto, IdentityUserUpdateDto>
{
    public override partial IdentityUserUpdateDto Map(IdentityUserDto source);
    public override partial void Map(IdentityUserDto source, IdentityUserUpdateDto destination);
}