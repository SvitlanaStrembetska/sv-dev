using Svbase.Core.Data.Entities;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class DistrictService : EntityService<IDistrictRepository, District>, IDistrictService
    {
        public DistrictService(IUnitOfWork unitOfWork,IRepositoryManager repositoryManager)
            :base(unitOfWork,repositoryManager,repositoryManager.Districts)
        {
            
        }
    }
}
