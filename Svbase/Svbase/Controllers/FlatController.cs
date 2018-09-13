using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;
using Svbase.Core.Data.Entities;

namespace Svbase.Controllers
{
    public class FlatController : GeneralController
    {
        private readonly IFlatService _flatService;

        public FlatController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _flatService = ServiceManager.FlatService;
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var flat = _flatService.GetById(id);
            if (flat == null)
            {
                RedirectToAction("List", "City");
            }
            return View(flat);
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(FlatCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("", "Invalid Name");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("List", "City"); //Todo page not found
            }

            var newFlatItem = model.Update(new Flat());
            _flatService.Add(newFlatItem);
            return RedirectToAction("Details", "Apartment", new { id = model.ApartmentId });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Edit(FlatCreateModel model)
        {
            if (model == null)
            {
                return Json(new { status = "error" });
            }

            var flat = _flatService.FindById(model.Id);
            if (flat == null)
            {
                return Json(new { status = "error" });
            }
            flat.Number = model.Name;
            _flatService.Update(flat);
            return Json(new { status = "success" });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _flatService.DeleteById(id);

            return Json(new { status = "success" });
        }

        [HttpGet]
        public ActionResult FindAddressByFlatId(int flatId)
        {
            var flatById = _flatService.FindById(flatId);
            BaseViewModel flat = new BaseViewModel
            {
                Id = flatById.Id,
                Name = flatById.Number
            };
            BaseViewModel apartment = new BaseViewModel
            {
                Id = flatById.ApartmentId,
                Name = flatById.Apartment.Name
            };
            BaseViewModel street = new BaseViewModel
            {
                Id = flatById.Apartment.StreetId,
                Name = flatById.Apartment.Street.Name
            };
            BaseViewModel city = new BaseViewModel
            {
                Id = flatById.Apartment.Street.CityId,
                Name = flatById.Apartment.Street.City.Name
            };

            return Json(new { flat, apartment, street, city }, JsonRequestBehavior.AllowGet);
        }

    }
}