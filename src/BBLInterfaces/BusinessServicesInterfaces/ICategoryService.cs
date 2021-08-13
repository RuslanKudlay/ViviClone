using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ViewsModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface ICategoryService
    {
        QueryCategoryModel GetAll(QueryCategoryModel query);       
        CategoryModel GetById(int id);
        CategoryModel GetBySubUrl(string subUrl);

        CategoryModel Add(CategoryModel model);
        CategoryModel Update(CategoryModel model);

        bool Delete(int id);
        bool Delete(string subUrl);

        List<WareCategoryValuesModel> FindCategoriesByGOW(string gowName, bool isParent = false);
        List<WareCategoryValuesModel> GetCategoryValues(SearchWareParamsModel searchParams, IQueryable<Ware> wares, IQueryable<WaresCategoryValues> wcv, IQueryable<Category> categories);
    }
}
