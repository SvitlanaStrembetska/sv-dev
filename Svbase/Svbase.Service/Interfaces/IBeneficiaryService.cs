using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IBeneficiaryService : IEntityService<Beneficiary>
    {
        IEnumerable<BeneficiaryCreateModel> GetAllBeneficiaries();
    }
}
