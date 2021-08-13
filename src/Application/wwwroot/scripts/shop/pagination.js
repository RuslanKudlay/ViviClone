isIncludesPageWhenParsingUrl();
updatePages();

$('.page-nav-item').click(function () {
    activatePageItem(this);
});

$('#lastPage').click(function () {
    let pageNumber = $('#lastPage').attr('numberPage');
    let lastPage = $(`div[pagenumber='${pageNumber}']`);
    lastPage.trigger('click');
});

$('#firstPage').click(function () {
    let pageNumber = $('#firstPage').attr('numberPage');
    let firstPage = $(`div[pagenumber='${pageNumber}']`);
    firstPage.trigger('click');
});

$('#nextPage').click(function () {
    let nextPage = $(`div[pagenumber='${parseInt(currentPage) + 1}']`);
    nextPage.trigger('click');
});

$('#prevPage').click(function () {
    let prevPage = $(`div[pagenumber='${parseInt(currentPage) - 1}']`);
    prevPage.trigger('click');
});

// Page number
$('#allPages').on('click', '.page-nav-item', function (event) {
    let pageNewNumber = event.target.getAttribute('pageNumber');
    currentPage = pageNewNumber;

    if (window.location.href.includes('page=')) {
        // replace
        replasePageNumberInUrl('page=' + pageNewNumber);
    }
    else {
        // add at first
        addPageNumberToUrl('page', pageNewNumber);
    }

    let filters = {
        isFiltrationActive: false,
        isUpdateSideSearchMenu: false,
        isUpdateBrands: false,
        isUpdatePriceSlider: false,
    }

    updateWaresBySelectedCategoryValues(filters);

    event.stopPropagation();
});

function activatePageItem(pageItem) {
    var buttons = $('.page-nav-item');
    for (var i = 0; i < buttons.length; i++) {
        $(buttons[i]).removeClass('active');
    }
    $(pageItem).addClass('active');
}

function addPageNumberToUrl(key, value) {
    let url = window.location.href;

    key = encodeURIComponent(key);
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
};

function replasePageNumberInUrl(newPageNumber) {
    let url = window.location.href;
    let pageOldNumber = "";

    let indexPage = url.indexOf('page=');
    indexPage += 5;

    for (; indexPage < url.length; indexPage++) {
        pageOldNumber += url[indexPage];
        if (pageOldNumber.includes(';')) {
            pageOldNumber = pageOldNumber.slice(0, pageOldNumber.length - 1);
            break;
        }
    }

    let newUrl = url.replace('page=' + pageOldNumber, newPageNumber);
    window.history.pushState({ path: newUrl }, '', newUrl);
}

function removePageNumberFromUrl() {
    let url = window.location.href;

    if (url.includes("page=1;")) {
        url = url.replace("page=1;", "");
    } else {
        url = url.replace("page=1", "");
    }

    let position = url.indexOf('search=');
    if (url.length === position + 7) {
        url = url.replace("?search=", "");
    }

    window.history.pushState({ path: url }, '', url);
};

