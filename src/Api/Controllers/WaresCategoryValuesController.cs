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
    public class WaresCategoryValuesController : ApplicationApiController
    {
        private IWaresCategoryValuesService _waresCategoryValuesService;      

        public WaresCategoryValuesController(IWaresCategoryValuesService wareService, UserManager<ApplicationUser> manager) : base(manager)
        {
            _waresCategoryValuesService = wareService;
        }

        [HttpPost]
        [Route("api/WaresCategoryValue/Add")]
        public IActionResult Add([FromBody]WaresCategoryValuesModel model)
        {
            return InvokeMethod(_waresCategoryValuesService.Add, model);           
        }

        [HttpGet]
        [Route("api/WaresCategoryValue")]
        public IActionResult Get()
        {
            return InvokeMethod(_waresCategoryValuesService.GetAll);
        }

        [HttpGet]
        [Route("api/WaresCategoryValue/{id}")]
        public IActionResult GetById(int id)
        {
            return InvokeMethod(_waresCategoryValuesService.GetById, id);
        }


        [HttpDelete]
        [Route("api/WaresCategoryValue/{id}")]
        public IActionResult Delete(int id)
        {
            return InvokeMethod(_waresCategoryValuesService.Delete, id);
        }

        [HttpPost]
        [Route("api/WaresCategoryValue/Update")]
        public IActionResult Update([FromBody]WaresCategoryValuesModel model)
        {
            return InvokeMethod(_waresCategoryValuesService.Update, model);
        }
    }
}
