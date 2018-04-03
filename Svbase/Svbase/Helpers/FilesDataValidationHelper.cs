using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Svbase.Models;

namespace Svbase.Helpers
{
    public class FilesDataValidationHelper
    {
        public bool ValidateFileColumns(DataTable dataTable, string fileName, ref List<string> errorModel)
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

            return errorModel.Any();
        }

        public FileValidationModel ValidateTableRows(DataRow row, int rowIndex, string fileName, ref List<string> errorList)
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

            if (firstName.Length <= 1)
            {
                errorList.Add("Надто коротке ім'я у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "'!");
            }

            if (lastName.Length <= 1)
            {
                errorList.Add("Надто коротке прізвище у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "'!");
            }

            if (middleName.Length <= 1)
            {
                errorList.Add("Надто коротке по батькові у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "'!");
            }

            if (apartmentNumber.Length == 0)
            {
                errorList.Add("Необхідно вказати номер будинку у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "'!");
            }

            if (streetName.Length <= 1)
            {
                errorList.Add("Назва вулиці у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' повинна містити хоча б одну літеру!");
            }

            if (cityName.Length <= 1)
            {
                errorList.Add("Назва міста у " + (rowIndex + 2) + " рядку таблиці '" + fileName + "' повинна містити хоча б одну літеру!");
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
                    FlatNumber = flatNumber,
                    ApartmentNumber = apartmentNumber,
                    ApartmentLetter = apartmentLetter,
                    ApartmentSide = apartmentSide,
                    StreetName = streetName,
                    CityName = cityName,
                    BirthdayDate = null
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
                FlatNumber = flatNumber,
                ApartmentNumber = apartmentNumber,
                ApartmentLetter = apartmentLetter,
                ApartmentSide = apartmentSide,
                StreetName = streetName,
                CityName = cityName,
                BirthdayDate = birthdayDate
            };
        }
    }
}