'use strict';

/* Controllers */

var app = angular.module('ngBus.controllers', []);

app.controller('ListCtrl', ['$scope', 'Config', 'Factory', '$location', 'svcCache', 'svcQueue',

    function ($scope, Config, Factory, $location, svcCache, svcQueue) {
        $scope.offset = 0;
        $scope.limit = 10;
        $scope.predicate = 'CreatedDate';
        $scope.reverse = false;
        $scope.permissions = permissions;

        $scope.condition = {
            Keyword: ""
        };

        $scope.currentPage = 1;
        $scope.totalRecord = 0;

        $scope.order = function (predicate, reverse) {
            $scope.reverse = reverse;
            $scope.predicate = predicate;
            $scope.reload();
        };

        // callback for ng-click 'edit':
        $scope.edit = function (id) {
            $location.path('/edit/' + id);
        };

        // callback for ng-click 'remove':
        $scope.delete = function (id, active) {
            Factory.delete(id, active).then(function () {
                $scope.reload();
            });
        };

        // callback for ng-click 'add':
        $scope.create = function () {
            $location.path('/create');
        };

        $scope.reload = function () {
            $('.overlay').removeClass('hidden');
            Factory.query($scope.predicate, $scope.reverse, $scope.offset, $scope.limit, $scope.condition).then(function (data) {
                if (data.Result == 1) {
                    $scope.records = data.Records;
                    $scope.totalRecord = data.TotalRecordCount;
                    $('.overlay').addClass('hidden');
                }
            });
        };

        $scope.reset = function () {
            $scope.offset = 0;
            $scope.limit = 10;
            $scope.predicate = 'CreatedDate';
            $scope.reverse = false;

            $scope.currentPage = 1;
            $scope.totalRecord = 0;

            $scope.condition = {
                Keyword: ""
            };

            //Reload
            $scope.reload();
        };

        $scope.fields = $.map(Config.fields, function (f) {
            if (f.list === true) {
                return f;
            }
            return null;
        });

        //Options
        $scope.options = {};
        $scope.$watch(function () { return svcCache.get('appReady'); }, function (newValue, oldValue, scope) {
            if (newValue === true || oldValue === true) {
                for (var i = 0; i < Config.fields.length; i++) {
                    var f = Config.fields[i];
                    if (f.type == "select") {
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
                            scope.condition[f.field] = optcache[0];
                        }
                    }
                }

                scope.reload();
            }
        });
        //Reload
        $scope.reload();
    }]);

app.controller('EditCtrl', ['$scope', '$routeParams', 'Config', 'Factory', '$location', 'svcCache', 'svcQueue', "$filter",
    function ($scope, $routeParams, Config, Factory, $location, svcCache, svcQueue, $filter) {
        //console.log(getpermission("edit_manager") && $routeParams.id != 1);
        if ($routeParams.id != 1) {
            $scope.record = {};
            $scope.update = function () {
                Factory.update($scope.record);
                $location.path('/');
            };

            $scope.back = function () {
                $location.path('/');
            };

            $scope.fields = $.map(Config.fields, function (f) {
                if (f.edit === true) {
                    return f;
                }
                return null;
            });

            //Options
            $scope.options = {};
            $scope.$watch(function () { return svcCache.get('appReady'); }, function (newValue, oldValue, scope) {
                if (newValue === true) {
                    //alert("Đây rồi!!");
                    for (i = 0; i < Config.fields.length; i++) {
                        var f = Config.fields[i];
                        if (f.type == "select") {
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
                            //alert(cacheKey);
                            var optcache = svcCache.get(cacheKey);
                            if (typeof optcache != "undefined" && optcache != null) {
                                scope.options[f.field] = optcache;
                                var match = false;
                                for (var k = 0; k < optcache.length && !match; k++) {
                                    if ((scope.record[f.field] == parseInt(optcache[k].Value)) || (typeof scope.record[f.field] != "undefined" && (scope.record[f.field].Value == optcache[k].Value))) {
                                        scope.record[f.field] = optcache[k];
                                        match = true;
                                    }
                                }
                                //if (!match) {
                                //    scope.record[f.field] = optcache[0];
                                //}
                            }
                        }
                    }
                }
            });

            //Records
            Factory.get($routeParams.id).then(function (data) {
                $scope.record = data;
            });
        }
        else {
            $location.path('/');
        }

    }]);

app.controller('CreateCtrl', ['$scope', 'Config', 'Factory', '$location', 'svcCache', 'svcQueue',
    function ($scope, Config, Factory, $location, svcCache, svcQueue) {
        if (true == true) {
            //if (getpermission("create_manager") == true) {
            $scope.record = {};

            $scope.insert = function () {
                Factory.insert($scope.record).then(function (data) {
                    $location.path('/');
                });
            };
            $scope.back = function () {
                $location.path('/');
            };

            $scope.fields = $.map(Config.fields, function (f) {
                if (f.edit === true) {
                    return f;
                }
                return null;
            });

            //Options
            $scope.options = {};
            $scope.$watch(function () { return svcCache.get('appReady'); }, function (newValue, oldValue, scope) {
                if (newValue === true) {
                    for (var i = 0; i < Config.fields.length; i++) {
                        var f = Config.fields[i];
                        if (f.type == "select") {
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
                                $scope.options[f.field] = optcache;
                                $scope.record[f.field] = optcache[0];
                            }
                        }
                    }
                }
            });
        }
        else {
            $location.path('/');
        }


    }]);