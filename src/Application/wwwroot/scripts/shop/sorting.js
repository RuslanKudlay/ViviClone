$('#sortingSelect').change(function () {
    let choosenTypeSorting = $('#sortingSelect').find('.sorting_chosed');
    if (choosenTypeSorting.length === 0) {
        addTypeSortingToUrl('sort', $('#sortingSelect').val());
        $('#sortingSelect option:selected').addClass('sorting_chosed');
    } else {
        replaseTypeSortingToUrl($('#sortingSelect option.sorting_chosed').val(), $('#sortingSelect').val());
        $('option.sorting_chosed').removeClass('sorting_chosed');
        $('#sortingSelect option:selected').addClass('sorting_chosed');
    }

    let filters = {
        isFiltrationActive: false,
        isUpdateSideSearchMenu: false,
        isUpdateBrands: false,
        isUpdatePriceSlider: false,
        skipBuildPages: true
    }

    updateWaresBySelectedCategoryValues(filters);
});

function addTypeSortingToUrl(key, value) {
    let url = window.location.href;

    key = encodeURIComponent(key);
    value = encodeURIComponent(value);

    if (url.includes("search")) {
        let newUrl = url + keySeparator + key + keyValueSeparator + value;
        window.history.pushState({ path: newUrl }, '', newUrl);
    }
    else {
        let newUrl = url + "?search=" + key + keyValueSeparator + value;
        window.history.pushState({ path: newUrl }, '', newUrl);
    }
}

function replaseTypeSortingToUrl(oldValue, newValue) {
    let url = window.location.href;

    oldvalue = encodeURIComponent(oldValue);
    newValue = encodeURIComponent(newValue);

    let newUrl = url.replace(oldvalue, newValue);
    window.history.pushState({ path: newUrl }, '', newUrl);
}