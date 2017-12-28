using System.Data.Entity.Infrastructure.Annotations;
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

        public PersonController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
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
                ModelState.AddModelError("", "Invalid FirthName");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }

            var newPersonItem = model.Update(new Person());
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