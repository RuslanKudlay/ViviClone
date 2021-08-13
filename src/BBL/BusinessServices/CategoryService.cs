using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.BBL.Common;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ViewsModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.BBL.BusinessServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IDbContextFactory _dbContextFactory;

        public CategoryService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public QueryCategoryModel GetAll(QueryCategoryModel queryModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var resultModel = new QueryCategoryModel();

                var query = context.Categories.AsQueryable();

                if (!string.IsNullOrEmpty(queryModel.NameContains))
                    query = query.Where(x => x.Name.Contains(queryModel.NameContains));


                if (!string.IsNullOrEmpty(queryModel.OrderBy))
                {
                    if (queryModel.OrderBy.Contains("name"))
                        query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrEmpty(queryModel.OrderByDesc))
                {
                    if (queryModel.OrderByDesc.Contains("name"))
                        query = query.OrderByDescending(x => x.Name);
                }

                resultModel.TotalCount = query.Count();

                if (queryModel.Skip != null) query = query.Skip((int)queryModel.Skip);

                if (queryModel.Take != null) query = query.Take((int)queryModel.Take);

                resultModel.Result = query.Include(x => x.CategoryValueses).ToList().Select(c => new CategoryModel
                {
                    Id = c.Id,
                    MetaDescription = c.MetaDescription,
                    MetaKeywords = c.MetaKeywords,
                    Name = c.Name,
                    SubUrl = c.SubUrl,
                    IsEnable = c.IsEnable,
                    CategoryValues = c.CategoryValueses?.Select(s => new CategoryValuesModel()
                    {
                        Id = s.Id,
                        IsEnable = s.IsEnable,
                        Name = s.Name
                    }).ToList()
                }).ToList();

                return resultModel;
            }
        }

        public CategoryModel GetById(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var category = context.Categories.Where(c => c.Id == id).Include(s => s.CategoryValueses).Select(c => new CategoryModel()
                {
                    Id = c.Id,
                    SubUrl = c.SubUrl,
                    IsEnable = c.IsEnable,
                    MetaDescription = c.MetaDescription,
                    MetaKeywords = c.MetaKeywords,
                    Name = c.Name,
                    CategoryValues = c.CategoryValueses == null ? null : c.CategoryValueses.Select(s => new CategoryValuesModel()
                    {
                        Id = s.Id,
                        IsEnable = s.IsEnable,
                        Name = s.Name
                    }).ToList()
                }).FirstOrDefault();

                if (category == null)
                    throw new Exception("Category not found");

                return category;
            }
        }

        public CategoryModel GetBySubUrl(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var test = context.Categories.Include(s => s.CategoryValueses).Where(c => c.SubUrl == subUrl).FirstOrDefault();
                var category = context.Categories.Include(s => s.CategoryValueses).Where(c => c.SubUrl == subUrl).Select(c => new CategoryModel()
                {
                    Id = c.Id,
                    SubUrl = c.SubUrl,
                    IsEnable = c.IsEnable,
                    MetaDescription = c.MetaDescription,
                    MetaKeywords = c.MetaKeywords,
                    Name = c.Name,
                    CategoryValues = c.CategoryValueses == null ? null : c.CategoryValueses.Select(s => new CategoryValuesModel()
                    {
                        Id = s.Id,
                        IsEnable = s.IsEnable,
                        Name = s.Name
                    }).ToList()
                }).FirstOrDefault();

                if (category == null)
                    throw new Exception("Category not found");

                return category;
            }
        }

        public CategoryModel Add(CategoryModel model)
        {
            using (var context = _dbContextFactory.Create())
            {


                var category = new Category()
                {
                    MetaDescription = model.MetaDescription,
                    MetaKeywords = model.MetaKeywords,
                    Name = model.Name,
                    SubUrl = model.SubUrl,
                };

                if (context.Categories.FirstOrDefault(x => x.SubUrl == model.SubUrl) != null)
                    throw new Exception("The specified category already exists");

                var categoryValues = model.CategoryValues.Select(s => new CategoryValues()
                {
                    Name = s.Name,
                    Category = category
                }).ToList();

                context.CategoryValueses.AddRange(categoryValues);
                context.Categories.Add(category);
                context.SaveChanges();

                model.Id = category.Id;

                return model;

            }
        }

        public CategoryModel Update(CategoryModel model)
        {
            using (var context = _dbContextFactory.Create())
            {
                var category = context.Categories.FirstOrDefault(c => c.Id == model.Id);

                if (category == null)
                    throw new Exception("Category not found");

                category.IsEnable = model.IsEnable;
                category.MetaDescription = model.MetaDescription;
                category.MetaKeywords = model.MetaKeywords;
                category.Name = model.Name;
                category.SubUrl = model.SubUrl;

                List<CategoryValues> listCategoryValues = null;

                if (model.CategoryValues != null && model.CategoryValues.Count > 0)
                {
                    listCategoryValues = model.CategoryValues.Select(s => new CategoryValues()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsEnable = s.IsEnable,
                        Category = category
                    }).ToList();

                    foreach (var item in listCategoryValues)
                    {
                        if (item.Id > 0)
                        {
                            var tmp = context.CategoryValueses.FirstOrDefault(x => x.Id == item.Id && x.Name != item.Name);
                            if (tmp != null)
                            {
                                tmp.Name = item.Name;
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            context.CategoryValueses.Add(item);
                            category.CategoryValueses.Add(item);
                        }
                    }
                }

                List<CategoryValues> removeCategoryValue = null;
                if (listCategoryValues != null)
                    removeCategoryValue = context.CategoryValueses.Where(x => (x.CategoryId == category.Id) && (!listCategoryValues.Any(c => c.Id == x.Id))).ToList();
                else
                    removeCategoryValue = context.CategoryValueses.Where(x => (x.CategoryId == category.Id)).ToList();

                if (removeCategoryValue != null)
                {
                    context.CategoryValueses.RemoveRange(removeCategoryValue);
                }

                context.SaveChanges();

                return model;
            }
        }

        public bool Delete(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var category = context.Categories.FirstOrDefault(c => c.Id == id);

                if (category == null)
                    throw new Exception("Category not found");

                var removeCategoryValue = context.CategoryValueses.Where(x => x.CategoryId == category.Id).ToList();


                context.CategoryValueses.RemoveRange(removeCategoryValue);
                context.Categories.Remove(category);

                context.SaveChanges();

                return true;
            }
        }

        public bool Delete(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var category = context.Categories.FirstOrDefault(c => c.SubUrl == subUrl);

                if (category == null)
                    throw new Exception("Category not found");

                var removeCategoryValue = context.CategoryValueses.Where(x => x.CategoryId == category.Id).ToList();


                context.CategoryValueses.RemoveRange(removeCategoryValue);
                context.Categories.Remove(category);

                context.SaveChanges();

                return true;
            }
        }

        public List<WareCategoryValuesModel> FindCategoriesByGOW(string gowName, bool isParent = false)
        {
            try
            {
                using (var context = _dbContextFactory.Create())
                {
                    List<Ware> wares = null;

                    wares = context.Wares.Include(w => w.WareGOWs).ToList();
                    wares.ForEach(w => w.WareGOWs.ForEach(wg => wg.GOW = context.WareGOWs.Include(g => g.GOW).Where(g => g.GOWId == wg.GOWId).FirstOrDefault().GOW));
                    wares = wares.Where(w => w.WareGOWs.Select(wg => wg.GOW).Where(x => w != null).Any(g => g.Name == gowName || g.SubUrl == gowName)).ToList();


                    var categoryValues = context.WCV.Include(w => w.Ware).Include(c => c.CategoryValueses).Include(c => c.CategoryValueses.Category)
                                                                                                         .Where(wcv => wares.Any(w => w.Id == wcv.WareId))
                                                                                                         .Select(c => new CategoryValuesModel()
                                                                                                         {
                                                                                                             Id = c.CategoryValueses.Id,
                                                                                                             Name = c.CategoryValueses.Name,
                                                                                                             IsEnable = c.CategoryValueses.IsEnable,
                                                                                                             CategoryId = c.CategoryValueses.CategoryId,
                                                                                                             Category = new CategoryModel()
                                                                                                             {
                                                                                                                 Id = c.CategoryValueses.Category.Id,
                                                                                                                 Name = c.CategoryValueses.Category.Name,
                                                                                                                 SubUrl = c.CategoryValueses.Category.SubUrl,
                                                                                                                 IsEnable = c.CategoryValueses.Category.IsEnable,
                                                                                                             }
                                                                                                         }).ToList();

                    List<WareCategoryValuesModel> categories = new List<WareCategoryValuesModel>();
                    foreach (var item in categoryValues)
                    {
                        if (!categories.Any(w => w.Category.Id == item.CategoryId))
                        {
                            categories.Add(new WareCategoryValuesModel()
                            {
                                Category = new CategoryModel()
                                {
                                    Id = item.Category.Id,
                                    Name = item.Category.Name,
                                    IsEnable = item.Category.IsEnable,
                                    MetaDescription = item.Category.MetaDescription,
                                    MetaKeywords = item.Category.MetaKeywords,
                                    SubUrl = item.Category.SubUrl,
                                },
                                CategoryValues = new List<TotalCategoryValuesModel>()
                                {
                                    new TotalCategoryValuesModel()
                                    {
                                        CategoryValue = item,
                                        Count = 1
                                    }
                                }
                            });
                        }
                        else
                        {
                            var category = categories.FirstOrDefault(c => c.Category.Id == item.CategoryId);

                            var categoryValueExist = category.CategoryValues.FirstOrDefault(x => x.CategoryValue.Id == item.Id);

                            if (categoryValueExist != null)
                            {
                                categoryValueExist.Count++;
                            }
                            else
                            {
                                category.CategoryValues.Add(new TotalCategoryValuesModel()
                                {
                                    CategoryValue = item,
                                    Count = 1
                                });
                            }
                        }
                    }

                    return categories.Count > 0 ? categories : null;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<WareCategoryValuesModel> GetCategoryValues(SearchWareParamsModel searchParams,
            IQueryable<Ware> wares,
            IQueryable<WaresCategoryValues> wcv,
            IQueryable<Category> categories)
        {
            var categoriesIn = new List<WareCategoryValuesModel>();
            var allCategories = categories.ToList();

            var categoryValuesByWares = wcv
                            .Include(i => i.Ware)
                            .Include(i1 => i1.CategoryValueses)
                            .Where(_wcv => wares.Any(w => w.Id == _wcv.WareId))
                            .GroupBy(g => g.CategoryValueId)
                            .Select(s => new TotalCategoryValuesModel
                            {
                                Count = s.Count(),
                                IsSelected = false,
                                CategoryValue = new CategoryValuesModel
                                {
                                    Id = s.First().CategoryValueses.Id,
                                    Name = s.First().CategoryValueses.Name,
                                    CategoryId = s.First().CategoryValueses.CategoryId,
                                }
                            })
                            .ToList();

            foreach (var category in allCategories)
            {
                var categoriesValuesIn = new List<TotalCategoryValuesModel>();
                foreach (var categoryValue in category.CategoryValueses)
                {
                    var categoryValueIn = new TotalCategoryValuesModel();
                    bool checkIsSelected = false;
                    if (categoryValuesByWares.Any(a => a.CategoryValue.Id == categoryValue.Id))
                    {
                        categoryValueIn.CategoryValue = new CategoryValuesModel
                        {
                            Id = categoryValue.Id,
                            Name = categoryValue.Name,
                            CategoryId = categoryValue.CategoryId,
                            IsDisabled = false
                        };
                        categoryValueIn.Count = categoryValuesByWares.Where(w => w.CategoryValue.Id == categoryValue.Id).Select(s => s.Count).First();
                        checkIsSelected = true;
                    }
                    else
                    {
                        categoryValueIn.CategoryValue = new CategoryValuesModel
                        {
                            Id = categoryValue.Id,
                            Name = categoryValue.Name,
                            CategoryId = categoryValue.CategoryId,
                            IsDisabled = true
                        };
                        categoryValueIn.Count = 0;
                    }

                    if (checkIsSelected && searchParams.SearchParams != null)
                    {
                        if (searchParams.SearchParams.Any(searchParam => EqualsNormalizeString(searchParam.CategoryName, category.Name) && searchParam.CategoryValues.Any(cv => EqualsNormalizeString(cv, categoryValueIn.CategoryValue.Name))))
                            categoryValueIn.IsSelected = true;

                    }

                    categoriesValuesIn.Add(categoryValueIn);
                }

                var wareCategoryValuesModel = new WareCategoryValuesModel
                {
                    Category = new CategoryModel
                    {
                        Id = category.Id,
                        Name = category.Name,
                        IsEnable = category.IsEnable,
                        MetaDescription = category.MetaDescription,
                        MetaKeywords = category.MetaKeywords,
                        SubUrl = category.SubUrl,
                        Index = category.Index
                    },
                    CategoryValues = categoriesValuesIn
                };

                categoriesIn.Add(wareCategoryValuesModel);
            }

            categoriesIn = categoriesIn
                .Where(w => w.CategoryValues.Count != 0)
                .OrderBy(o => o.Category.Index)
                .ToList();

            return categoriesIn;
        }

        private bool EqualsNormalizeString(string str1, string str2)
        {
            return string.Equals(str1.Normalize(), str2.Normalize(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
