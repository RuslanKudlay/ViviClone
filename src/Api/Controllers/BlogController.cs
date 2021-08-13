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
    public class BlogController : ApplicationApiController
    {
        private IBlogService blogService;

        public BlogController(IBlogService _blogService, UserManager<ApplicationUser> manager) : base(manager)
        {
            blogService = _blogService;
        }       

        [HttpGet]
        [Route("api/Post/{suburl}")]
        public IActionResult Get(string subUrl)
        {
            return InvokeMethod(blogService.GetPost, subUrl);
        }

        [HttpPost]
        [Route("api/Post/all")]
        public IActionResult GetAll([FromBody]QueryPostModel query)
        {
            return InvokeMethod(blogService.GetPosts, query);
        }

        [HttpPost]
        [Route("api/Post")]
        public IActionResult Save([FromBody]PostModel post)
        {            
            return InvokeMethod(blogService.SaveOrUpdate, post);
        }

        [HttpPut]
        [Route("api/Post")]
        public IActionResult Update([FromBody]PostModel post)
        {
            return InvokeMethod(blogService.SaveOrUpdate, post);
        }      

        [HttpDelete]
        [Route("api/Post/Delete/{id}")]
        public IActionResult Delete(int id)
        {             
            return InvokeMethod(blogService.DeletePost, id);
        }       

    }
}
