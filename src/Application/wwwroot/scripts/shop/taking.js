$('#showWaresCount').change(function () {
    let takeItems = $('#showWaresCount').find('.take_items_chosed');
    takeItemsCount = $('#showWaresCount').val();
    if (takeItems.length === 0) {
        addTakeItemsToUrl('take', takeItemsCount);
        $('#showWaresCount option:selected').addClass('take_items_chosed');
    } else {
        replaseTakeItemsInUrl('take=' + $('#showWaresCount option.take_items_chosed').val(), 'take=' + takeItemsCount);
        $('option.take_items_chosed').removeClass('take_items_chosed');
        $('#showWaresCount option:selected').addClass('take_items_chosed');
    }

    let filters = {
        isFiltrationActive: true,
        isUpdateSideSearchMenu: false,
        isUpdateBrands: false,
        isUpdatePriceSlider: false
    }

    updateWaresBySelectedCategoryValues(filters);
});

function addTakeItemsToUrl(key, value) {
    let url = window.location.href;

    key = encodeURIComponent(key);
    value = encodeURIComponent(value);

    let takeItems = key + keyValueSeparator + value;

    if (url.includes("search")) {
        if (url.includes("page=")) {
            pageNumber = "page=" + $('div .page-nav-item.active').attr('pageNumber');
            if (url.includes(pageNumber + keySeparator)) {
                url = url.replace(pageNumber + keySeparator, pageNumber + keySeparator + takeItems + keySeparator);
            }
            else {
                url += keySeparator + takeItems.slice(0, takeItems.length - 1);
            }

            let newUrl = url;
            window.history.pushState({ path: newUrl }, '', newUrl);
        }
        else {
            let indexOfStartSearch = url.indexOf("search=");
            indexOfStartSearch += 7;
            let newUrl = url.slice(0, indexOfStartSearch) + takeItems + keySeparator + url.slice(indexOfStartSearch);
            window.history.pushState({ path: newUrl }, '', newUrl);
        }
    }
    else {
        let newUrl = url + "?search=" + takeItems;
        window.history.pushState({ path: newUrl }, '', newUrl);
    }
}

function replaseTakeItemsInUrl(takeItemsOld, takeItemsNew) {
    let url = window.location.href;

    let newUrl = url.replace(takeItemsOld, takeItemsNew);
    window.history.pushState({ path: newUrl }, '', newUrl);
}