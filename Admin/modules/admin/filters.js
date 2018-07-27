'use strict';

/* Filters */

var app = angular.module('ngService.filters', []);

app.filter('interpolate', ['version', function (version) {
    return function (text) {
        return String(text).replace(/\%VERSION\%/mg, version);
    };
}]);

app.filter('svcDate', [
    'Config', function (Config) {
        return function (text) {
            return moment(text).format(Config.defaultConfig.dateFormat);
        };
    }
]);
app.filter('svcDateTime', [
    'Config', function (Config) {
        return function (text) {
            return moment(text).format(Config.defaultConfig.dateTimeFormat);
        };
    }
]);
app.filter('svcMoney', function () {
    return function (text) {
        var v = parseInt(text);
        if (isNaN(v)) {
            return 0;
        } else {
            return v.formatMoney(0, ',', '.');
        }
    };
}
);
app.filter('svcOption', [
    'svcCache', function (svcCache) {
        return function (text, fieldName, optionUrl) {
            var result = "";
            var cacheKey = 'options_' + fieldName + '_' + optionUrl;
            var optcache = svcCache.get(cacheKey);
            if (typeof (optcache) != "undefined") {
                for (var k = 0; k < optcache.length; k++) {
                    if (optcache[k].Value == text) {
                        result = optcache[k].DisplayText;
                    }
                }
            }
            return result;
        };
    }
]);
app.filter('svcActive', function () {
    return function (text) {
        var ac = "Đang hoạt động";
        var iac = "Đang khóa";
        var v = parseInt(text);
        if (v == 1) {
            return ac;
        }
        if (v == 0) {
            return iac;
        }
    };
});

