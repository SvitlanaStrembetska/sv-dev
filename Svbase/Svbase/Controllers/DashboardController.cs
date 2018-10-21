using System.Linq;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    [Authorize]
    public class DashboardController : GeneralController
    {
        private readonly IDashboardService _dashboardService;
        private readonly ICityService _cityService;
        private readonly IStreetService _streetService;
        private readonly IApartmentService _apartmentService;
        private readonly IFlatService _flatService;


        public DashboardController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _dashboardService = ServiceManager.DashboardService;
            _cityService = ServiceManager.CityService;
            _streetService = ServiceManager.StreetService;
            _apartmentService = ServiceManager.ApartmentService;
            _flatService = ServiceManager.FlatService;
        }

        public ActionResult Details()
        {
            CreateDefaultAddressIfNotExists();
            var dashboardViewModel = _dashboardService.GetDashboardViewModel();
            return View(dashboardViewModel);
        }

        public void CreateDefaultAddressIfNotExists()
        {
            if (_cityService.GetAll().Any(x => x.Name == Consts.DefaultAddress)) return;

            var newCityItem = new City { Name = Consts.DefaultAddress };
            var newStreetItem = new Street { Name = Consts.DefaultAddress, City = newCityItem };
            var newApartmentItem = new Apartment { Name = Consts.DefaultAddress, Street = newStreetItem };
            var newFlatItem = new Flat { Number = Consts.DefaultAddress, Apartment = newApartmentItem };

            _cityService.Add(newCityItem);
            _streetService.Add(newStreetItem);
            _apartmentService.Add(newApartmentItem);
            _flatService.Add(newFlatItem);
        }
    }
}