using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Svbase.Core.Data.Entities;
using Svbase.Models;
using Svbase.Core.Consts;

namespace Svbase.Helpers
{
    public class FilesDataValidationHelper
    {
        public bool ValidateFileColumns(DataTable dataTable, string fileName, IEnumerable<Beneficiary> beneficaries, ref List<string> errorModel)
        {
            var columnsName = dataTable.Columns;

            if (!columnsName.Contains("Прізвище"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Прізвище'");

            if (!columnsName.Contains("Ім'я"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Ім'я'");

            if (!columnsName.Contains("По батькові"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'По батькові'");

            if (!columnsName.Contains("Дата народження"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Дата народження'");

            if (!columnsName.Contains("Телефон 1"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Телефон 1'");

            if (!columnsName.Contains("Телефон 2"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Телефон 2'");

            if (!columnsName.Contains("Стаціонарний"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Стаціонарний'");

            if (!columnsName.Contains("Населений пункт"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Населений пункт'");

            if (!columnsName.Contains("Вулиця"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Вулиця'");

            if (!columnsName.Contains("Будинок"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Будинок'");

            if (!columnsName.Contains("Квартира"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Квартира'");

            if (!columnsName.Contains("Буква"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Буква'");

            if (!columnsName.Contains("Корпус"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Корпус'");

            if (!columnsName.Contains("Місце роботи"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Місце роботи'");

            if (!columnsName.Contains("Емейл"))
                errorModel.Add("Таблиця " + fileName + " не містить стовбець 'Емейл'");

            foreach (var beneficary in beneficaries)
            {
                if (!columnsName.Contains(beneficary.Name))
                    errorModel.Add("Таблиця " + fileName + " не містить стовбець '" + beneficary.Name + "'");
            }

            return errorModel.Any();
        }

        public FileValidationModel ValidateTableRows(DataRow row, int rowIndex, string fileName, IEnumerable<Beneficiary> beneficaries, ref List<string> errorList)
        {
            var firstName = row["Ім'я"].ToString().Trim();
            var lastName = row["Прізвище"].ToString().Trim();
            var middleName = row["По батькові"].ToString().Trim();
            var mobileTelephoneFirst = row["Телефон 1"].ToString().Trim();
            var mobileTelephoneSecond = row["Телефон 2"].ToString().Trim();
            var stationaryPhone = row["Стаціонарний"].ToString().Trim();
            var flatNumber = row["Квартира"].ToString().Trim();
            var apartmentNumber = row["Будинок"].ToString().Trim();
            var apartmentLetter = row["Буква"].ToString().Trim();
            var apartmentSide = row["Корпус"].ToString().Trim();
            var streetName = row["Вулиця"].ToString().Trim();
            var cityName = row["Населений пункт"].ToString().Trim();
            var getDate = row["Дата народження"].ToString().Trim();
            var workName = row["Місце роботи"].ToString().Trim();
            var email = row["Емейл"].ToString().Trim();
            var beneficariesList = new Dictionary<string, bool>();

            foreach (var beneficary in beneficaries)
            {
                beneficariesList.Add(beneficary.Name, row[beneficary.Name].ToString().ToUpper() == "TRUE");
            }

            if (firstName.Length == 0 && lastName.Length == 0)
            {
                errorList.Add("Ім'я або прізвище у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' не повинно бути порожнім!");
            }

            if (cityName.Length > 0 && Consts.CityRegex.Matches(cityName).Count != 0)
            {
                errorList.Add("Назва міста у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' містить заборонені символи!");
            }

            if (streetName.Length > 0 && Consts.StreetRegex.Matches(streetName).Count != 0)
            {
                errorList.Add("Назва вулиці у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' містить заборонені символи!");
            }
            
            if (apartmentNumber.Length > 0 && Consts.ApartmentRegex.Matches(apartmentNumber).Count != 0)
            {
                errorList.Add("Номер будинку у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' повинен містити лише цифри!");
            }

            if (email.Length > 0 && Consts.EmailRegex.Matches(email).Count == 0)
            {
                errorList.Add("Невірний формат емейлу в " + (rowIndex + 2) + " рядку таблиці '" + fileName + "'!");
            }

            if (getDate.Length == 0)
                return new FileValidationModel
                {
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName,
                    MobileTelephoneFirst = mobileTelephoneFirst,
                    MobileTelephoneSecond = mobileTelephoneSecond,
                    StationaryPhone = stationaryPhone,
                    FlatNumber = flatNumber.Length > 0 ? flatNumber : Consts.DefaultAddress,
                    ApartmentNumber = apartmentNumber.Length > 0 || apartmentLetter.Length > 0 || apartmentSide.Length > 0 ? apartmentNumber : Consts.DefaultAddress,
                    ApartmentLetter = apartmentLetter,
                    ApartmentSide = apartmentSide,
                    StreetName = streetName.Length > 0 ? streetName : Consts.DefaultAddress,
                    CityName = cityName.Length > 0 ? cityName : Consts.DefaultAddress,
                    BirthdayDate = null,
                    Beneficaries = beneficariesList,
                    WorkName = workName,
                    Email = email
                };

            DateTime? birthdayDate = null;
            try
            {
                var day = Convert.ToInt32(getDate.Substring(0, 2));
                var month = Convert.ToInt32(getDate.Substring(3, 2));
                var year = Convert.ToInt32(getDate.Substring(6, 4));

                if (day > 31 || day < 1)
                {
                    errorList.Add("День у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' невірний! Формат дати: день.місяць.рік (наприклад, 27.02.1990)");
                }
                if (month > 12 || month < 1)
                {
                    errorList.Add("Місяць у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' невірний! Формат дати: день.місяць.рік (наприклад, 27.02.1990)");
                }
                if (year > DateTime.Now.Year || year < 1890)
                {
                    errorList.Add("Рік у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' невірний! Формат дати: день.місяць.рік (наприклад, 27.02.1990)");
                }
                birthdayDate = new DateTime(year, month, day);
            }
            catch (Exception)
            {
                errorList.Add("Невірний формат дати у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "'! Формат дати: день.місяць.рік (наприклад, 27.02.1990)");
            }


            return new FileValidationModel
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                MobileTelephoneFirst = mobileTelephoneFirst,
                MobileTelephoneSecond = mobileTelephoneSecond,
                StationaryPhone = stationaryPhone,
                FlatNumber = flatNumber.Length > 0 ? flatNumber : Consts.DefaultAddress,
                ApartmentNumber = apartmentNumber.Length > 0 ? apartmentNumber : Consts.DefaultAddress,
                ApartmentLetter = apartmentLetter,
                ApartmentSide = apartmentSide,
                StreetName = streetName.Length > 0 ? streetName : Consts.DefaultAddress,
                CityName = cityName.Length > 0 ? cityName : Consts.DefaultAddress,
                BirthdayDate = birthdayDate,
                Beneficaries = beneficariesList,
                WorkName = workName,
                Email = email
            };
        }
    }
}