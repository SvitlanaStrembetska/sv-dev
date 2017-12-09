using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>,IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context)
            : base(context) { }

        public ApplicationUser GetByUserName(string userName)
        {
            return DbSet.FirstOrDefault(x => x.UserName == userName);
        }

        public bool CanAccessToSystem(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return false;

            userName = userName.ToLower();
            return DbSet.Any(x => x.UserName.ToLower() == userName);
        }
    }
}
