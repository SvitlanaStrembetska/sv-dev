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
        private readonly ICityService _cityService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IWorkService _workService;

        public ImportController(IServiceManager serviceManager)
            : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
            _cityService = ServiceManager.CityService;
            _beneficiaryService = ServiceManager.BeneficiaryService;
            _workService = ServiceManager.WorkService;
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
            var beneficaries = _beneficiaryService.GetAll().ToList();

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
                    var dataTable = ParseFile(extension, pathToExcelFile, Consts.ShowTableRowsCount, ref errorsList);
                    if (dataTable == null)
                    {
                        errorsList.Add("Не вдалося отримати дані із файлу '" + file.FileName + "'! Спробуйте зберегти файл вказавши тип файлу 'Книга Excel' або 'Книга Excel 97-2003' та знову його завантажити.");
                        continue;
                    }

                    //validate file columns
                    var errorModel = new List<string>();
                    var isFileColumnsNotValid = new FilesDataValidationHelper().ValidateFileColumns(dataTable, file.FileName, beneficaries, ref errorModel);
                    if (isFileColumnsNotValid)
                    {
                        errorsList.AddRange(errorModel);
                        continue;
                    }

                    // generate results
                    var filePreviewErrors = new List<string>();
                    var datalist = ConvertDataTableToDictionary(dataTable, file.FileName, beneficaries, ref filePreviewErrors);
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

                return errorsList.Any() ? Json(new { status = Consts.StatusError, errorsList }, JsonRequestBehavior.AllowGet) : Json(new { status = Consts.StatusSuccess, files }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = Consts.StatusError, errorsList }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteStageFile(string fileName)
        {
            var pathToExcelFile = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), fileName);

            if (!System.IO.File.Exists(pathToExcelFile))
                return Json(new { status = Consts.StatusError, message = "Не вдалося видалити файл '" + fileName + "'" });

            System.IO.File.Delete(pathToExcelFile);
            return Json(new { status = Consts.StatusSuccess }, JsonRequestBehavior.AllowGet);
        }

        private List<Dictionary<string, object>> ConvertDataTableToDictionary(DataTable dataTable, string fileName, IEnumerable<Beneficiary> beneficaries, ref List<string> errorList)
        {
            var rows = new List<Dictionary<string, object>>();
            var validationHelper = new FilesDataValidationHelper();

            foreach (DataRow dr in dataTable.Rows)
            {
                var validatedModel = validationHelper.ValidateTableRows(dr, dataTable.Rows.IndexOf(dr), fileName, beneficaries, ref errorList);
                if (errorList.Any())
                    continue;

                var row = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.ColumnName.ToUpper() == "ДАТА НАРОДЖЕННЯ" && validatedModel.BirthdayDate != null) 
                        row.Add(col.ColumnName, validatedModel.BirthdayDate.ToString().Trim().Substring(0, 10));
                    else if (beneficaries.Any(x => x.Name == col.ColumnName))
                        row.Add(col.ColumnName, dr[col].ToString().ToUpper() == "TRUE");
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
            if (filesName == null) return Json(new { status = Consts.StatusError, message = "Жоден файл не прикріплено!" });

            var beneficaries = _beneficiaryService.GetAll().ToList();
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
                var dataTable = ParseFile(extension, pathToExcelFile, Consts.ShowAllTableRowsCount, ref errorsList);
                if (dataTable == null)
                {
                    errorsList.Add("Не вдалося отримати дані із файлу '" + fileName + "'! Спробуйте зберегти файл вказавши тип файлу 'Книга Excel' або 'Книга Excel 97-2003' та знову його завантажити.");
                    continue;
                }

                //validate file columns
                var errorModel = new List<string>();
                var isFileColumnsNotValid = new FilesDataValidationHelper().ValidateFileColumns(dataTable, fileName, beneficaries, ref errorModel);
                if (isFileColumnsNotValid)
                {
                    errorsList.AddRange(errorModel);
                    continue;
                }

                //convert data rows to persons list
                var generalFileRowsErrorList = new List<string>();
                var personList = ConverDataRowsToPersonList(dataTable, fileName, beneficaries, ref generalFileRowsErrorList);
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

            return errorsList.Any() ? Json(new { status = Consts.StatusError, errorsList, successList }, JsonRequestBehavior.AllowGet) : Json(new { status = Consts.StatusSuccess, successList }, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<Person> ConverDataRowsToPersonList(DataTable dataTable, string fileName, IEnumerable<Beneficiary> beneficaries, ref List<string> generalFileRowsErrorList)
        {
            var personList = new List<Person>();
            var cityList = new List<City>();
            var streetList = new List<Street>();
            var apartmentList = new List<Apartment>();
            var flatList = new List<Flat>();
            var workList = new List<Work>();

            var dbCities = _cityService.GetAll().ToList();
            var dbWorks = _workService.GetAll().ToList();

            var fileDataValidationHelper = new FilesDataValidationHelper();
            var date = DateTime.Now;
            foreach (DataRow row in dataTable.Rows)
            {
                var newErrorList = new List<string>();

                var validatedModel = fileDataValidationHelper.ValidateTableRows(row, dataTable.Rows.IndexOf(row), fileName, beneficaries, ref newErrorList);
                if (newErrorList.Any())
                {
                    generalFileRowsErrorList.AddRange(newErrorList);
                    continue;
                }
                if (generalFileRowsErrorList.Any())
                    continue;

                var city = new City();
                var cityFromDb = new City();
                var street = new Street();
                var streetFromDb = new Street();
                var apartment = new Apartment();
                var apartmentFromDb = new Apartment();
                var flat = new Flat();
                var flatFromDb = new Flat();
                
                // city
                if (dbCities.Any(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper()))
                    cityFromDb = dbCities.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper());
                else if (cityList.Any(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper()))
                    city = cityList.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.CityName.ToUpper());
                else
                {
                    city = new City {Name = validatedModel.CityName};
                    cityList.Add(city);
                }

                // street
                if (cityFromDb != null && cityFromDb.Id != 0 && cityFromDb.Streets.Any(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper()))
                    streetFromDb = cityFromDb.Streets.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper());
                else if (streetList.Any(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper() && x.City.Name.ToUpper() == validatedModel.CityName.ToUpper()))
                    street = streetList.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.StreetName.ToUpper() && x.City.Name.ToUpper() == validatedModel.CityName.ToUpper());
                else
                {
                    street = cityFromDb != null && cityFromDb.Id != 0 ? new Street {Name = validatedModel.StreetName, City = cityFromDb} : new Street { Name = validatedModel.StreetName, City = city };
                    streetList.Add(street);
                }

                var apartmentName = validatedModel.ApartmentSide != ""
                    ? validatedModel.ApartmentNumber + " " + validatedModel.ApartmentLetter + " корпус:" + validatedModel.ApartmentSide
                    : (validatedModel.ApartmentNumber + " " + validatedModel.ApartmentLetter).Trim();
                // apartment
                if (streetFromDb != null && streetFromDb.Id != 0 && streetFromDb.Apartments.Any(x => x.Name.ToUpper() == apartmentName.ToUpper()))
                    apartmentFromDb = streetFromDb.Apartments.FirstOrDefault(x => x.Name.ToUpper() == apartmentName.ToUpper());
                else if (apartmentList.Any(x => x.Name.ToUpper() == apartmentName.ToUpper() 
                                        && x.Street.Name.ToUpper() == validatedModel.StreetName.ToUpper()
                                        && x.Street.City.Name.ToUpper() == validatedModel.CityName.ToUpper()))
                    apartment = apartmentList.FirstOrDefault(x => x.Name.ToUpper() == apartmentName.ToUpper()
                                        && x.Street.Name.ToUpper() == validatedModel.StreetName.ToUpper()
                                        && x.Street.City.Name.ToUpper() == validatedModel.CityName.ToUpper());
                else
                {
                    apartment = streetFromDb != null && streetFromDb.Id != 0
                        ? new Apartment
                        {
                            Name = apartmentName,
                            Street = streetFromDb
                        }
                        : new Apartment
                        {
                            Name = apartmentName,
                            Street = street
                        };
                    apartmentList.Add(apartment);
                }

                // flat
                if (apartmentFromDb != null && apartmentFromDb.Id != 0 && apartmentFromDb.Flats.Any(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper()))
                    flatFromDb = apartmentFromDb.Flats.FirstOrDefault(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper());
                else if (flatList.Any(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper()
                                           && x.Apartment.Name.ToUpper() == apartmentName.ToUpper()
                                           && x.Apartment.Street.Name.ToUpper() == validatedModel.StreetName.ToUpper()
                                           &&
                                           x.Apartment.Street.City.Name.ToUpper() == validatedModel.CityName.ToUpper()))
                    flat = flatList.FirstOrDefault(x => x.Number.ToUpper() == validatedModel.FlatNumber.ToUpper()
                                                        && x.Apartment.Name.ToUpper() == apartmentName.ToUpper()
                                                        && x.Apartment.Street.Name.ToUpper() == validatedModel.StreetName.ToUpper()
                                                        && x.Apartment.Street.City.Name.ToUpper() == validatedModel.CityName.ToUpper());
                else
                {
                    flat = apartmentFromDb != null && apartmentFromDb.Id != 0 ? new Flat { Number = validatedModel.FlatNumber, Apartment = apartmentFromDb } : new Flat { Number = validatedModel.FlatNumber, Apartment = apartment };
                    flatList.Add(flat);
                }

                var beneficariesList = new List<Beneficiary>();
                foreach (var beneficary in beneficaries)
                {
                    if (validatedModel.Beneficaries[beneficary.Name].ToString().ToUpper() == "TRUE")
                        beneficariesList.Add(beneficary);
                }

                Work workingPlace;
                if (dbWorks.Any(x => x.Name.ToUpper() == validatedModel.WorkName.ToUpper()))
                    workingPlace = dbWorks.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.WorkName.ToUpper());
                else if (workList.Any(x => x.Name.ToUpper() == validatedModel.WorkName.ToUpper()))
                    workingPlace = workList.FirstOrDefault(x => x.Name.ToUpper() == validatedModel.WorkName.ToUpper());
                else
                {
                    workingPlace = new Work {Name = validatedModel.WorkName};
                    workList.Add(workingPlace);
                }
                
                var person = new Person
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
                            flatFromDb != null && flatFromDb.Id != 0 ? flatFromDb : flat
                        },
                        Beneficiaries = beneficariesList,
                        Work = workingPlace,
                        Email = validatedModel.Email
                    };

                personList.Add(person);

            }
            var date2 = DateTime.Now;
            var res = date2 - date;
            
            return personList;
        }
    }
}