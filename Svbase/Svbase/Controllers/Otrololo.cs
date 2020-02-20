using Svbase.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using CsvHelper;

namespace Svbase.Controllers
{
    public class ImportModelCsvService
    {
        public IEnumerable<Person> Read(CsvReader csv)
        {
            var list = new List<Person>();
            csv.Read();
            while (csv.Read())
            {
                var firstName = csv.GetField<string>(0);
                var middleName = csv.GetField<string>(1);
                var secondName = csv.GetField<string>(2);
                var position = csv.GetField<string>(3);
                var partionType = csv.GetField<string>(4);
                var email = csv.GetField<string>(5);
                var mobileTelephoneFirst = csv.GetField<string>(6);
                var mobileTelephoneSecond = csv.GetField<string>(7);
                var stationaryPhone = csv.GetField<string>(8);
                var gender = csv.GetField<bool>(9);
                var birthdayDate = csv.GetField<DateTime?>(10);


                if (string.IsNullOrWhiteSpace(firstName)
                    || string.IsNullOrWhiteSpace(secondName)) break;

                list.Add(new Person
                {
                    FirstName = WebUtility.HtmlEncode(firstName),
                    MiddleName = WebUtility.HtmlEncode(middleName),
                    LastName = WebUtility.HtmlEncode(secondName),
                    Position = WebUtility.HtmlEncode(position),
                    PartionType = WebUtility.HtmlEncode(partionType),
                    Email = WebUtility.HtmlEncode(email),
                    MobileTelephoneFirst = WebUtility.HtmlEncode(mobileTelephoneFirst),
                    MobileTelephoneSecond = WebUtility.HtmlEncode(mobileTelephoneSecond),
                    //HomePhone = WebUtility.HtmlEncode(homePhone),
                    StationaryPhone = WebUtility.HtmlEncode(stationaryPhone),
                    Gender = gender,
                    BirthdayDate = birthdayDate,
                });
            }

            return list;
        }

        public bool Validate(CsvReader csv, out List<CsvErrorModel> error)
        {
            error = new List<CsvErrorModel>();

            var i = 1;
            while (csv.Read())
            {
                i++;
                if (i == 2) continue;
                string firstName;
                string middleName;
                string secondName;
                string position;
                string partionType;
                string email;
                string mobileTelephoneFirst;
                string mobileTelephoneSecond;
                //string homePhone;
                string stationaryPhone;
                string gender;
                DateTime? birthdayDate;

                if (!csv.TryGetField(0, out firstName))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(firstName),
                        Value = csv.GetField<string>(0),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(1, out middleName))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(middleName),
                        Value = csv.GetField<string>(1),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(2, out secondName))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(secondName),
                        Value = csv.GetField<string>(2),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(3, out position))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(position),
                        Value = csv.GetField<string>(3),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(4, out partionType))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(partionType),
                        Value = csv.GetField<string>(4),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(5, out email))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(email),
                        Value = csv.GetField<string>(5),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(6, out mobileTelephoneFirst))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(mobileTelephoneFirst),
                        Value = csv.GetField<string>(6),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(7, out mobileTelephoneSecond))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(mobileTelephoneSecond),
                        Value = csv.GetField<string>(7),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(8, out stationaryPhone))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(stationaryPhone),
                        Value = csv.GetField<string>(8),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(9, out gender))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(gender),
                        Value = csv.GetField<string>(9),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }

                if (!csv.TryGetField(10, out birthdayDate))
                {
                    error.Add(new CsvErrorModel
                    {
                        Number = i,
                        Type = nameof(birthdayDate),
                        Value = csv.GetField<string>(10),
                        ErrorMessage = "Invalid type. Should be string."
                    });
                }
            }

            return error.Count == 0;
        }
    }

    public class CsvErrorModel
    {
        public int Number { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ErrorMessage { get; set; }
    }
}