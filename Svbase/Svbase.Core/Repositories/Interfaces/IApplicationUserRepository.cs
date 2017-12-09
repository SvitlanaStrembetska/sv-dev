using Svbase.Core.Data.Entities;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        ApplicationUser GetByUserName(string userName);
        bool CanAccessToSystem(string userName);
    }
}
