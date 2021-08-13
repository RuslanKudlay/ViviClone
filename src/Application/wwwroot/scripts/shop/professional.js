$('#sideMenuOptions').on('click', '.checkbox_filter_proff', function (event) {
    let key = $(this).attr('data-search-key').toLowerCase();
    let value = $(this).attr('data-search-value').toLowerCase();

    if ($(this).hasClass('filter_checked')) {
        removeSearchValueFromURL(key, value);
        delete allValues.key;
        uncheckCheckbox($(this));
    } else {
        addSearchValueToURL(key, value);
        allValues[key] = value;
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
        isUpdateBrands: true,
        isUpdatePriceSlider: true
    }

    updateWaresBySelectedCategoryValues(filters);
});

$('#sideMenuOptions').on('click', '.checkbox_filter_title_proff', function (event) {
    let parent = $(this).parent();
    let key = $(this).attr('data-search-key').toLowerCase();
    let value = $(this).attr('data-search-value').toLowerCase();

    if (parent.find('.checkbox_filter_proff').hasClass('filter_checked')) {
        removeSearchValueFromURL(key, value);
        uncheckCheckbox($(this));
    } else {
        addSearchValueToURL(key, value);
        parent.find('.checkbox_filter_proff').addClass('filter_checked');
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
        isUpdateBrands: true,
        isUpdatePriceSlider: true
    }

    updateWaresBySelectedCategoryValues(filters);
});

function getSelectedProfValues() {
    // Creates an array of selected parameters in the form of pairs of category names and category values
    const prof = document.querySelector('.proff-sidebar');

    let res = false;
    if (prof) {
        const checkedProf = prof.querySelector('.filter_checked')

        checkedProf ? res = true : []
    }

    return res;
}