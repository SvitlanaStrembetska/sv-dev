using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svbase.Core.Models
{
    public class PersonSearchModel
    {
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }
        public bool IsFirstNameIncludedInSearch { get; set; }


        [Display(Name = "Прізвище")]
        public string LastName { get; set; }
        public bool IsLastNameIncludedInSearch { get; set; }


        [Display(Name = "По батькові")]
        public string MiddleName { get; set; }
        public bool IsMiddleNameIncludedInSearch { get; set; }


        [Phone]
        [Display(Name = "Мобільний телефон")]
        public string MobilePhone { get; set; }
        public bool IsMobilePhoneIncludedInSearch { get; set; }

        public IEnumerable<int> ColumnsIds { get; set; }
    }
}
