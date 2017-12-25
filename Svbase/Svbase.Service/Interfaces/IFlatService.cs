using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IFlatService : IEntityService<Flat>
    {
        FlatViewModel GetById(int id);
    }
}
