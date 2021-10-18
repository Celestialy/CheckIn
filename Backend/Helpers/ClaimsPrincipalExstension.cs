using CheckIn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public static class ClaimsPrincipalExstension
    {
        /// <summary>
        /// Gets a user object from current user
        /// </summary>
        /// <param name="principal"></param>
        /// <returns>an <see cref="User"/></returns>
        public static User ToUser(this ClaimsPrincipal principal)
        {
            User user = new User();
            user.Id = principal.FindFirst("ID").Value;
            user.Name = principal.FindFirst("name").Value;
            user.FirstName = principal.FindFirst(ClaimTypes.GivenName).Value;
            user.LastName = principal.FindFirst(ClaimTypes.Surname).Value;
            user.Mail = principal.FindFirst(ClaimTypes.Email).Value;
            user.CardId = principal.FindFirst("cardid").Value;
            //var s = principal.FindFirst("roles").Value;
            user.Roles = principal.FindAll("roles").Select(x => new Role { Name = x.Value }).ToList();
            user.Departments = principal.FindAll("department").Select(x => new Department { Id = int.Parse(x.Value.Split(':')[0]), Name = x.Value.Split(':')[1] }).ToList<Department>();
            return user;
        }
    }
}
