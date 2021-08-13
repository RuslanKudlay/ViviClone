using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.Auth;
using Application.EntitiesModels.Models.ChatModels;
using Application.EntitiesModels.Models.ViewsModel.RegistrationModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Application.Server.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISmtpClient _smtpClient;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthService _authService;

        private readonly IWishListService _wishListService;

        [TempData]
        public string ErrorMessage { get; set; }

        public AccountController(UserManager<ApplicationUser> userManage,
            SignInManager<ApplicationUser> signManager,
            ISmtpClient smtpClient,
            ILogger<AccountController> logger,
            IAuthService authService, 
            IWishListService wishListService)
        {
            _userManager = userManage;
            _signInManager = signManager;
            _smtpClient = smtpClient;
            _logger = logger;
            _authService = authService;
            _wishListService = wishListService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string result = await _authService.Register(model);
                if (!result.Contains("@"))
                {
                    TempData["errorRegistration"] = result;

                    return RedirectToAction("Index", "Shop");
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(result);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);

                    var resultSendEmail = _smtpClient.SendMail(model.Email, "Подтверждение аккаунта",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>подтверждение регистрациии</a>");

                    TempData["confirmRegistration"] = "true";
                    return RedirectToAction("Index", "Shop");
                }
            }

            var errorsModelState = "";
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errorsModelState += error.ErrorMessage + ";";
                }
            }

            TempData["errorRegistration"] = errorsModelState;
            return RedirectToAction("Index", "Shop");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {

            var result = await _authService.ConfirmEmail(userId, code);
            if (result)
            {
                return RedirectToAction("Index", "Shop");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LoginMVC(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginMVC(model);
                if (result == "LoginSuccess")
                {
                    return RedirectToAction("Index", "Shop");
                }
                else if (result != "LoginFailed")
                {
                    return Redirect(model.ReturnUrl);
                }

                TempData["errorLogin"] = "Failed Login";
                return RedirectToAction("Index", "Shop");
            }

            var errorsModelState = "";
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errorsModelState += error.ErrorMessage + ";";
                }
            }

            TempData["errorLogin"] = errorsModelState;
            return RedirectToAction("Index", "Shop");
        }

        [AllowAnonymous]
        public IActionResult SignIn(string provider, bool linkExternalProvider = false, bool mergeAccount = false)
        {
            string redirectUrl;
            if (linkExternalProvider)
            {
                redirectUrl = Url.Action("LinkExternalAccount", "Account");
            }
            else if (mergeAccount)
            {
                redirectUrl = Url.Action("MergeAccountBySocialNetworks", "Account");
            }
            else
            {
                redirectUrl = Url.Action("ResponseAfterSocialNetworks", "Account");
            }
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResponseAfterSocialNetworks()
        {
            await _authService.ResponseAfterSocialNetworks();
            return RedirectToAction("Index", "Shop");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();

            var returnUrl = Request.Headers["Referer"].ToString().ToLower().Contains("account") ? "/Shop" : Request.Headers["Referer"].ToString();

            return Redirect(returnUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordMVC(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                await _authService.ForgotPassword(model);
                TempData["resetPassword"] = model.Email;
            }

            return RedirectToAction("Index", "Shop");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordMVC(ResetPasswordModel model)
        {
            //Back to modal popup 'Reset Password'
            TempData["resetPassword"] = model.Email;

            if (!ModelState.IsValid)
            {
                var errorsModelState = "";
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errorsModelState += error.ErrorMessage + ";";
                    }
                }

                TempData["errorResetPassword"] = errorsModelState;
                return RedirectToAction("Index", "Shop");
            }

            var result = await _authService.ResetPasswordMVC(model);
            if (result == null)
            {
                TempData["resetPasswordConfirmation"] = "true";
                return RedirectToAction("Index", "Shop");
            }

            var errors = "";
            foreach (var error in result)
            {
                errors += error.Code + ";";
            }
            TempData["errorResetPassword"] = errors;


            return RedirectToAction("Index", "Shop");
        }

        [Authorize]
        public async Task<IActionResult> MergeAccountByEmail(UserModel model)
        {
            if(ModelState.IsValid)
            {
                int currentUserId = int.Parse(_userManager.GetUserId(User));
                var result = await _authService.MergeAccountByEmail(model, currentUserId);
                if(result)
                {
                    TempData["mergeAccount"] = "true";
                    return View("Details");
                }
                else
                {
                    TempData["errorLoginMerge"] = "Failed Login";
                    return View("Details");
                }
            }

            var errorsModelState = "";
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errorsModelState += error.ErrorMessage + ";";
                }
            }

            TempData["errorLogin"] = errorsModelState;
            return RedirectToAction("Details");
        }

        [Authorize]
        public async Task<IActionResult> MergeAccountBySocialNetworks()
        {
            var userOrSessionId = _userManager.GetUserId(HttpContext.User);
            int currentUserId = int.Parse(_userManager.GetUserId(User));
            var result = await _authService.MergeAccountBySocialNetworks(currentUserId, userOrSessionId);

            if(result)
            {
                TempData["mergeAccount"] = "true";
            }

            return View("Details");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ResetPasswordModel model)
        {
            var passwordState = ModelState["Password"].ValidationState;
            var passwordConfirmState = ModelState["ConfirmPassword"].ValidationState;

            if (passwordState.ToString() == "Invalid" || passwordConfirmState.ToString() == "Invalid")
            {
                var errorsModelState = "";
                var passwordStateErrors = ModelState["Password"].Errors;
                var passwordConfirmStateErrors = ModelState["ConfirmPassword"].Errors;

                foreach (var error in passwordStateErrors)
                {
                    errorsModelState += error.ErrorMessage + ";";
                }

                foreach (var error in passwordConfirmStateErrors)
                {
                    errorsModelState += error.ErrorMessage + ";";
                }

                TempData["changePassword"] = "true";
                TempData["errorResetPassword"] = errorsModelState;
                return RedirectToAction("Details");
            }

            model.Email = User.Identity.Name;
            model.Code = "ChangePassword";
            var result = await _authService.ResetPasswordMVC(model);
            if (result == null)
            {
                return RedirectToAction("Details");
            }

            var errors = "";
            foreach (var error in result)
            {
                errors += error.Code + ";";
            }

            TempData["changePassword"] = "true";
            TempData["errorResetPassword"] = errors;
            return RedirectToAction("Details");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult OpenLoginTab()
        {
            TempData.Clear();
            TempData["openLoginTab"] = "true";
            return RedirectToAction("Index", "Shop");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            await _signInManager.SignOutAsync();
            var relultDelete = await _userManager.DeleteAsync(user);

            return RedirectToAction("Index", "Shop");
        }

        [Authorize]
        public async Task<IActionResult> LinkExternalAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            await _authService.LinkExternalAccount(user);
            return RedirectToAction("Details");
        }

        [Authorize]
        public async Task<IActionResult> UnlinkExternalAccount(string provider)
        {
            var user = await _userManager.GetUserAsync(User);
            await _authService.UnlinkExternalAccount(user, provider);
            return RedirectToAction("Details");
        }

        [Authorize]
        public IActionResult Details()
        {
            return View();
        }

        [Authorize]
        [Route("/Account/OrderHistory")]
        public IActionResult OrderHistoryDetails()
        {
            return View("OrderHistory");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(ShopController.Index), "Shop");

        }

        [HttpGet]
        [Authorize]
        public IActionResult WishListUser()
        {
            int currentUserId = int.Parse(_userManager.GetUserId(User));
            var wishListUser = _wishListService.GetWishListsUser(currentUserId);
            if (wishListUser.Count > 0)
                return View("WishList", wishListUser.First());

            return View("WishList", new WishListModel());
        }
    }
}
