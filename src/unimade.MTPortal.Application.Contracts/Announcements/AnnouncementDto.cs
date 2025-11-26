using System;
using Volo.Abp.Application.Dtos;

namespace unimade.MTPortal.Announcements
{
    public class AnnouncementDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishDate { get; set; }
    }
}