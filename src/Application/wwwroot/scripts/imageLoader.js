// In each view component in which we load images asynchronously, in the end we must include a script for asynchronously loading images.
// This is necessary because this component is displayed asynchronously, and the image loading script specified in _XXXLayout is not executed twice,
// because at the time of loading, these tags not exists. Or you may call reloadImages method.
$(document).ready(function () {
    reloadImages();
});

function reloadImages() {
    $(".loadImage").each(function () {
        var that = $(this);
        var imageUrl = $(this).attr("data-imageurl");
        var image;
        if (imageUrl) {
            $.ajax({
                type: "GET",
                url: imageUrl,
                contentType: "image/jpg",
                success: function (result) {
                    image = result;
                    that.attr('src', image);
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
        else {
            that.attr('src', '/img/carousel-alt-img/Cosmetic_1.jpg');
        }
        if ($(this).attr("data-zoom-image")) {
            $(this).attr('data-zoom-image', image);
        }
    });

    $(".loadImage").each(function () {
        var that = $(this);
        var imageUrl = $(this).attr("data-imageurl-href");
        var image;
        if (imageUrl) {
            $.ajax({
                type: "GET",
                url: imageUrl,
                contentType: "image/jpg",
                success: function (result) {
                    image = result;
                    that.attr('href', image);
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
        else {
            that.attr('href', '/img/carousel-alt-img/Cosmetic_1.jpg');
        }
        if ($(this).attr("data-zoom-image")) {
            $(this).attr('data-zoom-image', image);
        }
    });

    // The following code is needed for uploading images in TinyMCE iframe
    $('#ware-text_ifr').contents().find('.loadImage').each(function () {
        var that = $(this);
        var imageUrl = $(this).attr("data-imageurl");
        var image;
        if (imageUrl) {
            $.ajax({
                type: "GET",
                url: imageUrl,
                contentType: "image/jpg",
                success: function (result) {
                    image = result;
                    that.attr('src', image);
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
        else {
            that.attr('src', '/img/carousel-alt-img/Cosmetic_1.jpg');
        }
        if ($(this).attr("data-zoom-image")) {
            $(this).attr('data-zoom-image', image);
        }
    });
}

