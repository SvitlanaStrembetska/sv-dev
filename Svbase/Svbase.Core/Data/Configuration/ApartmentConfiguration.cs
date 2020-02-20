using Svbase.Core.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Svbase.Core.Data.Configuration
{
    class ApartmentConfiguration : EntityTypeConfiguration<Apartment>
    {
        public ApartmentConfiguration()
        {
            ToTable("Apartment");
            HasKey(x => x.Id);

            HasRequired(x => x.Street)
                .WithMany(x => x.Apartments)
                .HasForeignKey(x => x.StreetId)
                .WillCascadeOnDelete(true);

            HasMany(x => x.Districts)
                .WithMany(x => x.Apartments)
                .Map(t => t.MapLeftKey("ApartmentId")
                    .MapRightKey("DistrictId")
                    .ToTable("ApartmentDistrict"));
        }
    }
}
