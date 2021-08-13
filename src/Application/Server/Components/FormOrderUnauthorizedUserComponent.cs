using Application.EntitiesModels.Models.ViewsModel.RegistrationModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class FormOrderUnauthorizedUserComponent : ViewComponent
    {
        public FormOrderUnauthorizedUserComponent()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Index", new RegisterViewModel());
        }
    }
}
