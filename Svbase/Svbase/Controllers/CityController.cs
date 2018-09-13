using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    public class CityController : GeneralController
    {
        private readonly ICityService _cityService;
        private readonly IStreetService _streetService;
        private readonly IApartmentService _apartmentService;
        private readonly IFlatService _flatService;

        public CityController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _cityService = ServiceManager.CityService;
            _streetService = ServiceManager.StreetService;
            _apartmentService = ServiceManager.ApartmentService;
            _flatService = ServiceManager.FlatService;
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(CityCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("","Invalid Name");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("List");
            }

            var newCityItem = model.Update(new City());
            var newStreetItem = new Street { Name = Consts.DefaultAddress, City = newCityItem };
            var newApartmentItem = new Apartment { Name = Consts.DefaultAddress, Street = newStreetItem };
            var newFlatItem = new Flat { Number = Consts.DefaultAddress, Apartment = newApartmentItem };
            
            _cityService.Add(newCityItem);
            _streetService.Add(newStreetItem);
            _apartmentService.Add(newApartmentItem);
            _flatService.Add(newFlatItem);

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    status = "success"
                });
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Edit(CityCreateModel model)
        {
            if (model == null)
            {
                return Json(new {status = "error"});
            }

            var city = _cityService.FindById(model.Id);
            if (city == null)
            {
                return Json(new {status = "error"});
            }
            city =  model.Update(city);
            _cityService.Update(city);
            return Json(new { status = "success" });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var city = _cityService.GetCityById(id);
            if (city == null)
            {
                RedirectToAction("List");
            }
            return View(city);
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _cityService.DeleteById(id);

            return Json(new { status = "success" });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult List()
        {
            return View(_cityService.GetCities());
        }
    }
}