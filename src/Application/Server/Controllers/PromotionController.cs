using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;

namespace Application.Server.Controllers
{
    public class PromotionController : Controller
    {
        private IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        public ActionResult GetAllPromotion()
        {
            var promotions = _promotionService.Get().OrderBy(p => p.LastUpdateDate).ToList();
            return View(promotions);
        }

        //public ActionResult GetPromotion(int id)
        //{
        //    var promotion = _promotionService.GetById(id);
        //    return View(promotion);
        //}

        public ActionResult GetPromotion(string subUrl)
        {
            var promotion = _promotionService.GetBySubUrl(subUrl);
            return View(promotion);
        }
    }
}