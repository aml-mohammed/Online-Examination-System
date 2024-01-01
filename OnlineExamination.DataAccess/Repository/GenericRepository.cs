using Microsoft.EntityFrameworkCore;
using OnlineExamination.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExamination.DataAccess.Repository
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        internal DbSet<T> dbset;
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
           
            _context = context;
            this.dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
           dbset.Add(entity);
            return entity;
        }

        public void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbset.Attach(entityToDelete);
            }
            dbset.Remove(entityToDelete);
        }

        public async Task<T> DeleteAsync(T entityToDelete)
        {
           if(_context.Entry(entityToDelete).State==EntityState.Detached)
            {
                dbset.Attach(entityToDelete);
            }
           
                dbset.Remove(entityToDelete);
                return entityToDelete;

        }

        public void DeleteById(object id)
        {
            T entityToDelete = dbset.Find(id);
            Delete(entityToDelete);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
           if(!this.disposed)
            {
                if(disposing)
                {
                   _context.Dispose();
                }
               
            }
            this.disposed = true;
         }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeproperty in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeproperty);
            }
            if(OrderBy != null)
            {
                return OrderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public T GetById(object id)
        {
            return dbset.Find(id);
        } 

        public async Task<T> GetByIdAsync(object id)
        {
            return await dbset.FindAsync(id);
        }

        public void Update(T entityToUpdate)
        {
            dbset.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task<T> UpdateAsync(T entityToUpdate)
        {
            dbset.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }
    }
}
