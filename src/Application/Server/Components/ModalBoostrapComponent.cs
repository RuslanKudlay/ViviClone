using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class ModalBoostrapComponent : ViewComponent
    {
        public ModalBoostrapComponent()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Index");
        }
    }
}