function buildPages(totalPageCount, isFiltrationActive) {
    $('#allPages').empty();
    totalPageCount = Math.floor(totalPageCount);

    let pagesPanel = $('#pagesPanel');

    if (pagesPanel === null || pagesPanel === undefined) {
        pagesPanel = $('.pagesPanel');
    }

    if (totalPageCount === 1) {
        pagesPanel.addClass('hideElement');
        removePageNumberFromUrl();
        return;
    } else {
        pagesPanel.removeClass('hideElement');
    }

    currentPage = parseInt(currentPage);
    let distanceBeetweenFirstAndCurrentPage = currentPage - 1;
    let distanceBeetweenLastAndCurrentPage = totalPageCount - currentPage;
    let optionalCountPages = 8;
    const neededCountPages = 9;

    let page = "";
    for (let index = 1; index <= totalPageCount; index++, optionalCountPages--) {
        if (isFiltrationActive === true) {
            if (index === 1) {
                if (window.location.href.includes('page=')) {
                    replasePageNumberInUrl('page=' + 1);
                } else {
                    addPageNumberToUrl('page', 1);
                }
                currentPage = 1;
                page = $(`<div pageNumber="${index}" class="page-nav-item active">${index}</div>`);
            }
            else {
                if (index === 8 && totalPageCount > neededCountPages) {
                    page = $(`<div pageNumber="${index}" class="page-nav-item">...</div>`);
                    page1 = $(`<div pageNumber="${totalPageCount}" class="page-nav-item">${totalPageCount}</div>`);
                    $('#allPages').append(page);
                    $('#allPages').append(page1);
                    break;
                } else {
                    page = $(`<div pageNumber="${index}" class="page-nav-item">${index}</div>`);
                }
            }
        }
        else {
            // Both sides have more than 4 pages
            if (distanceBeetweenFirstAndCurrentPage > 4 && distanceBeetweenLastAndCurrentPage > 4) {
                let pages = [
                    page1 = $(`<div pageNumber="1" class="page-nav-item">1</div>`),
                    page2 = $(`<div pageNumber="${currentPage - 3}" class="page-nav-item">...</div>`),
                    page3 = $(`<div pageNumber="${currentPage - 2}" class="page-nav-item">${currentPage - 2}</div>`),
                    page4 = $(`<div pageNumber="${currentPage - 1}" class="page-nav-item">${currentPage - 1}</div>`),
                    page5 = $(`<div pageNumber="${currentPage}" class="page-nav-item active">${currentPage}</div>`),
                    page6 = $(`<div pageNumber="${currentPage + 1}" class="page-nav-item">${currentPage + 1}</div>`),
                    page7 = $(`<div pageNumber="${currentPage + 2}" class="page-nav-item">${currentPage + 2}</div>`),
                    page8 = $(`<div pageNumber="${currentPage + 3}" class="page-nav-item">...</div>`),
                    page9 = $(`<div pageNumber="${totalPageCount}" class="page-nav-item">${totalPageCount}</div>`)
                ];

                pages.forEach(page => $('#allPages').append(page));
                break;
            }
            // One side have more than 4 pages
            else if ((distanceBeetweenFirstAndCurrentPage > 4 || distanceBeetweenLastAndCurrentPage > 4) && totalPageCount > neededCountPages) {
                if (distanceBeetweenFirstAndCurrentPage > 4) {
                    if (index === 1) {
                        page = $(`<div pageNumber="1" class="page-nav-item">1</div>`);
                    } else if (index === 2) {
                        page = $(`<div pageNumber="${totalPageCount - optionalCountPages}" class="page-nav-item">...</div>`);
                    } else if (totalPageCount - optionalCountPages == currentPage) {
                        page = $(`<div pageNumber="${currentPage}" class="page-nav-item active">${totalPageCount - optionalCountPages}</div>`);
                    } else {
                        page = $(`<div pageNumber="${totalPageCount - optionalCountPages}" class="page-nav-item">${totalPageCount - optionalCountPages}</div>`);
                        if (optionalCountPages === -1) break;
                    }
                } else {
                    if (currentPage === 1) {
                        if (index === 1) {
                            page = $(`<div pageNumber="${index}" class="page-nav-item active">${index}</div>`);
                        }
                        else {
                            if (index === 8) {
                                page = $(`<div pageNumber="${index}" class="page-nav-item">...</div>`);
                                page1 = $(`<div pageNumber="${totalPageCount}" class="page-nav-item">${totalPageCount}</div>`);
                                $('#allPages').append(page);
                                $('#allPages').append(page1);
                                break;
                            } else {
                                page = $(`<div pageNumber="${index}" class="page-nav-item">${index}</div>`);
                            }
                        }
                    } else {
                        if (index === 8) {
                            page = $(`<div pageNumber="${index}" class="page-nav-item">...</div>`);
                            page1 = $(`<div pageNumber="${totalPageCount}" class="page-nav-item">${totalPageCount}</div>`);
                            $('#allPages').append(page);
                            $('#allPages').append(page1);
                            break;
                        } else if (index === currentPage) {
                            page = $(`<div pageNumber="${index}" class="page-nav-item active">${index}</div>`);
                        } else {
                            page = $(`<div pageNumber="${index}" class="page-nav-item">${index}</div>`);
                        }
                    }
                }
            }
            else {
                if (index !== currentPage) {
                    page = $(`<div pageNumber="${index}" class="page-nav-item">${index}</div>`);
                }
                else {
                    page = $(`<div pageNumber="${index}" class="page-nav-item active">${index}</div>`);
                }
            }
        }

        $('#allPages').append(page);
    }
}

function updateSwitchesPages(totalPageCount) {
    if (currentPage === 1) {
        $('#prevPage').addClass('disabled-div');
        $('#firstPage').addClass('disabled-div');

        $('#nextPage').removeClass('disabled-div');
        $('#lastPage').removeClass('disabled-div');
    } else {
        $('#prevPage').removeClass('disabled-div');
        $('#firstPage').removeClass('disabled-div');
        if (currentPage === Math.floor(totalPageCount)) {
            $('#nextPage').addClass('disabled-div');
            $('#lastPage').addClass('disabled-div');
        }
        else {
            $('#nextPage').removeClass('disabled-div');
            $('#lastPage').removeClass('disabled-div');
        }
    }

    $('#lastPage').attr('numberPage', Math.floor(totalPageCount));
}

function updatePages(isFiltrationActive) {
    let totalPageCount = resultItemsCount / takeItemsCount + 1;

    buildPages(totalPageCount, isFiltrationActive);
    updateSwitchesPages(totalPageCount);
}

function isIncludesPageWhenParsingUrl() {
    let url = window.location.href;

    if (url.includes('page=')) {
        if (url.includes("page=")) {
            let url = window.location.href;
            let pageOldNumber = "";

            let indexPage = url.indexOf('page=');
            indexPage += 5;

            for (; indexPage < url.length; indexPage++) {
                pageOldNumber += url[indexPage];
                if (pageOldNumber.includes(';')) {
                    pageOldNumber = pageOldNumber.slice(0, pageOldNumber.length - 1);
                    break;
                }
            }

            currentPage = pageOldNumber;
        }
    }
}