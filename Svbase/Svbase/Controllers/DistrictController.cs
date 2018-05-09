using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    public class DistrictController : GeneralController
    {
        private readonly IDistrictService _districtService;
        private readonly IStreetService _streetService;


        public DistrictController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _districtService = ServiceManager.DistrictService;
            _streetService = ServiceManager.StreetService;
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult List()
        {
            var items = _districtService.GetAllDistricts();
            return View(items);
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(DistrictListModel model)
        {
            var result = _districtService.CreateDistrictBy(model);
            return result 
                ? DistrictsBy(model.DistrictType) 
                : RedirectToAction("Index","Dashboard");
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult DistrictsBy(DistrictType districtType)
        {
            var districts = _districtService.GetPanelBodyDistrictsBy(districtType);
            return PartialView("_DistrictsPanelBody", districts);

        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Delete(DistrictListModel model)
        {
            var result = _districtService.DeleteById(model.Id);
            return result != null
                ? DistrictsBy(model.DistrictType)
                : RedirectToAction("Index", "Dashboard");

        }

        //[Authorize(Roles = RoleConsts.Admin)]
        //[HttpPost]
        //public ActionResult Create(DistrictCreateModel model)//Todo 
        //{
        //    var isCreated = _districtService.CreateDistrictByModel(model);
        //    return !isCreated
        //        ? RedirectToAction("Create")
        //        : RedirectToAction("List");
        //}

        //[Authorize(Roles = RoleConsts.Admin)]
        //[HttpGet]
        //public ActionResult Details(int id)
        //{
        //    var district = _districtService.GetDistrictModelById(id);
        //    return View(district);
        //}

        //[Authorize(Roles = RoleConsts.Admin)]
        //[HttpPost]
        //public ActionResult Edit(DistrictCreateModel model)
        //{
        //    var isEdited = _districtService.EditDistrictByModel(model);

        //    return !isEdited 
        //        ? RedirectToAction("Details", new {id = model.Id}) 
        //        : RedirectToAction("List");
        //}
    }
}