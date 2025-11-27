using System;
using Volo.Abp.Application.Services;

namespace unimade.MTPortal.Announcements
{
    public interface IAnnouncementAppService :
        ICrudAppService<
            AnnouncementDto,
            Guid,
            AnnouncementGetListInput,
            CreateUpdateAnnouncementDto,
            CreateUpdateAnnouncementDto>
    {

    }
}
