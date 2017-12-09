using Svbase.Core.Data.Entities;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IApplicationUserService : IEntityService<ApplicationUser>
    {
        ApplicationUser GetByUserName(string userName);
        bool CanAccessToSystem(string userName);
    }
}
