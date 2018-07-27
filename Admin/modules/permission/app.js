'use strict';

var app = angular.module('ngBus', ['ngRoute', 'ngAnimate', 'ngSanitize', 'ngBus.caches', 'ngBus.controllers', 'ngBus.services', 'ngBus.config', 'ngBus.filters', 'ngBus.directives']);
app.config([
    '$routeProvider', function ($routeProvider) {
        $routeProvider.when('/', { templateUrl: '/modules/permission/partials/list.html', controller: 'ListCtrl' });
        $routeProvider.when('/edit/:id', { templateUrl: '/modules/permission/partials/edit.html', controller: 'EditCtrl' });
        $routeProvider.when('/create', { templateUrl: '/modules/permission/partials/create.html', controller: 'CreateCtrl' });
        $routeProvider.otherwise({ redirectTo: '/' });
    }
]);