using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    [Authorize]
    public class DashboardController : GeneralController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _dashboardService = ServiceManager.DashboardService;
        }
        public ActionResult Index()
        {
            var dashboardViewModel = _dashboardService.GetDashboardModel();
            return View(dashboardViewModel);
        }
        public ActionResult Details()
        {
            var dashboardViewModel = _dashboardService.GetDashboardViewModel();
            return View(dashboardViewModel);
        }

        public ActionResult PersonsByBeneficiariesId(int id)
        {
            return RedirectToAction("PersonsByBeneficiaryId", "Person", new { id });
        }
    }
}