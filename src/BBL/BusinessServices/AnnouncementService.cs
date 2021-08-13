using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IDbContextFactory _dbContextFactory;

        public AnnouncementService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }


        public AnnouncementModel Add(AnnouncementModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var newuser = new Announcement()
                {
                    Body = model.Body,
                    Date = model.Date,
                    IsEnable = model.IsEnable,
                    LastUpdateDate = model.LastUpdateDate,
                    MetaDescription = model.MetaDescription,
                    MetaKeywords = model.MetaKeywords,
                    SubUrl = model.SubUrl,
                    Title = model.Title

                };
                context.Announcements.Add(newuser);

                context.SaveChanges();

                model.Id = newuser.Id;

                return model;
            }
        }

        public QueryAnnouncementModel GetAll(QueryAnnouncementModel queryModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var resultModel = new QueryAnnouncementModel();

                var query = context.Announcements.AsQueryable();

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
                    return new AnnouncementModel
                    {
                        Id = item.Id,
                        IsEnable = item.IsEnable,
                        Body = item.Body,
                        SubUrl = item.SubUrl,
                        MetaKeywords = item.MetaKeywords,
                        MetaDescription = item.MetaDescription,
                        Date = item.Date.Date,
                        Title = item.Title,
                        LastUpdateDate = item.LastUpdateDate

                    };
                }).OrderByDescending(a => a.IsEnable).ToList();

                return resultModel;
            }
        }


        public List<AnnouncementModel> Get()
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Announcements.ToList().Select((item) =>
                {
                    return new AnnouncementModel
                    {
                        Id = item.Id,
                        IsEnable = item.IsEnable,
                        Body = item.Body,
                        SubUrl = item.SubUrl,
                        MetaKeywords = item.MetaKeywords,
                        MetaDescription = item.MetaDescription,
                        Date = item.Date.Date,
                        Title = item.Title,
                        LastUpdateDate = item.LastUpdateDate

                    };
                }).OrderByDescending(a => a.IsEnable).ToList();
            };
        }

        public AnnouncementModel GetById(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var item = context.Announcements.Where(a => a.Id == id).ToList().Select(an =>
                new AnnouncementModel
                {
                    Id = an.Id,
                    IsEnable = an.IsEnable,
                    Body = an.Body,
                    SubUrl = an.SubUrl,
                    MetaKeywords = an.MetaKeywords,
                    MetaDescription = an.MetaDescription,
                    Date = an.Date.Date,
                    Title = an.Title,
                    LastUpdateDate = an.LastUpdateDate
                }).FirstOrDefault();

                if (item == null)
                    throw new Exception("Announcement was not found");

                return item;
            }
        }

        public AnnouncementModel GetBySubUrl(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {

                var item = context.Announcements.Where(a => a.SubUrl == subUrl).ToList().Select(an =>
                                   new AnnouncementModel
                                   {
                                       Id = an.Id,
                                       IsEnable = an.IsEnable,
                                       Body = an.Body,
                                       SubUrl = an.SubUrl,
                                       MetaKeywords = an.MetaKeywords,
                                       MetaDescription = an.MetaDescription,
                                       Date = an.Date.Date,
                                       Title = an.Title,
                                       LastUpdateDate = an.LastUpdateDate.Date
                                   }).FirstOrDefault();

                if (item == null)
                    throw new Exception("Announcement was not found");

                return item;
            }
        }

        public AnnouncementModel Update(AnnouncementModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var item = context.Announcements.Where(a => a.Id == model.Id).FirstOrDefault();

                if (item == null)
                    throw new Exception("Announcement was not found");

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
                var item = context.Announcements.FirstOrDefault(b => b.Id == id);

                if (item == null)
                    throw new Exception("Announcement was not found");

                context.Announcements.Remove(item);

                context.SaveChanges();

                return true;

            }
        }

        public bool Delete(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var item = context.Announcements.FirstOrDefault(b => b.SubUrl == subUrl);

                if (item == null)
                    throw new Exception("Announcement was not found");

                context.Announcements.Remove(item);

                context.SaveChanges();

                return true;
            }
        }
    }

}
