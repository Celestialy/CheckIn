using CheckIn.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CheckIn.Shared.Models
{
    public class Pagination
    {
        //request
        /// <summary>
        /// Set number of items on page
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// Sets Current page
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// Sets what the list should be sorted after
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// What fields you want to have shown
        /// </summary>
        public string[] Fields { get; set; }
        /// <summary>
        /// Sets the search word for searching after items
        /// </summary>
        public string SearchQuery { get; set; }
        /// <summary>
        /// Sets what departments you want infomation from
        /// </summary>
        public string[] Departments { get; set; }
        /// <summary>
        /// Sets what roles you want infomation from
        /// </summary>
        public string[] Roles { get; set; }
        //Response
        /// <summary>
        /// Gets the total number of items
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// Gets the total number of pages
        /// </summary>
        public int totalPages { get; set; }

        /// <summary>
        /// Converts the model to query
        /// </summary>
        public string AsQuery 
        { 
            get 
            {
                string query = "?";
                if (pageSize > 0)
                {
                    query += "PageSize=" + pageSize + "&";
                }
                if (currentPage > 0)
                {
                    query += "PageNumber=" + currentPage + "&";
                }
                if (OrderBy != string.Empty)
                {
                    query += "OrderBy=" + OrderBy + "&";
                }
                if (!Fields.IsNullOrEmpty())
                {
                    query += "Fields=" + string.Join(",", Fields) + "&";
                }
                if (SearchQuery != string.Empty)
                {
                    query += "SearchQuery=" + SearchQuery + "&";
                }
                if (!Departments.IsNullOrEmpty())
                {
                    query += "Departments=" + string.Join(",", Departments) + "&";
                }
                if (!Roles.IsNullOrEmpty())
                {
                    query += "Roles=" + string.Join(",", Roles);
                }
                return query;
            } 
        }

        public void SetPageSize(int size)
        {
            this.pageSize = size;
        }
    }
}
