using Riok.Mapperly.Abstractions;
using System.Collections.Generic;
using unimade.MTPortal.Accouncements;
using unimade.MTPortal.Announcements;
using Volo.Abp.Identity;
using Volo.Abp.Mapperly;

namespace unimade.MTPortal;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MTPortalAnnouncementListMappers : MapperBase<List<Announcement>, List<AnnouncementDto>>
{
    public override partial List<AnnouncementDto> Map(List<Announcement> source);
    public override partial void Map(List<Announcement> source, List<AnnouncementDto> destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MTPortalAnnouncementMappers : MapperBase<Announcement, AnnouncementDto>
{
    public override partial AnnouncementDto Map(Announcement source);
    public override partial void Map(Announcement source, AnnouncementDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MTPortalIdentityUserListMappers: MapperBase<List<IdentityUser>, List<IdentityUserDto>>
{
    public override partial List<IdentityUserDto> Map(List<IdentityUser> source);
    public override partial void Map(List<IdentityUser> source, List<IdentityUserDto> destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MTPortalIdentityUserMappers : MapperBase<IdentityUser, IdentityUserDto>
{
    public override partial IdentityUserDto Map(IdentityUser source);
    public override partial void Map(IdentityUser source, IdentityUserDto destination);
}