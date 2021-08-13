using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Application;
using Microsoft.AspNetCore.Identity;
using Application.EntitiesModels.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TranslationController : ApplicationApiController
    {
        private readonly IStringLocalizer localizer;

        public TranslationController(IStringLocalizerFactory factory, UserManager<ApplicationUser> manager) : base(manager)
        {
            localizer = factory.Create("Program", "Application");
        }
        
        [HttpGet("getTranslate")]
        public IActionResult Translate(string language)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            var result = JObject.Parse(localizer.WithCulture(currentCulture)["AdminResources"].Value);
            return Ok(result);
        }
    }
}
