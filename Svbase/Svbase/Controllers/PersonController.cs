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
        public ActionResult SearcResult()
        {
            return PartialView();
        }

        public ActionResult Export()
        {
            return View();
        }
    }

}