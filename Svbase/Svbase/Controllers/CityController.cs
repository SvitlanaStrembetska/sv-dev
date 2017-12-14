using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    public class CityController : GeneralController
    {
        private readonly ICityService _cityService;

        public CityController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _cityService = ServiceManager.CityService;
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(CityCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("","Invalid Name");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var newCityItem = model.UpdateCity(new City());
            newCityItem = _cityService.Add(newCityItem);
            return Json(new {status = "success"});
        }
    }
}