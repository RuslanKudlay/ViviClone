using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using System.Text;
using Application.Utils;
using Application.EntitiesModels.Models;

namespace Application.Server.Controllers
{
    public class HomeController : Controller
    {
        private IAnnouncementService _announcementService;
        private IPromotionService _promotionService;
        private IBrandService _brandService;

        public HomeController(
            IBrandService brandService,
            IPromotionService promotionService,
            IAnnouncementService announcementService
            //IFeedbackService feedbackService,
            //IMenuItemService menuItemService,
            //IArticleService articleService
            )
        {
            _brandService = brandService;
            _promotionService = promotionService;
            _announcementService = announcementService;
            //_categoryService = categoryService;
            //_feedbackService = feedbackService;
            //_menuItemService = menuItemService;
            //_articleService = articleService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: ");
            stringBuilder.AppendLine("");
            stringBuilder.Append("Sitemap: " + PathUtils.CombinePaths(HttpContext.Request.Host.ToString(), "/sitemap"));

            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult BrandsLayout()
        {
            var brandsItem = _brandService.Get();
            return PartialView(brandsItem);
        }

        //public ActionResult GetBrand(int id)
        //{
        //    var brandItem = _brandService.GetBrandById(id);
        //    return View(brandItem);
        //}

        public ActionResult GetBrand(string subUrl)
        {
            var brandItem = _brandService.GetBrandBySubUrl(subUrl);
            return View(brandItem);
        }
        public ActionResult GetAnnouncementPromoution()
        {
            var promotions = (_promotionService.Get()).OrderBy(p => p.LastUpdateDate).Take(2).ToList();
            var announcements = (_announcementService.Get()).OrderBy(p => p.LastUpdateDate).Take(2).ToList();

            var news = new NewsModel();
            news.Announcements = announcements;
            news.Promotions = promotions;

            return View(news);
        }
    }
}