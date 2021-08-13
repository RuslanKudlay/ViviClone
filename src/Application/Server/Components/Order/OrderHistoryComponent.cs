using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components.Order
{
    public class OrderHistoryComponent : ViewComponent
    {
        private readonly IOrderService orderService;
        private readonly UserManager<ApplicationUser> userManager;


        public OrderHistoryComponent(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            this.orderService = orderService;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            QueryOrderModel queryOrder = new QueryOrderModel()
            {
                UserId = int.Parse(userManager.GetUserId(HttpContext.User)),
                Take = 10,
                Skip = 0
            };
            
            return View("Index", orderService.OrderQuery(queryOrder));
        }

    }
}
