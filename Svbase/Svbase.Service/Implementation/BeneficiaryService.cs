using System.Collections.Generic;
using System.Threading.Tasks;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class BeneficiaryService : EntityService<IBeneficiaryRepository, Beneficiary>, IBeneficiaryService
    {
        public BeneficiaryService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            :base(unitOfWork, repositoryManager,repositoryManager.Beneficiaries)
        {
            
        }

        public IEnumerable<BeneficiaryCreateModel> GetAllBeneficiaries()
        {
            var beneficiaries = RepositoryManager.Beneficiaries.GetAllBeneficiaries();
            return beneficiaries;
        }

        public IEnumerable<CheckboxItemModel> GetBeneficiariesForSelecting()
        {
            var beneficiaries = RepositoryManager.Beneficiaries.GetBeneficiariesForSelecting();
            return beneficiaries;
        }

        public async Task<List<BeneficiaryCreateModel>> GetAllBeneficiariesAsync()
        {
            var beneficiaries = await RepositoryManager.Beneficiaries.GetAllBeneficiariesAsync();
            return beneficiaries;
        }

        public async Task<List<CheckboxItemModel>> GetBeneficiariesForSelectingAsync()
        {
            var beneficiaries = await RepositoryManager.Beneficiaries.GetBeneficiariesForSelectingAsync();

            return beneficiaries;
        }
    }
}
