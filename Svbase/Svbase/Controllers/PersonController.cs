using System.Data.Entity.Infrastructure.Annotations;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Service.Factory;

namespace Svbase.Controllers
{
    public class PersonController : GeneralController
    {
        public PersonController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult SearcResult( )
        {
            return PartialView();
        }

        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Export()
        {
            return View();
        }
    }

}