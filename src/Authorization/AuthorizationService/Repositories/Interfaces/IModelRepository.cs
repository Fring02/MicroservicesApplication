using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories.Interfaces
{
    public interface IModelRepository<T>
    {
        Task<bool> Create(T model);
        Task<bool> Update(T model);
        Task<bool> Delete(T model);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);
    }
}
