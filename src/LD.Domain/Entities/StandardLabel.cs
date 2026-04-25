using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class StandardLabel : AuditableEntity
    {
        [Key]
        public int StandarId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? StandarIdStr { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string? PartNumber { get; set; } = string.Empty;

        public int clientId { get; set; }
        public int projectId { get; set; }

        public Project? Project { get; set; }
        public Client? Client { get; set; }





    }
}
