angular.module("gcApp.services", [])
    .factory("Model", ["$resource", "Config", function ($resource, Config) {

        var Model = $resource("", {}, {
            GetAllSlider: {
                url: "/Slider/GetAllSlider",
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
            GetSliderById: {
                url: "/Slider/GetSliderById",
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
            UpdateSlider: {
                url: "/Slider/UpdateSlider",
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
            DeleteSlider: {
                url: "/Slider/DeleteSlider",
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
            }
        });

        return Model;
    }])
    .service("popupService", function ($window) {
        this.showPopup = function (message) {
            return $window.confirm(message);
        }
    });