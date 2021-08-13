$("#searchElement").keyup(function (event) {
    if (event.target.value.length < 2) {
        $("#buttonSearch").addClass("disabled-button");
    }
});

$("#searchElement").focus(function (event) {
    $("#ui-id-1").css("display", "block");
});

$(function () {
    $.widget("custom.catcomplete", $.ui.autocomplete, {
        _create: function () {
            this._super();
            this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
        },
        _renderItem: function (ul, item) {
            var newText = String(item.value).replace(
                new RegExp(this.term, "gi"),
                "<span class='ui-state-highlight'>$&</span>");

            var divHighlight = "<div>" + newText + "</div>";
            var itemLink = divHighlight;

            var link;
            if (item.link) {
                if (item.link.includes(keyValueSeparator)) {
                    let key = item.link.split(keyValueSeparator)[0];
                    let value = item.link.split(keyValueSeparator)[1];

                    key = encodeURIComponent(key);
                    value = encodeURIComponent(value);
                    link = window.location.origin + "/Shop?search=" + key + keyValueSeparator + value;
                } else {
                    link = window.location.origin + ShopUrlSettings.WareDetails + '?subUrl=' + item.link;
                }

                itemLink = `<a href="${link}">` + divHighlight + "</a>";
            }

            return $(`<li></li>`)
                .data("item.autocomplete", item)
                .append(itemLink)
                .appendTo(ul);
        },
        _renderMenu: function (ul, items) {
            var that = this,
                currentCategory = "";
            $.each(items, function (index, item) {
                var li;
                if (item.category !== currentCategory) {
                    if (item.category) {
                        ul.append("<li class='ui-autocomplete-category'>" + item.category + "</li>");
                        currentCategory = item.category;
                    }
                }
                li = that._renderItemData(ul, item);
                if (item.category) {
                    li.attr("aria-label", item.category + " : " + item.label);
                }
            });
        },
    });

    $("#searchElement").catcomplete({
        delay: 350,
        minLength: 2,
        source: function (request, response) {
            $.ajax({
                url: ShopUrlSettings.SearchSuggestions,
                type: 'GET',
                data: { term: request.term, GOW: getSearchParamValue(window.location.href, 'groupOfWares', false, keySeparator, valueSeparator, keyValueSeparator)},
                dataType: 'json',
                success: function (data) {
                    response(data);
                }
            });
        },
        response: function (event, ui) {
            if (ui.content.length > 0) {
                $("#buttonSearch").removeClass("disabled-button");
            } else {
                var noResult = { value: "No results found", label: "No results found" };
                ui.content.push(noResult);
                $("#buttonSearch").addClass("disabled-button");
            }
        },
        select: function (event, ui) {
            if (ui.item.label === noResultsLabel) {
                event.preventDefault();
            }
        },
        focus: function (event, ui) {
            if (ui.item.label === noResultsLabel) {
                event.preventDefault();
            }
        }
    })
});

$('#buttonSearch').click(function (event) {
    event.preventDefault();
    let newSearchValue = $('#searchElement').val();
    let oldSearchValue = searchValue;
    searchValue = newSearchValue;
    if (searchValue !== oldSearchValue) {
        if (window.location.href.includes('text=')) {
            // replace
            replaseSearchTextInUrl(oldSearchValue, searchValue);
        }
        else {
            // add at first
            addSearchTextToUrl(searchValue);
        }

        let checkedBrands = $('#brandsSideMenu').find('div.filter_checked');
        let checkedCategories = $('#sideMenuOptions').find('div.filter_checked');

        if (checkedBrands.length > 0) {
            for (var i = 0; i < checkedBrands.length; i++) {
                checkedBrands[i].classList.remove("filter_checked");

                let key = checkedBrands[i].getAttribute('data-search-key').toLowerCase();
                let value = checkedBrands[i].getAttribute('data-search-value').toLowerCase();

                removeSearchValueFromURL(key, value);
            }
        }

        if (checkedCategories.length > 0) {
            for (var i = 0; i < checkedCategories.length; i++) {
                checkedCategories[i].classList.remove("filter_checked");

                let key = checkedCategories[i].getAttribute('data-search-key').toLowerCase();
                let value = checkedCategories[i].getAttribute('data-search-value').toLowerCase();

                removeSearchValueFromURL(key, value);
            }
        }

        let filters = {
            isFiltrationActive: true,
            isUpdateSideSearchMenu: true,
            isUpdateBrands: true,
            isUpdatePriceSlider: true
        }

        updateWaresBySelectedCategoryValues(filters);
    }
});

function addSearchTextToUrl(value) {
    let url = window.location.href;

    key = encodeURIComponent("text");
    value = encodeURIComponent(value);

    if (url.includes("search")) {
        let indexOfStartSearch = url.indexOf("search=");
        indexOfStartSearch += 7;
        let newUrl = url.slice(0, indexOfStartSearch) + key + keyValueSeparator + value + keySeparator + url.slice(indexOfStartSearch);
        window.history.pushState({ path: newUrl }, '', newUrl);
    }
    else {
        let newUrl = url + '?search=' + key + keyValueSeparator + value;
        window.history.pushState({ path: newUrl }, '', newUrl);
    }
}

function replaseSearchTextInUrl(oldSearchValue, newSearchValue) {
    let url = window.location.href;

    valueOld = encodeURIComponent(oldSearchValue);
    valueNew = encodeURIComponent(newSearchValue);

    let newUrl = url.replace("text=" + valueOld, "text=" + valueNew);
    window.history.pushState({ path: newUrl }, '', newUrl);
}