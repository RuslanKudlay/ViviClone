using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.Order;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.OrderModels;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory _dbContextFactory;

        private readonly IModelMapper modelMapper;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IDbContextFactory dbContextFactory, IModelMapper modelMapper, UserManager<ApplicationUser> userManager)
        {
            _dbContextFactory = dbContextFactory;
            this.modelMapper = modelMapper;
            this.userManager = userManager;
        }
      
        public QueryUserModel Get(QueryUserModel queryModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var resultModel = new QueryUserModel();

                var query = context.ApplicationUsers.AsNoTracking()
                    .Include(_ => _.UserRoles)
                        .ThenInclude(_ => _.Role)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(queryModel.EmailContains))
                {
                    query = query.Where(_ => _.Email.Contains(queryModel.EmailContains));
                }

                if (!string.IsNullOrEmpty(queryModel.OrderBy))
                {
                    if (queryModel.OrderBy.Contains("firstName"))
                        query = query.OrderBy(x => x.FirstName);
                    else if (queryModel.OrderBy.Contains("lastName"))
                        query = query.OrderBy(x => x.LastName);
                    else if (queryModel.OrderBy.Contains("name"))
                        query = query.OrderBy(x => x.Name);
                    else if (queryModel.OrderBy.Contains("email"))
                        query = query.OrderBy(x => x.Email);
                }

                if (!string.IsNullOrEmpty(queryModel.OrderByDesc))
                {
                    if (queryModel.OrderBy.Contains("firstName"))
                        query = query.OrderByDescending(x => x.FirstName);
                    else if (queryModel.OrderByDesc.Contains("lastName"))
                        query = query.OrderByDescending(x => x.LastName);
                    else if (queryModel.OrderByDesc.Contains("name"))
                        query = query.OrderByDescending(x => x.Name);
                    else if (queryModel.OrderByDesc.Contains("email"))
                        query = query.OrderByDescending(x => x.Email);
                }

                query = query.Where(user => user.UserRoles.All(_ => _.Role.Name != "SuperAdmin"));

                resultModel.TotalCount = query.Count();

                query = query.Skip(queryModel.Skip ?? 0);

                query = query.Take(queryModel.Take ?? 10);

                resultModel.Result = modelMapper.MapTo<List<ApplicationUser>, List<UserModel>>(query.ToList());

                return resultModel;
            }
        }

        public UserModel GetById(int userId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var user = context.ApplicationUsers.AsNoTracking()
                    .Include(_ => _.UserRoles)
                        .ThenInclude(_ => _.Role)
                    .Single(_ => _.Id == userId);
                return modelMapper.MapTo<ApplicationUser, UserModel>(user);
            }
        }

        public IEnumerable<ApplicationRole> GetRoles()
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.ApplicationRoles.AsNoTracking().Where(_ => _.Name != "SuperAdmin").ToList();
            }
        }

        public void ChangeRole(ApplicationRole role, int userId)
        {
            using (var context = _dbContextFactory.Create())
            {
                context.ApplicationUsersRoles.RemoveRange(context.ApplicationUsersRoles.Where(_ => _.UserId == userId));
                context.ApplicationUsersRoles.Add(new ApplicationUserRole() { UserId = userId, RoleId = role.Id});
                context.SaveChanges();
            }
        }

        public void ChangeStatus(UserStatus status, int userId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var user = context.ApplicationUsers.AsNoTracking()
                    .Single(_ => _.Id == userId);
                user.Status = status;
                context.ApplicationUsers.Update(user);
                context.SaveChanges();
            }
        }
    }
}
