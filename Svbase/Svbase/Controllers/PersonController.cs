using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CsvHelper;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    [Authorize]
    public class PersonController : GeneralController
    {
        private readonly IPersonService _personService;
        private readonly ICityService _cityService;
        private readonly IStreetService _streetService;
        private readonly IApartmentService _apartmentService;
        private readonly IFlatService _flatService;
        private readonly IBeneficiaryService _beneficiaryService;

        public PersonController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
            _cityService = ServiceManager.CityService;
            _streetService = ServiceManager.StreetService;
            _apartmentService = ServiceManager.ApartmentService;
            _flatService = ServiceManager.FlatService;
            _beneficiaryService = ServiceManager.BeneficiaryService;
        }

        public ActionResult Index()
        {
            var persons = _personService.GetPersons();

            return View(persons);
        }
        public ActionResult PersonsByBeneficiaryId(int id)
        {
            var persons = _personService.GetPersonsByBeneficiariesId(id);
            return View("Index",persons);
        }

        public ActionResult Create()
        {
            var beneficiaries = _beneficiaryService.GetBeneficiariesForSelecting().ToList();
            return View(new PersonViewModel { Beneficiaries = beneficiaries });
        }
        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(PersonViewModel model)
        {
            var isCreated = _personService.CreatePersonByModel(model);
            return !isCreated
                ? RedirectToAction("Create")
                : RedirectToAction("Index");
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

        //[HttpGet]
        //public ActionResult FilterStreetsByStreetSearchFilter(StreetSearchFilterModel filter)
        //{
        //    var streets = _personService.GetStretsBaseModelByStreetSearchFilter(filter);
        //    return PartialView("_FilterCheckBoxPartial", streets.ToList());
        //}

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

        //public ActionResult Edit()
        //{
        //    return View();
        //}
        //public ActionResult SearcResult()
        //{
        //    return PartialView();
        //}
        public ActionResult Import()
        {
            bool isValid;
            IEnumerable<Person> result = new List<Person>();
            var worker = new ImportModelCsvService();
            var error = new List<CsvErrorModel>();
            var model = new List<PersonViewModel>();

            using (var textReader = System.IO.File.OpenText(@"C:\Users\svitl\Desktop\test2.csv"))
            {
                var csv = new CsvReader(textReader);

                isValid = worker.Validate(csv, out error);
            }
            if (isValid)
            {
                using (var textReader = System.IO.File.OpenText(@"C:\Users\svitl\Desktop\test2.csv"))
                {
                    var csv = new CsvReader(textReader);
                    result = worker.Read(csv);

                    var entities = new List<Person>();
                    var i = 0;
                    do
                    {
                        entities.AddRange(_personService.AddRange(result.Skip(i).Take(i + 1000)));
                        i += 1000;
                    } while (i < result.Count());

                    foreach (var entity in entities)
                    {
                        var viewModel = new PersonViewModel();
                        viewModel.SetFields(entity);
                        model.Add(viewModel);
                    }
                }
            }
            else
            {
                model = new List<PersonViewModel>();
                //error
            }

            return View("Import", model);
        }

        public ActionResult Export()
        {
            return View();
        }
    }

}