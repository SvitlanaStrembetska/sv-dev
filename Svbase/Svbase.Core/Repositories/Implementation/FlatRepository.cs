using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

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
                    FirthName = p.FirstName,
                    LastName = p.LastName,
                    MiddleName = p.Patronymic
                })
            }).FirstOrDefault(x => x.Id == id);

            return flat;
        }
    }
}
