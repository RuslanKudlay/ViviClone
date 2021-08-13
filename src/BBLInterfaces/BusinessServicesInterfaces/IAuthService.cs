using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.Auth;
using Application.EntitiesModels.Models.ViewsModel.RegistrationModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IAuthService
    {
        /* Web API */
        Task<string> Login(LoginModel model);
        Task<string> CompleteRegistration(CompleteRegistrationModel model);
        Task<bool> SignUp(SignUpModel model);
        Task<bool> ForgotPassword(ForgotPasswordModel model);
        Task<bool> ResetPassword(ResetPasswordModel model);

        /* MVC */
        Task<string> Register(UserRegisterViewModel model);
        Task<bool> ConfirmEmail(string userId, string code);
        Task<string> LoginMVC(LoginViewModel model);
        Task<bool> ResponseAfterSocialNetworks();
        Task<List<IdentityError>> ResetPasswordMVC(ResetPasswordModel model);
        Task<bool> MergeAccounts(int currentUserId, int targetUserId, string userOrSessionId = null);
        Task<bool> MergeAccountByEmail(UserModel model, int currentUserId);
        Task<bool> MergeAccountBySocialNetworks(int currentUserId, string userOrSessionId);
        Task<bool> LinkExternalAccount(ApplicationUser user);
        Task<bool> UnlinkExternalAccount(ApplicationUser user, string provider);
    }
}
