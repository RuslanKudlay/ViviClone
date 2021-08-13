using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.Utils;
using Application.EntitiesModels.Models;

namespace Application.Server.Controllers
{
    public class SitemapController : Controller
    {
        private IPromotionService _promotionService;
        private IAnnouncementService _announcementService;
        private IBrandService _brandService;

        public SitemapController(
            IPromotionService promotionService,
            IAnnouncementService announcementService,
            IBrandService brandService)
        {
            _promotionService = promotionService;
            _announcementService = announcementService;
            _brandService = brandService;
        }
        // GET: Sitemap
        public IActionResult Index()
        {
            var sitemapItems = new List<SitemapItem>
            {
                new SitemapItem(PathUtils.CombinePaths(HttpContext.Request.Host.ToString(), "")),
                //new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority), "/home/About")),
                new SitemapItem(PathUtils.CombinePaths(HttpContext.Request.Host.ToString(), "/Contact"))
            };

            foreach (var item in _brandService.Get())
            {
                sitemapItems.Add(
                    new SitemapItem(
                        PathUtils.CombinePaths(HttpContext.Request.Host.ToString(), "Brand/" + item.SubUrl), null,
                        SitemapChangeFrequency.Monthly
                        ));
            }

            foreach (var item in _promotionService.Get())
            {
                sitemapItems.Add(
                    new SitemapItem(
                        PathUtils.CombinePaths(HttpContext.Request.Host.ToString(), "Promotion/" + item.SubUrl),
                        item.Date,
                        SitemapChangeFrequency.Daily
                        ));
            }

            foreach (var item in _announcementService.Get())
            {
                sitemapItems.Add(
                    new SitemapItem(
                        PathUtils.CombinePaths(HttpContext.Request.Host.ToString(), "Announcement/" + item.SubUrl), null,
                        //item.Date,
                        SitemapChangeFrequency.Daily
                        ));
            }


            return new SitemapResult(sitemapItems);
        }
    }
}