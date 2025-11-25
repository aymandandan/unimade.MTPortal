using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.TenantManagement;

namespace unimade.MTPortal.Tenants
{
    public class AppTenant : Tenant
    {
        public string Country { get; set; }
        public string ContactEmail { get; set; }
        public string DisplayName { get; set; }

        protected AppTenant() { } // For ORM

        public AppTenant(
            Guid id,
            string name,
            string country,
            string contactEmail,
            string displayName)
            : base(
                  id,
                  name,
                  null)
        {
            Country = country;
            ContactEmail = contactEmail;
            DisplayName = displayName;
        }

    }
}
