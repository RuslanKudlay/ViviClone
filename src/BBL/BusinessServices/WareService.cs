using Application.BBL.Common;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ViewsModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class WareService : IWareService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IModelMapper _modelMapper;
        private readonly ISearchService _searchService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<WareService> _logger;
        const int DEFAULT_TAKE_ITEMS = 50;

        public WareService(IDbContextFactory dbContextFactory,
            IModelMapper modelMapper,
            ISearchService searchService,
            IBrandService brandService,
            ICategoryService categoryService,
            ILogger<WareService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _modelMapper = modelMapper;
            _searchService = searchService;
            _brandService = brandService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public QueryWaresModel GetAll(QueryWaresModel queryModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var resultModel = new QueryWaresModel();

                var query = context.Wares.Include(w => w.Brand).Include(w => w.WCV).ThenInclude(wcv => wcv.CategoryValueses).ThenInclude(cv => cv.Category).AsQueryable();

                if (!string.IsNullOrEmpty(queryModel.NameContains))
                    query = query.Where(x => x.Name.Contains(queryModel.NameContains));

                if (!string.IsNullOrEmpty(queryModel.PriceContains))
                    query = query.Where(x => x.Price.ToString().Contains(queryModel.PriceContains));

                if (!string.IsNullOrEmpty(queryModel.GroupOfWareContains))
                    query = query.Where(x => x.WareGOWs.Any(y => y.GOW.Name.Contains(queryModel.GroupOfWareContains)));

                if (!string.IsNullOrEmpty(queryModel.VendoreCodeContains))
                    query = query.Where(x => x.VendorCode.Contains(queryModel.NameContains));

                if (!string.IsNullOrEmpty(queryModel.OrderBy))
                {
                    if (queryModel.OrderBy.Contains("name"))
                        query = query.OrderBy(x => x.Name);

                    else if (queryModel.OrderBy.Contains("price"))
                        query = query.OrderBy(x => x.Price);

                    else if (queryModel.OrderBy.Contains("vendor"))
                        query = query.OrderBy(x => x.VendorCode);
                }

                if (!string.IsNullOrEmpty(queryModel.OrderByDesc))
                {
                    if (queryModel.OrderByDesc.Contains("name"))
                        query = query.OrderByDescending(x => x.Name);

                    else if (queryModel.OrderByDesc.Contains("price"))
                        query = query.OrderByDescending(x => x.Price);

                    else if (queryModel.OrderByDesc.Contains("vendor"))
                        query = query.OrderByDescending(x => x.VendorCode);
                }

                resultModel.TotalCount = query.Count();

                var orderWaresByPrice = query.OrderBy(o => o.Price).ToList();

                resultModel.MinPrice = orderWaresByPrice.First().Price;
                resultModel.MaxPrice = orderWaresByPrice.Last().Price;

                if (queryModel.Skip != null) query = query.Skip((int)queryModel.Skip);

                if (queryModel.Take != null) query = query.Take((int)queryModel.Take);

                resultModel.Result = query.ToList().Select(x => GetModel(x)).ToList();

                return resultModel;
            }

        }

        public WareModel SaveOrUpdate(WareModel ware)
        {
            using (var context = _dbContextFactory.Create())
            {
                Ware wareDB = ware.Id > 0 ? context.Wares.FirstOrDefault(x => x.Id == ware.Id) : new Ware();

                wareDB.Name = ware.Name;
                wareDB.Price = ware.Price;
                wareDB.IsBestseller = ware.IsBestseller;
                wareDB.IsOnlyForProfessionals = ware.IsOnlyForProfessionals;
                wareDB.IsEnable = ware.IsEnable;
                wareDB.VendorCode = ware.VendorCode;
                wareDB.MetaDescription = ware.MetaDescription;
                wareDB.MetaKeywords = ware.MetaKeywords;
                wareDB.SubUrl = ware.SubUrl;
                wareDB.Text = ware.Text;
                wareDB.WareImage = ware.WareImage;
                wareDB.BrandId = ware.BrandId < 0 ? wareDB.BrandId : ware.BrandId;

                if (wareDB.Id <= 0)
                {
                    if (context.Wares.FirstOrDefault(x => x.SubUrl == ware.SubUrl) != null)
                        throw new Exception("The specified SubUrl already exists");

                    context.Wares.Add(wareDB);

                    wareDB.Id = 0;
                }
                else
                {
                    if (context.Wares.FirstOrDefault(x => x.SubUrl == ware.SubUrl && x.Id != wareDB.Id) != null)
                        throw new Exception("The specified SubUrl already exists");
                }

                UpdateWCVTable(context, wareDB, ware.CategoryValues);

                context.SaveChanges();

                var existingWareGows = context.WareGOWs.Where(x => x.WareId == wareDB.Id);
                if (existingWareGows != null)
                {
                    foreach (var wg in existingWareGows)
                    {
                        context.WareGOWs.Remove(wg);
                    }
                }

                context.SaveChanges();

                if (ware.GOWs != null)
                {
                    foreach (var gow in ware.GOWs)
                    {
                        if (gow.IsEnable)
                        {
                            var waregow = context.WareGOWs.Where(x => x.GOWId == gow.Id && x.WareId == gow.Id).FirstOrDefault();
                            WareGOW wareGow = new WareGOW()
                            {
                                GOWId = gow.Id,
                                WareId = wareDB.Id
                            };
                            if (waregow == null)
                            {
                                context.WareGOWs.Add(wareGow);
                            }
                        }
                    }
                }

                context.SaveChanges();

                return _modelMapper.MapTo<Ware, WareModel>(wareDB);
            }
        }

        public WareModel GetWare(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Wares.Include(w => w.Brand).ToList().Where(p => p.Id == id).Select(w => GetModel(w)).FirstOrDefault();
            }
        }

        public WareModel GetWareBySubUrl(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var wareEntity = context.Wares.AsNoTracking()
                    .Include(w => w.Brand)
                    .Include(_ => _.WareGOWs)
                        .ThenInclude(_ => _.GOW)
                            .ThenInclude(_ => _.Childs)
                    .Include(_ => _.WareGOWs)
                        .ThenInclude(_ => _.GOW)
                            .ThenInclude(_ => _.Parent)
                    .Include(_ => _.WCV)
                        .ThenInclude(_ => _.CategoryValueses)
                        .ThenInclude(_ => _.Category)
                    .FirstOrDefault(p => p.SubUrl == subUrl);
                var ware = GetModel(wareEntity);
                return ware;
            }
        }      

        public bool Delete(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var ware = context.Wares.FirstOrDefault(p => p.Id == id);

                if (ware == null)
                    throw new Exception("Ware not found");

                context.Wares.Remove(ware);

                context.SaveChanges();

                return true;
            }
        }

        public bool Delete(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var ware = context.Wares.FirstOrDefault(p => p.SubUrl == subUrl);

                if (ware == null)
                    throw new Exception("Ware not found");

                context.Wares.Remove(ware);

                context.SaveChanges();

                return true;
            }
        }
        private WareModel GetModel(Ware source)
        {
            WareModel ware = _modelMapper.MapTo<Ware, WareModel>(source);
            return ware;
        }

        public PaginationResultModel GetNextWares(PaginationRequest pagination)
        {
            var resultModel = new PaginationResultModel();
            using (var context = _dbContextFactory.Create())
            {
                var query = context.Wares.Skip(pagination.PageIndex).Take(pagination.PageSize);

                resultModel.TotalCount = context.Wares.Count();

                resultModel.Result = _modelMapper.MapTo<List<Ware>, List<WareModel>>(query.ToList());

                return resultModel;
            }
        }

        public void UpdateWCVTable(IApplicationDbContext context, Ware currentWare, List<CategoryValuesModel> categoryValues)
        {
            if (categoryValues != null)
            {
                if (currentWare.Id == 0)
                {
                    foreach (var item in categoryValues)
                    {
                        var wcv = context.WCV.FirstOrDefault(x => x.CategoryValueId == item.Id && x.WareId == currentWare.Id);
                        if (wcv == null)
                        {
                            context.WCV.Add(new WaresCategoryValues()
                            {
                                CategoryValueId = item.Id,
                                Ware = currentWare
                            });
                        }
                    }
                }
                else
                {
                    foreach (var item in categoryValues)
                    {
                        var wcv = context.WCV.FirstOrDefault(x => x.CategoryValueId == item.Id && x.WareId == currentWare.Id);
                        if (wcv == null)
                        {
                            context.WCV.Add(new WaresCategoryValues()
                            {
                                CategoryValueId = item.Id,
                                WareId = currentWare.Id
                            });
                        }
                    }
                    var wareAndCategoryValues = context.WCV.Where(w => w.WareId == currentWare.Id).ToList();

                    var removeWCV = wareAndCategoryValues.Where(x => !categoryValues.Any(c => c.Id == x.CategoryValueId)).ToList();
                    if (removeWCV != null)
                    {
                        context.WCV.RemoveRange(removeWCV);
                    }
                }
            }
        }

        public BrandsCategoriesByWares GetWaresBySearchParams(SearchWareParamsModel searchParams, ClaimsPrincipal principal)
        {
            try
            {
                using (var context = _dbContextFactory.Create())
                {
                    if (searchParams == null)
                    {
                        _logger.Log(LogLevel.Error, new EventId(LoggerId.Error), "SearchParams must be non-null");
                        searchParams = new SearchWareParamsModel();
                    }

                    IQueryable<Ware> wares = context.Wares.Where(w => w.IsEnable).AsQueryable();
                    var brandsCategoriesByWares = new BrandsCategoriesByWares();

                    wares = SelectWaresBySearchParams(searchParams, wares, principal);

                    int resultCount = wares.Count();

                    // Get Brand And Category By Wares
                    if (searchParams.AdjustSearch != null && searchParams.AdjustSearch.SkipSearchBrandsAndCategories != true)
                    {
                        brandsCategoriesByWares.SideSearchMenuModel = GetSideSearchMenu(searchParams, context, wares, principal);
                    }

                    var maxPrice = wares.ToList().Aggregate((agg, next) => next.Price > agg.Price ? next : agg).Price;
                    var minPrice = wares.ToList().Aggregate((agg, next) => next.Price < agg.Price ? next : agg).Price;

                    wares = PaginationWares(searchParams, wares);
                    wares = SortingWares(searchParams, wares);

                    var queryWaresModel = new QueryWaresModel()
                    {
                        Take = searchParams.Take,
                        OrderBy = searchParams.SortBy,
                        Result = wares.Select(w => GetModel(w)).ToList(),
                        ResultCount = resultCount,
                        MinPrice = minPrice,
                        MaxPrice = maxPrice
                    };

                    brandsCategoriesByWares.Wares = queryWaresModel;

                    return brandsCategoriesByWares;
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, new EventId(LoggerId.Error), e.Message);

                // Call function with default params
                return GetWaresBySearchParams(new SearchWareParamsModel(), principal);
            }
        }

        private int GetCategoryIdByName(string categoryName)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Categories.Where(c => c.Name == categoryName).Select(c => c.Id).Single();
            }
        }

        private IQueryable<Ware> SelectWaresBySearchParams(SearchWareParamsModel searchParams, IQueryable<Ware> wares, ClaimsPrincipal principal)
        {
            var userIdentity = (ClaimsIdentity)principal.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

            var res = roles.FirstOrDefault(r => r.Value == "Professional");
            if (res == null)
            {
                wares = wares.Where(w => !w.IsOnlyForProfessionals);
            }
            if(res != null && searchParams.ForProfessionals)
            {
                wares = wares.Where(w => w.IsOnlyForProfessionals);
            }
            // Filters ware list by gow name
            if (searchParams.GOWName?.Length > 0)
            {
                wares = wares
                    .Include(w => w.WareGOWs)
                        .ThenInclude(wgow => wgow.GOW)
                    .Where(w => w.WareGOWs
                        .Any(wgow => wgow.GOW.SubUrl == searchParams.GOWName));
            }

            if (searchParams.SearchText?.Length > 0)
            {
                if (_searchService.Search(searchParams))
                {
                    wares = SearchWares(searchParams.SearchText, wares);
                }
            }           

            if (searchParams.Price != null)
            {
                wares = wares
                    .Where(w => Math.Ceiling(w.Price) >= searchParams.Price.MinPrice - 1 && Math.Floor(w.Price) <= searchParams.Price.MaxPrice);
            }

            // Filters ware list by brand
            if (searchParams.BrandSubUrls?.Length > 0)
            {
                wares = wares
                    .Include(w => w.Brand)
                    .Where(w => searchParams.BrandSubUrls.Contains(w.Brand.SubUrl));
            }

            // Filters ware list by category value
            if (searchParams.SearchParams?.Length > 0)
            {
                foreach (var searchParam in searchParams.SearchParams)
                {
                    wares = wares
                        .Where(w => w.WCV
                            .Any(w1 => searchParam.CategoryValues.Contains(w1.CategoryValueses.Name) &&
                                        w1.CategoryValueses.CategoryId == GetCategoryIdByName(searchParam.CategoryName)));
                }
            }


            return wares;
        }

        private IQueryable<Ware> PaginationWares(SearchWareParamsModel searchParams, IQueryable<Ware> wares)
        {
            // Get wares by 'page' and 'take' params
            if (searchParams.Page != 1 || searchParams.Take != DEFAULT_TAKE_ITEMS)
            {
                if (searchParams.Page == 1)
                {
                    wares = wares.AsPagedQueryable(searchParams.Take);
                }
                else if (searchParams.Take == DEFAULT_TAKE_ITEMS)
                {
                    int skipItems = (searchParams.Page - 1) * DEFAULT_TAKE_ITEMS;
                    wares = wares.AsPagedQueryable(skipItems, DEFAULT_TAKE_ITEMS);
                }
                else
                {
                    int skipItems = (searchParams.Page - 1) * searchParams.Take;
                    wares = wares.AsPagedQueryable(skipItems, searchParams.Take);
                }
            }
            else
            {
                wares = wares.AsPagedQueryable(DEFAULT_TAKE_ITEMS);
            }

            return wares;
        }

        private IQueryable<Ware> SortingWares(SearchWareParamsModel searchParams, IQueryable<Ware> wares)
        {
            // Sorting ware list
            if (searchParams.SortBy != null)
            {
                switch (searchParams.SortBy)
                {
                    case "cheap":
                        {
                            wares = wares.AsSortedQueryable<Ware, double>("Price", SortDirection.Ascending);
                            break;
                        }
                    case "expensive":
                        {
                            wares = wares.AsSortedQueryable<Ware, double>("Price", SortDirection.Descending);
                            break;
                        }
                    case "nameDesc":
                        {
                            wares = wares.AsSortedQueryable<Ware, string>("Name", SortDirection.Descending);
                            break;
                        }
                    case "nameAsc":
                        {
                            wares = wares.AsSortedQueryable<Ware, string>("Name", SortDirection.Ascending);
                            break;
                        }
                        //by rate
                        //by popular
                        //by auction
                }
            }

            return wares;
        }

        private IQueryable<Ware> SearchWares(string searchValue, IQueryable<Ware> wares)
        {
            using (var context = _dbContextFactory.Create())
            {
                wares = wares
                    .Where(w => w.Name.Contains(searchValue) ||
                    w.Text.Contains(searchValue) ||
                    w.MetaDescription.Contains(searchValue) ||
                    w.MetaKeywords.Contains(searchValue));

                return wares;
            }
        }

        private SideSearchMenuModel GetSideSearchMenu(SearchWareParamsModel searchWareParamsModel, IApplicationDbContext context, IQueryable<Ware> wares, ClaimsPrincipal principal)
        {
            var sideSearchMenuModel = new SideSearchMenuModel();
            sideSearchMenuModel.Professional = new Professional { IsSelected = searchWareParamsModel.ForProfessionals };


            if (!searchWareParamsModel.AdjustSearch.SkipSearchBrands)
            {
                StateSideSearchMenu stateSideSearchMenu = SearchWareParamsModel.GetStateSideSearchMenu(searchWareParamsModel);
                var totalBrands = context.Brands.Where(w => w.IsEnable).AsQueryable();
                var userIdentity = (ClaimsIdentity)principal.Identity;
                var claims = userIdentity.Claims;
                var roleClaimType = userIdentity.RoleClaimType;
                var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

                var res = roles.FirstOrDefault(r => r.Value == "Professional");
                if (res == null)
                {
                    totalBrands = totalBrands.Select(t => new Brand
                    {
                        Id = t.Id,
                        IsEnable = t.IsEnable,
                        Wares = t.Wares.Where(w => w.IsOnlyForProfessionals == false).ToList(),
                        Position = t.Position,
                        Color = t.Color,
                        ColorTitle = t.ColorTitle,
                        LogoImage = t.LogoImage,
                        Name = t.Name,
                        ShortDescription = t.ShortDescription,
                        Body = t.Body,
                        SubUrl = t.SubUrl,
                        MetaDescription = t.MetaDescription,
                        MetaKeywords = t.MetaKeywords
                    }).AsQueryable();
                }
                if (res != null && searchWareParamsModel.ForProfessionals)
                {
                    totalBrands = totalBrands.Select(t => new Brand
                    {
                        Id = t.Id,
                        IsEnable = t.IsEnable,
                        Wares = t.Wares.Where(w => w.IsOnlyForProfessionals == true).ToList(),
                        Position = t.Position,
                        Color = t.Color,
                        ColorTitle = t.ColorTitle,
                        LogoImage = t.LogoImage,
                        Name = t.Name,
                        ShortDescription = t.ShortDescription,
                        Body = t.Body,
                        SubUrl = t.SubUrl,
                        MetaDescription = t.MetaDescription,
                        MetaKeywords = t.MetaKeywords
                    }).AsQueryable();
                }

                sideSearchMenuModel.Brands = _brandService.GetListBrands(searchWareParamsModel, stateSideSearchMenu, wares, totalBrands);
            }

            sideSearchMenuModel.Professional.Count = 0;
            sideSearchMenuModel.Professional.Count += wares.Where(w => w.IsOnlyForProfessionals == true).Count();
            sideSearchMenuModel.Professional.IsDisabled = sideSearchMenuModel.Professional.Count == 0 ? true : false;

            var wcv = context.WCV.AsQueryable();
            var categories = context.Categories.Include(i => i.CategoryValueses).Where(w => w.IsEnable && w.CategoryValueses.Any(w1 => w1.IsEnable)).AsQueryable();
            sideSearchMenuModel.WareCategoryValues = _categoryService.GetCategoryValues(searchWareParamsModel, wares, wcv, categories);
            return sideSearchMenuModel;
        }

        public WareModel AddToBestsellers(int id)
        {
            using(var context = _dbContextFactory.Create())
            {
                var ware = context.Wares.FirstOrDefault(res => res.Id == id);
                if(ware != null)
                {
                    ware.IsBestseller = true;

                    context.Wares.Update(ware);

                    context.SaveChanges();
                    return _modelMapper.MapTo<Ware, WareModel>(ware);
                }
                else
                    throw new Exception("Ware not found");
            }
        }

        public WareModel RemoveFromBestsellers(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var ware = context.Wares.FirstOrDefault(res => res.Id == id);
                if(ware != null )
                {
                    ware.IsBestseller = false;

                    context.Wares.Update(ware);

                    context.SaveChanges();
                    return _modelMapper.MapTo<Ware, WareModel>(ware);
                }
                throw new Exception("Ware not found");
            }
        }


        public QueryWaresModel GetBestsellers(ClaimsPrincipal principal)
        {
            using (var context = _dbContextFactory.Create())
            {
                var userIdentity = (ClaimsIdentity)principal.Identity;
                var claims = userIdentity.Claims;
                var roleClaimType = userIdentity.RoleClaimType;
                var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

                var res = roles.FirstOrDefault(r => r.Value == "Professional");

                var bestsellers = context.Wares.Select(w => GetModel(w)).Where(ware => ware.IsBestseller == true).ToList();
                if (res == null)
                {
                    bestsellers = bestsellers.Where(b => b.IsOnlyForProfessionals == false).ToList();
                }
                var queryWaresModel = new QueryWaresModel()
                {
                    Result = bestsellers
                };
                return queryWaresModel;
            }
        }
    }
}

