using System;

namespace Application.EntitiesModels.Models
{
    public class SearchWareParamsModel
    {
        public string GOWName { get; set; }
        public string[] BrandSubUrls { get; set; }
        public SelectedSearchParam[] SearchParams { get; set; }
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 50;
        public string SortBy { get; set; } = "popular";
        public PriceRange Price { get; set; }
        public string SearchText { get; set; }
        public bool ForProfessionals { get; set; }

        public AdditionalSearchParams AdjustSearch { get; set; } = new AdditionalSearchParams();

        public static StateSideSearchMenu GetStateSideSearchMenu(SearchWareParamsModel searchParams)
        {
            var result = searchParams != null ?
                    searchParams.SearchText?.Length > 0 ? StateSideSearchMenu.OnlySearch :
                        searchParams.GOWName?.Length > 0 ? StateSideSearchMenu.GOW : StateSideSearchMenu.WithoutParams :
                StateSideSearchMenu.Undefined;

            return result == StateSideSearchMenu.Undefined ? result : CheckStateSideSearchMenu(searchParams, result);
        }

        private static StateSideSearchMenu CheckStateSideSearchMenu(SearchWareParamsModel searchParams, StateSideSearchMenu stateSideSearchMenu)
        {
            StateSideSearchMenu state = StateSideSearchMenu.WithoutParams;

            if (searchParams.SearchParams?.Length > 0 && searchParams.BrandSubUrls?.Length > 0)
            {
                state = StateSideSearchMenu.BrandsParams;
            }
            else
            {
                if (searchParams.BrandSubUrls?.Length > 0)
                {
                    state = StateSideSearchMenu.OnlyBrands;
                }
                else if (searchParams.SearchParams?.Length > 0)
                {
                    state = StateSideSearchMenu.OnlyParams;
                }
            }

            bool isOnlySearch = stateSideSearchMenu is StateSideSearchMenu.OnlySearch;
            bool isOnlyGOW = stateSideSearchMenu is StateSideSearchMenu.GOW;

            switch (state)
            {
                case StateSideSearchMenu.BrandsParams:
                    return isOnlySearch ?
                        StateSideSearchMenu.SearchBrandsParams :
                        isOnlyGOW ? StateSideSearchMenu.GowBrandsParams : StateSideSearchMenu.BrandsParams;

                case StateSideSearchMenu.OnlyBrands:
                    return isOnlySearch ?
                        StateSideSearchMenu.SearchBrands :
                        isOnlyGOW ? StateSideSearchMenu.GowBrands : StateSideSearchMenu.OnlyBrands;

                case StateSideSearchMenu.OnlyParams:
                    return isOnlySearch ?
                        StateSideSearchMenu.SearchParams :
                        isOnlyGOW ? StateSideSearchMenu.GowParams : StateSideSearchMenu.OnlyParams;

                case StateSideSearchMenu.WithoutParams:
                    return stateSideSearchMenu;
            }

            return state;
        }
    }

    public class SelectedSearchParam
    {
        public string CategoryName { get; set; }
        public string[] CategoryValues { get; set; }
    }

    public class PriceRange
    {
        public int MinPrice;
        public int MaxPrice;
    }

    public class AdditionalSearchParams
    {
        public bool SkipSearchBrandsAndCategories { get; set; } = false;
        public bool SkipSearchBrands { get; set; } = false;
    }
}
