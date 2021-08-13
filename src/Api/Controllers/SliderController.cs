using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SliderController : ApplicationApiController
    {
        private readonly ISliderService _sliderService;
        public SliderController(ISliderService sliderService, UserManager<ApplicationUser> manager) : base(manager)
        {
            _sliderService = sliderService;
        }

        [HttpPost]
        [Route("api/Slider/all")]
        public IActionResult Get([FromBody] QuerySliderModel query)
        {
            return InvokeMethod(_sliderService.GetAll, query);
        }

        [HttpGet]
        [Route("api/Slider/{slideId}")]
        public IActionResult GetById(int slideId)
        {
            return InvokeMethod(_sliderService.GetById, slideId);
        }

        [HttpPut]
        [Route("api/Slider")]
        public IActionResult Update([FromBody] SliderModel model)
        {
            return InvokeMethod(_sliderService.UpdateSlide, model);
        }

        [HttpPost]
        [Route("api/Slider")]
        public IActionResult Save([FromBody] SliderModel model)
        {
            return InvokeMethod(_sliderService.AddSlide, model);
        }

        [HttpDelete]
        [Route("api/Slider/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            return InvokeMethod(_sliderService.Delete, id);
        }
    }
}
