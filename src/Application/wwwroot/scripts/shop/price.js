minPrice = parseInt($('#minPriceWare').attr('min'));
maxPrice = parseInt($('#maxPriceWare').attr('max'));

constMinPrice = minPrice;
constMaxPrice = maxPrice;

$('#maxPrice').val(maxPrice);
$('#minPrice').val(minPrice);

$(function () {
    $("#slider-range-price").slider({
        range: true,
        min: minPrice,
        max: maxPrice,
        values: [minPrice, maxPrice],
        slide: function (event, ui) {
            $("#minPrice").val(ui.values[0]);
            minPrice = parseInt($("#minPrice").val());

            $("#maxPrice").val(ui.values[1]);
            maxPrice = parseInt($("#maxPrice").val());
        }
    });

    $("#setPriceRange").click(function () {
        let url = window.location.href;

        minPrice = parseInt($("#minPrice").val());
        maxPrice = parseInt($("#maxPrice").val());

        $("#slider-range-price").slider("option", "values", [minPrice, maxPrice]);

        if (url.includes('price')) {
            replacePriceInUrl(oldMinPrice, oldMaxPrice, minPrice, maxPrice);
        }
        else {
            addPriceToUrl('price', minPrice, maxPrice);
        }

        oldMinPrice = minPrice;
        oldMaxPrice = maxPrice;

        let filters = {
            isFiltrationActive: true,
            isUpdateSideSearchMenu: true,
            isUpdateBrands: true,
            isUpdatePriceSlider: false
        }

        updateWaresBySelectedCategoryValues(filters);
    });

    $("#minPrice")
        .keyup(function () {
            minPrice = parseInt($(this).val());
            min = $("#slider-range-price").slider("option", "min");

            if (isNaN(minPrice))
                $(this).val(min)
            else $(this).val(minPrice);

            if (minPrice < min || minPrice > maxPrice) {
                $("#setPriceRange").prop("disabled", true);
                $(this).addClass("form_state_error");
                $('#maxPrice').addClass("form_state_error");
            } else {
                $("#setPriceRange").prop("disabled", false);
                $(this).removeClass("form_state_error");
                $('#maxPrice').removeClass("form_state_error");
            }

        });

    $("#maxPrice")
        .keyup(function () {
            maxPrice = parseInt($(this).val());
            max = $("#slider-range-price").slider("option", "max");

            if (isNaN(maxPrice))
                $(this).val(max)
            else $(this).val(maxPrice);

            if (maxPrice > max || maxPrice < minPrice) {
                $("#setPriceRange").prop("disabled", true);
                $(this).addClass("form_state_error");
                $('#minPrice').addClass("form_state_error");
            } else {
                $("#setPriceRange").prop("disabled", false);
                $(this).removeClass("form_state_error");
                $('#minPrice').removeClass("form_state_error");
            }
        });
});

function updatePriceSlider() {
    let newMinPrice = parseInt($('#minPriceWare').attr('min'));
    let newMaxPrice = parseInt($('#maxPriceWare').attr('max'));

    $("#slider-range-price").slider("option", "min", newMinPrice);
    $("#slider-range-price").slider("option", "max", newMaxPrice);
    $("#slider-range-price").slider("option", "values", [newMinPrice, newMaxPrice]);
    $("#minPrice").val(newMinPrice);
    $("#maxPrice").val(newMaxPrice);

    if (newMinPrice !== newMaxPrice) {
        $("#slider-range-price").slider("enable");
        $("#minPrice").prop("disabled", false);
        $("#maxPrice").prop("disabled", false);
        $("#setPriceRange").prop("disabled", false);
    } else {
        $("#setPriceRange").prop("disabled", true);
        $("#slider-range-price").slider("disable");
        $("#minPrice").prop("disabled", true);
        $("#maxPrice").prop("disabled", true);
    }
}

function addPriceToUrl(key, min, max) {
    let url = window.location.href;

    key = encodeURIComponent(key);

    let selectedPrice = key + keyValueSeparator + min + keyPriceSeparator + max;

    if (url.includes("search")) {
        if (url.includes("page=")) {
            if (url.includes("take=")) {

                takeItemsCount = "take=" + $('#showWaresCount').val();

                if (url.includes(takeItemsCount + keySeparator)) {
                    takeItemsCount += keySeparator;
                    url = url.replace(takeItemsCount, takeItemsCount + selectedPrice + keySeparator);
                } else {
                    url += keySeparator + selectedPrice;
                }

                window.history.pushState({ path: url }, '', url);

            } else {

                pageNumber = "page=" + currentPage;

                if (url.includes(pageNumber + keySeparator)) {
                    pageNumber += keySeparator;
                    url = url.replace(pageNumber, pageNumber + selectedPrice + keySeparator);
                } else {
                    url += keySeparator + selectedPrice;
                }

                window.history.pushState({ path: url }, '', url);
            }
            return;
        }
        else {
            let indexOfStartSearch = url.indexOf("search=");
            indexOfStartSearch += 7;
            let newUrl = url.slice(0, indexOfStartSearch) + selectedPrice + keySeparator + url.slice(indexOfStartSearch);
            window.history.pushState({ path: newUrl }, '', newUrl);
        }
    }
    else {
        let newUrl = url + "?search=" + selectedPrice;
        window.history.pushState({ path: newUrl }, '', newUrl);
    }
}

function replacePriceInUrl(minOld, maxOld, min, max) {
    let url = window.location.href;
    url = url.replace('price=' + minOld + keyPriceSeparator + maxOld, 'price=' + min + keyPriceSeparator + max);
    window.history.pushState({ path: url }, '', url);
}