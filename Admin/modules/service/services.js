angular.module("gcApp.services", [])
    .factory("Model", ["$resource", "Config", function ($resource, Config) {

        var Model = $resource("", {}, {
            InsertService: {
                url: "/Service/InsertService",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    return angular.toJson(data.obj);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    return result;
                }
            },
            GetAllService: {
                url: "/Service/GetAllService",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    return angular.toJson(data.obj);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    return result;
                }
            },
            GetServiceById: {
                url: "/Service/GetServiceById",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    return angular.toJson(data);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);

                    return result;
                }
            },
            UpdateService: {
                url: "/Service/UpdateService",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    return angular.toJson(data.obj);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    return result;
                }
            },
            DeleteService: {
                url: "/Service/DeleteService",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    return angular.toJson(data.obj);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    return result;
                }
            },
            GetImageByPortfolioId: {
                url: "/Service/GetImageByPortfolioId",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    return angular.toJson(data);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    console.log(result);
                    return result;
                }
            },
            AddImage: {
                url: "/Service/AddImage",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    return angular.toJson(data);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    return result;
                }
            },
            DeleteImage: {
                url: "/Service/DeleteImage",
                method: "POST",
                headers: {
                    //"X-ApiKey": Config.ApiKey
                },
                transformRequest: function (data, headersGetter) {
                    return angular.toJson(data);
                },
                transformResponse: function (data, headersGetter) {
                    var result = angular.fromJson(data);
                    return result;
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