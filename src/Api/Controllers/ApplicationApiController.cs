using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApplicationApiController : Controller
    {
        private readonly UserManager<ApplicationUser> _manager;

        protected ApplicationUser CurrentUser { get; set; }


        public ApplicationApiController(UserManager<ApplicationUser> manager)
        {
            _manager = manager;
        }

        protected IActionResult InvokeMethod<IN>(Func<IN> method)
        {
            try
            {
                return Ok(method());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }


        protected IActionResult InvokeMethod<IN, OUT>(Func<IN, OUT> method,IN model)
        {
            try
            {
                return Ok(method(model));
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }         
                
        }
      
        private async void GetCurrentUser()
        {
            CurrentUser =  await _manager.GetUserAsync(HttpContext.User);
        }


    }
}
