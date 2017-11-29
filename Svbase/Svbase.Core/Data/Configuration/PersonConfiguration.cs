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
        }
    }
}
