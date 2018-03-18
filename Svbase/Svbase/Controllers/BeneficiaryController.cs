using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    public class BeneficiaryController : GeneralController
    {
        private readonly IBeneficiaryService _beneficiaryService;

        public BeneficiaryController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _beneficiaryService = ServiceManager.BeneficiaryService;
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult List()
        {
            var beneficiaries = _beneficiaryService.GetAllBeneficiaries();
            return View(beneficiaries);
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Create(BeneficiaryCreateModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("", "Invalid Name");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("List");
            }

            var newItem = model.Update(new Beneficiary());
            _beneficiaryService.Add(newItem);
            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    status = "success"
                });
            }
            return RedirectToAction("List");
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpPost]
        public ActionResult Edit(BeneficiaryCreateModel model)
        {
            if (model == null)
            {
                return Json(new { status = "error" });
            }

            var beneficiary = _beneficiaryService.FindById(model.Id);
            if (beneficiary == null)
            {
                return Json(new { status = "error" });
            }
            beneficiary = model.Update(beneficiary);
            _beneficiaryService.Update(beneficiary);
            return Json(new { status = "success" });
        }

        [Authorize(Roles = RoleConsts.Admin)]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _beneficiaryService.DeleteById(id);

            return Json(new { status = "success" });
        }
    }
}