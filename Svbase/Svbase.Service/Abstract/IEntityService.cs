using System.Linq;
using Svbase.Core.Data.Abstract;

namespace Svbase.Service.Abstract
{
    public interface IEntityService<TEntity> where TEntity : IEntity
    {
        TEntity Add(TEntity entity);
        IQueryable<TEntity> GetAll();
        TEntity FindById(object id);
        void Update(TEntity entity);
        TEntity Delete(TEntity entity);
        TEntity DeleteById(object id);
    }
}
