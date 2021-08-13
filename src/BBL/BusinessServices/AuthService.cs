using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.Chat;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.Auth;
using Application.EntitiesModels.Models.ViewsModel.RegistrationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class AuthService : IAuthService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISmtpClient _smtpClient;
        private ILogger<AuthService> _logger;
        private IWishListService _wishListService;

        public const string KEY = "mysupersecret_secretkey!123";

        public AuthService(IDbContextFactory clistahrDbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ISmtpClient smtpClient,
            ILogger<AuthService> logger,
            IWishListService wishListService)
        {
            _dbContextFactory = clistahrDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _smtpClient = smtpClient;
            _logger = logger;
            _wishListService = wishListService;
        }

        public async Task<string> Register(UserRegisterViewModel model)
        {
            var applicationUser = new ApplicationUser()
            {
                CreatedDate = DateTime.Now,
                FirstName = model.FirstName,
                LastName = model.SecondName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                NormalizedEmail = model.Email.Normalize().ToUpperInvariant(),
                EmailConfirmed = false,
                IsEnabled = true,
                NormalizedUserName = model.Email.Normalize().ToUpperInvariant(),
                UserName = model.Email
            };

            var resultRegistration = await _userManager.CreateAsync(applicationUser, model.Password);

            if (!resultRegistration.Succeeded)
            {
                var errorsRegistration = "";
                foreach (var error in resultRegistration.Errors)
                {
                    errorsRegistration += error.Code + ";";
                }

                return errorsRegistration;
            }

            return applicationUser.Email;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            await _userManager.AddToRoleAsync(user, "User");
            var resultConfirmPassword = await _userManager.ConfirmEmailAsync(user, code);
            if (resultConfirmPassword.Succeeded)
            {
                await _wishListService.CreateWishList(user);
                await _signInManager.SignInAsync(user, false);

                return true;
            }
            else
                return false;
        }

        public async Task<string> LoginMVC(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);

                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return model.ReturnUrl;
                    }
                    else
                    {
                        return "LoginSuccess";
                    }
                }
            }
            return "LoginFailed";
        }

        public async Task<string> CompleteRegistration(CompleteRegistrationModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                var result = await _userManager.ConfirmEmailAsync(user, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    return await GenerateJwtToken(user);
                }
                return null;
            }
        }

        public async Task<bool> ResponseAfterSocialNetworks()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return true;
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                ApplicationUser user = new ApplicationUser
                {
                    CreatedDate = DateTime.Now,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Name).Value,
                    IsEnabled = true,
                    NormalizedEmail = info.Principal.FindFirst(ClaimTypes.Email).Value.ToUpperInvariant(),
                    EmailConfirmed = true,
                    NormalizedUserName = info.Principal.FindFirst(ClaimTypes.Name).Value.ToUpperInvariant(),
                    FirstName = info.Principal.FindFirst(ClaimTypes.Name).Value.Split(' ').First(),
                    LastName = info.Principal.FindFirst(ClaimTypes.Name).Value.Split(' ').Last()
                };

                IdentityResult createUserResult = await _userManager.CreateAsync(user);

                if (createUserResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (addLoginResult.Succeeded)
                    {
                        await _wishListService.CreateWishList(user);
                        await _signInManager.SignInAsync(user, false);
                        return true;
                    }
                }
                else
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        // If ExternalProvider has a same Email like other user account
                        // Then attach ExternalProvider to account (because we accepted email by letter)
                        if (error.Code == "DuplicateEmail")
                        {
                            var existUser = await _userManager.FindByEmailAsync(user.Email);
                            var userEmail = existUser.Email;
                            var resultChangeEmail = await _userManager.SetEmailAsync(existUser, "fixProblemDuplicateEmail@fix.com");

                            if (resultChangeEmail.Succeeded)
                            {
                                var addLoginResult = await _userManager.AddLoginAsync(existUser, info);
                                if (addLoginResult.Succeeded)
                                {
                                    resultChangeEmail = await _userManager.SetEmailAsync(existUser, userEmail);

                                    if (resultChangeEmail.Succeeded)
                                    {
                                        await _wishListService.CreateWishList(user);
                                        await _signInManager.SignInAsync(existUser, false);
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                }
                return true;
            }
        }

        public async Task<List<IdentityError>> ResetPasswordMVC(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return null;
            }

            IdentityResult resultResetPassword = null;
            var oldPassword = user.PasswordHash;
            var isOldAndNewPasswordEqual = _userManager.PasswordHasher.VerifyHashedPassword(user, oldPassword, model.Password);
            if (isOldAndNewPasswordEqual.ToString() == "Success")
            {
                var errors = new List<IdentityError>()
                    {
                        new IdentityError() {
                            Code = "OldAndNewPasswordEqual",
                            Description = "Old and new Password shouldn't be equal"
                    }};
                return errors;
            }

            if (model.Code == "ChangePassword")
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                resultResetPassword = await _userManager.ResetPasswordAsync(user, code, model.Password); ;
            }
            else
            {
                resultResetPassword = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            }

            if (resultResetPassword.Succeeded)
            {
                return null;
            }

            return resultResetPassword.Errors.ToList();
        }

        public async Task<bool> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return false;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _smtpClient.SendMail(user.Email, "BD | Password Recovery", $"Your password recovery code --> {code}");

            return true;
        }

        public async Task<string> Login(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any(role => role.Contains("Admin")))
                {
                    return await GenerateJwtToken(user);
                }
            }
            return null;
        }

        public async Task<bool> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.ResetPasswordAsync(user, model.Code.Replace(" ", "+"), model.Password);
            if (result.Succeeded) return true;
            return false;
        }

        public async Task<bool> SignUp(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _smtpClient.SendMail(user.Email, "BD | Email Confirmation", $"Your email confirmation code {code}");
                return true;
            }

            return false;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(14),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public async Task<bool> MergeAccounts(int currentUserId, int targetUserId, string userOrSessionId = null)
        {
            using (var context = _dbContextFactory.Create())
            {
                var targetUserOrders = context.Orders.Where(w => w.UserId == targetUserId).Select(s => s).ToList();
                var targetUserOrderHistory = context.OrderHistories.Where(w => w.UserId == targetUserId).Select(s => s).ToList();
                var targetUserWareVotes = context.WareVotes.Where(w => w.UserId == targetUserId).Select(s => s).ToList();
                var targetUserWishLists = context.WishLists.Where(w => w.UserId == targetUserId).Select(s => s).ToList();
                List<Chat> targetUserChats = null;
                if (userOrSessionId != null)
                {
                    targetUserChats = context.Chats.Where(w => w.SessionOrUserId == userOrSessionId).Select(s => s).ToList();
                }
                else
                {
                    targetUserChats = context.Chats.Where(w => w.SessionOrUserId == targetUserId.ToString()).Select(s => s).ToList();
                }

                targetUserOrders.ForEach(f => f.UserId = currentUserId);
                targetUserOrderHistory.ForEach(f => f.UserId = currentUserId);
                targetUserWareVotes.ForEach(f => f.UserId = currentUserId);
                targetUserChats.ForEach(f => f.SessionOrUserId = currentUserId.ToString());
                targetUserWishLists.ForEach(f => f.UserId = currentUserId);

                context.SaveChanges();

                await _userManager.DeleteAsync(await _userManager.FindByIdAsync(targetUserId.ToString()));
            }
            return true;
        }

        public async Task<bool> MergeAccountByEmail(UserModel model, int currentUserId)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var isEnabled = await _userManager.CheckPasswordAsync(user, model.Password);
                if (isEnabled)
                {
                    var resultMerge = await MergeAccounts(currentUserId, user.Id);
                    if (resultMerge)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        
        public async Task<bool> MergeAccountBySocialNetworks(int currentUserId, string userOrSessionId)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
            {
                var email = info.Principal.FindFirst(ClaimTypes.Email).Value;
                var targetUser = await _userManager.FindByEmailAsync(email);

                if (targetUser != null)
                {
                    var targetUserLogins = await _userManager.GetLoginsAsync(targetUser);
                    var signInBySocialNetworks = targetUserLogins
                        .Where(w => w.LoginProvider == info.LoginProvider && w.ProviderKey == info.ProviderKey).FirstOrDefault();

                    if (signInBySocialNetworks != null)
                    {
                        bool resultMerge;

                        if (userOrSessionId != currentUserId.ToString())
                        {
                            resultMerge = await MergeAccounts(currentUserId, targetUser.Id, userOrSessionId);
                        }
                        else
                        {
                            resultMerge = await MergeAccounts(currentUserId, targetUser.Id);
                        }

                        if (resultMerge)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public async Task<bool> LinkExternalAccount(ApplicationUser user)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
            {
                var identResult = await _userManager.AddLoginAsync(user, info);
                return true;
            }
            return false;
        }
        
        public async Task<bool> UnlinkExternalAccount(ApplicationUser user, string provider)
        {
            var userLogins = await _userManager.GetLoginsAsync(user);

            var userLogin = userLogins.Where(w => w.LoginProvider == provider).Select(s => s).FirstOrDefault();
            var identResult = await _userManager.RemoveLoginAsync(user, userLogin.LoginProvider, userLogin.ProviderKey);

            if(identResult.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
