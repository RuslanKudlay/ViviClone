using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.WishList;
using Application.EntitiesModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IWishListService
    {
        Task<bool> CreateWishList(ApplicationUser user);
        List<WishListModel> GetWishListsUser(int UserId);
        void AddWare(WareModel ware, int UserId);
        void RemoveWare(int wareWishId);
        void RemoveRangeWares(List<WishListWare> wishListWares);
    }
}
