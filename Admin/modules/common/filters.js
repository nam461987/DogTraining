angular.module("gcApp.filters", []).filter("svcDate", [
    "Config", function (Config) {
        return function (text) {
            if (text != null && text !== "") {
                return moment(text).format(Config.defaultConfig.dateFormat);
            }
            return "";
        };
    }
]).filter("svcDateTime", [
    "Config", function (Config) {
        return function (text) {
            if (text != null && text !== "") {
                return moment(text).format(Config.defaultConfig.dateTimeFormat);
            }
            return "";
        };
    }
]).filter("svcMoney", function () {
    return function (text) {
        var v = parseInt(text);
        if (isNaN(v)) {
            return 0;
        } else {
            return v.formatMoney(0, ",", ".");
        }
    };
}
    ).filter("svcOption", [
        "svcCache", "Config", function (svcCache, Config) {
            return function (text, fieldName, optionUrl) {
                var result = "";
                if (typeof text != "undefined" && text != null) {
                    var cacheKey = Config.getCacheKey(fieldName, optionUrl);
                    var optcache = svcCache.get(cacheKey);
                    //console.log(cacheKey, optcache);
                    if (typeof (optcache) != "undefined") {
                        for (var k = 0; k < optcache.length; k++) {
                            if (parseInt(optcache[k].Value) === text || optcache[k].Value === text || parseInt(optcache[k].Value) === parseInt(text.Value) || optcache[k].Value === text.Value) {
                                result = optcache[k].DisplayText;
                            }
                        }
                    }
                }
                return result;
            };
        }
    ]).filter("svcNumber", function () {
        return function (text) {
            var v = parseInt(text);
            if (isNaN(v)) {
                return 0;
            } else {
                return v.formatMoney(0, ",", ".");
            }
        };
    }
    ).filter("svcImage", function () {
        return function (text, width, height) {
            var w = parseInt(width); if (isNaN(w)) { w = 100 };
            var h = parseInt(height); if (isNaN(h)) { h = 100 };
            var result = "";
            if (text != "") {
                result = "<img src='{0}' width='{1}' height='{2}' alt='' />".format(text, w, h);
            } else {
                result = "<img src='/Content/img/no_image.jpg' width='{1}' height='{2}' alt='' />".format(w, h);
            }
            return result;
        };
    }
    ).filter("svcUpload", ["Config", function (Config) {
        return function (text) {
            var result = "";
            if (text != null && text !== "") {
                result = "<img src='{0}' class='thumb-md' alt='' />".format(Config.rootSite + text);
            }
            return result;
        };
    }
    ]).filter("svcActive", function () {
        return function (text) {
            var ac = "Active";
            var iac = "Inactive";
            var v = parseInt(text);
            if (v == 1) {
                return ac;
            }
            if (v == 0) {
                return iac;
            }
        };
    }
    );