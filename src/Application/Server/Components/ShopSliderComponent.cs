using Application.BBLInterfaces.BusinessServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class ShopSliderComponent: ViewComponent
    {
        private readonly ISliderService _sliderService;

        public ShopSliderComponent(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliderModels = _sliderService.Get();
            return View("Index", sliderModels);
        }
    }
}
