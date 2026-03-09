using LD.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Domain.Entities;
public class StorageType:AuditableEntity
{
    public int StorageTypeId { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;

}