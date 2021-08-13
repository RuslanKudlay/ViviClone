using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class AnnouncementPromoution : ViewComponent
    {
        private IAnnouncementService announcementService;
        private IPromotionService promotionService;



        public AnnouncementPromoution(IAnnouncementService _announcementService, IPromotionService _promotionService)
        {
            announcementService = _announcementService;
            promotionService = _promotionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var promotions =  promotionService.Get().OrderBy(p => p.LastUpdateDate).Take(2).ToList();
            var announcements = announcementService.Get();

            announcements.OrderBy(p => p.LastUpdateDate).Take(2).ToList();

            var news = new NewsModel();
            news.Announcements = announcements;
            news.Promotions = promotions;

            return View("Index",news);
        }

    }
}
