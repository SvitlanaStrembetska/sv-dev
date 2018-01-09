using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Consts;
using System.Data.Entity;

namespace Svbase.Core.Migrations.DbInitializer
{
    public class CreateIfNotExistWithSeed : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        private const string UserPassword = "Adm!nSvBase";
        private const string EmailDomain = "@svbase.com";
        private const string AdminLastName = "Admin";
        private const string UserLastName = "User";


        private static readonly string[] Admins =
        {
            AdminLastName
        };

        private static readonly string[] Users =
        {
            UserLastName
        };

        private static readonly string[][] Profiles =
        {
            new [] {"Admin", AdminLastName},
            new [] {"User", UserLastName }
        };

        private ApplicationDbContext _dbContext;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        protected override void Seed(ApplicationDbContext context)
        {
            InitializeDb(context);
        }

        //public void InitializeDb(ApplicationDbContext context)
        //{
        //    _dbContext = context;
        //    _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        //    _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    if (!_dbContext.Roles.Any())
        //    {
        //        InitUserAndRoles();
        //    }
        //}

        public static void InitializeDb(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                InitUserAndRoles(context);
            }
        }

        //private void InitUserAndRoles()
        //{
        //    var roleResult = InitRoles();
        //    roleResult.ContinueWith(x => InitUsers());
        //}

        private static void InitUserAndRoles(ApplicationDbContext context)
        {
            InitRoles(context);
            InitUsers(context);
        }

        //private async Task InitRoles()
        //{
        //    var roles = RoleConsts.GetAllRoles();
        //    foreach (var roleName in roles)
        //    {
        //        var role = new IdentityRole(roleName);
        //        await _roleManager.CreateAsync(role);
        //    }
        //    await _dbContext.SaveChangesAsync();
        //}

        private static void InitRoles(ApplicationDbContext context)
        {
            var roles = RoleConsts.GetAllRoles();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            foreach (var roleName in roles)
            {
                var role = new IdentityRole(roleName);
                roleManager.Create(role);
            }
            context.SaveChanges();
        }

        //private async void InitUsers()
        //{
        //    foreach (var profile in Profiles)
        //    {
        //        var email = profile[1] + EmailDomain;
        //        var user = new ApplicationUser
        //        {
        //            FirstName = profile[0],
        //            LastName = profile[1],
        //            Email = email,
        //            UserName = email,
        //            EmailConfirmed = true,
        //        };

        //        await _userManager.CreateAsync(user, UserPassword);
        //        await _dbContext.SaveChangesAsync();

        //        if (Admins.Contains(user.LastName))
        //        {
        //            await _userManager.AddToRoleAsync(user.Id, RoleConsts.Admin);
        //        }
        //        else if (Users.Contains(user.LastName))
        //        {
        //            await _userManager.AddToRoleAsync(user.Id, RoleConsts.User);
        //        }
        //    }
        //    await _dbContext.SaveChangesAsync();
        //}

        private static void InitUsers(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (var profile in Profiles)
            {
                var email = profile[1] + EmailDomain;
                var user = new ApplicationUser
                {
                    FirstName = profile[0],
                    LastName = profile[1],
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true,
                };

                userManager.Create(user, UserPassword);
                context.SaveChanges();

                if (Admins.Contains(user.LastName))
                {
                    userManager.AddToRole(user.Id, RoleConsts.Admin);
                }
                else if (Users.Contains(user.LastName))
                {
                    userManager.AddToRole(user.Id, RoleConsts.User);
                }
            }
            context.SaveChanges();
        }
    }
}
