using Svbase.Core.Data.Entities;

namespace Svbase.Core.Models
{
    public class BeneficiaryCreateModel : BaseViewModel
    {
        public bool CanDelete { get; set; }

        public Beneficiary Update(Beneficiary beneficiary)
        {
            beneficiary.Id = Id;
            beneficiary.Name = Name;
            return beneficiary;
        }
    }
}
