using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Application.BBL.BusinessServices
{
    public class SearchService : ISearchService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private const int COUNT_PROMOTIONS_IN_SEARCH_BOX = 10;

        public SearchService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public List<SuggestionsFromSearch> GetSuggestions(SearchSuggestions searchSuggestions)
        {
            var userIdentity = (ClaimsIdentity)searchSuggestions.Principal.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role).ToList();

            int totalSuggestions, remainingBalanceOfSuggestions;
            var allResults = new List<SuggestionsFromSearch>();
            allResults.AddRange(GetAllTitleFromWares(searchSuggestions.SearchValue, out totalSuggestions, searchSuggestions.GOW));

            if (totalSuggestions < COUNT_PROMOTIONS_IN_SEARCH_BOX)
            {
                remainingBalanceOfSuggestions = COUNT_PROMOTIONS_IN_SEARCH_BOX - totalSuggestions;

                allResults.AddRange(GetAllTitleFromGOW(searchSuggestions.SearchValue, ref remainingBalanceOfSuggestions));                

                if (remainingBalanceOfSuggestions > 0)
                {
                    allResults.AddRange(GetAllTitleFromBrands(searchSuggestions.SearchValue, ref remainingBalanceOfSuggestions));

                    if (remainingBalanceOfSuggestions > 0)
                    {
                        allResults.AddRange(GetAllTitleFromCategory(searchSuggestions.SearchValue, remainingBalanceOfSuggestions));
                    }
                }
            }
            var res = roles.FirstOrDefault(r => r.Value == "Professional");
            if (res == null)
            {
                allResults = allResults.Where(r => r.isOnlyForProfessional == false).ToList();
            }
            return allResults;
        }

        public bool Search(SearchWareParamsModel searchWareParamsModel)
        {
            string searchValue = searchWareParamsModel.SearchText;
            searchWareParamsModel.SearchText = null;
            bool isSearchInWares = false;

            string gowSubUrl = GetGOWSubUrl(searchValue);

            if (gowSubUrl?.Length > 0)
            {
                searchWareParamsModel.GOWName = gowSubUrl;
                return isSearchInWares;
            }
            else
            {
                string brandSubUrl = GetBrandSubUrl(searchValue);

                if (brandSubUrl?.Length > 0)
                {
                    if (searchWareParamsModel.BrandSubUrls?.Length > 0)
                    {
                        var existedBrandSubUrls = searchWareParamsModel.BrandSubUrls.ToList();
                        existedBrandSubUrls.Add(brandSubUrl);
                        searchWareParamsModel.BrandSubUrls = existedBrandSubUrls.ToArray();
                    }
                    else
                    {
                        searchWareParamsModel.BrandSubUrls = new string[] { brandSubUrl };
                    }

                    return isSearchInWares;
                }
                else
                {
                    string[] categotySearchParam = GetCategorySearchParamUrl(searchValue);

                    if (categotySearchParam?.Length > 0)
                    {
                        searchWareParamsModel.SearchParams =
                            new SelectedSearchParam[]
                            {
                            new SelectedSearchParam()
                            {
                                CategoryName = categotySearchParam[0],
                                CategoryValues = new string[] { categotySearchParam[1] }
                            }
                            };

                        return isSearchInWares;
                    }
                }
            }

            searchWareParamsModel.SearchText = searchValue;
            return true;
        }

        private List<SuggestionsFromSearch> GetAllTitleFromWares(string searchValue, out int totalSuggestions, string GOW)
        {
            using (var context = _dbContextFactory.Create())
            {
                List<SuggestionsFromSearch> allTitle = null;
                if (GOW == null || GOW.Length == 0)
                {
                    allTitle = context.Wares
                        .Where(w => w.Name.Contains(searchValue) || 
                                w.Text.Contains(searchValue))
                        .Select(s => new SuggestionsFromSearch
                        { Category = "Товары", Label = s.Name + Description(s.Text, searchValue), Link = s.SubUrl, isOnlyForProfessional = s.IsOnlyForProfessionals })
                        .Take(COUNT_PROMOTIONS_IN_SEARCH_BOX)
                        .ToList();
                }
                else
                {
                    var wares = context.Wares
                        .Include(w => w.WareGOWs)
                            .ThenInclude(wgow => wgow.GOW)
                        .Where(w => w.WareGOWs
                            .Any(wgow => wgow.GOW.SubUrl == GOW));

                    allTitle = wares
                        .Where(w => w.IsEnable && 
                            (w.Name.Contains(searchValue) || w.Text.Contains(searchValue)))
                        .Select(s => new SuggestionsFromSearch
                        { Category = "Товары", Label = s.Name + Description(s.Text, searchValue), Link = s.SubUrl, isOnlyForProfessional = s.IsOnlyForProfessionals })
                        .Take(COUNT_PROMOTIONS_IN_SEARCH_BOX)
                        .ToList();
                }

                totalSuggestions = allTitle.Count;
                return allTitle;
            }
        }

        private string Description(string text, string searchValue)
        {
            const int LENGTH_NEEDED = 40;
            if (text?.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase) == true)
            {
                if (text.Length <= LENGTH_NEEDED)
                {
                    return " | " + text;
                }
                else
                {
                    int indexOfSearchValue = text.IndexOf(searchValue, StringComparison.CurrentCultureIgnoreCase);
                    int surplus = LENGTH_NEEDED - searchValue.Length;
                    int LENGTH_SIDE_PARTS = surplus / 2;
                    string leftPart = text.Substring(0, indexOfSearchValue);
                    string rightPart = text.Substring(indexOfSearchValue + searchValue.Length);

                    if (leftPart.Length >= LENGTH_SIDE_PARTS && rightPart.Length >= LENGTH_SIDE_PARTS)
                    {
                        leftPart = leftPart.Substring(leftPart.Length - LENGTH_SIDE_PARTS);
                        rightPart = rightPart.Substring(0, LENGTH_SIDE_PARTS);

                        leftPart = "... " + leftPart.TrimStart();
                        rightPart = rightPart.TrimEnd() + "...";
                    }
                    else if (leftPart.Length <= LENGTH_SIDE_PARTS && rightPart.Length >= LENGTH_SIDE_PARTS)
                    {
                        leftPart = leftPart.Substring(0, leftPart.Length);
                        rightPart = rightPart.Substring(0, surplus - leftPart.Length);
                        rightPart = rightPart.TrimEnd() + "...";
                    }
                    else if (leftPart.Length >= LENGTH_SIDE_PARTS && rightPart.Length <= LENGTH_SIDE_PARTS)
                    {
                        leftPart = leftPart.Substring(leftPart.Length - surplus);
                        leftPart = "... " + leftPart.TrimStart();
                    }

                    return " | " + leftPart + searchValue + rightPart;
                }
            }
            else
            {
                return "";
            }
        }

        private List<SuggestionsFromSearch> GetAllTitleFromGOW(string searchValue, ref int remainingBalanceOfSuggestions)
        {
            using (var context = _dbContextFactory.Create())
            {
                var allTitle = context.GOWs
                    .Where(w =>
                        w.IsEnable && 
                        (w.Name.Contains(searchValue) || w.ShortDescription.Contains(searchValue)))
                    .Select(s => new SuggestionsFromSearch
                    { Category = "Группы товаров", Label = s.Name + Description(s.ShortDescription, searchValue), Link = $"groupOfWares={s.SubUrl}"})
                    .Take(remainingBalanceOfSuggestions)
                    .ToList();

                int result = allTitle.Count;

                remainingBalanceOfSuggestions = (result < remainingBalanceOfSuggestions) ?
                    remainingBalanceOfSuggestions -= result : 0;

                return allTitle;
            }
        }

        private List<SuggestionsFromSearch> GetAllTitleFromBrands(string searchValue, ref int remainingBalanceOfSuggestions)
        {
            using (var context = _dbContextFactory.Create())
            {
                var allTitle = context.Brands
                    .Where(w => w.IsEnable && 
                            w.Name.Contains(searchValue))
                    .Select(s => new SuggestionsFromSearch
                    { Category = "Бренды", Label = s.Name, Link=$"brands={s.SubUrl}" })
                    .Take(remainingBalanceOfSuggestions)
                    .ToList();
                int result = allTitle.Count;

                remainingBalanceOfSuggestions = (result < remainingBalanceOfSuggestions) ?
                    remainingBalanceOfSuggestions -= result : 0;

                return allTitle;
            }
        }

        private List<SuggestionsFromSearch> GetAllTitleFromCategory(string searchValue, int remainingBalanceOfSuggestions)
        {
            using (var context = _dbContextFactory.Create())
            {
                var queryCategoryValuesTitle = context.Categories
                .Include(x => x.CategoryValueses)
                .Select(s => s.CategoryValueses
                        .Where(w1 => w1.IsEnable && 
                                w1.Name.Contains(searchValue))
                        .Select(s1 => new SuggestionsFromSearch
                        { Category = "Категория", Label = s1.Name, Link = $"{s.Name}={s1.Name}" }))
                .Take(remainingBalanceOfSuggestions)
                .ToList();

                var allTitle = new List<SuggestionsFromSearch>();
                foreach (var titleList in queryCategoryValuesTitle)
                {
                    allTitle.AddRange(titleList);
                }

                return allTitle;
            }
        }

        private string GetGOWSubUrl(string searchValue)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.GOWs
                    .Where(w => w.IsEnable &&
                            string.Equals(w.Name, searchValue, StringComparison.OrdinalIgnoreCase))
                    .Select(s => s.SubUrl)
                    .FirstOrDefault();
            }
        }

        private string GetBrandSubUrl(string searchValue)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Brands
                    .Where(w => w.IsEnable && 
                            string.Equals(w.Name, searchValue, StringComparison.OrdinalIgnoreCase))
                    .Select(s => s.SubUrl)
                    .FirstOrDefault();
            }
        }

        private string[] GetCategorySearchParamUrl(string searchValue)
        {
            using (var context = _dbContextFactory.Create())
            {
                var categoryValueSubUrl = context.CategoryValueses
                    .Include(x => x.Category)
                    .Where(w => w.IsEnable && 
                            string.Equals(w.Name.ToLower(), searchValue.ToLower()))
                    .FirstOrDefault();

                return (categoryValueSubUrl?.Category != null) ?
                    new string[] { categoryValueSubUrl.Category.Name, categoryValueSubUrl.Name } : null;
            }
        }
    }
}
