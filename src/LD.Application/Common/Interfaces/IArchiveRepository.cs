using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces
{
    public interface IArchiveRepository<T>
    {
        Task<bool> ArchiveAsync(int id,bool isActive);
    }
}
