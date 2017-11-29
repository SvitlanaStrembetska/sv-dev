using System;
using System.Collections.Generic;
using Svbase.Core.Data.Abstract;

namespace Svbase.Core.Data.Entities
{
    public class Person : Entity<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string MobileTelephoneFirst { get; set; }
        public string MobileTelephoneSecond { get; set; }
        public string StationaryPhone { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string Position { get; set; }
        public int Gender { get; set; }

        public virtual ICollection<Flat> Flats { get; set; }
        public virtual ICollection<Beneficiary> Beneficiaries { get; set; }

    }
}