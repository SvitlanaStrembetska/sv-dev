using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class FlatService : EntityService<IFlatRepository, Flat>, IFlatService
    {
        public FlatService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            :base(unitOfWork,repositoryManager,repositoryManager.Flats)
        {
            
        }

        public FlatViewModel GetById(int id)
        {
            var flat = RepositoryManager.Flats.GetById(id);
            return flat;
        }
    }
}
