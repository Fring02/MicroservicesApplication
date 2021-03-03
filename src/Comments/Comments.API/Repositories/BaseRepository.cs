using Comments.API.Data;
using Comments.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comments.API.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected CommentContext _context;
        public BaseRepository(CommentContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(T model)
        {
            _context.Set<T>().Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(T model)
        {
            _context.Set<T>().Remove(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> Update(T model)
        {
            _context.Set<T>().Update(model);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
