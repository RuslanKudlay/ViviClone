using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ApplicationApiController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("api/User/all")]
        public IActionResult Get([FromBody]QueryUserModel query)
        {
            return InvokeMethod(userService.Get, query);
        }

        [HttpGet]
        [Route("api/User/{userId}")]
        public IActionResult GetById(int userId)
        {
            return InvokeMethod(userService.GetById, userId);
        }

        [HttpGet]
        [Route("api/User/roles")]
        public IActionResult GetRoles()
        {
            return InvokeMethod(userService.GetRoles);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [Route("api/User/change-role/{userId}")]
        public IActionResult ChangeRole([FromBody]ApplicationRole role, int userId)
        {
            try
            {
                userService.ChangeRole(role, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [Route("api/User/{userId}/change-status/{status}")]
        public IActionResult ChangeStatus(UserStatus status, int userId)
        {
            try
            {
                userService.ChangeStatus(status, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
