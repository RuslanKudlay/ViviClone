$('#brandsSideMenu').on('click', '.checkbox_filter_title_brand', function (event) {
    let parent = $(this).parent();
    let key = $(this).attr('data-search-key').toLowerCase();
    let value = $(this).attr('data-search-value').toLowerCase();

    if (parent.find('.checkbox_filter_brand').hasClass('filter_checked')) {

        removeSearchValueFromURL(key, value);
        uncheckCheckbox($(this));
    } else {
        addSearchValueToURL(key, value);
        parent.find('.checkbox_filter_brand').addClass('filter_checked');
    }

    if (window.location.href.includes('page=')) {
        replasePageNumberInUrl('page=' + 1);
    } else {
        addPageNumberToUrl('page', 1);
    }
    currentPage = 1;

    let filters = {
        isFiltrationActive: true,
        isUpdateSideSearchMenu: true,
        isUpdateBrands: false,
        isUpdatePriceSlider: true,
        checkSkipOption: true
    }

    updateWaresBySelectedCategoryValues(filters);
});

$('#brandsSideMenu').on('click', '.checkbox_filter_brand', function (event) {
    let key = $(this).attr('data-search-key').toLowerCase();
    let value = $(this).attr('data-search-value').toLowerCase();

    if ($(this).hasClass('filter_checked')) {
        removeSearchValueFromURL(key, value);
        uncheckCheckbox($(this));
    } else {
        addSearchValueToURL(key, value);
        $(this).addClass('filter_checked');
    }

    if (window.location.href.includes('page=')) {
        replasePageNumberInUrl('page=' + 1);
    } else {
        addPageNumberToUrl('page', 1);
    }
    currentPage = 1;

    let filters = {
        isFiltrationActive: true,
        isUpdateSideSearchMenu: true,
        isUpdateBrands: false,
        isUpdatePriceSlider: true,
        checkSkipOption: true
    }

    updateWaresBySelectedCategoryValues(filters);
});

function updateBrands(data) {
    $.ajax({
        url: ShopUrlSettings.UpdateBrandsSideMenu,
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: 'html'
    }).done(function (data, statusText, xhdr) {

        $('#brandsSideMenu').html(data);

        let disabledCheckedBrands = $('#brandsSideMenu').find('.disabled-button').children('div.filter_checked');

        if (disabledCheckedBrands.length > 0) {
            for (var i = 0; i < disabledCheckedBrands.length; i++) {
                disabledCheckedBrands[i].classList.remove("filter_checked");

                let key = disabledCheckedBrands[i].getAttribute('data-search-key').toLowerCase();
                let value = disabledCheckedBrands[i].getAttribute('data-search-value').toLowerCase();

                removeSearchValueFromURL(key, value);
            }
        }

    }).fail(function (xhdr, statusText, errorText) {
        $('#brandsSideMenu').text(JSON.stringify(xhdr));
        resultItemsCount = 0;
    });
}