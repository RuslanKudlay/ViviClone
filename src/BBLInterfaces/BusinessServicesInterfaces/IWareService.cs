using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IWareService
    {
        WareModel SaveOrUpdate(WareModel ware);
        QueryWaresModel GetAll(QueryWaresModel query);
        QueryWaresModel GetBestsellers(ClaimsPrincipal principal);
        WareModel GetWare(int id);
        WareModel GetWareBySubUrl(string subUrl);
        PaginationResultModel GetNextWares(PaginationRequest pagination);
        BrandsCategoriesByWares GetWaresBySearchParams(SearchWareParamsModel searchParams, ClaimsPrincipal principal);


        WareModel AddToBestsellers(int id);
        WareModel RemoveFromBestsellers(int id);
        bool Delete(int id);
        bool Delete(string subUrl);
    }
}
