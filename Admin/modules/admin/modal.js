'use strict';
var modal = angular.module('ngService.ngReallyClickModule', ['ui.bootstrap']);
modal.controller('ModalCtrl', [
    '$scope', function ($scope) {

        var $modalInstance = {};

        $scope.ok = function () {
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }
]);


modal.directive('ngReallyClick', ['$modal',
    function ($modal) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.bind('click', function () {
                    var message = attrs.ngReallyMessage || "Are you sure ?";

                    var modalHtml = '<div class="modal-body">' + message + '</div>';
                    modalHtml += '<div class="modal-footer"><button class="btn btn-primary" ng-click="ok()">OK</button><button class="btn btn-warning" ng-click="cancel()">Cancel</button></div>';

                    $modalInstance = $modal.open({
                        template: modalHtml,
                        controller: 'ModalCtrl'
                    });

                    $modalInstance.result.then(function () {
                        scope.ngReallyClick({ item: scope.item }); //raise an error : $digest already in progress
                    }, function () {
                        //Modal dismissed
                    });
                });

            }
        };
    }
  ]);