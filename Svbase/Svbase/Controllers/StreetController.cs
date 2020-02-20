﻿using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;
using System.Web.Mvc;

namespace Svbase.Controllers
{
    public class StreetController : GeneralController
    {
        private readonly IStreetService _streetService;
        private readonly IApartmentService _apartmentService;
        private readonly IFlatService _flatService;

        public StreetController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _streetService = ServiceManager.StreetService;
            _apartmentService = ServiceManager.ApartmentService;
            _flatService = ServiceManager.FlatService;
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(StreetCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("", @"Invalid Name");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("List", "City"); //Todo page not found
            }

            var newStreetItem = model.Update(new Street());
            var newApartmentItem = new Apartment { Name = Consts.DefaultAddress, Street = newStreetItem };
            var newFlatItem = new Flat { Number = Consts.DefaultAddress, Apartment = newApartmentItem };

            _streetService.Add(newStreetItem);
            _apartmentService.Add(newApartmentItem);
            _flatService.Add(newFlatItem);

            return RedirectToAction("Details", "City", new { id = model.CityId });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Edit(StreetCreateModel model)
        {
            if (model == null)
            {
                return Json(new { status = "error" });
            }

            var street = _streetService.FindById(model.Id);
            if (street == null)
            {
                return Json(new { status = "error" });
            }
            street.Name = model.Name;
            street.Pseudonym = model.Pseudonym;
            _streetService.Update(street);
            return Json(new { status = "success" });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var street = _streetService.GetStreetById(id);
            if (street == null)
            {
                RedirectToAction("List", "City");
            }
            return View(street);
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _streetService.DeleteById(id);

            return Json(new { status = "success" });
        }
    }
}