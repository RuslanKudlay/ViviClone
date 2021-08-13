using Application.EntitiesModels.Models;
using System.Collections.Generic;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface ICategoryValuesService
    {
        CategoryValuesModel Add(CategoryValuesModel model);
        CategoryValuesModel Update(CategoryValuesModel model);

        List<CategoryValuesModel> GetAll();
        CategoryValuesModel GetById(int id);
        List<CategoryValuesModel> GetByWareId(int wareId);
        CategoryValuesModel GetByName(string name);

        bool Delete(int id);
    }
}
