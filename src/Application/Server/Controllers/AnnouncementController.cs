using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;

namespace Application.Server.Controllers
{
    public class AnnouncementController : Controller
    {
        private IAnnouncementService _announcementService;

        public AnnouncementController(
            IAnnouncementService announcementService
            )
        {
            _announcementService = announcementService;
        }


        public ActionResult GetAllAnnouncement()
        {
            var announcements = (_announcementService.Get()).OrderBy(p => p.LastUpdateDate).ToList();
            return View(announcements);
        }

        public ActionResult GetAnnouncement(int id)
        {
            var announcement = _announcementService.GetById(id);
            return View(announcement);
        }

        //public ActionResult GetAnnouncement(string subUrl)
        //{
         //   var announcement = _announcementService.GetBySubUrl(subUrl);
         //   return View(announcement);
       // }
    }
}