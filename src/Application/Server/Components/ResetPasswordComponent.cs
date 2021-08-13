using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class ResetPasswordComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string email = null)
        {
            if (email != null)
            {
                ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
                resetPasswordModel.Email = email;

                return View("Index", resetPasswordModel);
            }

            return View("Index");
        }
    }
}
