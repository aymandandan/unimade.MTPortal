using System;
using Volo.Abp.Application.Dtos;

namespace unimade.MTPortal.Announcements
{
    public class AnnouncementGetListInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; } // for general search 
        public bool? IsPublished { get; set; } // for filtering by published status
        public Guid? ExcludeId { get; set; }

    }
}
