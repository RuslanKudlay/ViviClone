$(document).ready(function () {

    /* backstretch slider */
    //$.backstretch([
    //  //  "../../../Content/imgbg/bg-backstretch01.jpg",
    //  //  "../../../Content/imgbg/bg-backstretch02.jpg",
    //  //"../../../Content/imgbg/bg-backstretch03.jpg"   
    //    "../../../imgbg/img_1.jpg",
    //  "../../imgbg/img_2.jpg",
    //  "/imgbg/img_3.jpg"
    //], {
    //        fade: 3000,
    //        duration: 7000
    //    });

    /* navbar */
    $(window).scroll(function () {
        if ($(window).scrollTop() > 175) {
            $('.navbar-primary .navbar-nav > li.dropdown.open').removeClass('open');
            $('.navbar-primary .navbar-collapse.in').removeClass('in');
        }
        if ($(window).scrollTop() > 180) {
            $('.navbar-secondary-hide > .navbar').addClass('navbar-fixed-top container');
            $('.navbar-secondary').removeClass('navbar-secondary-hide');
        } else {
            $('.navbar-secondary > .navbar').removeClass('navbar-fixed-top container');
            $('.navbar-secondary').addClass('navbar-secondary-hide');
        }
    });

    $('.navbar-primary').clone().prependTo('.navbar-secondary');
    $('.navbar-secondary > .navbar').removeClass('navbar-primary');
    $('.navbar-secondary > .navbar .navbar-collapse').attr('id', 'bs-example-navbar-collapse-2');
    $('.navbar-secondary > .navbar .navbar-toggle').attr('data-target', '#bs-example-navbar-collapse-2');


    /* magnify image */
    //$('.portfolio-image').magnificPopup({
    //    delegate: '[data-image="image-group"]',
    //    type: 'image',
    //    gallery: {
    //        enabled: true
    //    },

    //});

    //$('.portfolio-image-alt').magnificPopup({
    //    type: 'image',
    //    delegate: 'a.image-zoom'
    //});


    /* scrolltop */
    $('.scrolltop').on('click', function (event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top
        }, 1500, 'easeInOutExpo');
        event.preventDefault();
    });


    /* masonry layout */
    var $container = $('.container-blog');
    $container.imagesLoaded(function () {
        $container.masonry();
    });


    /* map contact */
    //$("#map").gmap3({
    //    map: {
    //        options: {
    //            center: [45.536945, -73.510712],
    //            zoom: 10,
    //            scrollwheel: false
    //        }
    //    },
    //    marker: {
    //        values: [
    //            { latLng: [45.536945, -73.510712], data: "Longueuil, Quebec" }
    //        ],
    //        options: {
    //            draggable: false
    //        },
    //        events: {
    //            mouseover: function (marker, event, context) {
    //                var map = $(this).gmap3("get"),
    //                    infowindow = $(this).gmap3({ get: { name: "infowindow" } });
    //                if (infowindow) {
    //                    infowindow.open(map, marker);
    //                    infowindow.setContent(context.data);
    //                } else {
    //                    $(this).gmap3({
    //                        infowindow: {
    //                            anchor: marker,
    //                            options: { content: context.data }
    //                        }
    //                    });
    //                }
    //            },
    //            mouseout: function () {
    //                var infowindow = $(this).gmap3({ get: { name: "infowindow" } });
    //                if (infowindow) {
    //                    infowindow.close();
    //                }
    //            }
    //        }
    //    }
    //});
});