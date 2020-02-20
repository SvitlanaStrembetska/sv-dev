using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Svbase.Controllers
{
    public class DistrictController : GeneralController
    {
        private readonly IDistrictService _districtService;


        public DistrictController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _districtService = ServiceManager.DistrictService;
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Index()
        {
            var model = _districtService.GetDistrictViewInitDataModel(DistrictType.Сonstituency);
            return View(model);
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
                : RedirectToAction("Index");
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult DistrictsBy(DistrictType districtType)
        {
            var districts = _districtService.GetPanelBodyDistrictsBy(districtType);
            return PartialView("_DistrictsPanelBody", districts);
        }

        [HttpGet]
        public ActionResult DistrictsByType(DistrictType districtType)
        {
            return PartialView("_FilterMultiSelectPartial", _districtService.GetDistrictsByType(districtType).ToList());
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult StreetsBy(int cityId)
        {
            var streets = _districtService.GetPanelBodyStreetsBy(cityId);
            return PartialView("_DistrictsPanelBody", streets);

        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult ApartmentsBy(DistrictViewApartmentSearchFilter filter)
        {
            var apartments = _districtService.GetPanelBodyApartmentsBy(filter);
            return PartialView("_DistrictApartmentPanelBody", apartments);

        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Delete(DistrictListModel model)
        {
            var result = _districtService.DeleteById(model.Id);
            return result != null
                ? DistrictsBy(model.DistrictType)
                : RedirectToAction("Index");

        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult SaveDistrict(SaveDistrictModel model)
        {
            var result = _districtService.SaveDistrictBy(model);
            if (result)
            {
                return RedirectToAction("Index");
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { });

        }
    }
}