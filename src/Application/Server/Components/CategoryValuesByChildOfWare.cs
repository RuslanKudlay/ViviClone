using Application.BBLInterfaces.BusinessServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class CategoryValuesByChildOfWare : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public CategoryValuesByChildOfWare(ICategoryService _categoryService)
        {
            categoryService = _categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string groupOfWaresName)
        {
            var categoryWithCategoryValues = categoryService.FindCategoriesByGOW(groupOfWaresName,true);           

            return View("Index", categoryWithCategoryValues?.Take(3));
        }
    }
}
