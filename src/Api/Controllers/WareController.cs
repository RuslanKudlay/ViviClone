using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WareController : ApplicationApiController
    {
        private IWareService _wareService;

        public WareController(IWareService wareService, UserManager<ApplicationUser> manager) : base(manager)
        {
            _wareService = wareService;
        }

        [HttpPost]
        [HttpPut]
        [Route("api/Ware")]
        public IActionResult SaveOrUpdate([FromBody]WareModel model)
        {
            return InvokeMethod(_wareService.SaveOrUpdate, model);            
        }
        [HttpPost]
        [Route("api/Ware/all")]
        public IActionResult Get([FromBody]QueryWaresModel query)
        {
            return InvokeMethod(_wareService.GetAll, query);
        }

        [HttpGet]
        [Route("api/Ware/{subUrl}")]
        public IActionResult GetById(string subUrl)
        {
            return InvokeMethod(_wareService.GetWareBySubUrl, subUrl);
        }

        [HttpDelete]
        [Route("api/Ware/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            return InvokeMethod(_wareService.Delete, id);
        }        

        [HttpPost]
        [Route("api/Ware/GetWaresByPaging")]
        public IActionResult GetWarePaging([FromBody] PaginationRequest pagination)
        {
            return InvokeMethod(_wareService.GetNextWares, pagination);
        }
        [HttpPut]
        [Route("api/Ware/AddToBestsellers/{id}")]
        public IActionResult AddToBestseller(int id)
        {
            return InvokeMethod(_wareService.AddToBestsellers, id);
        }
        [HttpPut]
        [Route("api/Ware/RemoveFromBestsellers/{id}")]
        public IActionResult RemoveFromBestsellers(int id)
        {
            return InvokeMethod(_wareService.RemoveFromBestsellers, id);
        }
    } 
}
