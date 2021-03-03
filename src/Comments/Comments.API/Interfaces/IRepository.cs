using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<bool> Create(T model);
        Task<bool> Delete(T model);
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Update(T model);

    }
}
