using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
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
        public ActionResult DublicateSearch(int page = 1)
        {
            var persons = _personService.SearchDublicateByFirstAndLastName().ToList();

            return PartialView("_PersonsTablePartial", persons.ToPagedList(page, Consts.ShowRecordsPerPage));
        }

    }
}