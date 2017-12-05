using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class StreetRepository : GenericRepository<Street>, IStreetRepository
    {
        public StreetRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
