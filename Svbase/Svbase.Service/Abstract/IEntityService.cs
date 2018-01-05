using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data.Abstract;

namespace Svbase.Service.Abstract
{
    public interface IEntityService<TEntity> where TEntity : IEntity
    {
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> GetAll();
        TEntity FindById(object id);
        void Update(TEntity entity);
        TEntity Delete(TEntity entity);
        TEntity DeleteById(object id);
        TEntity Attach(TEntity entity);
    }
}
