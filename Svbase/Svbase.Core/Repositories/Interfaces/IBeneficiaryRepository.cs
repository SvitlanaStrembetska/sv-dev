using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IBeneficiaryRepository : IGenericRepository<Beneficiary>
    {
        IEnumerable<BeneficiaryCreateModel> GetAllBeneficiaries();
        IEnumerable<DashboardItemModel> GetDashboardBeneficiaries();
        IEnumerable<CheckboxItemModel> GetBeneficiariesForSelecting();
        
        Task<List<BeneficiaryCreateModel>> GetAllBeneficiariesAsync();
        Task<List<DashboardItemModel>> GetDashboardBeneficiariesAsync();
        Task<List<CheckboxItemModel>> GetBeneficiariesForSelectingAsync();
    }
}
