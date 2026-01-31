using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> GetById(int id);
        Task<List<T>?> GetMany();
        Task<bool> Create(T newModoe);
        Task<bool> Update(T modelToUpdate);
        Task<bool> Archive(int id);
    }
}
