angular.module("gcApp", ["ui.router", "ngResource", "ngAnimate", "ngSanitize", "gcApp.controllers", "gcApp.config", "gcApp.queue", "gcApp.services", "gcApp.caches", "gcApp.directives", "gcApp.filters"]);
angular.module("gcApp").config(function ($stateProvider) {
    $stateProvider.state("list", { // state for showing all lyt_PT_Customers
        url: "/",
        templateUrl: "/modules/grouppermission/partials/list.html",
        controller: "ListController"
    }).state("view", { //state for showing single lyt_PT_Customer
        url: "/view/:id",
        templateUrl: "/modules/grouppermission/partials/view.html",
        controller: "ViewController"
    }).state("new", { //state for adding a new lyt_PT_Customer
        url: "/new",
        templateUrl: "/modules/grouppermission/partials/add.html",
        controller: "CreateController"
    }).state("edit", { //state for updating a lyt_PT_Customer
        url: "/edit/:id",
        templateUrl: "/modules/grouppermission/partials/edit.html",
        controller: "EditController"
    });
}).run(function ($state) {
    $state.go("list"); //make a transition to lyt_PT_Customers state when app starts
});