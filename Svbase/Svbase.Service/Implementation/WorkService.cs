using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class WorkService : EntityService<IWorkRepository, Work>, IWorkService
    {
        public WorkService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            :base(unitOfWork, repositoryManager,repositoryManager.Works)
        {
            
        }

        public IEnumerable<WorkCreateModel> GetAllWorks()
        {
            return RepositoryManager.Works.GetAllWorks();
        }

        public IEnumerable<CheckboxItemModel> GetWorksForSelecting()
        {
            return RepositoryManager.Works.GetWorksForSelecting();
        }
    }
}
