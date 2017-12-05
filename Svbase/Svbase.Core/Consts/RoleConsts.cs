using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Svbase.Core.Consts
{
    public class RoleConsts
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static IList<string> GetAllRoles()
        {
            return typeof(RoleConsts)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.FieldType == typeof(string))
                .Select(x => (string)x.GetValue(null))
                .ToList();
        }
    }
}
