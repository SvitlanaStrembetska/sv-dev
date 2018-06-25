using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class WorkRepository : GenericRepository<Work>, IWorkRepository
    {
        public WorkRepository(ApplicationDbContext context)
            : base(context) {}

        public IEnumerable<WorkCreateModel> GetAllWorks()
        {
            return DbSet.Select(x => new WorkCreateModel
            {
                Id = x.Id,
                Name = x.Name,
                CanDelete = !x.Persons.Any()
            });
        }
        
        public IEnumerable<CheckboxItemModel> GetWorksForSelecting()
        {
            return DbSet.Select(x => new CheckboxItemModel
            {
                Id = x.Id,
                Name = x.Name,
                IsChecked = false
            });
        }
    }
}
