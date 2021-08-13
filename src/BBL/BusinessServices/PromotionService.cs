using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class PromotionService : IPromotionService
    {
        private IDbContextFactory _dbContextFactory;

        public PromotionService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }


        public PromotionModel Add(PromotionModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var newPromotion = new Promotion()
                {
                    Body = model.Body,
                    Date = model.Date,
                    IsEnable = model.IsEnable,
                    LastUpdateDate = model.LastUpdateDate,
                    MetaDescription = model.MetaDescription,
                    MetaKeywords = model.MetaKeywords,
                    Title = model.Title,
                    SubUrl = model.SubUrl,
                };

                context.Promotions.Add(newPromotion);

                context.SaveChanges();

                model.Id = newPromotion.Id;

                return model;
            }
        }

        public QueryPromotionModel GetAll(QueryPromotionModel queryModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var resultModel = new QueryPromotionModel();

                var query = context.Promotions.AsQueryable();

                if (!string.IsNullOrEmpty(queryModel.BodyContains))
                    query = query.Where(x => x.Body.Contains(queryModel.BodyContains));

                if (!string.IsNullOrEmpty(queryModel.TitleContains))
                    query = query.Where(x => x.Title.Contains(queryModel.TitleContains));


                if (!string.IsNullOrEmpty(queryModel.OrderBy))
                {
                    if (queryModel.OrderBy.Contains("title"))
                        query = query.OrderBy(x => x.Title);
                }

                if (!string.IsNullOrEmpty(queryModel.OrderByDesc))
                {
                    if (queryModel.OrderByDesc.Contains("title"))
                        query = query.OrderByDescending(x => x.Title);
                }

                resultModel.TotalCount = query.Count();

                if (queryModel.Skip != null) query = query.Skip((int)queryModel.Skip);

                if (queryModel.Take != null) query = query.Take((int)queryModel.Take);

                resultModel.Result = query.ToList().Select((item) =>
                {
                    return new PromotionModel
                    {
                        Id = item.Id,
                        IsEnable = item.IsEnable,
                        Body = item.Body,
                        SubUrl = item.SubUrl,
                        MetaKeywords = item.MetaKeywords,
                        MetaDescription = item.MetaDescription,
                        Date = item.Date,
                        Title = item.Title,
                        LastUpdateDate = item.LastUpdateDate
                    };
                }).OrderByDescending(a => a.IsEnable).ToList();

                return resultModel;
            }
        }


        public List<PromotionModel> Get()
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Promotions.ToList().Select((item) =>
                {
                    return new PromotionModel
                    {
                        Id = item.Id,
                        IsEnable = item.IsEnable,
                        Body = item.Body,
                        SubUrl = item.SubUrl,
                        MetaKeywords = item.MetaKeywords,
                        MetaDescription = item.MetaDescription,
                        Date = item.Date,
                        Title = item.Title,
                        LastUpdateDate = item.LastUpdateDate
                    };
                }).OrderByDescending(a => a.IsEnable).ToList();  
            };
        }

        public PromotionModel GetById(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
               var item = context.Promotions.Where(a => a.Id == id).ToList().Select(p =>
               new PromotionModel
               {
                   Id = p.Id,
                   IsEnable = p.IsEnable,
                   Body = p.Body,
                   SubUrl = p.SubUrl,
                   MetaKeywords = p.MetaKeywords,
                   MetaDescription = p.MetaDescription,
                   Date = p.Date,
                   Title = p.Title,
                   LastUpdateDate = p.LastUpdateDate
               }).FirstOrDefault();

                if (item == null)
                    throw new Exception("Promotion was not found");

                return item;
            }
        }

        public PromotionModel GetBySubUrl(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var item = context.Promotions.Where(a => a.SubUrl == subUrl).ToList().Select(p =>
                new PromotionModel
                {
                    Id = p.Id,
                    IsEnable = p.IsEnable,
                    Body = p.Body,
                    SubUrl = p.SubUrl,
                    MetaKeywords = p.MetaKeywords,
                    MetaDescription = p.MetaDescription,
                    Date = p.Date,
                    Title = p.Title,
                    LastUpdateDate = p.LastUpdateDate

                }).FirstOrDefault();

                return item;
            }
        }

        public PromotionModel Update(PromotionModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var item = context.Promotions.Where(a => a.Id == model.Id).FirstOrDefault();

                if (item == null)
                    throw new Exception("Promotion was not found");

                item.IsEnable = model.IsEnable;
                item.Body = model.Body;
                item.Date = model.Date;
                item.LastUpdateDate = model.LastUpdateDate;
                item.MetaDescription = model.MetaDescription;
                item.MetaKeywords = model.MetaKeywords;
                item.SubUrl = model.SubUrl;
                item.Title = model.Title;

                context.SaveChanges();

                return model;
            }
        }

        public bool Delete(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var item = context.Promotions.FirstOrDefault(b => b.Id == id);

                if (item == null)
                    throw new Exception("Promotion was not found");

                context.Promotions.Remove(item);

                context.SaveChanges();

                return true;
            }
        }

        public bool Delete(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var item = context.Promotions.FirstOrDefault(b => b.SubUrl == subUrl);

                if (item == null)
                    throw new Exception("Promotion was not found");

                context.Promotions.Remove(item);

                context.SaveChanges();

                return true;
            }
        }
    }
}
