using Svbase.Core.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Svbase.Core.Data.Configuration
{
    public class FlatConfiguration : EntityTypeConfiguration<Flat>
    {
        public FlatConfiguration()
        {
            ToTable("Flat");
            HasKey(x => x.Id);

            HasRequired(x => x.Apartment)
                .WithMany(x => x.Flats)
                .HasForeignKey(x => x.ApartmentId)
                .WillCascadeOnDelete(true);
        }
    }
}
