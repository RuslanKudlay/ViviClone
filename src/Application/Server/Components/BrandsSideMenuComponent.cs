using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class BrandsSideMenuComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(SideSearchMenuModel sideSearchMenuModel)
        {
            return View("Index", sideSearchMenuModel);
        }
    }
}
