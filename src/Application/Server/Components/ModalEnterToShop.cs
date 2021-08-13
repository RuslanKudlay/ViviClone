using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class ModalEnterToShop: ViewComponent
    {
        public ModalEnterToShop()
        {
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Index");
        }
    }
}
