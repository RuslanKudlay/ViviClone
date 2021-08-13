using Application.EntitiesModels.Models;
using System.Collections.Generic;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface ISearchService
    {
        List<SuggestionsFromSearch> GetSuggestions(SearchSuggestions searchSuggestions);
        bool Search(SearchWareParamsModel searchWareParamsModel);
    }
}
