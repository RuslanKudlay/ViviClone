using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using System.Collections.Generic;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IAnnouncementService
    {
        QueryAnnouncementModel GetAll(QueryAnnouncementModel query);
        List<AnnouncementModel> Get();
        AnnouncementModel GetById(int id);
        AnnouncementModel GetBySubUrl(string subUrl);

        AnnouncementModel Add(AnnouncementModel BrandModel);
        AnnouncementModel Update(AnnouncementModel BrandModel);

        bool Delete(int id);
        bool Delete(string subUrl);
    }
}
