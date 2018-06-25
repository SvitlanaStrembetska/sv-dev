using System;
using System.Collections.Generic;
using Svbase.Core.Data.Abstract;

namespace Svbase.Core.Data.Entities
{
    public class Person : Entity<int>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileTelephoneFirst { get; set; }
        public string MobileTelephoneSecond { get; set; }
        public string Email { get; set; }
        public string StationaryPhone { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public string Position { get; set; }
        public string PartionType { get; set; }
        public bool Gender { get; set; }

        public int? CityId { get; set; }
        public virtual City City{ get; set; }
        public int? StreetId { get; set; }
        public virtual Street Street { get; set; }
        public int? ApartmentId { get; set; }
        public virtual Apartment Apartment { get; set; }
        public virtual ICollection<Flat> Flats { get; set; }
        public virtual ICollection<Beneficiary> Beneficiaries { get; set; }

    }
}