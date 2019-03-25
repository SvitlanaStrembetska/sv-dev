using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    [Authorize]
    public class DublicateController : GeneralController
    {
        private readonly IPersonService _personService;

        public DublicateController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
        }

        [HttpPost]
        public ActionResult DublicateSearch(DublicateSearchType searchType, int page = 1)
        {
            IEnumerable<PersonSelectionModel> persons;
            switch (searchType)
            {
                case DublicateSearchType.FirstAndLastName:
                    persons = _personService.SearchDublicateByFirstAndLastName().AsEnumerable();
                    break;
                case DublicateSearchType.PhoneNumber:
                    persons = _personService.SearchDublicateByPhoneNumber().AsEnumerable();
                    break;
                default:
                    persons = _personService.SearchDublicateByFirstAndLastName().AsEnumerable();
                    break;
            }

            return PartialView("_PersonsTablePartial", persons.ToPagedList(page, Consts.ShowRecordsPerPage));
        }
    }
}