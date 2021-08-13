using Application.EntitiesModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IWaresCategoryValuesService
    {
        List<WaresCategoryValuesModel> GetAll();

        WaresCategoryValuesModel GetById(int id);

        WaresCategoryValuesModel Add(WaresCategoryValuesModel post);

        WaresCategoryValuesModel Update(WaresCategoryValuesModel post);

        bool Delete(int id);
    }
}
