using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace unimade.MTPortal.Users
{
    public class UserGetListInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; } // for general search
    }
}
