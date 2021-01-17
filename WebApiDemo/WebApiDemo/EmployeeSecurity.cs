using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeDataAccess;
namespace WebApiDemo
{
    public class EmployeeSecurity
    {
        public static bool Login(string username, string password)
        {
            EmployeeDBEntities entities = new EmployeeDBEntities();
            return entities.Users.Any(user =>user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.Password.Equals(password,StringComparison.OrdinalIgnoreCase));
        }
    }
}