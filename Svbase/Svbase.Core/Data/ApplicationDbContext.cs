using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Svbase.Core.Data.Configuration;
using Svbase.Core.Data.Entities;

namespace Svbase.Core.Data
{
    class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {

        #region DbSet
        public virtual DbSet<Apartment> Apartments { get; set; }
        public virtual DbSet<Beneficiary> Beneficiaries { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Flat> Flats { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Street> Streets { get; set; }
        #endregion

        public ApplicationDbContext()
            : base("DefaultConnection"){}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ApartmentConfiguration());
            modelBuilder.Configurations.Add(new FlatConfiguration());
            modelBuilder.Configurations.Add(new PersonConfiguration());
            modelBuilder.Configurations.Add(new StreetConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
