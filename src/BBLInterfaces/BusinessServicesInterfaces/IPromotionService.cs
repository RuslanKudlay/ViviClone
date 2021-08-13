using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using System.Collections.Generic;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IPromotionService
    {
        QueryPromotionModel GetAll(QueryPromotionModel query);
        List<PromotionModel> Get();
        PromotionModel GetById(int id);
        PromotionModel GetBySubUrl(string subUrl);

        PromotionModel Add(PromotionModel BrandModel);
        PromotionModel Update(PromotionModel BrandModel);

        bool Delete(int id);
        bool Delete(string subUrl);
    }
}
