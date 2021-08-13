'use strict';

$(document).ready(function () {

    var currentUrl = window.location.href
    var currentController = currentUrl.split("/")[3];
    var currentAction = currentUrl.split("/")[4];
    
    $('#menuActive li a').each(function (element) {

        var menuLink = $(this).attr("href");
        var menuLinkController = menuLink.split("/")[1]
        var menuLinkAction = menuLink.split("/")[2]

        if (currentController === "" && menuLink == '/' ||
            currentAction === menuLinkAction && typeof currentAction !== "undefined" ||
            currentController == menuLinkController && currentController !== 'Home') {

            $(this).parent().addClass("active");
        }
    });
});