﻿using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class AuthorizationComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Index");
        }
    }
}
