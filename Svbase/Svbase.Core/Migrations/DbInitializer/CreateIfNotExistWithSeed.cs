using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Consts;

namespace Svbase.Core.Migrations.DbInitializer
{
    class CreateIfNotExistWithSeed
    {
        private const string UserPassword = "Adm!nSvBase";
        private const string EmailDomain = "@svbase.com";
        private const string AdminLastName = "Admin";
        private const string UserLastName = "SvBaseUser";


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

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //public CreateIfNotExistWithSeed(IApplicationBuilder app)
        //{
        //    _dbContext = app.ApplicationServices.GetService<ApplicationDbContext>();
        //    _userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
        //    _roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();
        //}

        private async void InitUsers()
        {
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

                await _userManager.CreateAsync(user, UserPassword);
                await _dbContext.SaveChangesAsync();

                if (Admins.Contains(user.LastName))
                {
                    await _userManager.AddToRoleAsync(user.Id,RoleConst.Admin);
                }
                else if (Users.Contains(user.LastName))
                {
                    await _userManager.AddToRoleAsync(user.Id, RoleConst.User);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
