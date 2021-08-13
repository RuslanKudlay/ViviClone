using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class BasketComponent : ViewComponent
    {
        private readonly IBasketService basketService;

        public BasketComponent(IBasketService _basketService)
        {
            basketService = _basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isFullDisplay = false)
        {
            var basket = basketService.GetBasket();

            basket.IsFullDiplay = isFullDisplay;

            return View("Index", basket);
        }
    }
}
