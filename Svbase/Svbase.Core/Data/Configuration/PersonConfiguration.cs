using System.Data.Entity.ModelConfiguration;
using Svbase.Core.Data.Entities;

namespace Svbase.Core.Data.Configuration
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            ToTable("Person");
            HasKey(x => x.Id);

            HasMany(x => x.Flats)
                .WithMany(x => x.Persons)
                .Map(t => t.MapLeftKey("PersonId")
                    .MapRightKey("FlatId")
                    .ToTable("PersonFlat"));

            HasMany(x => x.Beneficiaries)
                .WithMany(x => x.Persons)
                .Map(t => t.MapLeftKey("PersonId")
                    .MapRightKey("BeneficiaryId")
                    .ToTable("PersonBeneficiary"));

            HasOptional(x => x.City)
                .WithMany(x => x.Persons)
                .HasForeignKey(x => x.CityId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Street)
                .WithMany(x => x.Persons)
                .HasForeignKey(x => x.StreetId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Apartment)
                .WithMany(x => x.Persons)
                .HasForeignKey(x => x.ApartmentId)
                .WillCascadeOnDelete(false);
        }
    }
}
