using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class WaresCategoryValuesService : IWaresCategoryValuesService
    {
        private readonly IDbContextFactory _dbContextFactory;

        public WaresCategoryValuesService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }   

        public List<WaresCategoryValuesModel> GetAll()
        {
            using (var context = _dbContextFactory.Create())
            {                
                var wareCategoryValues = context.WCV.Include(c => c.CategoryValueses).Include(w => w.Ware).ToList();

                return wareCategoryValues.Select(p => new WaresCategoryValuesModel
                {
                    Id = p.Id,
                    Ware = new WareModel()
                    {
                        Id = p.Ware.Id,
                        Name = p.Ware.Name,
                        Price = p.Ware.Price,
                        Text = p.Ware.Text,
                        VendorCode = p.Ware.VendorCode
                    },
                    CategoryValues = wareCategoryValues.Where(x => x.WareId == p.WareId).ToList().Select(c => new CategoryValuesModel()
                    {
                        Id = c.CategoryValueses.Id,
                        IsEnable = c.CategoryValueses.IsEnable,
                        Name = c.CategoryValueses.Name

                    }).ToList()
                }).ToList();
            }
        }

        public WaresCategoryValuesModel Add(WaresCategoryValuesModel waresCategoryValues)
        {
            using (var context = _dbContextFactory.Create())
            {
                var newWCV = new WaresCategoryValues
                {
                    WareId = waresCategoryValues.Ware.Id
                };

                context.WCV.Add(newWCV);

                context.SaveChanges();

                waresCategoryValues.Id = newWCV.Id;

                return waresCategoryValues;
            }
        }
        
        public WaresCategoryValuesModel GetById(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var waresCategoryValues = context.WCV.Include(c => c.CategoryValueses).Include(w => w.Ware).FirstOrDefault(p => p.Id == id);

                return new WaresCategoryValuesModel()
                {
                    Id = waresCategoryValues.Id,

                    Ware = new WareModel()
                    {
                        Id = waresCategoryValues.Ware.Id,
                        Text = waresCategoryValues.Ware.Text,
                        Name = waresCategoryValues.Ware.Name,
                        Price = waresCategoryValues.Ware.Price
                    }
                };
            }
        }

        public bool Delete(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var ware = context.WCV.FirstOrDefault(p => p.Id == id);

                context.WCV.Remove(ware);

                context.SaveChanges();

                return true;
            }
        }       

        public WaresCategoryValuesModel Update(WaresCategoryValuesModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var waresCategoryValues = context.WCV.FirstOrDefault(p => p.Id == model.Id);

                if (waresCategoryValues == null)
                {
                    throw new Exception("Was not found ");
                }
                
                waresCategoryValues.WareId = context.Wares.FirstOrDefault(c => c.Name == model.Ware.Name).Id;

                context.SaveChanges();

                return model;
            }
        }
    }
}
