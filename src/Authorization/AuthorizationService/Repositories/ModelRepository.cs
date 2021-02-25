using AuthorizationService.Data;
using AuthorizationService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Repositories
{
    public class ModelRepository<T> : IModelRepository<T> where T : class
    {
        protected ModelContext Context;
        public ModelRepository(ModelContext context)
        {
            Context = context;
        }
        public async virtual Task<bool> Create(T model)
        {
            Context.Set<T>().Add(model);
            return await Context.SaveChangesAsync() > 0;
        }

        public async virtual Task<bool> Delete(T model)
        {
            Context.Set<T>().Remove(model);
            return await Context.SaveChangesAsync() > 0;
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async virtual Task<T> GetById(Guid id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async virtual Task<bool> Update(T model)
        {
            Context.Set<T>().Update(model);
            return await Context.SaveChangesAsync() > 0;
        }
    }
}
