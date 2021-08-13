using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ViewsModel;
using System.Collections.Generic;
using System.Linq;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IBrandService
    {
        QueryBrandModel GetAll(QueryBrandModel query);
        List<BrandModel> Get();
        BrandModel GetBrandBySubUrl(string subUrl);

        IEnumerable<TotalBrandsModel> GetListBrands(SearchWareParamsModel searchParams, StateSideSearchMenu stateSideSearchMenu, IQueryable<Ware> wares, IQueryable<Brand> totalBrands);

        BrandModel AddBrand(BrandModel BrandModel);
        BrandModel UpdateBrand(BrandModel BrandModel);

        bool Delete(int id);
        bool Delete(string subUrl);
    }
}
