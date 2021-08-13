using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class UserInfoComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IModelMapper modelMapper;

        public UserInfoComponent(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IModelMapper modelMapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.modelMapper = modelMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           var userInfo = userManager.GetUserAsync(HttpContext.User);

           var result = modelMapper.MapTo<ApplicationUser, UserModel>(await userInfo);

           result.Logins = (await userManager.GetLoginsAsync(await userInfo)).Select(s => s.LoginProvider).ToList();

           return View("Index", result);
        }
    }
}
