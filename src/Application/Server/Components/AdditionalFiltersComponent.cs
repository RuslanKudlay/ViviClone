using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Server.Components
{
    public class AdditionalFiltersComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(QueryWaresModel queryWaresModel)
        {
            var additionalFilters = new AdditionalFilters();

            additionalFilters.TakeItems.Where(w => w.Value == queryWaresModel.Take).FirstOrDefault().IsSelected = true;

            foreach(var sortBy in additionalFilters.SortBy)
            {
                var selectedSortBy = sortBy.SortBy.Where(w => w.Value == queryWaresModel.OrderBy).FirstOrDefault();

                if(selectedSortBy != null)
                {
                    selectedSortBy.IsSelected = true;
                    break;
                }
            }
          
            return View("Index", additionalFilters);
        }
    }
}
