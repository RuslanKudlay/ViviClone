using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components.OnlineChat
{
    public class ChatComponent : ViewComponent
    {

        public ChatComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Index");
        }
    }
}