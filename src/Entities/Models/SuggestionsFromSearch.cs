using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public class SuggestionsFromSearch
    {
        public string Label { get; set; }
        public string Category { get; set; }
        public string Link { get; set; }
        public bool isOnlyForProfessional { get; set; }
    }

    public class SearchSuggestions
    {
        public string SearchValue { get; set; }
        public string GOW { get; set; }
        public ClaimsPrincipal Principal { get; set; }
    }
}
