using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PromotionController : ApplicationApiController
    {
        private IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService, UserManager<ApplicationUser> manager) : base(manager)
        {
            _promotionService = promotionService;
        }

        [HttpPost]
        [Route("api/Promotion")]
        public IActionResult Save([FromBody]PromotionModel model)
        {
            return InvokeMethod(_promotionService.Add, model);
        }


        [HttpPut]
        [Route("api/Promotion")]
        public IActionResult Update([FromBody]PromotionModel model)
        {
            return InvokeMethod(_promotionService.Update, model);
        }

        [HttpPost]
        [Route("api/Promotion/all")]
        public IActionResult GetAll(QueryPromotionModel query)
        {
            return InvokeMethod(_promotionService.GetAll, query);          
        }

        [HttpGet]
        [Route("api/Promotion/{subUrl}")]
        public IActionResult Get(string subUrl)
        {
            return InvokeMethod(_promotionService.GetBySubUrl, subUrl);
        }


        [HttpDelete]
        [Route("api/Promotion/Delete/{id}")]
        public IActionResult Delete(int id, [FromQuery(Name = "subUrl")] string subUrl)
        {
            if(id > 0)
             return InvokeMethod(_promotionService.Delete, id);

            return InvokeMethod(_promotionService.Delete, subUrl);
        }
        
    }
}
