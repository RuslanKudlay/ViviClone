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
    public class CategoryController : ApplicationApiController
    {
        private ICategoryService categoryService;

        public CategoryController(ICategoryService _categoryService, UserManager<ApplicationUser> manager) : base(manager)
        {
            categoryService = _categoryService;
        }

        [HttpPost]
        [Route("api/Category/all")]
        public IActionResult GetAll(QueryCategoryModel query)
        {           
            return InvokeMethod(categoryService.GetAll, query);
        }

        [HttpPost]
        [Route("api/Category")]
        public IActionResult Save([FromBody]CategoryModel post)
        {           
            return InvokeMethod(categoryService.Add, post);
        }

        [HttpGet]
        [Route("api/Category/{subUrl}")]
        public IActionResult Get(string subUrl)
        {
            return InvokeMethod(categoryService.GetBySubUrl, subUrl);
        }

        [HttpPut]
        [Route("api/Category")]
        public IActionResult Update([FromBody]CategoryModel model)
        {           
            return InvokeMethod(categoryService.Update, model);           
        }

        [HttpDelete]
        [Route("api/Category/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            return InvokeMethod(categoryService.Delete, id);
        }
    }
}
