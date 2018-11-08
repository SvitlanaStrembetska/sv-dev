﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using OfficeOpenXml;
using Svbase.Controllers.Abstract;
using Svbase.Core.Consts;
using Svbase.Core.Models;
using Svbase.Service.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Controllers
{
    public class ExportController : GeneralController
    {
        private readonly IPersonService _personService;
        private readonly IBeneficiaryService _beneficiaryService;

        public ExportController(IServiceManager serviceManager) : base(serviceManager)
        {
            _personService = ServiceManager.PersonService;
            _beneficiaryService = ServiceManager.BeneficiaryService;
        }

        [Authorize]
        public void ExportToExcel(FilterFileImportModel filter)
        {
            var pck = new ExcelPackage();
            var ws = pck.Workbook.Worksheets.Add("Люди");
            var isDataFind = false;

            if (filter?.ColumnsIds != null)
            {
                List<PersonSelectionModel> persons;
                if (filter.DistrictIds == null && filter.CityIds == null && filter.StreetIds == null &&
                     filter.ApartmentIds == null && filter.FlatIds == null)
                {
                    persons = _personService.GetAll().Include(x => x.Beneficiaries)
                        .Include(x => x.Flats.Select(y => y.Apartment).Select(z => z.Street).Select(k => k.City))
                        .Include(x => x.Work)
                        .Include(x => x.Apartment).Select(x => new PersonSelectionModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.LastName,
                            DateBirth = x.BirthdayDate,
                            Gender = x.Gender,
                            Position = x.Position,
                            FirstMobilePhone = x.MobileTelephoneFirst,
                            SecondMobilePhone = x.MobileTelephoneSecond,
                            HomePhone = x.StationaryPhone,
                            Email = x.Email,
                            PartionType = x.PartionType,
                            Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                            {
                                Id = b.Id,
                                Name = b.Name
                            }).ToList(),
                            City = new BaseViewModel
                            {
                                Name = x.Flats.FirstOrDefault().Apartment.Street.City.Name
                            },
                            Street = new BaseViewModel
                            {
                                Name = x.Flats.FirstOrDefault().Apartment.Street.Name
                            },
                            Apartment = new BaseViewModel
                            {
                                Name = x.Flats.FirstOrDefault().Apartment.Name
                            },
                            Flat = new BaseViewModel
                            {
                                Name = x.Flats.FirstOrDefault().Number
                            },
                            Work = x.Work
                        }).ToList();
                }
                else
                    persons = _personService.SearchPersonsByFilter(filter).ToList();

                //if beneficaries exists 
                var personsList = new List<PersonSelectionModel>();
                if (filter.BeneficariesChecked != null && filter.BeneficariesChecked.Any())
                {
                    foreach (var person in persons)
                    {
                        if (filter.BeneficariesChecked.Any(id => person.Beneficiaries.Any(x => x.Id.ToString().Equals(id))))
                            personsList.Add(person);
                        else if (filter.BeneficariesChecked.Any(x => x.Contains("0")) && !person.Beneficiaries.Any())
                            personsList.Add(person);
                    }
                }

                //if empty result
                if ((filter.BeneficariesChecked != null && personsList.Any()) || (filter.BeneficariesChecked == null && filter.BeneficariesUnchecked != null && persons.Any()))
                {
                    isDataFind = true;

                    //generate file header
                    var isColumnLastNameExists = filter.ColumnsIds.Contains(Consts.LastNameId);
                    var isColumnFirstNameExists = filter.ColumnsIds.Contains(Consts.FirstNameId);
                    var isColumnMiddleNameExists = filter.ColumnsIds.Contains(Consts.MiddleNameId);
                    var isColumnFirstMobilePhoneExists = filter.ColumnsIds.Contains(Consts.FirstMobilePhoneId);
                    var isColumnSecondMobilePhoneExists = filter.ColumnsIds.Contains(Consts.SecondMobilePhoneId);
                    var isColumnHomePhoneExists = filter.ColumnsIds.Contains(Consts.HomePhoneId);
                    var isColumnDateBirthExists = filter.ColumnsIds.Contains(Consts.DateBirthId);
                    var isColumnEmailExists = filter.ColumnsIds.Contains(Consts.EmailId);
                    var isColumnAdressExists = filter.ColumnsIds.Contains(Consts.AddressId);
                    var isColumnWorkExists = filter.ColumnsIds.Contains(Consts.WorkPlaceId);

                    int column = 1;
                    var columnIndexes = new Dictionary<string, int>();
                    if (isColumnLastNameExists)
                    {
                        ws.Cells[1, column].Value = "Прізвище";
                        columnIndexes["Прізвище"] = column++;
                    }
                    if (isColumnFirstNameExists)
                    {
                        ws.Cells[1, column].Value = "Ім'я";
                        columnIndexes["Ім'я"] = column++;
                    }
                    if (isColumnMiddleNameExists)
                    {
                        ws.Cells[1, column].Value = "По батькові";
                        columnIndexes["По батькові"] = column++;
                    }
                    if (isColumnFirstMobilePhoneExists)
                    {
                        ws.Cells[1, column].Value = "Телефон 1";
                        columnIndexes["Телефон 1"] = column++;
                    }
                    if (isColumnSecondMobilePhoneExists)
                    {
                        ws.Cells[1, column].Value = "Телефон 2";
                        columnIndexes["Телефон 2"] = column++;
                    }
                    if (isColumnHomePhoneExists)
                    {
                        ws.Cells[1, column].Value = "Стаціонарний";
                        columnIndexes["Стаціонарний"] = column++;
                    }
                    if (isColumnDateBirthExists)
                    {
                        ws.Column(column).Style.Numberformat.Format = "dd-mm-yyyy";
                        ws.Cells[1, column].Value = "Дата народження";
                        columnIndexes["Дата народження"] = column++;
                    }
                    if (isColumnEmailExists)
                    {
                        ws.Cells[1, column].Value = "Емейл";
                        columnIndexes["Емейл"] = column++;
                    }
                    if (isColumnAdressExists)
                    {
                        ws.Cells[1, column].Value = "Населений пункт";
                        columnIndexes["Населений пункт"] = column++;
                        ws.Cells[1, column].Value = "Вулиця";
                        columnIndexes["Вулиця"] = column++;
                        ws.Cells[1, column].Value = "Будинок";
                        columnIndexes["Будинок"] = column++;
                        ws.Cells[1, column].Value = "Квартира";
                        columnIndexes["Квартира"] = column++;
                        ws.Cells[1, column].Value = "Буква";
                        columnIndexes["Буква"] = column++;
                        ws.Cells[1, column].Value = "Корпус";
                        columnIndexes["Корпус"] = column++;
                    }
                    if (isColumnWorkExists)
                    {
                        ws.Cells[1, column].Value = "Місце роботи";
                        columnIndexes["Місце роботи"] = column++;
                    }

                    var beneficiaries = _beneficiaryService.GetAll().ToList();

                    if (filter.BeneficariesChecked != null)
                        foreach (var beneficaryId in filter.BeneficariesChecked.Where(beneficaryId => !beneficaryId.Equals("0")))
                        {
                            var beneficiaryName = beneficiaries.FirstOrDefault(x => x.Id == beneficaryId.AsInt()).Name;
                            ws.Cells[1, column].Value = beneficiaryName;
                            columnIndexes[beneficiaryName] = column++;
                        }

                    var rowNumber = 2;
                    if (filter.BeneficariesChecked != null && personsList.Any())
                    {
                        foreach (var person in personsList)
                        {
                            ws = fillRow(ws, columnIndexes, rowNumber++, person, isColumnLastNameExists, isColumnFirstNameExists, isColumnMiddleNameExists,
                                isColumnFirstMobilePhoneExists, isColumnSecondMobilePhoneExists, isColumnHomePhoneExists,
                                isColumnDateBirthExists, isColumnEmailExists, isColumnWorkExists, isColumnAdressExists, filter.BeneficariesChecked);
                        }
                    }
                    else
                    {
                        foreach (var person in persons)
                        {
                            ws = fillRow(ws, columnIndexes, rowNumber++, person, isColumnLastNameExists, isColumnFirstNameExists, isColumnMiddleNameExists,
                                isColumnFirstMobilePhoneExists, isColumnSecondMobilePhoneExists, isColumnHomePhoneExists,
                                isColumnDateBirthExists, isColumnEmailExists, isColumnWorkExists, isColumnAdressExists, filter.BeneficariesChecked);
                        }
                    }
                }
            }

            if (!isDataFind)
            {
                ws.Cells[1, 1].Style.Font.Bold = true;
                ws.Cells[1, 1].Style.Font.Size = 14;
                ws.Cells[1, 1].Value = "Жодних записів по вашому критерію не знайдено";
            }

            ws.Cells["A:BZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "People.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }


        private ExcelWorksheet fillRow(ExcelWorksheet ws, Dictionary<string, int> columnIndexes, int rowNumber, PersonSelectionModel person, bool isColumnLastNameExists, bool isColumnFirstNameExists,
            bool isColumnMiddleNameExists, bool isColumnFirstMobilePhoneExists, bool isColumnSecondMobilePhoneExists,
            bool isColumnHomePhoneExists, bool isColumnDateBirthExists, bool isColumnEmailExists, bool isColumnWorkExists,
            bool isColumnAdressExists, IEnumerable<string> beneficariesChecked)
        {
            if (isColumnLastNameExists)
                ws.Cells[rowNumber, columnIndexes["Прізвище"]].Value = person.LastName;
            if (isColumnFirstNameExists)
                ws.Cells[rowNumber, columnIndexes["Ім'я"]].Value = person.FirstName;
            if (isColumnMiddleNameExists)
                ws.Cells[rowNumber, columnIndexes["По батькові"]].Value = person.MiddleName;
            if (isColumnFirstMobilePhoneExists)
                ws.Cells[rowNumber, columnIndexes["Телефон 1"]].Value = person.FirstMobilePhone;
            if (isColumnSecondMobilePhoneExists)
                ws.Cells[rowNumber, columnIndexes["Телефон 2"]].Value = person.SecondMobilePhone;
            if (isColumnHomePhoneExists)
                ws.Cells[rowNumber, columnIndexes["Стаціонарний"]].Value = person.HomePhone;
            if (isColumnDateBirthExists)
                ws.Cells[rowNumber, columnIndexes["Дата народження"]].Value = person.DateBirth;
            if (isColumnEmailExists)
                ws.Cells[rowNumber, columnIndexes["Емейл"]].Value = person.Email;
            if (isColumnWorkExists)
                ws.Cells[rowNumber, columnIndexes["Місце роботи"]].Value = person.Work?.Name;

            if (isColumnAdressExists)
            {
                ws.Cells[rowNumber, columnIndexes["Населений пункт"]].Value = person.City.Name;
                ws.Cells[rowNumber, columnIndexes["Вулиця"]].Value = person.Street.Name;
                ws.Cells[rowNumber, columnIndexes["Квартира"]].Value = person.Flat.Name;

                var apartment = person.Apartment?.Name?.Split(' ') ?? new string[] { };
                switch (apartment.Length)
                {
                    case 1:
                        ws.Cells[rowNumber, columnIndexes["Будинок"]].Value = apartment[0];
                        ws.Cells[rowNumber, columnIndexes["Буква"]].Value = "";
                        ws.Cells[rowNumber, columnIndexes["Корпус"]].Value = "";
                        break;
                    case 2:
                        ws.Cells[rowNumber, columnIndexes["Будинок"]].Value = apartment[0];
                        ws.Cells[rowNumber, columnIndexes["Буква"]].Value = apartment[1];
                        ws.Cells[rowNumber, columnIndexes["Корпус"]].Value = "";
                        break;
                    case 3:
                        ws.Cells[rowNumber, columnIndexes["Будинок"]].Value = apartment[0];
                        ws.Cells[rowNumber, columnIndexes["Буква"]].Value = apartment[1];
                        ws.Cells[rowNumber, columnIndexes["Корпус"]].Value = apartment[2].Split(':')[1];
                        break;
                }
            }

            if (beneficariesChecked == null)
                return ws;

            foreach (var beneficaryId in beneficariesChecked)
            {
                if (person.Beneficiaries.Any(x => x.Id.ToString().Equals(beneficaryId)))
                {
                    ws.Cells[rowNumber, columnIndexes[person.Beneficiaries.FirstOrDefault(x => x.Id.ToString().Equals(beneficaryId)).Name]].Value = true;
                }
            }
            return ws;
        }
    }
}