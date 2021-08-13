using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
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
    public class GOWController : ApplicationApiController
    {
        private IGOWService _gowService;

        private readonly IBrandService brandService;

        public GOWController(IGOWService gowService, UserManager<ApplicationUser> manager, IBrandService brandService) : base(manager)
        {
            _gowService = gowService;
            this.brandService = brandService;
        }

        [HttpPost]
        [Route("api/GroupOfWares")]
        public IActionResult Add([FromBody]GOWModel model)
        {
            return InvokeMethod(_gowService.Add, model);
        }

        [HttpGet]
        [Route("api/GroupOfWares/{subUrl}")]
        public IActionResult Get(string subUrl)
        {
            return InvokeMethod(_gowService.GetBySubUrl, subUrl);
        }

        [HttpGet]
        [Route("api/GroupOfWares/all")]
        public IActionResult Get()
        {
            return InvokeMethod(_gowService.Get);
        }

        [HttpDelete]
        [Route("api/GroupOfWares/Delete/{id:int}")]
        public IActionResult Delete(int id, [FromQuery(Name = "subUrl")] string subUrl)
        {
            if (id > 0)
                return InvokeMethod(_gowService.Delete, id);

            return InvokeMethod(_gowService.Delete, subUrl);
        }

        [HttpPut]
        [Route("api/GroupOfWares")]
        public IActionResult Update([FromBody]GOWModel model)
        {
            return InvokeMethod(_gowService.Update, model);
        }

        [HttpGet]
        [Route("api/GroupOfWares/Tree")]
        public IActionResult GetTree()
        {
            return InvokeMethod(_gowService.GetTreeGOW);
        }

        [HttpGet]
        [Route("api/GroupOfWares/GetByWareSubUrl/{subUrl}")]
        public IActionResult GetGowsByWareSubUrl(string subUrl)
        {
            return InvokeMethod(_gowService.GetGowsByWareSubUrl, subUrl);
        }
    }
}
