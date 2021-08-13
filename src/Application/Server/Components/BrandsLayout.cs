using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class BrandsLayout : ViewComponent
    {
        private IBrandService brandService;



        public BrandsLayout(IBrandService _brandService)
        {
            brandService = _brandService;           
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brandsItem = brandService.Get();

            return View("Index", brandsItem);
        }

    }
}
