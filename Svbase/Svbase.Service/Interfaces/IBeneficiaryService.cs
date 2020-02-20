using System.Collections.Generic;
using System.Threading.Tasks;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IBeneficiaryService : IEntityService<Beneficiary>
    {
        IEnumerable<BeneficiaryCreateModel> GetAllBeneficiaries();
        IEnumerable<CheckboxItemModel> GetBeneficiariesForSelecting();
        Task<List<BeneficiaryCreateModel>> GetAllBeneficiariesAsync();
        Task<List<CheckboxItemModel>> GetBeneficiariesForSelectingAsync();
    }
}
