using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IApartmentRepository : IGenericRepository<Apartment>
    {
        ApartmentViewModel GetById(int id);
        IEnumerable<BaseViewModel> GetFlatBaseModelByApartmentIds(IList<int> apartmentIds);
        IEnumerable<BaseViewModel> GetFlatsBaseModelByApartmentId(int id);
    }
}
