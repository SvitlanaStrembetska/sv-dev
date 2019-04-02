using System;
using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Enums;
using Svbase.Core.Models;

namespace Svbase.Helpers
{
    public class PersonFilterHelper
    {
        public IQueryable<PersonSelectionModel> FilterPersonsByBeneficiary(IQueryable<PersonSelectionModel> persons, IEnumerable<string> checkedBeneficariesId)
        {
            var personsLists = persons.Where(
                x => checkedBeneficariesId.Any(id => x.Beneficiaries.Any(y => y.Id.ToString().Equals(id)))
                || (checkedBeneficariesId.Any(z => z.Contains("0")) && !x.Beneficiaries.Any()));

            return personsLists;
        }

        public IEnumerable<PersonSelectionModel> OrderPersonsBy(IQueryable<PersonSelectionModel> persons, SortOrder sortOrder, ColumnName firstSortOrder, ColumnName secondSortOrder, ColumnName thirdSortOrder, int skip, int take)
        {
            var firstOrder = GetColumnName(firstSortOrder);
            var secondOrder = GetColumnName(secondSortOrder);
            var thirdOrder = GetColumnName(thirdSortOrder);

            var isFirstOrderNotNull = isColumnNameNull(firstSortOrder);

            IEnumerable<PersonSelectionModel> orderedPeople = null;

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    if (firstSortOrder != 0 && secondSortOrder != 0 && thirdSortOrder != 0)
                    {
                        orderedPeople = persons.OrderBy(isFirstOrderNotNull).ThenBy(firstOrder).ThenBy(secondOrder).ThenBy(thirdOrder).Skip(skip).Take(take);
                    }
                    else if (firstSortOrder != 0 && secondSortOrder != 0 && thirdSortOrder == 0)
                        orderedPeople = persons.OrderBy(isFirstOrderNotNull).ThenBy(firstOrder).ThenBy(secondOrder).Skip(skip).Take(take);
                    else if (firstSortOrder != 0 && secondSortOrder == 0 && thirdSortOrder == 0)
                        orderedPeople = persons.OrderBy(isFirstOrderNotNull).ThenBy(firstOrder).Skip(skip).Take(take);
                    break;
                case SortOrder.Descending:
                    if (firstSortOrder != 0 && secondSortOrder != 0 && thirdSortOrder != 0)
                        orderedPeople = persons.OrderByDescending(firstOrder).ThenBy(secondOrder).ThenBy(thirdOrder).Skip(skip).Take(take);
                    else if (firstSortOrder != 0 && secondSortOrder != 0 && thirdSortOrder == 0)
                        orderedPeople = persons.OrderByDescending(firstOrder).ThenBy(secondOrder).Skip(skip).Take(take);
                    else if (firstSortOrder != 0 && secondSortOrder == 0 && thirdSortOrder == 0)
                        orderedPeople = persons.OrderByDescending(firstOrder).Skip(skip).Take(take);
                    break;
            }

            return orderedPeople ?? persons.Skip(skip).Take(take).ToList();
        }

