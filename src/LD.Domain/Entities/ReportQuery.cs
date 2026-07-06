using LD.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LD.Domain.Entities;

[Table("ReportQueries")]
public class ReportQuery : AuditableEntity
{
    [Key]
    public int ReportQueryId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string SqlQuery { get; set; } = string.Empty;
}
