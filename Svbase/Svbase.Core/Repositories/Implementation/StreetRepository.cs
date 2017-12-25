using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class StreetRepository : GenericRepository<Street>, IStreetRepository
    {
        public StreetRepository(ApplicationDbContext context)
            : base(context) { }

        public StreetViewModel GetStreetById(int id)
        {
            var street = DbSet.Select(x => new StreetViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CanDelete = !x.Apartments.Any(),
                Apartments = x.Apartments.Select(a => new ApartmentCreateModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    CanDelete = !a.Flats.Any()
                })
            }).FirstOrDefault(x => x.Id == id);
            return street;
        }
    }
}
