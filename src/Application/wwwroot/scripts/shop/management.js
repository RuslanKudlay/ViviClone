//#region Ware filtration
// "http://hostname:port/shop?search=ТИП=Сухая кожа,Очищение / Отшелушивание ;НАЗНАЧЕНИЕ=Увлажнение;brand=ella-bache"
// "http://hostname:port/shop?search=ТИП=Сухая кожа,Очищение / Отшелушивание ;НАЗНАЧЕНИЕ=Увлажнение;brand=ella-bache"
// "http://hostname:port/shop?search=ТИП=Сухая кожа,Очищение / Отшелушивание ;НАЗНАЧЕНИЕ=Увлажнение;УПАКОВКА=50 мл;groupOfWares=uvlazhnenie"

function uncheckCheckbox(checkbox) {
    checkbox = checkbox.parent().find('.filter_checked');
    if (checkbox.length) {
        checkbox.removeClass('filter_checked');
    }
}

function addSearchValueToURL(key, value) {
    let url = window.location.href;
    let typeSort = "";
    if (url.includes("sort")) {
        let indexOfSort = url.indexOf("sort");
        if (url.includes(";sort")) {
            typeSort = url.substring(indexOfSort - 1, url.length);
            url = url.replace(typeSort, "");
        }
        else {
            typeSort = url.substring(indexOfSort, url.length);
            url = url.replace("?search=" + typeSort, "");
        }
        typeSort = typeSort.replace(";", "");
    }
    let pageNumber = "";
    if (url.includes("page")) {
        pageNumber = "page=" + $('div .page-nav-item.active').attr('pageNumber');
        if (url.includes(pageNumber + keySeparator)) {
            url = url.replace(pageNumber + keySeparator, "");
        }
        else {
            url = url.replace("?search=" + pageNumber, "");
        }
        pageNumber += keySeparator;
    }
    let searchParams = getSearchParams(url, keySeparator, valueSeparator, keyValueSeparator);

    key = encodeURIComponent(key);
    value = encodeURIComponent(value);

    if (searchParams) {
        const searchParam = searchParams.find(searchParam => searchParam.key.toLowerCase() == key.toLowerCase());
        if (searchParam) {
            searchParam.values.push(value);
        } else {
            searchParams.push({
                key: key,
                values: [value]
            });
        }
    } else {
        searchParams = [];
        searchParams.push({
            key: key,
            values: [value]
        });
    }

    let newUrl = window.location.protocol + '//' + window.location.host + '/Shop?search=' + pageNumber + searchParams.map(searchParam => searchParam.key + keyValueSeparator + searchParam.values.join(valueSeparator)).join(keySeparator);

    if (typeSort.length != 0) {
        newUrl += keySeparator + typeSort;
    }

    window.history.pushState({ path: newUrl }, '', newUrl);
}

function removeSearchValueFromURL(key, value) {
    let url = window.location.href;
    let searchParams = getSearchParams(url, keySeparator, valueSeparator, keyValueSeparator);

    key = encodeURIComponent(key);
    value = encodeURIComponent(value);

    if (searchParams) {
        const searchParamIndex = searchParams.findIndex(searchParam => searchParam.key.toLowerCase() == key.toLowerCase());
        if (searchParamIndex !== -1) {
            searchParams[searchParamIndex].values.splice(searchParams[searchParamIndex].values.indexOf(value), 1);
            if (searchParams[searchParamIndex].values.length === 0) {
                searchParams.splice(searchParamIndex, 1);
            }
        }
    }

    let newUrl = window.location.protocol + '//' + window.location.host + (searchParams && searchParams.length !== 0 ? '/Shop?search=' : '') + searchParams.map(searchParam => searchParam.key + keyValueSeparator + searchParam.values.join(valueSeparator)).join(keySeparator);
    if (!newUrl.includes('Shop')) {
        newUrl += '/Shop';
    }
    window.history.pushState({ path: newUrl }, '', newUrl);
}

function getSearchParams(url, keySeparator, valueSeparator, keyValueSeparator) {
    if (url.includes('?search=')) {
        const searchParams = url.substring(url.indexOf('=') + 1);
        const keyValuesPairsStrArr = searchParams.split(keySeparator);
        const keyValuesPairs = [];

        for (let i = 0; i < keyValuesPairsStrArr.length; i++) {
            const splittedKeyValuePair = keyValuesPairsStrArr[i].split(keyValueSeparator);
            keyValuesPairs.push({
                key: splittedKeyValuePair[0],
                values: splittedKeyValuePair[1].split(valueSeparator)
            });
        }

        return keyValuesPairs;
    }
    return null;
}

