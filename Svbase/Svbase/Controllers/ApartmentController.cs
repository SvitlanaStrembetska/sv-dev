using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    public class ApartmentController : GeneralController
    {
        private readonly IApartmentService _apartmentService;

        public ApartmentController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _apartmentService = ServiceManager.ApartmentService;
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(ApartmentCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("", "Invalid Name");
            }

            if (!ModelState.IsValid)
            {
                //return RedirectToAction("De");//Todo page not found
            }

            var newApartmentItem = model.Update(new Apartment());
            newApartmentItem = _apartmentService.Add(newApartmentItem);
            return RedirectToAction("Details", new { id = newApartmentItem.Id });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var apartment = _apartmentService.GetById(id);
            if (apartment == null)
            {
                RedirectToAction("List", "City");
            }
            return View(apartment);
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Edit(ApartmentCreateModel model)
        {
            if (model == null)
            {
                return Json(new { status = "error" });
            }

            var apartment = _apartmentService.FindById(model.Id);
            if (apartment == null)
            {
                return Json(new { status = "error" });
            }
            apartment.Name = model.Name;
            _apartmentService.Update(apartment);
            return Json(new { status = "success" });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _apartmentService.DeleteById(id);
            return null;
        }
    }
}