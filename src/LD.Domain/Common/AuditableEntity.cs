using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }

        public string? CreatedByUserId { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public string? LastModifiedByUserId { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string? DeletedByUserId { get; set; }

        // Soft delete
        public bool IsActive { get; set; } = true;
    }

}
