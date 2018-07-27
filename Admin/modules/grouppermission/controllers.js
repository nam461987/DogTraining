angular.module("gcApp.controllers", [])
    .controller("ListController", function ($scope, $state, popupService, $window, Model, Config, svcCache, svcQueue) {
        //$scope.modules = [];
        //$scope.permissions = [];

        $scope.info = {
            selectGroup: 0,
            selectModule: '',
            selectPermission: {}
        };

        //$scope.choosePermission = [];

        $scope.reset = function () {
            //$scope.getGroup();
            $scope.init();
        };

        $scope.options = {};

        $scope.reloadRoute = function () {
            $(".overlay").removeClass("hidden");
            $scope.options.GroupId = Model.getRoute({}, function () {
                $(".overlay").addClass("hidden");
            });

        }

        $scope.reloadModule = function () {
            $(".overlay").removeClass("hidden");
            $scope.options.Module = Model.getModule({}, function () {
                $(".overlay").addClass("hidden");
            });
        }

        $scope.reloadPermission = function () {
            var groupId = $scope.info.selectGroup;
            var module = $scope.info.selectModule;
            console.log(groupId, module);
            if (groupId > 0 && module != "") {
                $(".overlay").removeClass("hidden");
                $scope.options.Permission = Model.getPermission({
                    GroupId: groupId,
                    Module: module
                }, function (data) {
                    //console.log("DATA", data);
                    for (var i = 0; i < data.length; i++) {
                        $scope.info.selectPermission[data[i].Id] = data[i].Status;
                    }
                    $(".overlay").addClass("hidden");
                });
            }
        }


        $scope.init = function () {
            $scope.options = {};
            $scope.reloadRoute();
            $scope.reloadModule();
        };

        $scope.$watchGroup(["info.selectGroup", "info.selectModule"], function (newValue, oldValue, scope) {
            //console.log(newValue, oldValue);
            if (newValue != oldValue) {
                scope.info.selectPermission = {};
                //$scope.options["Permission"] = [];
                $scope.reloadPermission();
            }
        });

        $scope.changePermission = function () {
            console.log($scope.info.selectPermission);
        }
        //console.log(kq === 1);
        console.log($scope.info.selectGroup === 1);
        $scope.apply = function () {
            //console.log($scope.info.selectPermission);
            if ($scope.info.selectGroup == 1) {
                if (userIdLogin == 1) {
                    if ($scope.info.selectPermission) {
                        $(".overlay").removeClass("hidden");
                        for (var k in $scope.info.selectPermission) {
                            if ($scope.info.selectPermission.hasOwnProperty(k)) {
                                $scope.applySuccess = Model.applyPermission({
                                    GroupId: $scope.info.selectGroup,
                                    PermissionId: k,
                                    Status: $scope.info.selectPermission[k]
                                }, function () {
                                    $(".overlay").addClass("hidden");
                                });
                            }
                        }
                    }
                }
                else if (userIdLogin != 1 && userIdLogin != 2) {
                    alert("Xin lỗi!! Bạn không thể chỉnh sửa quyền của Administrator!!");
                    $scope.reloadPermission();
                }
            }
            else if ($scope.info.selectGroup == 2) {
                if (userIdLogin == 1 || userIdLogin == 2) {
                    if ($scope.info.selectPermission) {
                        $(".overlay").removeClass("hidden");
                        for (var k in $scope.info.selectPermission) {
                            if ($scope.info.selectPermission.hasOwnProperty(k)) {
                                $scope.applySuccess = Model.applyPermission({
                                    GroupId: $scope.info.selectGroup,
                                    PermissionId: k,
                                    Status: $scope.info.selectPermission[k]
                                }, function () {
                                    $(".overlay").addClass("hidden");
                                });
                            }
                        }
                    }
                }
                else if (userIdLogin != 1 && userIdLogin != 2) {
                    alert("Xin lỗi!! Bạn không thể chỉnh sửa quyền của quản trị!!");
                    $scope.reloadPermission();
                }
            }
            if ($scope.info.selectGroup != 1 && $scope.info.selectGroup != 2) {
                if ($scope.info.selectPermission) {
                    $(".overlay").removeClass("hidden");
                    for (var k in $scope.info.selectPermission) {
                        if ($scope.info.selectPermission.hasOwnProperty(k)) {
                            $scope.applySuccess = Model.applyPermission({
                                GroupId: $scope.info.selectGroup,
                                PermissionId: k,
                                Status: $scope.info.selectPermission[k]
                            }, function () {
                                $(".overlay").addClass("hidden");
                            });
                        }
                    }
                }
            }
        }

        $scope.reset();

    });