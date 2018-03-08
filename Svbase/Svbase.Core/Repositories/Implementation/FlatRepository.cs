using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;
using System.Collections.Generic;

namespace Svbase.Core.Repositories.Implementation
{
    public class FlatRepository : GenericRepository<Flat>, IFlatRepository
    {
        public FlatRepository(ApplicationDbContext context)
            : base(context) { }

        public FlatViewModel GetById(int id)
        {
            var flat = DbSet.Select(x => new FlatViewModel
            {
                Id = x.Id,
                Name = x.Number,
                CanDelete = !x.Persons.Any(),
                ApartmentId = x.ApartmentId,
                Persons = x.Persons.Select(p => new PersonListModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    MiddleName = p.MiddleName
                })
            }).FirstOrDefault(x => x.Id == id);

            return flat;
        }

        public IEnumerable<int> GetPersonIdsByFlatIds(List<int> flatIds)
        {
            if (flatIds == null || !flatIds.Any())
                return new List<int>();

            var flats = DbSet
                .Where(x => flatIds.Contains(x.Id));

            if (!flats.Any())
                return new List<int>();

            var flatPersons = flats.Select(x => x.Persons);
            var persons = new List<Person>();
            foreach (var flatPerson in flatPersons)
            {
                persons.AddRange(flatPerson);
            }
            var personsIds = persons.Select(p => p.Id);
            return personsIds;
        }

        //public IEnumerable<ApartmentFilterModel> GetFilterFlatsByApartmentIds(IList<int> apartmentIds)
        //{
        //    if (apartmentIds == null || !apartmentIds.Any())
        //    {
        //        return new List<ApartmentFilterModel>();
        //    }

        //    var flatDb = DbSet
        //      .Where(x => apartmentIds.Contains(x.Id));

        //    var streets = apartmentsDb
        //        .Select(x => x.s)
        //}
    }
}
