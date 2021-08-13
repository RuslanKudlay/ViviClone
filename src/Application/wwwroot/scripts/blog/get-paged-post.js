var query =
    {
        BlogId: 1,
        PostSubUrl: "",
        Skip: 0,
        Take: 5,
        OrderBy: "",
        OrderByDesc: "",
        DateCreatedFrom: "",
        DateCreatedTo: "",
        PostQuantity: 0,
        ContainTitle: ""
    }

function getPagedPost(pageNumber) {
    if (pageNumber > 0) {
        query.Skip = (pageNumber - 1) * query.Take;
    }
    else {
        disableNextPageBtn() ?
            pageNumber = getNextPage() :
            "";
    }
    if (pageNumber) {
        $.post("http://localhost:44339/Blog/Index", query).
            done(function (response) {
                $('#section-icon-box').replaceWith("");
                $('#blogList').replaceWith(response);
                toggleActive(pageNumber);
            });
    }
}

var toggleActive = function (pageNumber) {
    $('.page-nav > .active').removeClass("active");
    $('#nav-item-' + pageNumber).addClass("active");
}

var getNextPage = function () {
    var currentPage = $('.page-nav > .active').text();
    query.Skip = currentPage * query.Take;

    return +currentPage + 1;
}

var disableNextPageBtn = function () {
    var currentPage = $('.page-nav > .active').text();
    var pageQuantity = $('.page-nav').children().length - 1

    return pageQuantity > currentPage;
}