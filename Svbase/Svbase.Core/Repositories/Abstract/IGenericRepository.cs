using System;
using Svbase.Core.Data.Abstract;
using System.Linq;

namespace Svbase.Core.Repositories.Abstract
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : IEntity
    {
        IQueryable<IEntity> GetAll();
        TEntity Find(object id);
        TEntity Add(TEntity entity);
        void Update(TEntity entity);
        TEntity Delete(TEntity entity);
        TEntity DeleteById(object id);
    }
}