function getSearchParamValue(url, paramValueKey, isArray, keySeparator, valueSeparator, keyValueSeparator) {
    if (url.includes('?search=')) {
        const searchParams = url.substring(url.indexOf('=') + 1);
        const keyValuesPairsStrArr = searchParams.split(keySeparator);

        for (let i = 0; i < keyValuesPairsStrArr.length; i++) {
            const splittedKeyValuePair = keyValuesPairsStrArr[i].split(keyValueSeparator);
            if (splittedKeyValuePair[0] === paramValueKey) {
                if (isArray) {
                    return splittedKeyValuePair[1].split(valueSeparator);
                } else {
                    return splittedKeyValuePair[1];
                }
            }
        }
    }
    return null;
}

function updateWaresBySelectedCategoryValues(filters) {
    // Gets an array of selected parameters in the form of pairs of category names and category values
    const selectedParams = getSelectedCategoriesAndCategoryValues();

    const isProf = getSelectedProfValues();

    // Creates object for searching wares
    const data = {
        SearchParams: selectedParams,
        GOWName: getSearchParamValue(window.location.href, 'groupOfWares', false, keySeparator, valueSeparator, keyValueSeparator),
        BrandSubUrls: getSearchParamValue(window.location.href, 'brands', true, keySeparator, valueSeparator, keyValueSeparator),
        Page: parseInt(currentPage),
        Take: parseInt($('#showWaresCount').val()),
        SortBy: $('#sortingSelect').val(),
        SearchText: searchValue,
        Price: null,
        ForProfessionals: isProf,
        AdjustSearch: {
            SkipSearchBrandsAndCategories: false,
            SkipSearchBrands: false,
            SkipSearchCategories: false
        }
    };

    let brandsCategoriesWares = {
        Wares: null,
        SideSearchMenuModel: {
            Brands: null,
            WareCategoryValues: null
        },
        Professional: null
    };

    if (!filters.isUpdateSideSearchMenu && !filters.isUpdateBrands && !filters.isUpdatePriceSlider) {
        data.AdjustSearch.SkipSearchBrandsAndCategories = true;
    }

    if (filters.checkSkipOption) {
        if (data.SearchParams.length === 0) {
            data.AdjustSearch.SkipSearchBrands = true;
        }
    }

    if (window.location.href.includes('price')) {
        data.Price = { MinPrice: minPrice, MaxPrice: maxPrice }
    }

    // Sends post request for searching wares
    $.ajax({
        url: ShopUrlSettings.WareByCategoryValues,
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: 'html'
    }).done(function (data, statusText, xhdr) {
        let dataJSON = JSON.parse(data);
        brandsCategoriesWares.Wares = dataJSON.wares;
        brandsCategoriesWares.SideSearchMenuModel.Professional = dataJSON.sideSearchMenuModel?.professional

        $.ajax({
            url: ShopUrlSettings.UpdateWaresComponent,
            type: 'POST',
            data: JSON.stringify(brandsCategoriesWares.Wares),
            contentType: 'application/json; charset=utf-8',
            dataType: 'html'
        }).done(function (data, statusText, xhdr) {
            $('#pasteWaresHere').html(data);
            resultItemsCount = parseInt($('#resultCountItems').attr('totalCount'));
            reloadImages();

            if (filters.isUpdatePriceSlider) {
                updatePriceSlider();
            }

            if (!filters.skipBuildPages) {
                updatePages(filters.isFiltrationActive);
            }
        });

        if (filters.isUpdateSideSearchMenu) {
            brandsCategoriesWares.SideSearchMenuModel.WareCategoryValues = dataJSON.sideSearchMenuModel.wareCategoryValues;
            updateSideSearchMenu(brandsCategoriesWares.SideSearchMenuModel);
        }

        if (filters.isUpdateBrands) {
            brandsCategoriesWares.SideSearchMenuModel.Brands = dataJSON.sideSearchMenuModel.brands;
            updateBrands(brandsCategoriesWares.SideSearchMenuModel);
        }
    });
}