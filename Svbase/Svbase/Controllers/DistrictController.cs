using System.Linq;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
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
        [HttpGet]
        public ActionResult Create()
        {
            var streets = _streetService.GetStreetsForSelecting().ToList();
            return View(new DistrictCreateModel { Streets = streets });
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