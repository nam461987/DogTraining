angular.module("gcApp.services", [])
    .factory("Model", ["$resource", "Config", function ($resource, Config) {

        var Model = $resource("", {}, {
            getRoute: {
                url: "/Admin/GetGroupOption",
                method: "GET",
                isArray: true,
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    if (result.Result === 1) {
                        return result.Records;
                    }
                    return [];
                }
            },
            getModule: {
                url: "/Admin/GetModuleOption",
                method: "GET",
                isArray: true,
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    if (result.Result === 1) {
                        return result.Records;
                    }
                    return [];
                }
            },
            getPermission: {
                url: "/Admin/GetPermissionByGroupAndModule",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                isArray: true,
                transformRequest: function (data, headersGetter) {
                    var obj = {
                        GroupId: data.GroupId,
                        Module: data.Module
                    };

                    return angular.toJson(obj);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    if (result.Result === 1) {

                        return result.Records;
                    }
                    return [];
                }
            },
            applyPermission: {
                url: "/Admin/InsertOrUpdatePermission",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    var obj = {
                        GroupId: data.GroupId,
                        PermissionId: data.PermissionId,
                        Status: data.Status
                    };

                    return angular.toJson(obj);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    if (result.Result === 1) {

                        return result.Result;
                    }
                    return result.Result === 0;
                }
            }
        });

        return Model;
    }])
    .service("popupService", function ($window) {
        this.showPopup = function (message) {
            return $window.confirm(message);
        }
    });