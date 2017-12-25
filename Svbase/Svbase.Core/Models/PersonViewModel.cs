using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Svbase.Core.Data.Entities;

namespace Svbase.Core.Models
{
    public class PersonListModel
    {
        public int Id { get; set; }
        [Display(Name = "FirthName")]
        public string FirthName { get; set; }
        [Display(Name = "MiddleName")]
        public string MiddleName { get; set; }
        [Display(Name = "LastName")]
        public string LastName { get; set; }
    }

    public class PersonViewModel  : PersonListModel
    {
        [Phone]
        public string FirthtMobilePhone { get; set; }
        [Phone]
        public string SecondMobilePhone { get; set; }
        [Phone]
        public string HomePhone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime DateBirth { get; set; }
        public string Position { get; set; }
        public string PartionType { get; set; }
        public bool Gender { get; set; }

        public int BeneficiaryId { get; set; }
        public int DistrictId { get; set; }
        public int FlatId { get; set; }
    }
}
