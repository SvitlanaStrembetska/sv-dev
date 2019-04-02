using System.Linq;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Models;
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
            var skip = (page - 1)*Consts.ShowRecordsPerPage;
            var people = GetPopleByDuplicateSearch(searchType);
            
            return PartialView("_PersonsTablePartial", people.Skip(skip).Take(Consts.ShowRecordsPerPage).ToList());
        }

        [HttpPost]
        public ActionResult DublicateSearchPagesCount(DublicateSearchType searchType)
        {
            var people = GetPopleByDuplicateSearch(searchType);
            
            var pagesCount = (people.Count() + Consts.ShowRecordsPerPage - 1)/Consts.ShowRecordsPerPage;
            return PartialView("PageBlock", new PageModel {PagesCount = pagesCount});
        }

        public IQueryable<PersonSelectionModel> GetPopleByDuplicateSearch(DublicateSearchType searchType)
        {
            IQueryable<PersonSelectionModel> people;
            switch (searchType)
            {
                case DublicateSearchType.FirstAndLastName:
                    people = _personService.SearchDublicateByFirstAndLastName();
                    break;
                case DublicateSearchType.PhoneNumber:
                    people = _personService.SearchDublicateByPhoneNumber();
                    break;
                default:
                    people = _personService.SearchDublicateByFirstAndLastName();
                    break;
            }
            return people;
        }
    }
}