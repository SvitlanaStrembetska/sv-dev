using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IWorkRepository : IGenericRepository<Work>
    {
        IEnumerable<WorkCreateModel> GetAllWorks();
        IEnumerable<CheckboxItemModel> GetWorksForSelecting();
    }
}
