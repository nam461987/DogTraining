'use strict';

var cache = angular.module('ngBus.caches', []);
cache.factory('svcCache', ['$cacheFactory', function ($cacheFactory) {
    return $cacheFactory('svc-cache');
}]);
cache.run([
    'Config', 'svcCache', 'svcQueue', function (Config, svcCache, svcQueue) {
        svcCache.removeAll();
        svcCache.put('appReady', false);
        var oqKey = [];
        for (var i = 0; i < Config.fields.length; i++) {
            if (typeof Config.fields[i].option != "undefined") {
                switch (typeof Config.fields[i].option) {
                    case "string":
                        var cacheKey = 'options_' + Config.fields[i].field + '_' + Config.fields[i].option;
                        var optcache = svcCache.get(cacheKey);
                        if (typeof (optcache) == "undefined") {
                            oqKey.push(cacheKey);
                            svcQueue({ method: "GET", url: Config.fields[i].option }).then(function (result) {
                                var odata = result.data;
                                if (odata.Result == 1) {
                                    var ridx = oqKey.shift();
                                    svcCache.put(ridx, odata.Records); //Store to cache
                                    if (oqKey.length == 0) {
                                        svcCache.put('appReady', true);
                                    }
                                }
                            });
                        }
                        break;

                    case "object":
                        var cacheKey2 = 'options_' + Config.fields[i].field + '_array';
                        var optcache2 = svcCache.get(cacheKey2);
                        if (typeof (optcache2) == "undefined") {
                            svcCache.put(cacheKey2, Config.fields[i].option); //Store to cache
                        }

                        break;
                }
            }
        }
    }
]);