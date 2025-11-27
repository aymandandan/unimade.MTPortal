using Riok.Mapperly.Abstractions;
using unimade.MTPortal.Announcements;
using Volo.Abp.Mapperly;

namespace unimade.MTPortal.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class MTPortalAnnouncementMappers : MapperBase<AnnouncementDto, CreateUpdateAnnouncementDto>
{
    public override partial CreateUpdateAnnouncementDto Map(AnnouncementDto source);
    public override partial void Map(AnnouncementDto source, CreateUpdateAnnouncementDto destination);
}
