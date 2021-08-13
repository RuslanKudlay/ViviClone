using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class BestsellersComponent : ViewComponent
    {
        private readonly IWareService _wareService;
        public BestsellersComponent(IWareService wareService)
        {
            _wareService = wareService;
        }
        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal principal)
        {
            var queryWares = _wareService.GetBestsellers(principal);
            return View("Index", queryWares);
        }
    }
}
