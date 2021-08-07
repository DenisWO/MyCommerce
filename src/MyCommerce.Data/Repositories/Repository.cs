using Microsoft.EntityFrameworkCore;
using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Models;
using MyCommerce.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyCommerce.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly MyCommerceDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public Repository(MyCommerceDbContext db)
        {
            dbContext = db;
            dbSet = dbContext.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<Entity> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task Add(TEntity entity)
        {
            dbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Remove(Guid id)
        {
            dbSet.Remove(new TEntity { Id = id});
            await SaveChanges();
        }


        public virtual async Task Update(TEntity entity)
        {
            dbSet.Update(entity);
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await dbContext.SaveChangesAsync();
        }


        public virtual void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}
