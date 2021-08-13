using Application.BBL.Common;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.WishList;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class WishListService : IWishListService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IModelMapper _modelMapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWareService _wareService;



        public WishListService(
            IDbContextFactory dbContextFactory,
            IModelMapper modelMapper,
            IWareService wareService, 
            UserManager<ApplicationUser> userManager)
        {
            _dbContextFactory = dbContextFactory;

            _modelMapper = modelMapper;
            _wareService = wareService;
            _userManager = userManager;
        }

        public List<WishListModel> GetWishListsUser(int UserId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var wishLists = context.WishLists.Where(w => w.UserId == UserId)
                    .Include(wares => wares.WishListWares)
                    .ThenInclude(wares => wares.Ware)
                    .ToList();

                  List <WishListModel> wishListModels = wishLists
                    .Select(s => _modelMapper.MapTo<WishList, WishListModel>(s))
                    .ToList();

                foreach (var wishList in wishListModels)
                {
                    wishList.TotalPrice = GetTotalPrice(wishList.WishListWareModel);
                }

                return wishListModels;
            }
        }

        public void AddWare(WareModel ware, int UserId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var wishListIds = context.WishLists.Where(w => w.UserId == UserId).Select(s => s.Id).ToList();
                var wishListWares = context.WishListWares.ToList();

                context.WishListWares.Add(new WishListWare
                {
                    DateAdded = DateTime.Now,
                    WareId = ware.Id,
                    WishListId = wishListIds.First()
                });

                context.SaveChanges();
            }
        }

        public void RemoveWare(int wareWishId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var wareWish = context.WishListWares.Where(w => w.Id == wareWishId).First();

                if(wareWish == null)
                {
                    throw new Exception("Item not found");
                }

                context.WishListWares.Remove(wareWish);

                context.SaveChanges();
            }
        }

        public void RemoveRangeWares(List<WishListWare> wishListWares)
        {
            using (var context = _dbContextFactory.Create())
            {       
                context.WishListWares.RemoveRange(wishListWares);

                context.SaveChanges();
            }
        }

        private double GetTotalPrice(List<WishListWareModel> wishListWareModels)
        {
            double totalPrice = 0;

            foreach(var wishWareModel in wishListWareModels)
            {
                totalPrice += wishWareModel.Ware.Price;
            }

            return totalPrice;
        }

        public async Task<bool> CreateWishList(ApplicationUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);

            using (var context = _dbContextFactory.Create())
            {
                context.WishLists.Add(new WishList() { Name = "Мой список желаний", UserId = int.Parse(userId) });

                context.SaveChanges();
            }

            return true;
        }
    }
}

