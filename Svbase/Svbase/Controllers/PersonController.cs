using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    public class PersonController : GeneralController
    {
        private readonly IPersonService _personService;
        private readonly ICityService _cityService;
        private readonly IStreetService _streetService;
        private readonly IApartmentService _apartmentService;
        private readonly IFlatService _flatService;

        public PersonController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
            _cityService = ServiceManager.CityService;
            _streetService = ServiceManager.StreetService;
            _apartmentService = ServiceManager.ApartmentService;
            _flatService = ServiceManager.FlatService;
        }

        public ActionResult Index()
        {
            var persons = _personService.GetPersons();
           
            return View(persons);
        }
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(PersonViewModel model)
        {
            if (string.IsNullOrEmpty(model.FirstName))
            {
                ModelState.AddModelError("", "Invalid FirstName");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            var newPersonItem = model.Update(new Person());
            var flats = new List<Flat>
            {
                new Flat
                {
                    Id = model.FlatId
                }
            };
            foreach (var flat in flats)
            {
                _flatService.Attach(flat);
            }
            newPersonItem.Flats = flats;
            newPersonItem = _personService.Add(newPersonItem);
            return RedirectToAction("Details", new { id = newPersonItem.Id });
        }
        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var person = _personService.GetPersonById(id);
            if (person == null)
            {
                RedirectToAction("Index");
            }
            return View(person);
        }

        [HttpGet]
        public ActionResult FilterDistricts()
        {
            var districts = _personService.GetDistrictsForFilter();
            return PartialView("_FilterCheckBoxPartial", districts);
        }

        [HttpGet]
        public ActionResult FilterCities()
        {
            var cities = _personService.GetCitiesBaseViewModels();
            return PartialView("_FilterCheckBoxPartial", cities);
        }

        [HttpGet]
        public ActionResult FilterStreet()
        {
            var streets = _personService.GetStreetsForFilter();
            return PartialView("_FilterCheckBoxPartial", streets);
        }

        [HttpGet]
        public ActionResult FilterStreetsByStreetSearchFilter(StreetSearchFilterModel filter)
        {
            var streets = _personService.GetStretsBaseModelByStreetSearchFilter(filter);
            return PartialView("_FilterCheckBoxPartial", streets.ToList());
        }

        [HttpGet]
        public ActionResult FilterApartmentsByStreetIds(IList<int> streetIds)
        {
            var apartments = _personService.GetApartmentsBaseModelByStreetIds(streetIds);
            return PartialView("_FilterCheckBoxPartial", apartments.ToList());
        }

        [HttpGet]
        public ActionResult FilterFlatsByApartmentIds(IList<int> apartmentIds)
        {
            var apartments = _personService.GetFlatsBaseModelByApatrmentIds(apartmentIds);
            return PartialView("_FilterCheckBoxPartial", apartments.ToList());
        }

        [HttpGet]
        public ActionResult OptionSelectCitiesPartial()
        {
            var cities = _personService.GetCitiesBaseViewModels();
            return PartialView("_OptionSelectBasePartial", cities);
        }

        [HttpGet]
        public ActionResult OptionSelectStreetsPartial(int cityId)
        {
            var streets = _cityService.GetStreetsBaseModelByCityId(cityId);
            return PartialView("_OptionSelectBasePartial", streets);
        }

        [HttpGet]
        public ActionResult OptionSelectApartmentPartial(int streetId)
        {
            var apartments = _streetService.GetApartmentsBaseModelByStreetId(streetId);
            return PartialView("_OptionSelectBasePartial", apartments);
        }

        [HttpGet]
        public ActionResult OptionSelectFlatPartial(int apartmentId)
        {
            var flats = _apartmentService.GetFlatsBaseModelByApartmentId(apartmentId);
            return PartialView("_OptionSelectBasePartial", flats);
        }

        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult SearcResult()
        {
            return PartialView();
        }

        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Export()
        {
            return View();
        }
    }

}