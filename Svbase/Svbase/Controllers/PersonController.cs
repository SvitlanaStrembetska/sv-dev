using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;
using PagedList;
using Svbase.Helpers;
using Svbase.Models;

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
        private readonly PersonFilterHelper _personFilterHelper;

        public PersonController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
            _cityService = ServiceManager.CityService;
            _streetService = ServiceManager.StreetService;
            _apartmentService = ServiceManager.ApartmentService;
            _flatService = ServiceManager.FlatService;
            _beneficiaryService = ServiceManager.BeneficiaryService;
            _personFilterHelper = new PersonFilterHelper();
        }

        [HttpGet]
        public ActionResult Index(FilterFileImportModel filter, int page=1)
        {
            var skip = (page - 1) * Consts.ShowRecordsPerPage;

            //=================  generate beneficiaries list for filter  =================   
            var beneficiariesList = new List<Beneficiary>();
            foreach (var beneficary in _beneficiaryService.GetAll())
                beneficiariesList.Add(new Beneficiary { Id = beneficary.Id, Name = beneficary.Name });
            ViewBag.Beneficaries = beneficiariesList;
            //=================  end generate beneficiaries list for filter  =============
            
            var people = filter.DistrictIds != null ? _personService.SearchPersonsByFilter(filter) : _personService.GetPersons().Where(x => x.IsDead == filter.IsDeadPerson);
            ViewBag.PagesCount = (people.Count() + Consts.ShowRecordsPerPage - 1) / Consts.ShowRecordsPerPage;

            if (filter.BeneficariesChecked == null || !filter.BeneficariesChecked.Any())
            {
                var benChecked = beneficiariesList.Select(x => x.Id.ToString()).ToList();
                benChecked.Add("0");
                ViewBag.BeneficariesChecked = benChecked;
                return View(people.Skip(skip).Take(Consts.ShowRecordsPerPage).ToList());
            }

            //=================  generate beneficiaries unchecked list for filter (from Dashboard)  ================= 
            ViewBag.BeneficariesChecked = filter.BeneficariesChecked.ToList();
            //=================  end generate beneficiaries unchecked list for filter (from Dashboard)  ============= 
            

            return View(_personFilterHelper.FilterPersonsByBeneficiary(people, filter.BeneficariesChecked).Skip(skip).Take(Consts.ShowRecordsPerPage).ToList());
        }

        [HttpPost]
        public ActionResult PostIndex(FilterFileImportModel filter, int page = 1)
        {
            var skip = (page - 1) * Consts.ShowRecordsPerPage;
            var people = GetPeopleByFilter(filter);

            if (filter.BeneficariesChecked == null || !filter.BeneficariesChecked.Any())
                return PartialView("_PersonsTablePartial", _personFilterHelper.OrderPersonsBy(people, filter.SortOrder, filter.FirstSortOrder,
                    filter.SecondSortOrder, filter.ThirdSortOrder, skip, Consts.ShowRecordsPerPage));

            return PartialView("_PersonsTablePartial", _personFilterHelper.OrderPersonsBy(
                _personFilterHelper.FilterPersonsByBeneficiary(people, filter.BeneficariesChecked), 
                filter.SortOrder, filter.FirstSortOrder, filter.SecondSortOrder, filter.ThirdSortOrder,
                skip, Consts.ShowRecordsPerPage));
        }

        [HttpPost]
        public ActionResult PostIndexPagesCount(FilterFileImportModel filter)
        {
            var people = GetPeopleByFilter(filter);
            var pagesCount = (people.Count() + Consts.ShowRecordsPerPage - 1) / Consts.ShowRecordsPerPage;
            return PartialView("PageBlock", new PageModel { PagesCount = pagesCount });
        }

        private IQueryable<PersonSelectionModel> GetPeopleByFilter(FilterFileImportModel filter)
        {
            IQueryable<PersonSelectionModel> people;

            if (filter.DistrictIds != null || filter.CityIds != null || filter.StreetIds != null ||
                filter.ApartmentIds != null || filter.FlatIds != null)
                people = _personService.SearchPersonsByFilter(filter);
            else
                people = _personService.GetPersons().Where(x => x.IsDead == filter.IsDeadPerson);

            if (filter.StartDate != null && filter.EndDate != null)
                people = people.Where(x => x.DateBirth >= filter.StartDate && x.DateBirth <= filter.EndDate);
            else if (filter.StartDate != null)
                people = people.Where(x => x.DateBirth >= filter.StartDate);
            else if (filter.EndDate != null)
                people = people.Where(x => x.DateBirth <= filter.EndDate);
            else if (filter.StartDateMonth != null && filter.EndDateMonth != null)
                people = people.Where(x => x.DateBirth.Value.Day >= filter.StartDateMonth.Value.Day
                                           && x.DateBirth.Value.Month >= filter.StartDateMonth.Value.Month
                                           && x.DateBirth.Value.Day <= filter.EndDateMonth.Value.Day
                                           && x.DateBirth.Value.Month <= filter.EndDateMonth.Value.Month);
            else if (filter.StartDateMonth != null)
                people =
                    people.Where(
                        x =>
                            x.DateBirth.Value.Day >= filter.StartDateMonth.Value.Day &&
                            x.DateBirth.Value.Month >= filter.StartDateMonth.Value.Month);
            else if (filter.EndDateMonth != null)
                people =
                    people.Where(
                        x =>
                            x.DateBirth.Value.Day <= filter.EndDateMonth.Value.Day &&
                            x.DateBirth.Value.Month <= filter.EndDateMonth.Value.Month);
            return people;
        }

        [HttpPost]
        public ActionResult SearchPeople(PersonSearchModel searchFields, int page = 1)
        {
            var skip = (page - 1) * Consts.ShowRecordsPerPage;
            var persons = GetPeopleBySearch(searchFields);

            return PartialView("_PersonsTablePartial", persons.Skip(skip).Take(Consts.ShowRecordsPerPage).ToList());
        }

        [HttpPost]
        public ActionResult SearchPeoplePagesCount(PersonSearchModel searchFields, int page = 1)
        {
            var people = GetPeopleBySearch(searchFields);

            var pagesCount = (people.Count() + Consts.ShowRecordsPerPage - 1) / Consts.ShowRecordsPerPage;
            return PartialView("PageBlock", new PageModel { PagesCount = pagesCount });
        }


        public IQueryable<PersonSelectionModel> GetPeopleBySearch(PersonSearchModel searchFields)
        {
            IQueryable<PersonSelectionModel> people;

            if (searchFields.IsFirstNameIncludedInSearch || searchFields.IsLastNameIncludedInSearch || searchFields.IsMiddleNameIncludedInSearch || searchFields.IsMobilePhoneIncludedInSearch)
                people = _personService.SearchPersonsByFields(searchFields);
            else
                people = _personService.GetPersons().Where(x => x.IsDead == false);

            return people;
        }

        [HttpGet]
        public ActionResult All(int page = 1)
        {
            var skip = (page - 1) * Consts.ShowRecordsPerPage;
            return PartialView("_PersonsTablePartial", _personService.GetPersons().Where(x => x.IsDead == false).Skip(skip).Take(Consts.ShowRecordsPerPage));
        }

        public ActionResult AllPagesCount()
        {
            var people = _personService.GetPersons().Where(x => x.IsDead == false);

            var pagesCount = (people.Count() + Consts.ShowRecordsPerPage - 1) / Consts.ShowRecordsPerPage;
            return PartialView("PageBlock", new PageModel { PagesCount = pagesCount });
        }

        public ActionResult Create()
        {
            return View(new PersonAndFullAddressViewModel {Beneficiaries = _beneficiaryService.GetBeneficiariesForSelecting().ToList()});
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(PersonAndFullAddressViewModel model)
        {
            if (model.LastName == null && model.FirstName == null)
                return RedirectToAction("Create");

            Flat flat;
            if (model.FlatId == 0 && model.ApartmentId == 0 && model.StreetId == 0)
                flat = _flatService.GetAll().Where(x => x.Number == Consts.DefaultAddress && x.Apartment.Name == Consts.DefaultAddress && x.Apartment.Street.Name == Consts.DefaultAddress && x.Apartment.Street.City.Id == model.CityId).ToList().FirstOrDefault();
            else if (model.FlatId == 0 && model.ApartmentId == 0 && model.StreetId != 0)
                flat = _flatService.GetAll().Where(x => x.Number == Consts.DefaultAddress && x.Apartment.Name == Consts.DefaultAddress && x.Apartment.Street.Id == model.StreetId && x.Apartment.Street.City.Id == model.CityId).ToList().FirstOrDefault();
            else if (model.FlatId == 0 && model.ApartmentId != 0 && model.StreetId != 0)
                flat = _flatService.GetAll().Where(x => x.Number == Consts.DefaultAddress && x.Apartment.Id == model.ApartmentId && x.Apartment.Street.Id == model.StreetId && x.Apartment.Street.City.Id == model.CityId).ToList().FirstOrDefault();
            else
                flat = _flatService.FindById(model.FlatId);

            var isCreated = _personService.CreatePersonByModel(model, flat);
            return !isCreated ? RedirectToAction("Create", model) : RedirectToAction("Index", "Person");
        }

        public ActionResult Edit(int id)
        {
            var person = _personService.GetPersonWithAddressById(id);
            var personBeneficiaries = person.Beneficiaries;
            foreach (var beneficary in _beneficiaryService.GetAll().ToList().Where(beneficary => !personBeneficiaries.Any(x => x.Id == beneficary.Id)))
            {
                personBeneficiaries.Add(new CheckboxItemModel
                {
                    Id = beneficary.Id, Name = beneficary.Name, IsChecked = false
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

            var flats = new List<Flat> {_flatService.FindById(model.FlatId)};
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

            return Json(new {status = "success"});
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

        [HttpPost]
        public ActionResult FilterStreetsByCityIds(IList<int> cityIds)
        {
            var streets = _personService.GetFilterStreetsByCityIds(cityIds).ToList();
            return PartialView("FilterItemPartial", streets);
        }

        [HttpPost]
        public ActionResult FilterApartmentsByStreetIds(IList<int> streetIds)
        {
            var filterApartments = _streetService.GetFilterApartmentsByStreetIds(streetIds);
            return PartialView("_FilterApartmentPartial", filterApartments.ToList());
        }

        [HttpPost]
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
            var streets = _cityService.GetStreetsBaseModelByCityId(cityId).Where(x => x.Name == Consts.DefaultAddress);
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
    }
}