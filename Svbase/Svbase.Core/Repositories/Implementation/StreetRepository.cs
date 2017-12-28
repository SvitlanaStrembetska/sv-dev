using System.Collections.Generic;
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

        public IEnumerable<StreetSelectModel> GetStreetsForSelecting()
        {
            var streets = DbSet.Select(x => new StreetSelectModel
            {
                Id = x.Id,
                Name = x.Name,
                CityName = x.City.Name,
                IsChecked = false
            });

            return streets;
        }

        public IEnumerable<Street> GetStreetsByDistrictId(int id)
        {
            var streets = DbSet.Where(x => x.Districts.Select(d => d.Id).Contains(id));
            return streets;
        }
    }
}
