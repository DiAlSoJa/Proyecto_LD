using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities
{
    public class ScanSaveType : AuditableEntity
    {
        public int ScanSaveTypeId { get; set; }
        public string ScanSaveTypeName { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string? Description { get; set; }



    }

}
