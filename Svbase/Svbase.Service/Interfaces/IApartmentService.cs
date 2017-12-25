using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IApartmentService : IEntityService<Apartment>
    {
        ApartmentViewModel GetById(int id);
    }
}
