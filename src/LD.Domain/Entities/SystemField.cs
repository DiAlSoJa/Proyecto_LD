using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class SystemField : AuditableEntity
    {
        public int SystemFieldId { get; set; }
        public string SystemFieldName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;

        // Orden en UI
        public int Order { get; set; }

    }
}
