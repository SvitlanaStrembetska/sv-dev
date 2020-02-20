using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        IEnumerable<WorkCreateModel> GetAllWorks();
        IEnumerable<CheckboxItemModel> GetWorksForSelecting();
    }
}
