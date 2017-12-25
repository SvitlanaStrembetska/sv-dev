using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IFlatRepository : IGenericRepository<Flat>
    {
        FlatViewModel GetById(int id);
    }
}
