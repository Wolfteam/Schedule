using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Schedule.API.Models.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(object id);
        IEnumerable<TEntity> GetAll();
        void Remove(object id);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entity);
        void Update(object id, TEntity entity);
        void Update(TEntity entity);
    }
}
