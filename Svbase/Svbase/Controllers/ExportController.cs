using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Svbase.Controllers.Abstract;
using Svbase.Core.Data.Entities;
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
            if(filter?.ColumnsName == null) return;

            List<PersonSelectionModel> persons = new List<PersonSelectionModel>(); 
            if ((filter.DistrictIds == null && filter.CityIds == null && filter.StreetIds == null && filter.ApartmentIds == null && filter.FlatIds == null)) { 
                var pers = _personService.GetAll().Include(x=>x.Beneficiaries).Include(x=>x.Flats.Select(y=>y.Apartment).Select(z=>x.Street).Select(k=>k.City)).Include(x=>x.Work).Include(x=>x.Apartment);
                
                foreach (var person in pers)
                {
                    var beneficaries = person.Beneficiaries.ToList();
                    var personFlat = person.Flats.FirstOrDefault();
                    var newPerson = new PersonSelectionModel
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
                        BeneficariesList = beneficaries,
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
                        },
                        Work = new Work
                        {
                            Name = person.Work.Name
                        }
                    };
                    persons.Add(newPerson);
                }
            }
            else
                persons = _personService.SearchPersonsByFilter(filter).ToList();

            //if beneficaries exists 
            var personsList = new List<PersonSelectionModel>();
            if (filter.BeneficariesUnchecked != null && filter.BeneficariesUnchecked.Any())
            {
                personsList.AddRange(persons.Where(person => !filter.BeneficariesUnchecked.Any(column => person.BeneficariesList != null && person.BeneficariesList.Any(x => x.Name.ToUpper() == column.ToUpper()))));
            }

            //if empty result
            if ((filter.BeneficariesUnchecked != null && filter.BeneficariesUnchecked.Any() && !personsList.Any()) || !persons.Any()) return;

            //generate file header
            var isColumnLastNameExists = filter.ColumnsName.Contains("Прізвище");
            var isColumnFirstNameExists = filter.ColumnsName.Contains("Ім'я");
            var isColumnMiddleNameExists = filter.ColumnsName.Contains("По батькові");
            var isColumnFirstMobilePhoneExists = filter.ColumnsName.Contains("Мобільний телефон 1");
            var isColumnSecondMobilePhoneExists = filter.ColumnsName.Contains("Мобільний телефон 2");
            var isColumnHomePhoneExists = filter.ColumnsName.Contains("Домашній номер телефону");
            var isColumnDateBirthExists = filter.ColumnsName.Contains("Дата народження");
            var isColumnEmailExists = filter.ColumnsName.Contains("Електронна пошта");
            var isColumnAdressExists = filter.ColumnsName.Contains("Адреса");
            var isColumnWorkExists = filter.ColumnsName.Contains("Місце роботи");
            var dataTable = new DataTable();

            if (isColumnLastNameExists)
                dataTable.Columns.Add("Прізвище", typeof(string));
            if (isColumnFirstNameExists)
                dataTable.Columns.Add("Ім'я", typeof(string));
            if (isColumnMiddleNameExists)
                dataTable.Columns.Add("По батькові", typeof(string));
            if (isColumnFirstMobilePhoneExists)
                dataTable.Columns.Add("Телефон 1", typeof(string));
            if (isColumnSecondMobilePhoneExists)
                dataTable.Columns.Add("Телефон 2", typeof(string));
            if (isColumnHomePhoneExists)
                dataTable.Columns.Add("Стаціонарний", typeof(string));
            if (isColumnDateBirthExists)
                dataTable.Columns.Add("Дата народження", typeof(string));
            if (isColumnEmailExists)
                dataTable.Columns.Add("Емейл", typeof(string));
            if (isColumnAdressExists)
            {
                dataTable.Columns.Add("Населений пункт", typeof(string));
                dataTable.Columns.Add("Вулиця", typeof(string));
                dataTable.Columns.Add("Будинок", typeof(string));
                dataTable.Columns.Add("Квартира", typeof(string));
                dataTable.Columns.Add("Буква", typeof(string));
                dataTable.Columns.Add("Корпус", typeof(string));
            }
            if (isColumnWorkExists)
                dataTable.Columns.Add("Місце роботи", typeof(string));

            foreach (var beneficaryName in filter.BeneficariesChecked)
                dataTable.Columns.Add(beneficaryName, typeof(string));

            //generate table header and body if filter contains beneficaries 
            if (filter.BeneficariesUnchecked != null && filter.BeneficariesUnchecked.Any() && personsList.Any())
            {
                foreach (var person in personsList)
                {
                    dataTable.Rows.Add(fillRow(dataTable.NewRow(), person, isColumnLastNameExists, isColumnFirstNameExists, isColumnMiddleNameExists,
                        isColumnFirstMobilePhoneExists, isColumnSecondMobilePhoneExists, isColumnHomePhoneExists,
                        isColumnDateBirthExists, isColumnEmailExists, isColumnWorkExists, isColumnAdressExists, filter.BeneficariesChecked));
                }
            }
            else
            {
                foreach (var person in persons)
                {
                    dataTable.Rows.Add(fillRow(dataTable.NewRow(), person, isColumnLastNameExists, isColumnFirstNameExists, isColumnMiddleNameExists,
                        isColumnFirstMobilePhoneExists, isColumnSecondMobilePhoneExists, isColumnHomePhoneExists,
                        isColumnDateBirthExists, isColumnEmailExists, isColumnWorkExists, isColumnAdressExists, filter.BeneficariesChecked));
                }
            }

            if (dataTable.Columns.Count == 0) return;

            var grid = new GridView { DataSource = dataTable };
            grid.DataBind();

            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment; filename=Persons.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            var sw = new StringWriter();
            var htmlTextWriter = new HtmlTextWriter(sw);

            grid.RenderControl(htmlTextWriter);

            Response.Output.Write(sw.ToString());
            Response.End();
        }

        private DataRow fillRow(DataRow row, PersonSelectionModel person, bool isColumnLastNameExists, bool isColumnFirstNameExists,
            bool isColumnMiddleNameExists, bool isColumnFirstMobilePhoneExists, bool isColumnSecondMobilePhoneExists,
            bool isColumnHomePhoneExists, bool isColumnDateBirthExists, bool isColumnEmailExists, bool isColumnWorkExists,
            bool isColumnAdressExists, IEnumerable<string> beneficariesChecked)
        {
            if (isColumnLastNameExists)
                row["Прізвище"] = person.LastName;
            if (isColumnFirstNameExists)
                row["Ім'я"] = person.FirstName;
            if (isColumnMiddleNameExists)
                row["По батькові"] = person.MiddleName;
            if (isColumnFirstMobilePhoneExists)
                row["Телефон 1"] = person.FirstMobilePhone;
            if (isColumnSecondMobilePhoneExists)
                row["Телефон 2"] = person.SecondMobilePhone;
            if (isColumnHomePhoneExists)
                row["Стаціонарний"] = person.HomePhone;
            if (isColumnDateBirthExists)
                row["Дата народження"] = person.DateBirth;
            if (isColumnEmailExists)
                row["Емейл"] = person.Email;
            if (isColumnWorkExists)
                row["Місце роботи"] = person.Work.Name;

            if (isColumnAdressExists)
            {
                row["Населений пункт"] = person.City.Name;
                row["Вулиця"] = person.Street.Name;
                row["Квартира"] = person.Flat.Name;

                var apartment = person.Apartment?.Name?.Split(' ') ?? new string[] { };
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

            foreach (var beneficaryName in beneficariesChecked)
                row[beneficaryName] = person.BeneficariesList.Any(x => x.Name.ToUpper() == beneficaryName.ToUpper());

            return row;
        }
    }
}