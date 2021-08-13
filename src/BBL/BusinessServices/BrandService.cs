using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Application.EntitiesModels.Models.ViewsModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class BrandService : IBrandService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IModelMapper modelMapper;
        private readonly IGOWService gowService;
        private readonly IPictureAttacherService pictureAttacherService;

        public BrandService(IDbContextFactory dbContextFactory, IModelMapper modelMapper, IGOWService gowService, IPictureAttacherService pictureAttacherService)
        {
            _dbContextFactory = dbContextFactory;
            this.modelMapper = modelMapper;
            this.gowService = gowService;
            this.pictureAttacherService = pictureAttacherService;
        }

        public BrandModel AddBrand(BrandModel brandModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var brand = new Brand();
                var isAdding = true;
                if (brandModel.Id > 0)
                {
                    brand = context.Brands.Where(a => a.Id == brandModel.Id).FirstOrDefault();
                    if (brand == null)
                        throw new Exception("Brand was not updated , brand not found");
                    isAdding = false;
                }

                brand.IsEnable = brandModel.IsEnable;
                brand.Position = brandModel.Position;
                brand.Color = brandModel.Color;
                brand.ColorTitle = brandModel.ColorTitle;
                brand.LogoImage = brandModel.LogoImage;
                brand.Name = brandModel.Name;
                brand.ShortDescription = brandModel.ShortDescription;
                brand.Body = brandModel.Body;
                brand.SubUrl = brandModel.SubUrl;
                brand.MetaKeywords = brandModel.MetaKeywords;
                brand.MetaDescription = brandModel.MetaDescription;
                if (isAdding)
                {
                    context.Brands.Add(brand);
                }

                context.SaveChanges();

                return brandModel;
            }

        }

        public List<BrandModel> Get()
        {
            using (var context = _dbContextFactory.Create())
            {
                var res = context.Brands.ToList().Select((brand) => modelMapper.MapTo<Brand, BrandModel>(brand)
                ).OrderByDescending(a => a.IsEnable).ThenBy(a => a.Position).ToList();
                return res;
            }
        }

        public QueryBrandModel GetAll(QueryBrandModel queryModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var resultModel = new QueryBrandModel();

                var query = context.Brands.AsQueryable();

                if (!string.IsNullOrEmpty(queryModel.NameContains))
                    query = query.Where(x => x.Name.Contains(queryModel.NameContains));

                if (!string.IsNullOrEmpty(queryModel.BodyContains))
                    query = query.Where(x => x.Body.ToString().Contains(queryModel.BodyContains));

                if (!string.IsNullOrEmpty(queryModel.MetaKeywordsContains))
                    query = query.Where(x => x.MetaKeywords.ToString().Contains(queryModel.MetaKeywordsContains));

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

                resultModel.Result = query.ToList().Select(x => GetModel(x)).ToList();

                return resultModel;
            }
        }

        public BrandModel GetBrandBySubUrl(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                 var brand = context.Brands.Where(a => a.SubUrl == subUrl).Include(b => b.Wares).ToList().Select(b => 
                 modelMapper.MapTo<Brand, BrandModel>(b)).FirstOrDefault();

                if (brand == null)
                    throw new Exception("Brand was not found");

                // LogoImage is like '/api/getImage/97' string
                if (!string.IsNullOrWhiteSpace(brand.LogoImage))
                {
                    var imageId = int.Parse(brand.LogoImage.Split('/')[3]);
                    var image = pictureAttacherService.GetPictureData(imageId);
                    var base64image = image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image, 0, image.Length)) : "";

                    brand.Base64Image = base64image;
                }

                return brand;
            }
        }

        public BrandModel UpdateBrand(BrandModel brandModel)
        {
            using (var context = _dbContextFactory.Create())
            {
                var brand = context.Brands.Where(a => a.Id == brandModel.Id).FirstOrDefault();

                if (brand == null)
                    throw new Exception("Brand was not found");

                brand.IsEnable = brandModel.IsEnable;
                brand.Position = brandModel.Position;
                brand.Color = brandModel.Color;
                brand.ColorTitle = brandModel.ColorTitle;
                brand.LogoImage = brandModel.LogoImage;
                brand.Name = brandModel.Name;
                brand.ShortDescription = brandModel.ShortDescription;
                brand.Body = brandModel.Body;
                brand.SubUrl = brandModel.SubUrl;
                brand.MetaKeywords = brandModel.MetaKeywords;
                brand.MetaDescription = brandModel.MetaDescription;

                context.SaveChanges();

                return brandModel;
            }
        }

        public bool Delete(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var brand = context.Brands.FirstOrDefault(b => b.Id == id);

                if (brand == null)
                    throw new Exception("Brand was not found");

                context.Brands.Remove(brand);

                context.SaveChanges();

                return true;
            }
        }

        public bool Delete(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var brand = context.Brands.FirstOrDefault(b => b.SubUrl == subUrl);

                if (brand == null)
                    throw new Exception("Brand was not found");

                context.Brands.Remove(brand);

                return true;

            }
        }

        private void GetAllGowsWithChilds(ICollection<GOW> gows, List<GOW> outputArr)
        {
            foreach(var gow in gows)
            {
                outputArr.Add(gow);

                if (gow.Childs != null && gow.Childs.Count > 0)
                    GetAllGowsWithChilds(gow.Childs, outputArr);
            }

        }

        private GOW SaveGowWithChilds(GOWModel gow, IApplicationDbContext context)
        {
            var _gow = modelMapper.MapTo<GOWModel, GOW>(gow);
            List<GOW> childs = new List<GOW>();

            if (gow.Id == 0)
            {
                foreach (var child in gow.Childs)
                {
                    childs.Add(SaveGowWithChilds(child, context));
                }

                _gow.Childs = childs;

                context.GOWs.Add(_gow);
            }

            else
            {
                var dbGow = context.GOWs.FirstOrDefault(x => x.Id == _gow.Id);

                dbGow.Name = _gow.Name;

                foreach(var child in gow.Childs)
                {
                    childs.Add(SaveGowWithChilds(child, context));
                }

                dbGow.Childs = childs;

                dbGow.SubUrl = gow.SubUrl;

                _gow = dbGow;
            }

            return _gow;
        }

        public IEnumerable<TotalBrandsModel> GetListBrands(SearchWareParamsModel searchParams, StateSideSearchMenu stateSideSearchMenu, IQueryable<Ware> wares, IQueryable<Brand> totalBrands)
        {
            var brandsIn = new List<Brand>();
            var brandsOut = new List<Brand>();
            bool isCountCalculate = false;

            switch (stateSideSearchMenu)
            {
                case StateSideSearchMenu.WithoutParams:
                case StateSideSearchMenu.OnlyBrands:
                    brandsIn = totalBrands
                        .Include(i => i.Wares)
                        .ToList();
                    isCountCalculate = true;
                    break;
                case StateSideSearchMenu.OnlyParams:
                case StateSideSearchMenu.BrandsParams:
                case StateSideSearchMenu.OnlySearch:
                case StateSideSearchMenu.SearchParams:
                case StateSideSearchMenu.SearchBrandsParams:
                case StateSideSearchMenu.GOW:
                case StateSideSearchMenu.GowParams:
                case StateSideSearchMenu.GowBrandsParams:
                    brandsIn = wares
                        .GroupBy(g => g.Brand)
                        .Select(s => s.First().Brand)
                        .ToList();
                    brandsOut = totalBrands
                        .Where(w => !brandsIn.Any(b => b.Id == w.Id))
                        .ToList();
                    break;

                case StateSideSearchMenu.SearchBrands:
                case StateSideSearchMenu.GowBrands:
                    IQueryable<IQueryable<Ware>> listWaresIn = null;
                    if (stateSideSearchMenu == StateSideSearchMenu.SearchBrands)
                    {
                        listWaresIn = totalBrands
                            .Include(i => i.Wares)
                            .Select(s => s.Wares
                                .Where(w => w.IsEnable && (w.Name.Contains(searchParams.SearchText) ||
                                        w.MetaDescription.Contains(searchParams.SearchText) ||
                                        w.MetaKeywords.Contains(searchParams.SearchText)))
                                .AsQueryable());
                    }
                    else
                    {
                        listWaresIn = totalBrands
                        .Include(i => i.Wares)
                            .ThenInclude(ii => ii.WareGOWs)
                                .ThenInclude(iii => iii.GOW)
                        .Select(s => s.Wares
                            .Where(w => w.IsEnable &&
                                w.WareGOWs.Any(wgow => wgow.GOW.SubUrl == searchParams.GOWName))
                        .AsQueryable());
                    }

                    var waresIn = new List<Ware>();

                    foreach (var item in listWaresIn)
                    {
                        waresIn.AddRange(item.ToList());
                    }

                    brandsIn = totalBrands
                        .Include(i => i.Wares)
                        .Where(w => w.IsEnable && 
                            w.Wares.Any(a => waresIn.Any(w1 => w1.Id == a.Id)))
                        .ToList();

                    foreach (var brand in brandsIn)
                    {
                        brand.Wares = waresIn.Where(w => w.BrandId == brand.Id).ToList();
                    }

                    brandsOut = totalBrands
                        .Where(w => !brandsIn.Any(b => b.Id == w.Id))
                        .ToList();
                    isCountCalculate = true;
                    break;
            }

            List<TotalBrandsModel> updatetList = new List<TotalBrandsModel>();

            for (int i = 0; i < brandsIn.Count; i++)
            {
                int countWares;
                if (isCountCalculate)
                {
                    countWares = brandsIn[i].Wares.Count;
                }
                else
                {
                    countWares = wares.Count(w => w.Brand.Id == brandsIn[i].Id);
                }

                TotalBrandsModel totalBrandsModel = new TotalBrandsModel
                {
                    Brand = new BrandModel
                    {
                        Id = brandsIn[i].Id,
                        IsEnable = brandsIn[i].IsEnable,
                        Position = brandsIn[i].Position,
                        Color = brandsIn[i].Color,
                        ColorTitle = brandsIn[i].ColorTitle,
                        LogoImage = brandsIn[i].LogoImage,
                        Name = brandsIn[i].Name,
                        ShortDescription = brandsIn[i].ShortDescription,
                        Body = brandsIn[i].Body,
                        SubUrl = brandsIn[i].SubUrl,
                        MetaKeywords = brandsIn[i].MetaKeywords,
                        MetaDescription = brandsIn[i].MetaDescription
                    },
                    Count = countWares,
                    IsDisabled = countWares == 0 ? true : false 
                };

                updatetList.Add(totalBrandsModel);
            }

            for (int i = 0; i < brandsOut.Count; i++)
            {
                TotalBrandsModel totalBrandsModel = new TotalBrandsModel
                {
                    Brand = new BrandModel
                    {
                        Id = brandsOut[i].Id,
                        IsEnable = brandsOut[i].IsEnable,
                        Position = brandsOut[i].Position,
                        Color = brandsOut[i].Color,
                        ColorTitle = brandsOut[i].ColorTitle,
                        LogoImage = brandsOut[i].LogoImage,
                        Name = brandsOut[i].Name,
                        ShortDescription = brandsOut[i].ShortDescription,
                        Body = brandsOut[i].Body,
                        SubUrl = brandsOut[i].SubUrl,
                        MetaKeywords = brandsOut[i].MetaKeywords,
                        MetaDescription = brandsOut[i].MetaDescription
                    },
                    IsDisabled = true
                };

                updatetList.Add(totalBrandsModel);
            }

            updatetList = updatetList.OrderBy(o => o.Brand.Position).ThenBy(t => t.Brand.Name).ToList();

            if (searchParams.BrandSubUrls != null)
                foreach (var brand in updatetList)
                {
                    if (searchParams.BrandSubUrls.Any(brandSubUrl => EqualsNormalizeString(brandSubUrl, brand.Brand.SubUrl)))
                    {
                        brand.IsSelected = true;
                    }
                }

            return updatetList;
        }

        private bool EqualsNormalizeString(string str1, string str2)
        {
            return string.Equals(str1.Normalize(), str2.Normalize(), StringComparison.InvariantCultureIgnoreCase);
        }

        private BrandModel GetModel(Brand source)
        {
            return new BrandModel()
            {
                Id = source.Id,
                IsEnable = source.IsEnable,
                Position = source.Position,
                Color = source.Color,
                ColorTitle = source.ColorTitle,
                Name = source.Name,
                LogoImage = source.LogoImage,
                ShortDescription = source.ShortDescription,
                Body = source.Body,
                SubUrl = source.SubUrl,
                MetaDescription = source.MetaDescription,
                MetaKeywords = source.MetaKeywords,
            };
        }
    }
}
