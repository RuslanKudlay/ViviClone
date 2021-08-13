using Application.BBL.Common;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.ViewsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application.BBL.BusinessServices
{
    public class BasketService : IBasketService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWareService wareService;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public BasketService(IDbContextFactory dbContextFactory,
                             IHttpContextAccessor httpContextAccessor,
                             IWareService wareService)
        {
            _dbContextFactory = dbContextFactory;
            _httpContextAccessor = httpContextAccessor;
            this.wareService = wareService;
        }

        public void AddWareToBasket(int wareId)
        {
            using (var context = _dbContextFactory.Create())
            {
                WareModel ware = wareService.GetWare(wareId);
                BasketModel basket = new BasketModel();
                string BasketStr = _session.GetString("Basket");

                if (string.IsNullOrEmpty(BasketStr))
                {
                    basket.BasketItems.Add(new BasketItemModel { Ware = ware, WareQuantity = 1 });

                    basket.TotalPrice = this.SetTotalPrice(basket);

                    _session.SetString("Basket", JsonConvert.SerializeObject(basket));
                }
                else
                {
                    basket = JsonConvert.DeserializeObject<BasketModel>(BasketStr);

                    var basketItem = basket.BasketItems.Where(bi => bi.Ware.Id == ware.Id);
                    if (basketItem.Count() == 0)
                    {
                        basket.BasketItems.Add(new BasketItemModel { Ware = ware, WareQuantity = 1 });
                    }
                    else
                    {
                        basketItem.First().WareQuantity += 1;
                    }

                    basket.TotalPrice = this.SetTotalPrice(basket);
                    _session.SetString("Basket", JsonConvert.SerializeObject(basket));
                }
            }
        }

        public void AddWareToBasket(string wareSubUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                WareModel ware = wareService.GetWareBySubUrl(wareSubUrl);
                BasketModel basket = new BasketModel();
                string BasketStr = _session.GetString("Basket");

                if (string.IsNullOrEmpty(BasketStr))
                {
                    basket.BasketItems.Add(new BasketItemModel { Ware = ware, WareQuantity = 1 });

                    basket.TotalPrice = this.SetTotalPrice(basket);

                    _session.SetString("Basket", JsonConvert.SerializeObject(basket));
                }
                else
                {
                    basket = JsonConvert.DeserializeObject<BasketModel>(BasketStr);

                    var basketItem = basket.BasketItems.Where(bi => bi.Ware.Id == ware.Id);
                    if (basketItem.Count() == 0)
                    {
                        basket.BasketItems.Add(new BasketItemModel { Ware = ware, WareQuantity = 1 });
                    }
                    else
                    {
                        basketItem.First().WareQuantity += 1;
                    }

                    basket.TotalPrice = this.SetTotalPrice(basket);
                    _session.SetString("Basket", JsonConvert.SerializeObject(basket));
                }
            }
        }

        public BasketModel GetBasket()
        {
            using (var context = _dbContextFactory.Create())
            {
                BasketModel basket = new BasketModel();
                string BasketStr = _session.GetString("Basket");
                if (!string.IsNullOrEmpty(BasketStr))
                {
                    basket = JsonConvert.DeserializeObject<BasketModel>(BasketStr);
                }

                return basket;
            }
        }

        public int GetWareQuantityInBasket()
        {
            using (var context = _dbContextFactory.Create())
            {
                int itemsQuantity = 0;
                string BasketStr = _session.GetString("Basket");
                if (BasketStr == null)
                {
                    itemsQuantity = 0;
                }
                else
                {
                    BasketModel basket = JsonConvert.DeserializeObject<BasketModel>(BasketStr);
                    itemsQuantity = basket.BasketItems.Sum(i => i.WareQuantity);
                }                

                return itemsQuantity;
            }
        }

        public void RemoveItemFromBasket(int wareId)
        {
            using (var context = _dbContextFactory.Create())
            {
                string BasketStr = _session.GetString("Basket");
                if (!string.IsNullOrEmpty(BasketStr))
                {
                    BasketModel basket = JsonConvert.DeserializeObject<BasketModel>(BasketStr);
                    var basketItem = basket.BasketItems.Where(bi => bi.Ware.Id == wareId).First();
                    basket.BasketItems.Remove(basketItem);

                    basket.TotalPrice = this.SetTotalPrice(basket);
                    _session.SetString("Basket", JsonConvert.SerializeObject(basket));
                }               
            }
        }

        public void SetCountWareInBasket(int wareId, int wareCount)
        {
            using (var context = _dbContextFactory.Create())
            {
                string BasketStr = _session.GetString("Basket");
                if (!string.IsNullOrEmpty(BasketStr))
                {
                    BasketModel basket = JsonConvert.DeserializeObject<BasketModel>(BasketStr);
                    var basketItem = basket.BasketItems.Where(bi => bi.Ware.Id == wareId).First();
                    basketItem.WareQuantity = wareCount;

                    basket.TotalPrice = this.SetTotalPrice(basket);
                    _session.SetString("Basket", JsonConvert.SerializeObject(basket));
                }
            }
        }

        public void ClearBasket()
        {
            _session.SetString("Basket", string.Empty);
        }

        private double SetTotalPrice(BasketModel basket)
        {
            double totalPrice = 0;
            foreach (var item in basket.BasketItems)
            {
                totalPrice += item.Ware.Price * item.WareQuantity;
            }

            return totalPrice;
        }


    }
}