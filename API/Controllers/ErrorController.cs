using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ErrorController : BaseAPIController
    {
       
        [HttpGet("not-found")]
        public ActionResult GetNotFound() {
         return NotFound(new ProblemDetails {
            Title = "Sorry! Specified Player Not Found.",
            Status = 404,
            Detail = "Gamer Says - 'Please try again!'"
         });
        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequest() {
            return BadRequest(new ProblemDetails {
                Title = "This is a bad request - 400",
                Status = 400,
                Detail ="Could not resolve your problem. Try other fixes"
            }); 
        }

        [HttpGet("validationError")]
        public ActionResult GetValidationError() {
        ModelState.AddModelError("Error1", "This is the error 1");
        ModelState.AddModelError("Error2", "This is the error 2");
        ModelState.AddModelError("Error3", "This is the error 3");
        ModelState.AddModelError("Error4", "This is the error 4");
        return ValidationProblem();
        }

        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorized() {
            return Unauthorized(new ProblemDetails {
                Title = "Sorry! You are not Authorized Properly",
                Status = 401,
                Detail ="Please check again, while entering the details!"
            });
        }

       [HttpGet("serverError")]
       public ActionResult GetServerError() {
        throw new Exception("This is an Internal Server Error!");
       } 
    }
}

      
     

     