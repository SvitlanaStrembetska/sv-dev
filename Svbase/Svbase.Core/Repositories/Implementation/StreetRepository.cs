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

        public IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetIds(IList<int> streetIds)
        {
            if (streetIds == null) return new List<BaseViewModel>();
            var streets = DbSet.Where(x => streetIds.Contains(x.Id));
            if (!streets.Any()) return new List<BaseViewModel>();

            var apartmentsLists = streets
                .Select(x => x.Apartments
                    .Select(s => new BaseViewModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList()
                ).ToList();

            var apartments = new List<BaseViewModel>();
            apartments = apartmentsLists
                .Aggregate(apartments, (current, items) => current.Union(items).ToList());//Union arrays
            apartments = apartments.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();//Distinct by field
            return apartments;
        }

        public IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetId(int id)
        {
            var street = DbSet.FirstOrDefault(x => x.Id == id);
            if (street == null) return new List<BaseViewModel>();
            var apartments = street.Apartments.Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
            return apartments;
        }
    }
}
