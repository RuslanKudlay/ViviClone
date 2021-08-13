using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.BBLInterfaces.BusinessServicesInterfaces;

namespace Application.Server.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult AllCategories()
        {
            //var enabledCategories = _categoryService.GetEnabledCategories();

            return null;// PartialView("AllCategories", enabledCategories);

        }
    }
}