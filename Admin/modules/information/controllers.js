'use strict';

var app = angular.module('gcApp.controllers', []);

app.controller("UpdateController",

    function ($scope, $stateParams, svcCache, svcQueue, $location, Config, Model) {
        $scope.btntext = "Update";
        $scope.record = {};

        $scope.fields = $.map(Config.fields, function (f) {
            if (f.edit === true) {
                return f;
            }
            return null;
        });

        $scope.back = function () {
            $location.path('/');
        };

        $scope.ok = function () {
            var obj = {};
            for (var j = 0; j < Config.fields.length; j++) {
                var f = Config.fields[j];
                if (typeof $scope.record[f.field] != "undefined") {
                    switch (f.type) {
                        case 'date':
                            //obj._d[f.field] = moment(record[f.field], Config.defaultConfig.dateFormat).format(Config.defaultConfig.isoDateTimeFormat);
                            obj[f.field] = moment($scope.record[f.field], "MM/DD/YYYY 00:00:00").format(Config.defaultConfig.isoDateTimeFormat);
                            break;
                        case "datetime":
                            obj[f.field] = moment($scope.record[f.field], "MM/DD/YYYY HH:mm:ss").format(Config.defaultConfig.isoDateTimeFormat);
                            break;
                        case "time":
                            obj[f.field] = $scope.record[f.field].format(Config.defaultConfig.timeFormat);
                            break;
                        case "select":
                            obj[f.field] = $scope.record[f.field].Value;
                            break;
                        case "select2":
                            obj[f.field] = $scope.record[f.field];
                            break;
                        case "textarea":
                            obj[f.field] = $scope.record[f.field] != null && $scope.record[f.field].length > 0 ? $scope.record[f.field].replace(/\r?\n/g, '<br />') : "";
                            break;

                        default:
                            obj[f.field] = $scope.record[f.field];
                            break;
                    }
                }
            }
            $scope.applySuccess = Model.UpdateInformation({
                obj: obj
            }, function (response) {
                if (response.Result == 1) {
                    alert("Successful change");
                }
                else {
                    alert("Failed !!! Try again");
                }
            });
        };

        //Records
        Model.GetInformationById({ id: 1 }, function (response) {
            if (response.Result == 1) {
                $scope.record = response.Records;
            }
        });
    });