using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IUserService
    {
        QueryUserModel Get(QueryUserModel queryModel);
        UserModel GetById(int userId);
        IEnumerable<ApplicationRole> GetRoles();
        void ChangeRole(ApplicationRole role, int userId);
        void ChangeStatus(UserStatus status, int userId);
    }
}
