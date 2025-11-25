using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;

namespace unimade.MTPortal.Users
{
    public class AppUser : IdentityUser
    {
        public UserType UserType { get; set; }

        protected AppUser() { } // For ORM

        public AppUser(
            Guid id,
            string userName,
            string email,
            Guid? tenantId = null)
            : base(
                  id,
                  userName,
                  email,
                  tenantId)
        {
            UserType = UserType.Public;
        }

        public bool IsStaffUser() => UserType == UserType.Staff;

        public bool IsPublicUser() => UserType == UserType.Public;
    }
}
