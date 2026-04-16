using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class ScanConfiguration : AuditableEntity
    {
        public int ScanConfigurationId { get; set; }
        public int ProjectId { get; set; }


        public int SystemFieldId { get; set; }
        public SystemField SystemField { get; set; } = null!;

        // Campo del cliente (texto libre o catálogo)
        public string ClientField { get; set; } = null!;

        public int ScanTypeId { get; set; }
        public ScanType ScanType { get; set; } = null!;
        public string? ScanValue { get; set; }

        public int SaveTypeId { get; set; }
        public ScanSaveType SaveType { get; set; } = null!;
        public int SaveValue { get; set; }

        public int Order { get; set; }
        public Project? Project { get; set; }



    }

}
