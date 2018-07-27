'use strict';

/* Directives */

var app = angular.module('ngBus.directives', []);
app.directive('appVersion', ['version', function (version) {
    return function (scope, elm, attrs) {
        elm.text(version);
    };
}]);

app.directive('dateselect', ['Config', function (Config) {
    return {
        require: 'ngModel',
        link: function (scope, el, attr, ngModel) {
            $(el).datepicker({
                format: Config.defaultConfig.datePickerFormat,
                autoclose: true
                //onSelect: function (dateText) {
                //    scope.$apply(function () {
                //        ngModel.$setViewValue(dateText);
                //    });
                //}
            });

            //$(el).datepicker('update');

            el.next().find('i').on('click', function (e) {
                e.preventDefault();
                $(el).datepicker('show');
            });

            ngModel.$parsers.push(function (data) {
                //return data;
                return moment(data, Config.defaultConfig.dateFormat).startOf('day');
            });
            ngModel.$formatters.push(function (data) {
                if (typeof data != "undefined" && data != null) {
                    return data.startOf('day').format(Config.defaultConfig.dateFormat);
                }
                return "";
            });
        }
    };
}]);

app.directive('datetimeselect', ['Config', function (Config) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, el, attr, ngModel) {
            $(el).datetimepicker({
                format: Config.defaultConfig.dateTimeFormat
            });

            el.on('dp.change', function (event) {
                scope.$apply(function () {
                    var date = moment(event.date);
                    ngModel.$setViewValue(date.format(Config.defaultConfig.dateTimeFormat));
                });
            });

            ngModel.$parsers.push(function (data) {
                return data;
            });
            ngModel.$formatters.push(function (data) {
                return moment(data).format(Config.defaultConfig.dateTimeFormat);
            });
        }
    };
}]);

app.directive('timeselect', ['Config', function (Config) {
    return {
        require: 'ngModel',
        link: function (scope, el, attr, ngModel) {
            $(el).timepicker({
                showMeridian: false,
                showInputs: false,
                minuteStep: 1
            });
            //el.on('changeTime.timepicker', function (e) {
            //    scope.$apply(function () {
            //        ngModel.$setViewValue(e.value);
            //    });
            //});           
            el.next().find('i').on('click', function (e) {
                e.preventDefault();
                $(el).timepicker('showWidget');
            });
            ngModel.$parsers.push(function (data) {
                return moment(data, Config.defaultConfig.timeFormat);
            });
            ngModel.$formatters.push(function (data) {
                return data.format(Config.defaultConfig.timeFormat);
            });
        }
    };
}]);

app.directive('ngConfirmClick', [function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('click', function () {
                var message = attrs.ngConfirmMessage;
                if (message && confirm(message)) {
                    scope.$apply(attrs.ngConfirmClick);
                }
            });
        }
    };
}]);

app.directive('svcPagination', [
    function () {
        return {
            restrict: 'E',
            link: function (scope, element, attrs) {
                scope.$watchGroup(['totalRecord', 'currentPage', 'limit'], function () {
                    //var pages = [];
                    var totalPage = Math.ceil(parseInt(attrs.totalRecord) / (parseInt(attrs.limit)));

                    var numPrevPages = 5;
                    var numNextPages = 5;

                    var prevPagesArr = [];
                    for (var i = scope.currentPage - numPrevPages; i < scope.currentPage; i++) {
                        if (i >= 1) {
                            prevPagesArr.push(i);
                        }
                    }

                    var nextPagesArr = [];
                    for (var j = scope.currentPage + 1; j < scope.currentPage + numNextPages; j++) {
                        if (j <= totalPage) {
                            nextPagesArr.push(j);
                        }
                    }

                    scope.prevPagesArr = prevPagesArr;
                    scope.nextPagesArr = nextPagesArr;
                    scope.totalPage = totalPage;
                });
                scope.setCurrent = function (c) {
                    scope.currentPage = c;
                    scope.offset = (c - 1) * attrs.limit;
                    scope.reload();
                };

                scope.pageOptions = [{
                    Value: 10,
                    DisplayText: "10"
                }, {
                    Value: 50,
                    DisplayText: "50"
                }, {
                    Value: 100,
                    DisplayText: "100"
                }, {
                    Value: 500,
                    DisplayText: "500"
                }, {
                    Value: 1000,
                    DisplayText: "1000"
                }, {
                    Value: 10000,
                    DisplayText: "All"
                }];

                scope.pageLimit = { Value: parseInt(attrs.limit) };

                scope.$watch("pageLimit", function (newValue, oldValue, scope) {
                    if (newValue != oldValue) {
                        scope.limit = parseInt(newValue.Value);
                        scope.currentPage = 1;
                        scope.offset = 0;
                        scope.reload();
                    }
                });
            },
            templateUrl: "/modules/admin/partials/pagination.html"
        };
    }
]);

app.directive('myClickOnce', function ($timeout) {
    var delay = 500;

    return {
        restrict: 'A',
        priority: -1,
        link: function (scope, elem) {
            var disabled = false;

            function onClick(evt) {
                if (disabled) {
                    evt.preventDefault();
                    evt.stopImmediatePropagation();
                } else {
                    disabled = true;
                    $timeout(function () { disabled = false; }, delay, false);
                }
            }

            scope.$on('$destroy', function () { elem.off('click', onClick); });
            elem.on('click', onClick);
        }
    };
});