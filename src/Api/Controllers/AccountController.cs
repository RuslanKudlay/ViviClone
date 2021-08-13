using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(IAuthService authService, RoleManager<ApplicationRole> roleManager)
        {
            this.authService = authService;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var token = await authService.Login(model);
            if (token == null)
            {
                return BadRequest("Failed login. The username or password is probably invalid.");
            }   
            else
            {
                return Ok(new { token });
            }
        }


        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await authService.SignUp(model);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (await authService.ForgotPassword(model))
            {
                return Ok();
            }
            else
            {
                return BadRequest("User does not exist.");
            }
        }

        [HttpPost("completeRegistration")]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistrationModel model)
        {
            var token = await authService.CompleteRegistration(model);
            if (token == null)
            {
                return BadRequest("Registration is not completed. The username or password is probably invalid.");
            }
            else
            {
                return Ok(new { token });
            }
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (await authService.ResetPassword(model))
            {
                return Ok();
            }
            else
            {
                return BadRequest("Password has not been reset. Probably the password or code is not valid.");
            }
        }
    }
}