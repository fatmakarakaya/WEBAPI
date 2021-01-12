using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CRUD_JQUERY.Controllers
{
    public class EmployeeController : ApiController
    {
        EMPLOYEEEntities db = new EMPLOYEEEntities();


        //.../api/Employee GET METODU
        //public IEnumerable<employee> Get()
        //{
        //    return db.employees.ToList();
        //}

        //.../api/Employee?gender=male&top2
        //URL de query string kullanımı

        public HttpResponseMessage Get(string gender = "all", int? top = 0)
        {
            IQueryable<employee> query = db.employees;

            switch (gender)
            {
                case "all":
                    break;
                case "male":
                case "female":
                    query = query.Where(e => e.gender.ToLower() == gender);
                    break;
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "geçersiz cinsiyet değeri girdiniz.");
            }

            if (top > 0)
            {
                query = query.Take(top.Value);
            }

            return Request.CreateResponse(HttpStatusCode.OK, query.ToList());
        }
        //.../api/Employee/[id] 
        public HttpResponseMessage Get(int Id)
        {
            employee employee = db.employees.FirstOrDefault(x => x.id == Id);
            if (employee == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "çalışan bulunamadı.");

            }
            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        //.../api/Employee POST METODU
        public HttpResponseMessage Post(employee employee)
        {
            try
            {
                db.employees.Add(employee);
                if (db.SaveChanges() > 0)
                {
                    HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + employee.id);

                    return message;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Veri eklenemedi.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //.../api/Employee/[İD] PUT METODU
        public HttpResponseMessage Put(employee employee, int Id)
        {
            try
            {
                employee emp = db.employees.FirstOrDefault(e => e.id == Id);

                if (emp == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Çalışan id:" + employee.id);

                }
                else
                {
                    emp.firstname = employee.firstname;
                    emp.surname = employee.surname;
                    emp.salary = employee.salary;
                    emp.gender = employee.gender;

                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, employee);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Güncelleme yapılamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //.../api/Employee/[id] DELETE METODU
        public HttpResponseMessage Delete(int Id)
        {
            try
            {
                employee emp = db.employees.FirstOrDefault(x => x.id == Id);
                if (emp == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Çalışan id: " + Id);
                }
                else
                {
                    db.employees.Remove(emp);
                    if (db.SaveChanges() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "İd'si " + Id + " olan kayıt silinemedi");
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }



    }

}

