using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Factory
{
    public interface IRepositoryManager
    {
        IApartmentRepository Apartments { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        IBeneficiaryRepository Beneficiaries { get; }
        ICityRepository Cities { get; }
        IDistrictRepository Districts { get; }
        IFlatRepository Flats { get; }
        IPersonRepository Persons { get; }
        IStreetRepository Streets { get; }
    }
}
