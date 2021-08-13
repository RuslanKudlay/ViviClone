using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.ViewsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class WareQuantityInBasketComponent : ViewComponent
    {
        private readonly IBasketService _basketService;

        public WareQuantityInBasketComponent(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int itemsQuantity = _basketService.GetWareQuantityInBasket();
            return View("Index", itemsQuantity);
        }
    }
}
