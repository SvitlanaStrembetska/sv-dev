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
    }
}
