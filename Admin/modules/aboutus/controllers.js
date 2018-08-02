'use strict';

var app = angular.module('gcApp.controllers', []);

app.controller("UpdateController",

    function ($scope, $stateParams, svcCache, svcQueue, $location, Config, Model, Upload) {
        $scope.btntext = "Update";
        $scope.record = {};

        var isDone = false;
        $scope.uploadFiles = function (files, errFiles) {
            $scope.files = files;
            $scope.errFiles = errFiles;
            angular.forEach(files, function (file) {
                isDone = false;
                file.upload = Upload.upload({
                    url: '/Upload/MultipleUpload',
                    data: { file: file }
                });

                file.upload.then(function (response) {
                    $scope.record.Avatar = response.data;
                }, function (response) {
                    console.log(response);
                    if (response.status > 0)
                        $scope.errorMsg = response.status + ': ' + response.data;
                }, function (evt) {
                    console.log(evt);
                    file.progress = Math.min(100, parseInt(100.0 *
                        evt.loaded / evt.total));
                    if (evt.loaded == evt.total) {
                        isDone = true;
                    }
                    $scope.uploadalert = isDone ? "Done" : "Uploading...";
                });
            });
        }

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
            $scope.applySuccess = Model.UpdateAboutUs({
                obj: obj
            }, function (response) {
                if (response.Result == 1) {
                    alert("Changed success");
                }
                else {
                    alert("Failed !!! Try again");
                }
            });
        };

        //Options
        $scope.options = {};
        $scope.$watch(function () { return svcCache.get('appReady'); }, function (newValue, oldValue, scope) {
            if (oldValue === false) {
                for (var i = 0; i < Config.fields.length; i++) {
                    var f = Config.fields[i];
                    if (f.type == "select" || f.type == "select2") {
                        scope.options[f.field] = [];
                        //var cacheKey = 'options_' + f.field + '_' + f.option;

                        var cacheKey = "";
                        switch (typeof f.option) {
                            case "object":
                                cacheKey = 'options_' + f.field + '_array';
                                break;
                            default:
                                cacheKey = 'options_' + f.field + '_' + f.option;
                                break;
                        }

                        var optcache = svcCache.get(cacheKey);
                        if (typeof optcache != "undefined") {
                            scope.options[f.field] = optcache;
                            //$scope.record[f.field] = optcache[0].Value;
                        }
                    }
                }
            }
        });

        //Records
        Model.GetAboutUsById({ id: 2 }, function (response) {
            if (response.Result == 1) {
                $scope.record = response.Records;
            }
        });
    });