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
            : base(context){}

        public ApartmentViewModel GetById(int id)
        {
            var apartment = DbSet.Select(x => new ApartmentViewModel
            {
                Id = x.Id,
                Name = x.Name,
                StreetId = x.StreetId,
                Flats = x.Flats.Select(f=>new FlatCreateModel
                {
                    Id = f.Id,
                    Name = f.Number,
                    ApartmentId = f.ApartmentId,
                    CanDelete = !f.Persons.Any()
                })
            }).FirstOrDefault(x => x.Id == id);
            return apartment;
        }
    }
}
