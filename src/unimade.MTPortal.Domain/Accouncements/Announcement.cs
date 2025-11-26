using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace unimade.MTPortal.Accouncements
{
    public class Announcement : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }

        public string Title { get; private set; }

        public string Content { get; private set; }

        public bool IsPublished { get; private set; }

        public DateTime? PublishDate { get; private set; }

        protected Announcement() { } // For ORM

        public Announcement(Guid id, string title, string content, Guid? tenantId = null)
            : base(id)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Content = Check.NotNullOrWhiteSpace(content, nameof(content));
            TenantId = tenantId;
            IsPublished = false;
        }

        public void UpdateDetails(string title, string content)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Content = Check.NotNullOrWhiteSpace(content, nameof(content));
        }

        public void Publish()
        {
            IsPublished = true;
            PublishDate = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            IsPublished = false;
            PublishDate = null;
        }

        public void SchedulePublication(DateTime publishDate)
        {
            if (publishDate <= DateTime.UtcNow)
            {
                throw new ArgumentException("Publish date must be in the future.", nameof(publishDate));
            }
            PublishDate = publishDate;
        }

    }
}
