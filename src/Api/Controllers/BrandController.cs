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
using System.Threading.Tasks;

namespace Application.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BrandController : ApplicationApiController
    {
        private IBrandService _brandService;

        public BrandController(IBrandService brandService, UserManager<ApplicationUser> manager) : base(manager)
        {
            _brandService = brandService;
        }

        [HttpPost]
        [Route("api/Brand")]
        public IActionResult Save([FromBody] BrandModel brand)
        {
            return InvokeMethod(_brandService.AddBrand, brand);
        }

        [HttpPut]
        [Route("api/Brand")]
        public IActionResult Update([FromBody] BrandModel brand)
        {
            return InvokeMethod(_brandService.UpdateBrand, brand);
        }

        [HttpPost]
        [Route("api/Brand/all")]
        public IActionResult GetAll(QueryBrandModel query)
        {
            return InvokeMethod(_brandService.GetAll, query);          
        }

        [HttpGet]
        [Route("api/Brand/{subUrl}")]
        public IActionResult Get(string subUrl)
        {
            return InvokeMethod(_brandService.GetBrandBySubUrl, subUrl);
        }

        [HttpDelete]
        [Route("api/Brand/Delete/{id:int}")]
        public IActionResult Delete(int id, [FromQuery(Name = "subUrl")] string subUrl)
        {
            if(id > 0)
                return InvokeMethod(_brandService.Delete, id);     
            
            return InvokeMethod(_brandService.Delete, subUrl);
        }

    }
}
