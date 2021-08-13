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
    public class SideMenuGOWComponent : ViewComponent
    {
        private readonly IGOWService _groupOfWare;

        public SideMenuGOWComponent(IGOWService groupOfWare)
        {
            _groupOfWare = groupOfWare;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = new MenuModel()
            {
                GroupOfWares = _groupOfWare.GetTreeGOW()
            };

            return View("Index", result);
        }
    }    
}
