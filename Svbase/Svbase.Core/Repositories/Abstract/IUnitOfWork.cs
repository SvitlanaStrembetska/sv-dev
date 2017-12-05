using System;

namespace Svbase.Core.Repositories.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
    }
}
