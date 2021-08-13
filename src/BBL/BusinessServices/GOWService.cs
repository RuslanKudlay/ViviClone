using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.BBL.BusinessServices
{
    public class GOWService : IGOWService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IModelMapper modelMapper;

        public GOWService(IDbContextFactory dbContextFactory, IModelMapper modelMapper)
        {
            _dbContextFactory = dbContextFactory;
            this.modelMapper = modelMapper;
        }

        public List<GOWModel> Get()
        {
            using (var context = _dbContextFactory.Create())
            {
                var gows = context.GOWs.Select(gow => modelMapper.MapTo<GOW, GOWModel>(gow)).ToList();
                return gows;
            }
        }

        public GOWModel Add(GOWModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var res = context.GOWs.FirstOrDefault(GOW => GOW.SubUrl == model.SubUrl);
                if (res == null)
                {
                    var newGOW = new GOW()
                    {
                        Name = model.Name,
                        IsEnable = model.IsEnable,
                        MetaDescription = model.MetaDescription,
                        ShortDescription = model.ShortDescription,
                        MetaKeywords = model.MetaKeywords,
                        ParentId = model.ParentId,
                        SubUrl = model.SubUrl,
                        LogoImage = model.LogoImage,
                        IconString = model.IconString,
                        Childs = context.GOWs.Where(g => model.Childs.Any(c => c.Id == g.Id)).ToList()
                    };
                    context.GOWs.Add(newGOW);

                    context.SaveChanges();

                    model.Id = newGOW.Id;

                    return model;
                }
                throw new Exception("This group of products already exists");
            }
        }

        public GOWModel Update(GOWModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var groupOfWare = context.GOWs.Include(c => c.Childs).FirstOrDefault(c => c.Id == model.Id);

                if (groupOfWare == null)
                    throw new Exception("Group of Wares not found");

                groupOfWare.IsEnable = model.IsEnable;
                groupOfWare.Name = model.Name;
                groupOfWare.SubUrl = model.SubUrl;
                groupOfWare.MetaDescription = model.MetaDescription;
                groupOfWare.MetaKeywords = model.MetaKeywords;
                groupOfWare.ShortDescription = model.ShortDescription;
                groupOfWare.LogoImage = model.LogoImage;
                groupOfWare.IconString = model.IconString;
                groupOfWare.ParentId = model.ParentId;

                if (model.Childs != null || model.Childs.Count != 0)
                {
                    groupOfWare.Childs.Clear();
                    groupOfWare.Childs = context.GOWs.Where(g => model.Childs.Any(c => c.Id == g.Id)).ToList();
                }

                context.SaveChanges();

                return model;
            }
        }

        public bool Delete(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var groupOfWare = context.GOWs.AsNoTracking()
                    .Include(_ => _.Childs)
                    .ThenInclude(_ => _.Childs)
                    .FirstOrDefault(c => c.Id == id);

                if (groupOfWare == null)
                {
                    throw new Exception("Group of Ware not found");
                }

                if (groupOfWare.Childs != null)
                {
                    foreach (var child in groupOfWare.Childs)
                    {
                        if (child != null)
                        {
                            context.GOWs.Remove(child);
                        }
                    }
                    context.GOWs.RemoveRange(groupOfWare.Childs);
                }
                context.GOWs.Remove(groupOfWare);
                context.SaveChanges();

                return true;
            }
        }

        public bool Delete(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var groupOfWare = context.GOWs.FirstOrDefault(c => c.SubUrl == subUrl);

                if (groupOfWare == null)
                    throw new Exception("Group of Ware not found");

                context.GOWs.Remove(groupOfWare);

                context.SaveChanges();

                return true;
            }
        }

        public List<GOWModel> GetTreeGOW()
        {
            using (var context = _dbContextFactory.Create())
            {
                var gows = context.GOWs.Include(gow => gow.Parent)
                    .Include(gow => gow.WareGOWs)
                    .Include(gow => gow.Childs)
                        .ThenInclude(gow => gow.Childs)
                    .Where(gow => gow.Parent == null)
                    .Where(gow => gow.WareGOWs.Count() > 0)
                    .Select(gow => modelMapper.MapTo<GOW, GOWModel>(gow))
                    .ToList();
                return gows;
            }
        }

        public List<GOWModel> GetGowsByWareSubUrl(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var gows = context.WareGOWs.Where(wgow => wgow.Ware.SubUrl == subUrl)
                    .Include(wgow => wgow.GOW)
                    .Select(wgow => modelMapper.MapTo<GOW, GOWModel>(wgow.GOW))
                    .ToList();
                return gows;
            }
        }

        public GOWModel GetBySubUrl(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var gow = context.GOWs.Where(g => g.SubUrl == subUrl)
                    .Include(g => g.Parent)
                    .Select(g => modelMapper.MapTo<GOW, GOWModel>(g))
                    .Single();
                return gow;
            }
        }
    }
}
