using Svbase.Service.Interfaces;

namespace Svbase.Service.Factory
{
    public interface IServiceManager
    {
        IApartmentService ApartmentService { get; }
        IApplicationUserService ApplicationUserService { get; }
        IBeneficiaryService BeneficiaryService { get; }
        ICityService CityService { get; }
        IDistrictService DistrictService { get; }
        IFlatService FlatService { get; }
        IPersonService PersonService { get; }
        IStreetService StreetService { get; }
    }
}
