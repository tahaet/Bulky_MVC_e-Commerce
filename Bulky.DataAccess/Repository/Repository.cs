using Bulky.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> _dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }
        public void Add(T entity)
        {
           _dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter,string? includeProperties=null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = _dbSet;   
            }
            else
            {
                query = _dbSet.AsNoTracking();
            }
            if (!string.IsNullOrEmpty(includeProperties))
			{
				foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(property);
				}
			}
			return query.FirstOrDefault(filter);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter is not null) query = query.Where(filter);  
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var property in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
           _dbSet.RemoveRange(entities);
        }
    }

}
