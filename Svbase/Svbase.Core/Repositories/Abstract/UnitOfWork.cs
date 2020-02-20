using Svbase.Core.Data;
using System;

namespace Svbase.Core.Repositories.Abstract
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _dbContext;

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="context">The object context</param>
        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        #endregion

        #region Destructor

        /// <summary>
        ///     Destructor
        /// </summary>
        ~UnitOfWork()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }
        }

        #endregion

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
        #endregion
    }

}
