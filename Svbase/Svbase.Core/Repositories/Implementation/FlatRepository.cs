using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class FlatRepository : GenericRepository<Flat>, IFlatRepository
    {
        public FlatRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
