using System.Text.RegularExpressions;

namespace Svbase.Core.Consts
{
    public class Consts
    {
        public const string SiteUrl = "https://svbase.net";
        public const string HttpPath = "http://";
        public const string HttpsPath = "https://";

        public const string StatusSuccess = "success";
        public const string StatusError = "error";
        public const string RequestDirect = "request-direct";

        public const string PersonIndexPath = "/Person/Index";

        public const int ShowTableRowsCount = 5;
        public const int ShowAllTableRowsCount = -1;

        public const int ShowRecordsPerPage = 25;

        public static readonly Regex CityRegex = new Regex("[^\\sА-Ща-щЬьЮюЯяЇїІіЄєҐґ'’-]");
        public static readonly Regex StreetRegex = new Regex("[^\\s0-9А-Ща-щЬьЮюЯяЇїІіЄєҐґ'’\\.-]");
        public static readonly Regex ApartmentRegex = new Regex("[^0-9]");
        public static readonly Regex EmailRegex = new Regex("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

        public const string DefaultAddress = "Не визначено";
    }
}
