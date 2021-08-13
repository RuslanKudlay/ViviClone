using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class WareSpecifications : ViewComponent
    {
        private readonly ICategoryValuesService _categoryValuesService;

        public WareSpecifications(ICategoryValuesService categoryValuesService)
        {
            _categoryValuesService = categoryValuesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int wareId)
        {
            var categoryValues = _categoryValuesService.GetByWareId(wareId);
            return View("Index", categoryValues);
        }
    }
}
