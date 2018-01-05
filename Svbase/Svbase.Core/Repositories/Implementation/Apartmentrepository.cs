using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class ApartmentRepository : GenericRepository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(ApplicationDbContext context)
            : base(context) { }

        public ApartmentViewModel GetById(int id)
        {
            var apartment = DbSet.Select(x => new ApartmentViewModel
            {
                Id = x.Id,
                Name = x.Name,
                StreetId = x.StreetId,
                Flats = x.Flats.Select(f => new FlatCreateModel
                {
                    Id = f.Id,
                    Name = f.Number,
                    ApartmentId = f.ApartmentId,
                    CanDelete = !f.Persons.Any()
                })
            }).FirstOrDefault(x => x.Id == id);
            return apartment;
        }

        public IEnumerable<BaseViewModel> GetFlatBaseModelByApartmentIds(IList<int> apartmentIds)
        {
            if (apartmentIds == null) return new List<BaseViewModel>();
            var apartments = DbSet.Where(x => apartmentIds.Contains(x.Id));
            if (!apartments.Any()) return new List<BaseViewModel>();

            var flatsLists = apartments
                .Select(x => x.Flats
                    .Select(s => new BaseViewModel
                    {
                        Id = s.Id,
                        Name = s.Number
                    }).ToList()
                ).ToList();

            var flats = new List<BaseViewModel>();
            flats = flatsLists
                .Aggregate(flats, (current, items) => current.Union(items).ToList());//Union arrays
            flats = flats.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();//Distinct by field
            return flats;
        }

        public IEnumerable<BaseViewModel> GetFlatsBaseModelByApartmentId(int id)
        {
            var apartment = DbSet.FirstOrDefault(x => x.Id == id);
            if (apartment == null) return new List<BaseViewModel>();
            var flats = apartment.Flats.Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Number
            });
            return flats;
        }
    }
}
