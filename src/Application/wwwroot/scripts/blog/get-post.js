window.onload = function () {
    $('#nav-item-1').addClass("active");
}

var query =
{
    BlogId : 1,
    PostSubUrl: "",
    Skip: 0,
    Take: 0,
    OrderBy: "",
    OrderByDesc: "",
    DateCreatedFrom: "",
    DateCreatedTo: "",
    PostQuantity: 0,
    ContainTitle: ""
    }

function getPost(subUrl) {
    query.PostSubUrl = subUrl;
    $.post("http://localhost:44339/Blog/GetPost", query).
        done(function (response) {
            document.getElementById('blogList').innerHTML = response;
        });
}
