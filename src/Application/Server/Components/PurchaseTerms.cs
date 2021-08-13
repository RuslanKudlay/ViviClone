using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class PurchaseTerms : ViewComponent
    {
        public PurchaseTerms()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View("Index");
        }
    }
}

