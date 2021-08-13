using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models.QueryModels;

namespace Application.Server.Controllers
{    
    public class BlogController : Controller
    {
        private readonly IBlogService blogService;

        public BlogController(IBlogService _blogService)
        {
            blogService = _blogService;
        }

        public ActionResult Index()
        {
            ViewBag.Paged = null;

            var posts = blogService.GetPosts(new QueryPostModel());

            return View(posts);
        }

        [HttpPost]
        public ActionResult Index(QueryPostModel query)
        {
            ViewBag.Paged = true; 
            var posts = blogService.GetPosts(query);
            return View(posts);
        }

        [HttpPost]
        public ActionResult GetPost([FromRoute] string subUrl)
        {
            var post = blogService.GetPost(subUrl);

            return PartialView("Post", post);
        }

    }
}