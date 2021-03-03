using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentServer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Create(T model);
        Task<bool> Delete(T model);
        Task<T> GetById(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Update(T model);

    }
}
