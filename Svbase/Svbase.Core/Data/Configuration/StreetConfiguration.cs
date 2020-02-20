﻿using Svbase.Core.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Svbase.Core.Data.Configuration
{
    public class StreetConfiguration : EntityTypeConfiguration<Street>
    {
        public StreetConfiguration()
        {
            ToTable("Street");
            HasKey(x => x.Id);

            HasRequired(x => x.City)
                .WithMany(x => x.Streets)
                .HasForeignKey(x => x.CityId)
                .WillCascadeOnDelete(true);


        }
    }
}
