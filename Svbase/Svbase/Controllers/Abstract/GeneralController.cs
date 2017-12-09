using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity.Owin;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;
using Svbase.Core.Repositories.Abstract;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers.Abstract
{
    public abstract class GeneralController : Controller
    {
        private HttpContextBase _context;
        private ApplicationUserManager _userManager;
        private ApplicationUser _currentUser;
        private ApplicationSignInManager _signInManager;

        protected SystemRole UserRole;
        protected IUnitOfWork UnitOfWork;
        public IServiceManager ServiceManager;

        protected IApplicationUserService ApplicationUserService;

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser;
                }
                var identity = _context.User.Identity;
                _currentUser = ApplicationUserService.GetByUserName(identity.Name);

                return _currentUser;
            }
            set { _currentUser = value; }
        }

        protected GeneralController(IServiceManager serviceManager)
        {
            ServiceManager = serviceManager;
            ApplicationUserService = ServiceManager.ApplicationUserService;
        }

        protected override void Initialize(RequestContext rc)
        {
            _context = rc.HttpContext;
            base.Initialize(rc);
        }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                UserRole = GetUserRole(context.HttpContext.User);
                ValidateAccessToSytem(username);
            }
            base.OnActionExecuting(context);
        }


        private SystemRole GetUserRole(IPrincipal principal)
        {
            if (principal.IsInRole(RoleConsts.Admin))
            {
                return SystemRole.Admin;
            }
            if (principal.IsInRole(RoleConsts.User))
            {
                return SystemRole.User;
            }

            throw new InvalidOperationException("User can't be without role!");
        }

        private void ValidateAccessToSytem(string userName)
        {
            var canAccess = ApplicationUserService.CanAccessToSystem(userName);
            if (!canAccess)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
            }
        }
    }
}