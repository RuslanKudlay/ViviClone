using Application.EntitiesModels.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.BBL.ModelBinders
{
    public class SearchParamsBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(SearchWareParamsModel))
            {
                return new SearchParamsModelBinder();
            }
            return null;
        }
    }

    public class SearchParamsModelBinder : IModelBinder
    {
        private const char categorySeparator = ';';
        private const char valueSeparator = ',';
        private const char priceSeparator = '-';

        private readonly Type searchModelType = typeof(SearchWareParamsModel);

        private readonly Dictionary<string, string> searchParamToPropertyNameDict = new Dictionary<string, string>() {
            { "groupOfWares", "GOWName" }
        };

        private readonly Dictionary<string, string> searchIsProfessional = new Dictionary<string, string>()
        {
            { "professional", "Professional" }
        };

        private readonly Dictionary<string, string> stringArraysDict = new Dictionary<string, string>() {
            { "brands", "BrandSubUrls" }
        };

        private readonly Dictionary<string, string> searchParamPage = new Dictionary<string, string>() {
            { "page", "Page" }
        };

        private readonly Dictionary<string, string> searchParamSortBy = new Dictionary<string, string>() {
            { "sort", "SortBy" }
        };

        private readonly Dictionary<string, string> searchParamTake = new Dictionary<string, string>() {
            { "take", "Take" }
        };

        private readonly Dictionary<string, string> searchParamPrice = new Dictionary<string, string>() {
            { "price", "Price" }
        };
        
        private readonly Dictionary<string, string> searchParamText = new Dictionary<string, string>() {
            { "text", "SearchText" }
        };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // This method parse lines similar to those below:
            // "page=2"
            // "page=2;take=75"
            // "page=2;take=75;sort=nameAsc"
            // "page=2;take=75;brands=brands=firstbrand;sort=nameAsc"
            // "page=2;take=75;brands=brands=firstbrand,secondbrand;sort=nameAsc"
            // "page=2;take=75;brands=brands=firstbrand,secondbrand;sort=nameAsc"
            // "page=2;take=75;brands=brands=firstbrand,secondbrand;ДЕЙСТВИЕ=крем;sort=nameAsc"
            // "page=2;take=75;brands=brands=firstbrand,secondbrand;ДЕЙСТВИЕ=крем;sort=nameAsc"
            // "page=2;price=451-800;take=75;brands=brands=firstbrand,secondbrand;ДЕЙСТВИЕ=крем;sort=nameAsc"
            // "page=2;price=451-800;take=75;brands=brands=firstbrand,secondbrand;ДЕЙСТВИЕ=крем;УПАКОВКА=50 мл;sort=nameAsc"

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (value != null)
            {
                var result = CreateSearchWareParamsModel(value);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                var result = new SearchWareParamsModel();
                bindingContext.Result = ModelBindingResult.Success(result);
            }

            return Task.CompletedTask;
        }

        private SearchWareParamsModel CreateSearchWareParamsModel(string searchString)
        {
            var searchWareParamsModel = new SearchWareParamsModel();
            SetProperties(searchWareParamsModel, searchString);

            var listSearchParams = new List<SelectedSearchParam>(searchWareParamsModel.SearchParams);
            
            return searchWareParamsModel;
        }

        private void SetProperties(SearchWareParamsModel searchWareParamsModel, string searchString)
        {
            List<SelectedSearchParam> selectedSearchParams = new List<SelectedSearchParam>();
            string[] selectedSearchParamsStr = searchString.Split(categorySeparator);

            for (int i = 0; i < selectedSearchParamsStr.Length; i++)
            {
                string[] splittedData = selectedSearchParamsStr[i].Split('=');
                if (splittedData.Length != 2)
                {
                    throw new ArgumentException($"Search param doesn't contain only 1 '=': {selectedSearchParamsStr[i]}");
                }

                // Set string arrays
                if (stringArraysDict.ContainsKey(splittedData[0]))
                {
                    searchModelType.GetProperty(stringArraysDict[splittedData[0]]).SetValue(searchWareParamsModel, splittedData[1].Split(valueSeparator));
                    continue;
                }
                //Set isProfessional
                if(searchIsProfessional.ContainsKey(splittedData[0]))
                {
                    searchModelType.GetProperty("ForProfessionals").SetValue(searchWareParamsModel, true);
                    continue;
                }
                // Set properties
                if (searchParamToPropertyNameDict.ContainsKey(splittedData[0]))
                {
                    searchModelType.GetProperty(searchParamToPropertyNameDict[splittedData[0]]).SetValue(searchWareParamsModel, splittedData[1]);
                    continue;
                }

                // Set Page
                if (searchParamPage.ContainsKey(splittedData[0]))
                {
                    searchModelType.GetProperty(searchParamPage[splittedData[0]]).SetValue(searchWareParamsModel, int.Parse(splittedData[1]));
                    continue;
                }

                // Set Type Sort
                if (searchParamSortBy.ContainsKey(splittedData[0]))
                {
                    searchModelType.GetProperty(searchParamSortBy[splittedData[0]]).SetValue(searchWareParamsModel, splittedData[1]);
                    continue;
                }
                
                // Set Take
                if (searchParamTake.ContainsKey(splittedData[0]))
                {
                    searchModelType.GetProperty(searchParamTake[splittedData[0]]).SetValue(searchWareParamsModel, int.Parse(splittedData[1]));
                    continue;
                }

                // Set Price
                if (searchParamPrice.ContainsKey(splittedData[0]))
                {
                    string minMaxPrices = splittedData[1];
                    int minPrice = int.Parse(minMaxPrices.Split(priceSeparator)[0]);
                    int maxPrice = int.Parse(minMaxPrices.Split(priceSeparator)[1]);

                    PriceRange priceRangeValues = new PriceRange() { MinPrice = minPrice, MaxPrice = maxPrice};
                    searchModelType.GetProperty(searchParamPrice[splittedData[0]]).SetValue(searchWareParamsModel, priceRangeValues);
                    continue;
                }
                
                // Set Search Text
                if (searchParamText.ContainsKey(splittedData[0]))
                {
                    searchModelType.GetProperty(searchParamText[splittedData[0]]).SetValue(searchWareParamsModel, splittedData[1]);
                    continue;
                }

                selectedSearchParams.Add(CreateSelectedSearchParam(selectedSearchParamsStr[i]));
            }       

            searchWareParamsModel.SearchParams = selectedSearchParams.ToArray();
        }

        private SelectedSearchParam CreateSelectedSearchParam(string selectedSearchParamStr)
        {
            string[] splittedData = selectedSearchParamStr.Split('=');

            string categoryName = splittedData[0];
            string[] categoryValues = splittedData[1].Split(valueSeparator);

            SelectedSearchParam selectedSearchParam = new SelectedSearchParam()
            {
                CategoryName = categoryName,
                CategoryValues = categoryValues
            };

            return selectedSearchParam;
        }
    }
}
