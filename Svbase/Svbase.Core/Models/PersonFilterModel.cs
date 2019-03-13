using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Svbase.Core.Enums;

namespace Svbase.Core.Models
{
    public class PersonFilterModel
    {
        //[Display(Name = "Початкова дата")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        //[Display(Name = "Кінцева дата")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDateMonth { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDateMonth { get; set; }
    }

    public class PersonFilterViewModel:PersonFilterModel
    {
        [Display(Name = "Категорії")]
        public IEnumerable<SelectListItem> Beneficiaries { get; set; }
        [Display(Name = "Округи")]
        public IEnumerable<SelectListItem> Districts { get; set; }
        public IEnumerable<ColumnName> FirstSortOrder { get; set; }
        public IEnumerable<ColumnName> SecondSortOrder { get; set; }
        public IEnumerable<ColumnName> ThirdSortOrder { get; set; }
    }
}
