$('#sideMenuOptions').on('click', '.checkbox_filter_category', function (event) {
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

$('#sideMenuOptions').on('click', '.checkbox_filter_title_category', function (event) {
    let parent = $(this).parent();
    let key = $(this).attr('data-search-key').toLowerCase();
    let value = $(this).attr('data-search-value').toLowerCase();

    if (parent.find('.checkbox_filter_category').hasClass('filter_checked')) {
        removeSearchValueFromURL(key, value);
        uncheckCheckbox($(this));
    } else {
        addSearchValueToURL(key, value);
        parent.find('.checkbox_filter_category').addClass('filter_checked');
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

function updateSideSearchMenu(data) {
    // Sends post request for update Side Search Menu
    $.ajax({
        url: ShopUrlSettings.UpdateSideSearchMenu,
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: 'html'
    }).done(function (data, statusText, xhdr) {
        $('#sideMenuOptions').html(data);
    }).fail(function (xhdr, statusText, errorText) {
        $('#sideMenuOptions').text(JSON.stringify(xhdr));
        resultItemsCount = 0;
    });
}

function getSelectedCategoriesAndCategoryValues() {
    // Creates an array of selected parameters in the form of pairs of category names and category values
    const allCategories = document.querySelectorAll('.categories-sidebar');
    const selectedCategoriesAndCategoryValues = [];

    for (let i = 0; i < allCategories.length; i++) {
        const checkedItems = allCategories[i].querySelectorAll('.filter_checked');

        let nameSelectedCategory = allCategories[i].querySelector('.sidebar-title-section');

        if (checkedItems && checkedItems.length !== 0 && nameSelectedCategory.innerText.toLowerCase() !== 'бренды') {

            selectedCategoriesAndCategoryValues.push({
                categoryName: nameSelectedCategory.innerText,
                categoryValues: []
            });

            for (let j = 0; j < checkedItems.length; j++) {
                selectedCategoriesAndCategoryValues[selectedCategoriesAndCategoryValues.length - 1].categoryValues.push(checkedItems[j].nextElementSibling.innerText);
            }
        }
    }

    return selectedCategoriesAndCategoryValues;
}