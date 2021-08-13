using Application.EntitiesModels.Models;
using System.Collections.Generic;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IGOWService
    {
        GOWModel Add(GOWModel brandModel);

        GOWModel Update(GOWModel brandModel);

        List<GOWModel> Get();
        GOWModel GetBySubUrl(string subUrl);
        List<GOWModel> GetTreeGOW();
        List<GOWModel> GetGowsByWareSubUrl(string subUrl);

        bool Delete(int id);
        bool Delete(string subUrl);
    }
}
