using Svbase.Core.Data.Entities;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class ApplicationUserService : EntityService<IApplicationUserRepository, ApplicationUser>, IApplicationUserService
    {
        public ApplicationUserService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            :base(unitOfWork,repositoryManager,repositoryManager.ApplicationUsers)
        {
            
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return RepositoryManager.ApplicationUsers.GetByUserName(userName);
        }

        public bool CanAccessToSystem(string userName)
        {
            return RepositoryManager.ApplicationUsers.CanAccessToSystem(userName);
        }
    }
}
