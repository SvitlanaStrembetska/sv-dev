using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IFlatRepository : IGenericRepository<Flat>
    {
        FlatViewModel GetById(int id);
        IEnumerable<int> GetPersonIdsByFlatIds(List<int> flatIds);
    }
}
