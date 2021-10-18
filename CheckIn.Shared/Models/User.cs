using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckIn.Shared.Models
{
    public class User
    {
        /// <summary>
        /// User Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Full user name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Users first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Users last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Users mail
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// Users card
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// Users roles
        /// </summary>
        public ICollection<Role> Roles { get; set; }
        /// <summary>
        /// Users departments
        /// </summary>
        public ICollection<Department> Departments { get; set; }
        
    }
    public class Role
    {
        /// <summary>
        /// Role Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }
    }
    public class Department
    {
        /// <summary>
        /// Department Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Department name
        /// </summary>
        public string Name { get; set; }

        
    }


    
}
