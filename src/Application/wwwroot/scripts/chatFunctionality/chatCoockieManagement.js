function coockieManagement() {

    var getCookie = function (name) {
        var matches = document.cookie.match(new RegExp(
            "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
        ));
        return matches ? decodeURIComponent(matches[1]) : undefined;
    }

    var setCookie = function (name, value, options) {
        options = options || {};

        var expires = options.expires;
        var updatedCookie;

        if (typeof expires === "number" && expires) {
            var d = new Date();
            var exp = d.getTime() + expires * 60000;
            d.setTime(exp);
            expires = options.expires = d;
        }
        if (expires && expires.toUTCString) {
            options.expires = expires.toUTCString();
        }

        if (value && value !== null) {
            value = encodeURIComponent(value);
            updatedCookie = name + "=" + value;
        }
        else {
            value = encodeURIComponent(readCookie(name));
            updatedCookie = name + "=" + value;
        }

        for (var propName in options) {
            updatedCookie += "; " + propName;
            var propValue = options[propName];
            if (propValue !== true) {
                updatedCookie += "=" + propValue;
            }
        }

        document.cookie = updatedCookie;
    }

    var deleteCookie = function (name) {
        setCookie(name, "", {
            expires: -1
        });
    }

    var readCookie = function (name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) === ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    return {
        getCookie: getCookie,
        setCookie: setCookie,
        deleteCookie: deleteCookie
    };
}