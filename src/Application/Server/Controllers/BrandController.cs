using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandService brandservice;

        public BrandController(IBrandService brandservice)
        {
            this.brandservice = brandservice;
        }

        public IActionResult Index()
        {
            IEnumerable<BrandModel> brands = brandservice.Get();
            return View(brands);
        }

        public IActionResult BrandDetails(string suburl)
        {
            var brand = brandservice.GetBrandBySubUrl(suburl);
            return View("Brand", brand);
        }
    }
}
