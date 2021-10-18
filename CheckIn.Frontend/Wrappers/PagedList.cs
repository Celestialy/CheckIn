using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Wrappers
{
    public class PagedList<T> where T : class
    {
        /// <summary>
        /// Contains a list of: <typeparamref name="T"/> 
        /// </summary>
        public List<T> Items { get; set; }
        /// <summary>
        /// Contains pageination information
        /// </summary>
        public Pagination Pagination { get; set; }
    }
}
