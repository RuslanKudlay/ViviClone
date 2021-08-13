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
    public class CategoryValuesService : ICategoryValuesService
    {        
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IWareService _wareService;

        public CategoryValuesService(IDbContextFactory dbContextFactory, IWareService wareService)
        {
            _dbContextFactory = dbContextFactory;
            _wareService = wareService;
        }

        public List<CategoryValuesModel> GetAll()
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.CategoryValueses.Include(c => c.Category).Select(c => new CategoryValuesModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsEnable = c.IsEnable,
                    CategoryName = c.Category.Name
                }).ToList();
            }
        }       
        

        public CategoryValuesModel GetById(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var categoryValues = context.CategoryValueses.FirstOrDefault(c => c.Id == id);

                if (categoryValues == null)
                    throw new Exception("Category Values not found");

                return new CategoryValuesModel()
                {
                    Id = categoryValues.Id,
                    Name = categoryValues.Name,
                    IsEnable = categoryValues.IsEnable,
                    CategoryName = categoryValues.Name,
                    CategoryId = categoryValues.Id
                };
            }
        }

        public CategoryValuesModel Add(CategoryValuesModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var newCategoryValues = new CategoryValues()
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                    IsEnable = model.IsEnable,
                };

                context.CategoryValueses.Add(newCategoryValues);

                context.SaveChanges();

                model.Id = newCategoryValues.Id;

                return model;
            }
        }

        public CategoryValuesModel Update(CategoryValuesModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var category = context.CategoryValueses.FirstOrDefault(c => c.Id == model.Id);

                if (category == null)
                    throw new Exception("Category Values not found");

                category.IsEnable = model.IsEnable;
                category.Name = model.Name;
                category.CategoryId = model.CategoryId;

                context.SaveChanges();

                return model;
            }
        }

        public bool Delete(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var category = context.CategoryValueses.FirstOrDefault(c => c.Id == id);

                if (category == null)
                    throw new Exception("Category  Values not found");

                context.CategoryValueses.Remove(category);

                context.SaveChanges();

                return true;
            }
        }

        public List<CategoryValuesModel> GetByWareId(int wareId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var categoryValueIds = context.WCV.Where(wcv => wcv.WareId == wareId).Select(wcv => wcv.CategoryValueId);

                var categoryValues = context.CategoryValueses.Where(cv => categoryValueIds.Contains(cv.Id));

                return categoryValues.Select(cv => new CategoryValuesModel()
                {
                    Id = cv.Id,
                    Name = cv.Name,
                    IsEnable = cv.IsEnable,
                    CategoryName = cv.Category.Name,
                    CategoryId = cv.CategoryId
                }).ToList();
            }
        }

        public CategoryValuesModel GetByName(string name)
        {
            try
            {
                using(var context = _dbContextFactory.Create())
                {
                    var result = context.CategoryValueses.FirstOrDefault(x => x.Name == name);

                    return new CategoryValuesModel()
                    {
                        Id = result.Id,
                        Name = result.Name,
                        CategoryId = result.CategoryId,
                    };
                }
            }
            catch(Exception e)
            {
                throw new Exception("Ware not found." + e.Message);
            }
        }
    }
}
