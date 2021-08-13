using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class WaresComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(QueryWaresModel wares)
        {
            return View("Index", wares);
        }
    }    
}
