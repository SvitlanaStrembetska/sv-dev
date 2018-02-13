using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IBeneficiaryRepository : IGenericRepository<Beneficiary>
    {
        IEnumerable<BeneficiaryCreateModel> GetAllBeneficiaries();
        IEnumerable<DashboardItemModel> GetDashboardBeneficiaries();
        IEnumerable<CheckboxItemModel> GetBeneficiariesForSelecting();
        IEnumerable<PersonViewModel> GetPersonsByBeneficiariesId(int beneficiaryId);
    }
}
