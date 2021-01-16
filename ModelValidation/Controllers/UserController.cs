using ModelValidation.Filter;
using ModelValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ModelValidation.Controllers
{
    [MyModelAttribute]
    public class UserController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Post([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.Created, "Kullanıcı oluşturuldu.");
            }
            else
            {
                var errorList = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList();
                var errorMessage = string.Join(Environment.NewLine, errorList);

                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
            }
        }
    }
}
