using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
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
    public class CategoryValuesController : ApplicationApiController
    {
        private ICategoryValuesService _categoryValuesService;

        public CategoryValuesController(ICategoryValuesService categoryValuesService, UserManager<ApplicationUser> manager) : base(manager)
        {
            _categoryValuesService = categoryValuesService;
        }

        [HttpPost]
        [Route("api/CategoryValues")]
        public IActionResult Add([FromBody]CategoryValuesModel model)
        {
            return InvokeMethod(_categoryValuesService.Add, model);           
        }

        [HttpPost]
        [Route("api/CategoryValues/all")]
        public IActionResult Get()
        {
            return InvokeMethod(_categoryValuesService.GetAll);
        }

        [HttpGet]
        [Route("api/CategoryValues")]
        public IActionResult Get([FromQuery(Name = "id")] int id, [FromQuery(Name = "subUrl")] string subUrl)
        {
            return InvokeMethod(_categoryValuesService.GetById, id);
        }


        [HttpDelete]
        [Route("api/CategoryValues")]
        public IActionResult Delete([FromQuery(Name = "id")] int id, [FromQuery(Name = "subUrl")] string subUrl)
        {
            return InvokeMethod(_categoryValuesService.Delete, id);
        }

        [HttpPost]
        [Route("api/CategoryValues")]
        public IActionResult Update([FromBody]CategoryValuesModel model)
        {
            return InvokeMethod(_categoryValuesService.Update, model);
        }
    }
}
