using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class GroupOfWareByParentComponent : ViewComponent
    {
        private readonly IGOWService _groupOfWares;

        public GroupOfWareByParentComponent(IGOWService groupOfWares)
        {
            _groupOfWares = groupOfWares;
        }
        public async Task<IViewComponentResult> InvokeAsync(string groupOfWaresName = null)
        {
            var groupOfWare = new { };//_groupOfWares.GetAll();

            return View("Index", groupOfWare);
        }

    }    
}
