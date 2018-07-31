angular.module("gcApp", ["ui.router", "ui.select2", "ngFileUpload", "ngResource", "ngAnimate", "ngSanitize", "gcApp.controllers", "gcApp.config", "gcApp.queue", "gcApp.services", "gcApp.caches", "gcApp.directives", "gcApp.filters"]);
angular.module("gcApp").config(function ($stateProvider) {
    $stateProvider.state("list", { // state for showing all lyt_PT_Customers
        url: "/",
        templateUrl: "/modules/slider/partials/list.html",
        controller: "ListController"
    }).state("edit", { //state for adding a new lyt_PT_Customer
        url: "/edit/:id",
        templateUrl: "/modules/slider/partials/create.html",
        controller: "UpdateController"
    });
}).run(function ($state) {
    $state.go("list"); //make a transition to lyt_PT_Customers state when app starts
});
