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
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnnouncementController : ApplicationApiController
    {
        private IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService, UserManager<ApplicationUser> manager) : base(manager)
        {
            _announcementService = announcementService;
        }


        [HttpPost]
        [Route("api/Announcement")]
        public IActionResult Save([FromBody]AnnouncementModel model)
        {
            return InvokeMethod(_announcementService.Add, model);
        }

        [HttpPut]
        [Route("api/Announcement")]
        public IActionResult Update([FromBody]AnnouncementModel model)
        {
            return InvokeMethod(_announcementService.Update, model);
        }

        [HttpPost]
        [Route("api/Announcement/all")]
        public IActionResult GetAll(QueryAnnouncementModel model)
        {
            return InvokeMethod(_announcementService.GetAll, model);        
        }

        [HttpGet]
        [Route("api/Announcement/{subUrl}")]
        public IActionResult Get(string subUrl)
        {
            return InvokeMethod(_announcementService.GetBySubUrl, subUrl);
        }

        [HttpDelete]
        [Route("api/Announcement/Delete/{id}")]
        public IActionResult Delete(int id, [FromQuery(Name = "subUrl")] string subUrl)
        {
            if (id > 0)
                return InvokeMethod(_announcementService.Delete, id);

            return InvokeMethod(_announcementService.Delete, subUrl);
        }

    }
}
