using System;
using System.Collections.Generic;

namespace Svbase.Models
{
    public class FileValidationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public string MobileTelephoneFirst { get; set; }
        public string MobileTelephoneSecond { get; set; }
        public string StationaryPhone { get; set; }
        public string FlatNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string ApartmentLetter { get; set; }
        public string ApartmentSide { get; set; }
        public string StreetName { get; set; }
        public string CityName { get; set; }
        public string WorkName { get; set; }
        public Dictionary<string, bool> Beneficaries { get; set; }
    }
}