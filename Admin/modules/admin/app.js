'use strict';

var app = angular.module('ngService', ['ngRoute', 'ngAnimate', 'ngSanitize', 'ngService.caches', 'ngService.controllers', 'ngService.services', 'ngService.config', 'ngService.filters', 'ngService.directives']);
app.config([
    '$routeProvider', function ($routeProvider) {
        $routeProvider.when('/', { templateUrl: '/modules/admin/partials/list.html', controller: 'ListCtrl' });
        $routeProvider.when('/edit/:id', { templateUrl: '/modules/admin/partials/edit.html', controller: 'EditCtrl' });
        $routeProvider.when('/create', { templateUrl: '/modules/admin/partials/create.html', controller: 'CreateCtrl' });
        $routeProvider.otherwise({ redirectTo: '/' });
    }
]);