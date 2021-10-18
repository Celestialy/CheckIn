using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Helper
{
    public static class ModelExstensions
    {
        /// <summary>
        /// gets a string array from the departments a user is in
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        public static string[] toStringArray(this ICollection<Department> departments)
        {
            return departments.Select(x => x.Name).ToArray();
        }
    }
}
