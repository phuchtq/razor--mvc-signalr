using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Base
{
    public class GenericRepository<T> where T : class
    {
        protected Sp25PharmaceuticalDBContext _context;

        public GenericRepository()
        {
            _context ??= new Sp25PharmaceuticalDBContext();
        }

        public GenericRepository(Sp25PharmaceuticalDBContext context)
        {
            _context = context;
        }

        public List<T> GetAll() => _context.Set<T>().ToList();

        public async Task<List<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task CreateAsync(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }
    }
}
