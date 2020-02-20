using System.ComponentModel.DataAnnotations;

namespace Svbase.Core.Enums
{
    public enum DuplicateSearchType
    {
        [Display(Name = "Імені та прізвищу")]
        FirstAndLastName = 1,
        [Display(Name = "Номерах телефонів")]
        PhoneNumber = 2
    }
}
