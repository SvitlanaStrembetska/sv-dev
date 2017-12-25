
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IStreetService : IEntityService<Street>
    {
        StreetViewModel GetStreetById(int id);
    }
}
