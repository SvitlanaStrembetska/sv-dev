using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Data.Entities;
using Svbase.Helpers;
using Svbase.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    [Authorize]
    public class ImportController : GeneralController
    {
        private readonly IPersonService _personService;
        private readonly IFlatService _flatService;
        private readonly IApartmentService _apartmentService;
        private readonly IStreetService _streetService;
        private readonly ICityService _cityService;

        public ImportController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
            _flatService = ServiceManager.FlatService;
            _apartmentService = ServiceManager.ApartmentService;
            _streetService = ServiceManager.StreetService;
            _cityService = ServiceManager.CityService;
        }

        public ActionResult UploadDocument()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName("test2.xlsx");
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    file.SaveAs(path);
                }
            }

            return RedirectToAction("UploadDocument");
        }
      
        
          
        [Authorize]
        [ActionName("Importexcel")]
        [HttpPost]
        public JsonResult ImportExcel(IList<HttpPostedFileBase> multipleFiles)
        {
            var files = new List<UploadFilesModel>();
            var errorsList = new List<string>();

            if (!multipleFiles.Any())
                errorsList.Add("Жоден файл не прикріплено!");
            else
            {
                foreach (var file in multipleFiles)
                {
                    if (errorsList.Any())
                    {
                        errorsList.Add("Через попередні помилки наступний файл '" + file.FileName + "' не завантажено!");
                        continue;
                    }
                    if (file.ContentLength == 0)
                    {
                        errorsList.Add("Файл '" + file.FileName + "' порожній");
                        continue;
                    }

                    //check file extension
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    string[] validFileTypes = { ".xls", ".xlsx" };
                    if (!validFileTypes.Contains(extension))
                    {
                        errorsList.Add("Невірне розширення файлу '" + file.FileName + "'! Файл повинен бути в .xls або .xlsx форматі");
                        continue;
                    }

                    //save file to /Content/Uploads
                    string pathToExcelFile = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), file.FileName);
                    if (!Directory.Exists(pathToExcelFile))
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    if (System.IO.File.Exists(pathToExcelFile))
                        System.IO.File.Delete(pathToExcelFile);
                    file.SaveAs(pathToExcelFile);

                    //delete old files
                    DeleteOldFiles(Server.MapPath("~/Content/Uploads"));

                    //parse file
                    var dataTable = ParseFile(extension, pathToExcelFile, GeneralConsts.ShowTableRowsCount, ref errorsList);
                    if (dataTable == null)
                    {
                        errorsList.Add("Не вдалося отримати дані із файлу '" + file.FileName + "'! Спробуйте зберегти файл вказавши тип файлу 'Книга Excel' або 'Книга Excel 97-2003' та знову його завантажити.");
                        continue;
                    }

                    //validate file columns
                    var errorModel = new List<string>();
                    var isFileColumnsNotValid = new FilesDataValidationHelper().ValidateFileColumns(dataTable, file.FileName, ref errorModel);
                    if (isFileColumnsNotValid)
                    {
                        errorsList.AddRange(errorModel);
                        continue;
                    }

                    // generate results
                    var filePreviewErrors = new List<string>();
                    var datalist = ConvertDataTableToDictionary(dataTable, file.FileName, ref filePreviewErrors);
                    if (filePreviewErrors.Any())
                    {
                        errorsList.AddRange(filePreviewErrors);
                        continue;
                    }

                    files.Add(new UploadFilesModel()
                    {
                        FileName = file.FileName,
                        DataList = datalist
                    });
                }

                if (errorsList.Any())
                    foreach (var pathToExcelFile in multipleFiles.Select(file => string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), file.FileName)).Where(pathToExcelFile => System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }

                return errorsList.Any() ? Json(new { status = GeneralConsts.StatusError, errorsList }, JsonRequestBehavior.AllowGet) : Json(new { status = GeneralConsts.StatusSuccess, files }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = GeneralConsts.StatusError, errorsList }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteStageFile(string fileName)
        {
            var pathToExcelFile = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), fileName);

            if (!System.IO.File.Exists(pathToExcelFile))
                return Json(new { status = GeneralConsts.StatusError, message = "Не вдалося видалити файл '" + fileName + "'" });

            System.IO.File.Delete(pathToExcelFile);
            return Json(new { status = GeneralConsts.StatusSuccess }, JsonRequestBehavior.AllowGet);
        }

        private List<Dictionary<string, object>> ConvertDataTableToDictionary(DataTable dataTable, string fileName, ref List<string> errorList)
        {
            var rows = new List<Dictionary<string, object>>();
            var validationHelper = new FilesDataValidationHelper();

            foreach (DataRow dr in dataTable.Rows)
            {
                var validatedModel = validationHelper.ValidateTableRows(dr, dataTable.Rows.IndexOf(dr), fileName, ref errorList);
                if (errorList.Any())
                    continue;

                var row = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.ColumnName.ToUpper() == "ДАТА НАРОДЖЕННЯ" && validatedModel.BirthdayDate != null) 
                        row.Add(col.ColumnName, validatedModel.BirthdayDate.ToString().Trim().Substring(0, 10));
                    else
                        row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            return rows;
        }

        private void DeleteOldFiles(string directoryPath)
        {
            foreach (
                var file in
                    Directory.GetFiles(directoryPath)
                        .Where(file => DateTime.Now - new FileInfo(file).CreationTime >= TimeSpan.FromHours(6)))
            {
                System.IO.File.Delete(file);
            }
        }

        private DataTable ParseFile(string extension, string pathToExcelFile, int showTableRowsCount, ref List<string> errorsList)
        {
            DataTable dataTable = null;
            string connString;

            if (extension.Trim() == ".xls")
            {
                connString =
                    string.Format(
                        "Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;",
                        pathToExcelFile);
                dataTable = ConvertDataHelper.ConvertXslXtoDataTable(pathToExcelFile, connString, showTableRowsCount, ref errorsList);
            }
            else if (extension.Trim() == ".xlsx")
            {
                connString =
                    string.Format(
                        "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";",
                        pathToExcelFile);
                dataTable = ConvertDataHelper.ConvertXslXtoDataTable(pathToExcelFile, connString, showTableRowsCount, ref errorsList);
            }

            return dataTable;
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveFilesDataToDb(IList<string> filesName)
        {
            if (filesName == null) return Json(new { status = GeneralConsts.StatusError, message = "Жоден файл не прикріплено!" });

            var errorsList = new List<string>();
            var successList = new List<string>();

            foreach (var fileName in filesName)
            {
                if (errorsList.Any())
                {
                    errorsList.Add("Через попередні помилки операції з файлом '" + fileName + "' не виконалися");
                    continue;
                }

                var pathToExcelFile = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), fileName);
                if (!System.IO.File.Exists(pathToExcelFile))
                {
                    errorsList.Add("Файл" + filesName + " не завантажено!");
                    continue;
                }

                //parse file
                var extension = Path.GetExtension(fileName).ToLower();
                var dataTable = ParseFile(extension, pathToExcelFile, GeneralConsts.ShowAllTableRowsCount, ref errorsList);
                if (dataTable == null)
                {
                    errorsList.Add("Не вдалося отримати дані із файлу '" + fileName + "'! Спробуйте зберегти файл вказавши тип файлу 'Книга Excel' або 'Книга Excel 97-2003' та знову його завантажити.");
                    continue;
                }

                //validate file columns
                var errorModel = new List<string>();
                var isFileColumnsNotValid = new FilesDataValidationHelper().ValidateFileColumns(dataTable, fileName, ref errorModel);
                if (isFileColumnsNotValid)
                {
                    errorsList.AddRange(errorModel);
                    continue;
                }

                //convert data rows to persons list
                var generalFileRowsErrorList = new List<string>();
                var personList = ConverDataRowsToPersonList(dataTable, fileName, ref generalFileRowsErrorList);
                if (generalFileRowsErrorList.Any())
                {
                    errorsList.AddRange(generalFileRowsErrorList);
                    continue;
                }

                //save persons
                _personService.AddRange(personList);
                successList.Add("Дані з файлу '" + fileName + "' успішно збережені!");
            }

            foreach (var pathToExcelFile in filesName.Select(fileName => string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), fileName)).Where(pathToExcelFile => System.IO.File.Exists(pathToExcelFile)))
            {
                System.IO.File.Delete(pathToExcelFile);
            }

            return errorsList.Any() ? Json(new { status = GeneralConsts.StatusError, errorsList, successList }, JsonRequestBehavior.AllowGet) : Json(new { status = GeneralConsts.StatusSuccess, successList }, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<Person> ConverDataRowsToPersonList(DataTable dataTable, string fileName, ref List<string> generalFileRowsErrorList)
        {
            var personList = new List<Person>();
            var cityList = new List<City>();
            var streetList = new List<Street>();
            var apartmentList = new List<Apartment>();
            var flatList = new List<Flat>();

            var cities = _cityService.GetAll();
            var streets = _streetService.GetAll();
            var apartments = _apartmentService.GetAll();
            var flats = _flatService.GetAll();
            var persons = _personService.GetAll();

            foreach (DataRow row in dataTable.Rows)
            {
                var newErrorList = new List<string>();
                var validatedModel = new FilesDataValidationHelper().ValidateTableRows(row, dataTable.Rows.IndexOf(row), fileName, ref newErrorList);
                if (newErrorList.Any())
                {
                    generalFileRowsErrorList.AddRange(newErrorList);
                    continue;
                }
                if (generalFileRowsErrorList.Any())
                    continue;

                var city = new City { Name = validatedModel.CityName };
                if (!(cities.Any(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper()) || cityList.FindIndex(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper()) >= 0))
                    cityList.Add(city);
                else if (cities.Any(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper()))
                    city = cities.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper());
                else if (cityList.FindIndex(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper()) >= 0)
                    city = cityList.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper());

                var street = new Street { Name = validatedModel.StreetName, City = city };
                if (!(streets.Any(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper() && x.City.Name.ToUpper() == validatedModel.CityName.ToUpper())
                        || streetList.FindIndex(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper() && x.City.Name.ToUpper() == validatedModel.CityName.ToUpper()) >= 0))
                    streetList.Add(street);
                else if (streets.Any(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper() && x.City.Name.ToUpper() == validatedModel.CityName.ToUpper()))
                    street = streets.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper() && x.City.Name.ToUpper() == validatedModel.CityName.ToUpper());
                else if (streetList.FindIndex(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper() && x.City.Name.ToUpper() == validatedModel.CityName.ToUpper()) >= 0)
                    street = streetList.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper() && x.City.Name.ToUpper() == validatedModel.CityName.ToUpper());

                var apartment = new Apartment
                {
                    Name = validatedModel.ApartmentSide != "" ? validatedModel.ApartmentNumber + " " + validatedModel.ApartmentLetter + " корпус:" + validatedModel.ApartmentSide : validatedModel.ApartmentNumber + " " + validatedModel.ApartmentLetter,
                    Street = street
                };
                if (!(apartments.Any(x => x.Name.ToUpper() == apartment.Name.ToUpper()
                    && x.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                    && x.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper()) ||
                    apartmentList.FindIndex(x => x.Name.ToUpper() == apartment.Name.ToUpper()
                    && x.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                    && x.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper()) >= 0))
                    apartmentList.Add(apartment);
                else if (apartments.Any(x => x.Name.ToUpper() == apartment.Name.ToUpper()
                        && x.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                        && x.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper()))
                    apartment = apartments.FirstOrDefault(x => x.Name.ToUpper() == apartment.Name.ToUpper()
                                                               && x.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                                                               && x.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper());
                else if (apartmentList.FindIndex(x => x.Name.ToUpper() == apartment.Name.ToUpper()
                         && x.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                         && x.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper()) >= 0)
                    apartment = apartmentList.FirstOrDefault(x => x.Name.ToUpper() == apartment.Name.ToUpper()
                                                               && x.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                                                               && x.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper());

                var flat = new Flat { Number = validatedModel.FlatNumber, Apartment = apartment };
                if (!(flats.Any(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper() &&
                        x.Apartment.Name.ToUpper() == apartment.Name.ToUpper()
                        && x.Apartment.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                        && x.Apartment.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper()) ||
                        flatList.FindIndex(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper() &&
                        x.Apartment.Name.ToUpper() == apartment.Name.ToUpper()
                        && x.Apartment.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                        && x.Apartment.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper()) >= 0))
                    flatList.Add(flat);
                else if (flats.Any(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper() &&
                                        x.Apartment.Name.ToUpper() == apartment.Name.ToUpper()
                                        && x.Apartment.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                                        &&
                                        x.Apartment.Street.City.Name.ToUpper() ==
                                        apartment.Street.City.Name.ToUpper()))
                    flat = flats.FirstOrDefault(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper() &&
                                                     x.Apartment.Name.ToUpper() == apartment.Name.ToUpper()
                                                     &&
                                                     x.Apartment.Street.Name.ToUpper() ==
                                                     apartment.Street.Name.ToUpper()
                                                     &&
                                                     x.Apartment.Street.City.Name.ToUpper() ==
                                                     apartment.Street.City.Name.ToUpper());
                else if (flatList.FindIndex(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper() &&
                         x.Apartment.Name.ToUpper() == apartment.Name.ToUpper()
                         && x.Apartment.Street.Name.ToUpper() == apartment.Street.Name.ToUpper()
                         && x.Apartment.Street.City.Name.ToUpper() == apartment.Street.City.Name.ToUpper()) >= 0)
                    flat = flatList.FirstOrDefault(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper() &&
                                                     x.Apartment.Name.ToUpper() == apartment.Name.ToUpper()
                                                     &&
                                                     x.Apartment.Street.Name.ToUpper() ==
                                                     apartment.Street.Name.ToUpper()
                                                     &&
                                                     x.Apartment.Street.City.Name.ToUpper() ==
                                                     apartment.Street.City.Name.ToUpper());


                var isPersonAlreadyExistsInList = false;
                var isPersonAlreadyExistsInDb = false;
                var person = new Person()
                {
                    FirstName = validatedModel.FirstName,
                    LastName = validatedModel.LastName,
                    MiddleName = validatedModel.MiddleName,
                    BirthdayDate = validatedModel.BirthdayDate,
                    MobileTelephoneFirst = validatedModel.MobileTelephoneFirst,
                    MobileTelephoneSecond = validatedModel.MobileTelephoneSecond,
                    StationaryPhone = validatedModel.StationaryPhone,
                    Flats = new List<Flat>
                        {
                            flat
                        }
                };

                if (!persons.Any())
                {
                    if (!(personList.Count > 0))
                        personList.Add(person);
                    else
                    {
                        var personFlat = person.Flats.FirstOrDefault();

                        var personsList = new List<Person>();
                        personsList.AddRange(personList);

                        foreach (var personInList in personsList)
                        {
                            var personFlatInList = personInList.Flats.FirstOrDefault();
                            if (personFlat != null && personFlatInList != null &&
                                personInList.FirstName.ToUpper() == person.FirstName.ToUpper()
                                && personInList.LastName.ToUpper() == person.LastName.ToUpper()
                                && personInList.MiddleName.ToUpper() == person.MiddleName.ToUpper()
                                && personInList.BirthdayDate == person.BirthdayDate
                                && personFlatInList.Number.ToUpper() == personFlat.Number.ToUpper()
                                && personFlatInList.Apartment.Name.ToUpper() == personFlat.Apartment.Name.ToUpper()
                                &&
                                personFlatInList.Apartment.Street.Name.ToUpper() ==
                                personFlat.Apartment.Street.Name.ToUpper()
                                &&
                                personFlatInList.Apartment.Street.City.Name.ToUpper() ==
                                personFlat.Apartment.Street.City.Name.ToUpper())
                            {
                                isPersonAlreadyExistsInList = true;
                                break;
                            }
                        }
                        if (!isPersonAlreadyExistsInList)
                            personList.Add(person);
                    }
                }
                else
                {
                    var personFlat = person.Flats.FirstOrDefault();
                    var personsList = new List<Person>();
                    personsList.AddRange(personList);

                    foreach (var personInList in personsList)
                    {
                        var personFlatInList = personInList.Flats.FirstOrDefault();
                        if (personFlat != null && personFlatInList != null &&
                            personInList.FirstName.ToUpper() == person.FirstName.ToUpper()
                            && personInList.LastName.ToUpper() == person.LastName.ToUpper()
                            && personInList.MiddleName.ToUpper() == person.MiddleName.ToUpper()
                            && personInList.BirthdayDate == person.BirthdayDate
                            && personFlatInList.Number.ToUpper() == personFlat.Number.ToUpper()
                            && personFlatInList.Apartment.Name.ToUpper() == personFlat.Apartment.Name.ToUpper()
                            && personFlatInList.Apartment.Street.Name.ToUpper() ==
                            personFlat.Apartment.Street.Name.ToUpper()
                            && personFlatInList.Apartment.Street.City.Name.ToUpper() ==
                            personFlat.Apartment.Street.City.Name.ToUpper())
                        {
                            isPersonAlreadyExistsInList = true;
                            break;
                        }
                    }

                    foreach (var dbPerson in persons.Include(x => x.Flats.Select(y => y.Apartment).Select(y => y.Street).Select(y => y.City)))
                    {
                        foreach (var dbPersonFlat in dbPerson.Flats)
                        {
                            if (personFlat != null &&
                            dbPerson.FirstName.ToUpper() == person.FirstName.ToUpper()
                            && dbPerson.LastName.ToUpper() == person.LastName.ToUpper()
                            && dbPerson.MiddleName.ToUpper() == person.MiddleName.ToUpper()
                            && dbPerson.BirthdayDate == person.BirthdayDate
                            && dbPersonFlat.Number.ToUpper() == personFlat.Number.ToUpper()
                            && dbPersonFlat.Apartment.Name.ToUpper() == personFlat.Apartment.Name.ToUpper()
                            && dbPersonFlat.Apartment.Street.Name.ToUpper() ==
                            personFlat.Apartment.Street.Name.ToUpper()
                            && dbPersonFlat.Apartment.Street.City.Name.ToUpper() ==
                            personFlat.Apartment.Street.City.Name.ToUpper())
                            {
                                isPersonAlreadyExistsInDb = true;
                                break;
                            }
                        }
                    }

                    if (!isPersonAlreadyExistsInList && !isPersonAlreadyExistsInDb)
                        personList.Add(person);
                }
            }
            return personList;
        }
    }
}