using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebApiDemo.Controllers
{
    public class EmployeesController : ApiController
    {
        public HttpResponseMessage GetEmployees(string gender = "All")
        {
            EmployeeDBEntities entities = new EmployeeDBEntities();
            switch (gender.ToLower())
            {
                case "all":
                    return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                case "male":
                    return Request.CreateResponse(HttpStatusCode.OK, 
                        entities.Employees.Where(e=>e.Gender.ToLower() == "male").ToList());
                case "female":
                    return Request.CreateResponse(HttpStatusCode.OK,
                        entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Values for Gender must be Male or Female");
            }
        }

        [HttpGet]
        public HttpResponseMessage ReturnEmployee(int id)
        {
            try
            {
                EmployeeDBEntities entities = new EmployeeDBEntities();
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
                EmployeeDBEntities entities = new EmployeeDBEntities();
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
                EmployeeDBEntities entities = new EmployeeDBEntities();
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
                EmployeeDBEntities entities = new EmployeeDBEntities();
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
