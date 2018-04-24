using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Svbase.Controllers.Abstract;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    public class ExportController : GeneralController
    {
        private readonly IPersonService _personService;

        public ExportController(IServiceManager serviceManager) : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
        }

        [Authorize]
        public void ExportExcel(FilterFileImportModel filter)
        {
            IQueryable<PersonSelectionModel> persons; 
            if (filter == null || (filter.DistrictIds == null && filter.CityIds == null && filter.StreetIds == null && filter.ApartmentIds == null && filter.FlatIds == null)) { 
                var pers = _personService.GetAll();
                persons = from person in pers
                    let personFlat = person.Flats.FirstOrDefault()
                    select new PersonSelectionModel
                    {

                        Id = person.Id,
                        FirstName = person.FirstName,
                        MiddleName = person.MiddleName,
                        LastName = person.LastName,
                        DateBirth = person.BirthdayDate,
                        Gender = person.Gender,
                        Position = person.Position,
                        FirstMobilePhone = person.MobileTelephoneFirst,
                        SecondMobilePhone = person.MobileTelephoneSecond,
                        HomePhone = person.StationaryPhone,
                        Email = person.Email,
                        PartionType = person.PartionType,
                        City = new BaseViewModel
                        {
                            Name = personFlat.Apartment.Street.City.Name
                        },
                        Street = new BaseViewModel
                        {
                            Name = personFlat.Apartment.Street.Name
                        },
                        Apartment = new BaseViewModel
                        {
                            Name = personFlat.Apartment.Name
                        },
                        Flat = new BaseViewModel
                        {
                            Name = personFlat.Number
                        }
                    };
            }
            else
                persons = _personService.SearchPersonsByFilter(filter);

            if (!persons.Any()) return;

            var isColumnLastNameExists = filter.ColumnsName.Contains("Прізвище");
            var isColumnFirstNameExists = filter.ColumnsName.Contains("Ім'я");
            var isColumnMiddleNameExists = filter.ColumnsName.Contains("По батькові");
            var isColumnAdressExists = filter.ColumnsName.Contains("Адреса");
            var isColumnFirstMobilePhoneExists = filter.ColumnsName.Contains("Телефон 1");
            var dataTable = new DataTable();

            if (isColumnLastNameExists)
                dataTable.Columns.Add("Прізвище", typeof(string));
            if (isColumnFirstNameExists)
                dataTable.Columns.Add("Ім'я", typeof(string));
            if (isColumnMiddleNameExists)
                dataTable.Columns.Add("По батькові", typeof(string));
            if (isColumnAdressExists)
            {
                dataTable.Columns.Add("Населений пункт", typeof(string));
                dataTable.Columns.Add("Вулиця", typeof(string));
                dataTable.Columns.Add("Будинок", typeof(string));
                dataTable.Columns.Add("Квартира", typeof(string));
                dataTable.Columns.Add("Буква", typeof(string));
                dataTable.Columns.Add("Корпус", typeof(string));
            }
            if (isColumnFirstMobilePhoneExists)
                dataTable.Columns.Add("Телефон 1", typeof(string));

            foreach (var person in persons)
            {
                var row = dataTable.NewRow();
                if (isColumnLastNameExists)
                    row["Прізвище"] = person.LastName;
                if (isColumnFirstNameExists)
                    row["Ім'я"] = person.FirstName;
                if (isColumnMiddleNameExists)
                    row["По батькові"] = person.MiddleName;
                if (isColumnAdressExists)
                {
                    row["Населений пункт"] = person.City.Name;
                    row["Вулиця"] = person.Street.Name;
                    row["Квартира"] = person.Flat.Name;

                    var apartment = person.Apartment?.Name?.Split(' ') ?? new string[] {};
                    switch (apartment.Length)
                    {
                        case 1:
                            row["Будинок"] = apartment[0];
                            row["Буква"] = "";
                            row["Корпус"] = "";
                            break;
                        case 2:
                            row["Будинок"] = apartment[0];
                            row["Буква"] = apartment[1];
                            row["Корпус"] = "";
                            break;
                        case 3:
                            row["Будинок"] = apartment[0];
                            row["Буква"] = apartment[1];
                            row["Корпус"] = apartment[2].Split(':')[1];
                            break;
                    }
                }
                if (isColumnFirstMobilePhoneExists)
                    row["Телефон 1"] = person.FirstMobilePhone;

                dataTable.Rows.Add(row);
            }

            if (dataTable.Columns.Count == 0) return;

            var grid = new GridView { DataSource = dataTable };
            grid.DataBind();

            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment; filename=Product.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            var sw = new StringWriter();
            var htmlTextWriter = new HtmlTextWriter(sw);

            grid.RenderControl(htmlTextWriter);

            Response.Output.Write(sw.ToString());
            Response.End();
        }
    }
}