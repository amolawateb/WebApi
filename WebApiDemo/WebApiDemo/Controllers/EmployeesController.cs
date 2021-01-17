using EmployeeDataAccess;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApiDemo.Controllers
{
    [EnableCorsAttribute("*", "*", "*")]
    public class EmployeesController : ApiController
    {
        EmployeeDBEntities entities = new EmployeeDBEntities();

        [BasicAuthentication]
        public HttpResponseMessage GetEmployees(string gender = "All")
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            switch (username.ToLower())
            {
                case "male":
                    return Request.CreateResponse(HttpStatusCode.OK, 
                        entities.Employees.Where(e=>e.Gender.ToLower() == "male").ToList());
                case "female":
                    return Request.CreateResponse(HttpStatusCode.OK,
                        entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                default:
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public HttpResponseMessage ReturnEmployee(int id)
        {
            try
            {
                var entity = entities.Employees.FirstOrDefault(e => e.Id == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + " not found");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage NewEmployee(Employee emp)
        {
            try
            {
                entities.Employees.Add(emp);
                entities.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                message.Headers.Location = new Uri(Request.RequestUri + "/" + emp.Id.ToString());
                return message;
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPut]
        public HttpResponseMessage UpdateEmployee([FromBody]int id, [FromUri]Employee emp)
        {
            try
            {
                var entity = entities.Employees.FirstOrDefault(e => e.Id == id);

                if (entity != null)
                {
                    entity.Name = emp.Name;
                    entity.Email = emp.Email;
                    entity.Department = emp.Department;
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id.ToString() + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteEmployee(int id)
        {
            try
            {
                var entity = entities.Employees.FirstOrDefault(e => e.Id == id);

                if (entity != null)
                {
                    entities.Employees.Remove(entity);
                    entities.SaveChanges(); 
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + entity.Id.ToString() + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
