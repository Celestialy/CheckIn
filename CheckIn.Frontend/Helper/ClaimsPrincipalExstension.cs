using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CheckIn.Frontend.Helper
{
    public static class ClaimsPrincipalExstension
    {
        /// <summary>
        /// Gets a user object from current user
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static User ToUser(this ClaimsPrincipal principal)
        {
            User user = new User();
            user.Id = principal.FindFirst("ID").Value;
            user.Name = principal.FindFirst("name").Value;
            user.FirstName = principal.FindFirst("given_name").Value;
            user.LastName = principal.FindFirst("family_name").Value;
            user.Mail = principal.FindFirst("email").Value;
            user.CardId = principal.FindFirst("cardid").Value;
            //var s = principal.FindFirst("roles").Value;
            user.Roles = principal.FindAll("roles").Select(x => new Role { Name = x.Value }).ToList();
            user.Departments = principal.FindAll("department").Select(x => new Department { Id = int.Parse(x.Value.Split(':')[0]), Name = x.Value.Split(':')[1] }).ToList<Department>();
            return user;
        }
    }
}
