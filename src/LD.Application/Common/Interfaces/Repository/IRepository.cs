using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>?> GetManyAsync();
        Task<bool> CreateAsync(T newModoe);
        Task<bool> UpdateAsync(T modelToUpdate);
        //Task<bool> Exists(int id);

    }
}
