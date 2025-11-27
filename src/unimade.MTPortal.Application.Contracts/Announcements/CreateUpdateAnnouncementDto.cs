using System;
using System.ComponentModel.DataAnnotations;

namespace unimade.MTPortal.Announcements
{
    public class CreateUpdateAnnouncementDto
    {
        [Required]
        [StringLength(AnnouncementConsts.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsPublished { get; set; } = false;
        public DateTime? PublishDate { get; set; }
    }
}
