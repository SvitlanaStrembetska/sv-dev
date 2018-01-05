using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class StreetService : EntityService<IStreetRepository, Street>, IStreetService
    {
        public StreetService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            :base(unitOfWork, repositoryManager, repositoryManager.Streets)
        {
            
        }

        public StreetViewModel GetStreetById(int id)
        {
            var street = RepositoryManager.Streets.GetStreetById(id);
            return street;
        }

        public IEnumerable<StreetSelectModel> GetStreetsForSelecting()
        {
            var streets = RepositoryManager.Streets.GetStreetsForSelecting();
            return streets;
        }

        public IEnumerable<Street> GetStreetsByDistrictId(int id)
        {
            var streets = RepositoryManager.Streets.GetStreetsByDistrictId(id);
            return streets;
        }

        public IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetId(int id)
        {
            var apartments = RepositoryManager.Streets.GetApartmentsBaseModelByStreetId(id);
            return apartments;
        }
    }
}