        public IEnumerable<PersonSelectionModel> OrderPersonsBy(IQueryable<PersonSelectionModel> persons, SortOrder sortOrder, ColumnName firstSortOrder, ColumnName secondSortOrder, ColumnName thirdSortOrder)
        {
            var firstOrder = GetColumnName(firstSortOrder);
            var secondOrder = GetColumnName(secondSortOrder);
            var thirdOrder = GetColumnName(thirdSortOrder);

            var isFirstOrderNotNull = isColumnNameNull(firstSortOrder);

            IEnumerable<PersonSelectionModel> orderedPeople = null;

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    if (firstSortOrder != 0 && secondSortOrder != 0 && thirdSortOrder != 0)
                    {
                        orderedPeople = persons.OrderBy(isFirstOrderNotNull).ThenBy(firstOrder).ThenBy(secondOrder).ThenBy(thirdOrder);
                    }
                    else if (firstSortOrder != 0 && secondSortOrder != 0 && thirdSortOrder == 0)
                        orderedPeople = persons.OrderBy(isFirstOrderNotNull).ThenBy(firstOrder).ThenBy(secondOrder);
                    else if (firstSortOrder != 0 && secondSortOrder == 0 && thirdSortOrder == 0)
                        orderedPeople = persons.OrderBy(isFirstOrderNotNull).ThenBy(firstOrder);
                    break;
                case SortOrder.Descending:
                    if (firstSortOrder != 0 && secondSortOrder != 0 && thirdSortOrder != 0)
                        orderedPeople = persons.OrderByDescending(firstOrder).ThenBy(secondOrder).ThenBy(thirdOrder);
                    else if (firstSortOrder != 0 && secondSortOrder != 0 && thirdSortOrder == 0)
                        orderedPeople = persons.OrderByDescending(firstOrder).ThenBy(secondOrder);
                    else if (firstSortOrder != 0 && secondSortOrder == 0 && thirdSortOrder == 0)
                        orderedPeople = persons.OrderByDescending(firstOrder);
                    break;
            }

            return orderedPeople ?? persons.ToList();
        }

        private Func<PersonSelectionModel, object> GetColumnName(ColumnName columnName)
        {
            switch (columnName)
            {
                case ColumnName.Id:
                    return model => model.Id;
                case ColumnName.LastName:
                    return model => model.LastName;
                case ColumnName.FirstName:
                    return model => model.FirstName;
                case ColumnName.MiddleName:
                    return model => model.MiddleName;
                case ColumnName.MobileTelephoneFirst:
                    return model => model.FirstMobilePhone;
                case ColumnName.MobileTelephoneSecond:
                    return model => model.SecondMobilePhone;
                case ColumnName.StationaryPhone:
                    return model => model.HomePhone;
                case ColumnName.BirthdayDate:
                    return model => model.DateBirth;
                case ColumnName.Email:
                    return model => model.Email;
                case ColumnName.CityName:
                    return model => model.City.Name;
                case ColumnName.StreetName:
                    return model => model.Street.Name;
                case ColumnName.ApartmentName:
                    return model => model.Apartment.Name;
                case ColumnName.FlatName:
                    return model => model.Flat.Name;
                case ColumnName.WorkName:
                    return model => model.Work.Name;
                default:
                    return null;
            }
        }
        private Func<PersonSelectionModel, bool> isColumnNameNull(ColumnName columnName)
        {
            switch (columnName)
            {
                case ColumnName.Id:
                    return model => true;
                case ColumnName.LastName:
                    return model => string.IsNullOrEmpty(model.LastName);
                case ColumnName.FirstName:
                    return model => string.IsNullOrEmpty(model.FirstName);
                case ColumnName.MiddleName:
                    return model => string.IsNullOrEmpty(model.MiddleName);
                case ColumnName.MobileTelephoneFirst:
                    return model => string.IsNullOrEmpty(model.FirstMobilePhone);
                case ColumnName.MobileTelephoneSecond:
                    return model => string.IsNullOrEmpty(model.SecondMobilePhone);
                case ColumnName.StationaryPhone:
                    return model => string.IsNullOrEmpty(model.HomePhone); 
                case ColumnName.BirthdayDate:
                    return model => model.DateBirth == null;
                case ColumnName.Email:
                    return model => string.IsNullOrEmpty(model.Email);
                case ColumnName.CityName:
                    return model => string.IsNullOrEmpty(model.City.Name);
                case ColumnName.StreetName:
                    return model => string.IsNullOrEmpty(model.Street.Name);
                case ColumnName.ApartmentName:
                    return model => string.IsNullOrEmpty(model.Apartment.Name);
                case ColumnName.FlatName:
                    return model => string.IsNullOrEmpty(model.Flat.Name);
                case ColumnName.WorkName:
                    return model => string.IsNullOrEmpty(model.Work.Name);
                default:
                    return null;
            }
        }
    }
}