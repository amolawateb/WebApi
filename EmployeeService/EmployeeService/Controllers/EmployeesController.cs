using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeService.Controllers
{
    [Authorize]
    public class EmployeesController : ApiController
    {
        EmployeeDBEntities employeesDb = new EmployeeDBEntities();

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            EmployeeDBEntities employeesDb = new EmployeeDBEntities();
            return employeesDb.Employees.ToList();
        }
    }
}
