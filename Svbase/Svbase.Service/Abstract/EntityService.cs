using System;
using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data.Abstract;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;

namespace Svbase.Service.Abstract
{
    public class EntityService<TRepo, TEntity> :IEntityService<TEntity>
        where TEntity : class, IEntity
        where TRepo : IGenericRepository<TEntity>
    {
        private readonly TRepo _repository;
        protected IUnitOfWork UnitOfWork { get; }
        protected IRepositoryManager RepositoryManager { get; }
        protected EntityService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager, TRepo repository)
        {
            UnitOfWork = unitOfWork;
            _repository = repository;
            RepositoryManager = repositoryManager;
        }

        public virtual TEntity Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            TEntity addedEntity = _repository.Add(entity);
            UnitOfWork.Commit();
            return addedEntity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            var result = entities.Select(entity => _repository.Add(entity)).ToList();
            UnitOfWork.Commit();
            return result;
        }

        IQueryable<TEntity> IEntityService<TEntity>.GetAll()
        {
            return _repository.GetAll();
        }

        public virtual TEntity FindById(object id)
        {
            return _repository.Find(id);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _repository.Update(entity);
            UnitOfWork.Commit();
        }

        public virtual TEntity Delete(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            TEntity deletedEntity = _repository.Delete(entity);
            UnitOfWork.Commit();
            return deletedEntity;
        }

        public virtual TEntity DeleteById(object id)
        {
            TEntity deletedEntity = _repository.DeleteById(id);
            UnitOfWork.Commit();
            return deletedEntity;
        }

        public TEntity Attach(TEntity entity)
        {
            return _repository.Attach(entity);
        }
    }
}
