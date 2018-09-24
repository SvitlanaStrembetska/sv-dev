using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CsvHelper;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;
using PagedList;

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

        public ActionResult Index(FilterFileImportModel filter, int page = 1)
        {
            if (!_cityService.GetAll().Any(x => x.Name == Consts.DefaultAddress))
            {
                var newCityItem = new City { Name = Consts.DefaultAddress };
                var newStreetItem = new Street { Name = Consts.DefaultAddress, City = newCityItem };
                var newApartmentItem = new Apartment { Name = Consts.DefaultAddress, Street = newStreetItem };
                var newFlatItem = new Flat { Number = Consts.DefaultAddress, Apartment = newApartmentItem };

                _cityService.Add(newCityItem);
                _streetService.Add(newStreetItem);
                _apartmentService.Add(newApartmentItem);
                _flatService.Add(newFlatItem);
            }

            IQueryable<PersonSelectionModel> persons;

            if (Request.IsAjaxRequest())
            {
                if (filter.DistrictIds != null || filter.CityIds != null || filter.StreetIds != null || filter.ApartmentIds != null || filter.FlatIds != null)
                {
                    persons = _personService.SearchPersonsByFilter(filter);
                }
                else
                {
                    persons = _personService.GetPersons();
                }
                if (filter.ColumnsName == null || !filter.ColumnsName.Any())
                    return PartialView("_PersonsTablePartial", persons.ToPagedList(page, Consts.ShowRecordsPerPage));

                var personsList = new List<PersonSelectionModel>();
                foreach (var person in persons)
                {
                    if (filter.ColumnsName.Any(column => person.Beneficiaries.Any(x => x.Name.ToUpper() == column.ToUpper()))) continue;

                    if (filter.ColumnsName.Any(x => x.Contains("Без категорії")) && person.Beneficiaries.Any())
                        personsList.Add(person);
                    else if (!filter.ColumnsName.Any(x => x.Contains("Без категорії")))
                        personsList.Add(person);

                }
                return PartialView("_PersonsTablePartial", personsList.ToPagedList(page, Consts.ShowRecordsPerPage));
            }

            var beneficariesList = new List<string>();
            foreach (var beneficary in _beneficiaryService.GetAll())
            {
                beneficariesList.Add(beneficary.Name);
            }
            ViewBag.Beneficaries = beneficariesList;

            persons = _personService.GetPersons();

            return View(persons.ToPagedList(page, Consts.ShowRecordsPerPage));
        }

        [HttpGet]
        public ActionResult All(int page = 1)
        {
            var persons = _personService.GetPersons();

            return PartialView("SelectionPersonPartial", persons);
        }

        public ActionResult PersonsByBeneficiaryId(int id, int page = 1)
        {
            var persons = _personService.GetPersonsByBeneficiariesId(id);
            return View("Index", persons.ToPagedList(page, Consts.ShowRecordsPerPage));
        }

        public ActionResult Create()
        {
            return View(new PersonAndFullAddressViewModel { Beneficiaries = _beneficiaryService.GetBeneficiariesForSelecting().ToList() });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(PersonAndFullAddressViewModel model)
        {
            if (model.LastName == null && model.FirstName == null)
                return RedirectToAction("Create");

            var isCreated = _personService.CreatePersonByModel(model);
            return !isCreated
                ? RedirectToAction("Create", model)
                : RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var person = _personService.GetPersonWithAddressById(id);
            var personBeneficiaries = person.Beneficiaries;
            foreach (var beneficary in _beneficiaryService.GetAll().ToList().Where(beneficary => !personBeneficiaries.Any(x => x.Id == beneficary.Id)))
            {
                personBeneficiaries.Add(new CheckboxItemModel
                {
                    Id = beneficary.Id,
                    Name = beneficary.Name,
                    IsChecked = false 
                });
            }
            person.Beneficiaries = personBeneficiaries;
            return PartialView("_PersonEditBody", person);
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Edit(PersonAndFullAddressViewModel model)
        {

            if (model.LastName == null && model.FirstName == null)
                return RedirectToAction("Edit", model);

            var person = _personService.FindById(model.Id);
            if (person == null)
                return RedirectToAction("Index");

            var personBeneficiaries = person.Beneficiaries.ToList();
            foreach (var beneficary in personBeneficiaries)
            {
                person.Beneficiaries.Remove(beneficary);
            }

            var personFlats = person.Flats.ToList();
            foreach (var flat in personFlats)
            {
                person.Flats.Remove(flat);
            }
            
            person = model.Update(person);


            if (model.Beneficiaries != null && model.Beneficiaries.Any())
            {
                var selectedBeneficiaries = model.Beneficiaries.Where(x => x.IsChecked).ToList();
                foreach (var beneficiary in selectedBeneficiaries)
                {
                    var benef = _beneficiaryService.FindById(beneficiary.Id);
                    person.Beneficiaries.Add(benef);
                    _beneficiaryService.Attach(benef);
                }
            }

            var flats = new List<Flat> { _flatService.FindById(model.FlatId) };
            foreach (var flat in flats)
            {
                _flatService.Attach(flat);
            }

            person.Flats = flats;
            _personService.Update(person);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _personService.DeleteById(id);

            return Json(new { status = "success" });
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
            return PartialView("_FilterCityPartial", cities);
        }

        [HttpGet]
        public ActionResult FilterStreet()
        {
            var streets = _personService.GetStreetsForFilter();
            return PartialView("_FilterCheckBoxPartial", streets);
        }


        [HttpPost]
        public ActionResult SearchPersonsByFilter(FilterFileImportModel filter)
        {
            var persons = _personService.SearchPersonsByFilter(filter);
            return PartialView("_PersonsTablePartial", persons.ToPagedList(1, Consts.ShowRecordsPerPage));
        }
        //[HttpGet]
        //public ActionResult FilterStreetsByStreetSearchFilter(StreetSearchFilterModel filter)
        //{
        //    var streets = _personService.GetStretsBaseModelByStreetSearchFilter(filter);
        //    return PartialView("_FilterCheckBoxPartial", streets.ToList());
        //}

        [HttpGet]
        public ActionResult FilterStreetsByCityIds(IList<int> cityIds)
        {
            var streets = _personService.GetFilterStreetsByCityIds(cityIds).ToList();
            return PartialView("FilterItemPartial", streets);
        }

        [HttpGet]
        public ActionResult FilterApartmentsByStreetIds(IList<int> streetIds)
        {
            var filterApartments = _streetService.GetFilterApartmentsByStreetIds(streetIds);
            return PartialView("_FilterApartmentPartial", filterApartments.ToList());
        }

        [HttpGet]
        public ActionResult FilterFlatsByApartmentIds(IList<int> apartmentIds)
        {
            var flats = _apartmentService.GetFilterFlatsByApartmentIds(apartmentIds);
            return PartialView("_FilterFlatPartial", flats.ToList());
        }

        [HttpGet]
        public ActionResult OptionSelectWorksPartial()
        {
            var works = _personService.GetWorksBaseViewModels();
            return PartialView("_OptionSelectBasePartial", works);
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
        public ActionResult OptionSelectDefaultStreetsPartial(int cityId)
        {
            var streets = _cityService.GetStreetsBaseModelByCityId(cityId).Where(x=>x.Name == Consts.DefaultAddress);
            return PartialView("_OptionSelectBasePartial", streets);
        }

        [HttpGet]
        public ActionResult OptionSelectApartmentPartial(int streetId)
        {
            var apartments = _streetService.GetApartmentsBaseModelByStreetId(streetId);
            return PartialView("_OptionSelectBasePartial", apartments);
        }

        [HttpGet]
        public ActionResult OptionSelectDefaultApartmentPartial(int streetId)
        {
            var apartments = _streetService.GetApartmentsBaseModelByStreetId(streetId).Where(x => x.Name == Consts.DefaultAddress);
            return PartialView("_OptionSelectBasePartial", apartments);
        }

        [HttpGet]
        public ActionResult OptionSelectFlatPartial(int apartmentId)
        {
            var flats = _apartmentService.GetFlatsBaseModelByApartmentId(apartmentId);
            return PartialView("_OptionSelectBasePartial", flats);
        }

        public ActionResult OptionSelectDefaultFlatPartial(int apartmentId)
        {
            var flats = _apartmentService.GetFlatsBaseModelByApartmentId(apartmentId).Where(x => x.Name == Consts.DefaultAddress);
            return PartialView("_OptionSelectBasePartial", flats);
        }


        public ActionResult Import()
        {
            bool isValid;

            IEnumerable<Person> result = new List<Person>();
            var worker = new ImportModelCsvService();
            var error = new List<CsvErrorModel>();
            var model = new List<PersonViewModel>();
            var fileName = Path.GetFileName("test2.csv");
            var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
            using (var textReader = System.IO.File.OpenText(path))
            {
                var csv = new CsvReader(textReader);

                isValid = worker.Validate(csv, out error);
            }
            if (isValid)
            {
                using (var textReader = System.IO.File.OpenText(path))
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