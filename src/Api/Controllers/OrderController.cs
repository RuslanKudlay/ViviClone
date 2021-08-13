using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.OrderModels;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ApplicationApiController
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISmtpClient _smtpClient;

        public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager, ISmtpClient smtpClient) : base(userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
            _smtpClient = smtpClient;
        }

        [HttpPost]
        [Route("api/Order/all")]
        public IActionResult Get([FromBody]QueryOrderModel query)
        {
            return InvokeMethod(_orderService.Get, query);
        }

        [HttpGet]
        [Route("api/Order/{id}")]
        public IActionResult GetById(Guid id)
        {
            return InvokeMethod(_orderService.GetById, id);
        }

        [HttpDelete]
        [Route("api/Order/Delete/{orderId}")]
        public IActionResult Delete(Guid orderId)
        {
            return InvokeMethod(_orderService.Delete, orderId);
        }

        [HttpGet]
        [Route("api/Order/Statuses/all")]
        public IActionResult GetStatuses()
        {
            try
            {
                return Ok(_orderService.GetStatuses());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("api/Order/DeliveryServices/all")]
        public IActionResult GetDeliveryServices()
        {
            try
            {
                return Ok(_orderService.GetDeliveryServices());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("api/Order")]
        public IActionResult SaveOrUpdate([FromBody]OrderModel model)
        {
            return InvokeMethod(_orderService.AddOrUpdateOrder, model);
        }

        [HttpPost]
        [Route("api/Order/Send")]
        public async Task<JsonResult> SendLetter([FromBody]OrderModel order)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                _smtpClient.SendMail(order.User.Email, $"Your order {order.OrderNumber} has arrived", $"Your order {order.OrderNumber} has arrived. Please, pick up your order in the delivery service in the near future");
                return new JsonResult(new { IsSuccessed = true });
            }
            catch
            {
                return new JsonResult(new { IsSuccessed = false });
            }
        }
    }
}
