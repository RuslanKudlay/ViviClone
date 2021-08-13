using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Application.Api.Controllers
{
    [AllowAnonymous]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        [Produces("application/json")]
        [Route("api/Search/search")]
        public IActionResult Search(string term, string GOW)
        {
            return Ok(_searchService.GetSuggestions(new SearchSuggestions { SearchValue = term, GOW = GOW , Principal = User})); 
        }
    }
}
