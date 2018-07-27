'use strict';

/* Services */

/*
 http://docs.angularjs.org/api/ngResource.$resource

 Default ngResources are defined as

 'get':    {method:'GET'},
 'save':   {method:'POST'},
 'query':  {method:'GET', isArray:true},
 'remove': {method:'DELETE'},
 'delete': {method:'DELETE'}

 */

var services = angular.module('ngBus.services', []);

services.factory('Factory', ['Config', '$http', 'svcCache', function (Config, $http, svcCache) {
    var service = {};

    service.query = function (predicate, reverse, offset, limit, condition) {
        var od = {};
        od[predicate] = (reverse == true ? "DESC" : "ASC");
        var obj = {
            _c: {
                //"OperatorId": oid,
                //"Status": 1
            },
            _f: $.map(Config.fields, function (f) {
                if (f.list === true) {
                    return f.field;
                }
                return null;
            }).join(','),
            _od: od,
            _os: offset,
            _lm: limit
        };

        for (var k in condition) {
            if (condition.hasOwnProperty(k)) {
                switch (k) {
                    case "Keyword":
                        if (condition[k].length > 3) {
                            obj._c["Name"] = "PATINDEX('%{0}%',Name) > 0".format(condition[k]);
                        }
                        break;
                    default:
                        if (condition[k].Value != "0") {
                            obj._c[k] = condition[k].Value;
                        }
                        break;
                }
            }
        }

        return $http({
            method: "POST",
            url: "/Admin/Get_Group_List",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).then(function (response) {
            var data = response.data;
            var records = [];
            if (data.Result == 1) {
                for (var i = 0; i < data.Records.length; i++) {
                    //data.Records[i].pop();
                    records.push(data.Records[i]);
                }
            }
            return {
                Result: data.Result,
                Records: records,
                TotalRecordCount: data.TotalRecordCount,
                Message: data.Message
            };
        });
    };

    service.delete = function (id, status) {
        status = parseInt(status);
        if (status == 1) {
            var obj = {
                Id: id, Status: 0
            };
        }
        if (status == 0) {
            var obj = {
                Id: id, Status: 1
            };
        }
        console.log(obj);
        return $http({
            method: "POST",
            url: "/Admin/Delete_Group",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).then(function (response) {
            var data = response.data;
            if (data.Result == 1 && data.Records.length > 0) {
                return data.Records[0].Id;
            }
            return 0;
        });
    };

    service.update = function (record) {
        var obj = {
            Id: record.Id,
            ModifiedDate: moment().format(Config.defaultConfig.isoDateTimeFormat)
        };
        //console.log(obj);
        for (var j = 0; j < Config.fields.length; j++) {
            var f = Config.fields[j];
            if (typeof record[f.field] != "undefined") {
                switch (f.type) {
                    case 'date':
                        obj[f.field] = record[f.field].format(Config.defaultConfig.isoDateTimeFormat);
                        break;
                    case "datetime":
                        obj[f.field] = record[f.field].format(Config.defaultConfig.isoDateTimeFormat);
                        break;
                    case "time":
                        obj[f.field] = record[f.field].format(Config.defaultConfig.timeFormat);
                        break;
                    case "select":
                        obj[f.field] = record[f.field].Value;
                        break;
                    case "password":
                        if (record[f.field] != null && record[f.field] !== "") {
                            obj[f.field] = record[f.field];
                        }
                        break;
                    default:
                        obj[f.field] = record[f.field];
                        break;
                }
            }
        }

        console.log(obj);

        return $http.post("/Admin/Update_Group", obj).then(function (response) {
            var data = response.data;
            console.log(response);
            if (data.Result == 1 && data.Records.length > 0) {
                return data.Records[0][0];
            }
            return 0;
        });
    };

    service.get = function (id) {
        var obj = {
            _c: {
                "Id": id
            }
        };

        return $http({
            method: "POST",
            url: "/Admin/Get_Group_List",
            data: JSON.stringify(obj),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).then(function (response) {
            var data = response.data;
            var records = [];
            if (data.Result == 1) {

                var fields = $.map(Config.fields, function (f) {
                    if (f.edit === true) {
                        return f;
                    }
                    return null;
                });

                for (var i = 0; i < data.Records.length; i++) {
                    var record = {};
                    for (var j = 0; j < fields.length; j++) {
                        if (typeof (fields[j].type) != "undefined") {
                            switch (fields[j].type) {
                                case 'date':
                                    data.Records[i][fields[j].field] = moment(data.Records[i][fields[j].field], Config.defaultConfig.isoDateTimeFormat);
                                    break;
                                case "datetime":
                                    data.Records[i][fields[j].field] = moment(data.Records[i][fields[j].field], Config.defaultConfig.isoDateTimeFormat);
                                    break;
                                case "time":
                                    data.Records[i][fields[j].field] = moment(data.Records[i][fields[j].field], Config.defaultConfig.timeFormat);
                                    break;
                                case "password":
                                    data.Records[i][fields[j].field] = "";
                                    break;
                                case "select":

                                    //var cacheKey = 'options_' + Config.fields[j].field + '_' + Config.fields[j].option;
                                    var cacheKey = "";
                                    switch (typeof fields[j].option) {
                                        case "object":
                                            cacheKey = 'options_' + fields[j].field + '_array';
                                            break;
                                        default:
                                            cacheKey = 'options_' + fields[j].field + '_' + fields[j].option;
                                            break;
                                    }
                                    var optcache = svcCache.get(cacheKey);
                                    if (typeof optcache != "undefined" && optcache != null) {
                                        var match = false;
                                        for (var k = 0; k < optcache.length && !match; k++) {
                                            if (data.Records[i][fields[j].field] == parseInt(optcache[k].Value)) {
                                                data.Records[i][fields[j].field] = optcache[k];
                                                match = true;
                                            }
                                        }
                                        if (!match) {
                                            if (fields[j].depend !== true) {
                                                data.Records[i][fields[j].field] = optcache[0];
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        record[fields[j].field] = data.Records[i][fields[j].field];
                    }
                    records.push(record);
                }
            }
            return records[0];
        });
    };

    service.insert = function (record) {
        console.log(record);
        var obj = {};

        for (var j = 0; j < Config.fields.length; j++) {
            var f = Config.fields[j];
            if (typeof record[f.field] != "undefined") {
                switch (f.type) {
                    case 'date':
                        //obj._d[f.field] = moment(record[f.field], Config.defaultConfig.dateFormat).format(Config.defaultConfig.isoDateTimeFormat);
                        obj[f.field] = record[f.field].format(Config.defaultConfig.isoDateTimeFormat);
                        break;
                    case "datetime":
                        obj[f.field] = record[f.field].format(Config.defaultConfig.isoDateTimeFormat);
                        break;
                    case "time":
                        obj[f.field] = record[f.field].format(Config.defaultConfig.timeFormat);
                        break;
                    case "select":
                        obj[f.field] = record[f.field].Value;
                        break;

                    default:
                        obj[f.field] = record[f.field];
                        break;
                }
            }
        }

        return $http.post("/Admin/Create_Group", obj).then(function (response) {
            var data = response.data;
            if (data.Result == 1 && data.Records != null) {
                return data.Records.Id;
            }
            return 0;
        });
    };
    return service;
}]);

services.factory('svcQueue', function ($q, $http) {
    var queue = [];
    var execNext = function () {
        var task = queue[0];
        $http(task.c).then(function (data) {
            queue.shift();
            task.d.resolve(data);
            if (queue.length > 0) execNext();
        }, function (err) {
            task.d.reject(err);
        })
            ;
    };
    return function (config) {
        var d = $q.defer();
        queue.push({ c: config, d: d });
        if (queue.length === 1) execNext();
        return d.promise;
    };
});