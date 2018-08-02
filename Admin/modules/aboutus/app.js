angular.module("gcApp", ["ui.router", "ui.select2", "ngCkeditor", "ngFileUpload", "ngResource", "ngAnimate", "ngSanitize", "gcApp.controllers", "gcApp.config", "gcApp.queue", "gcApp.services", "gcApp.caches", "gcApp.directives", "gcApp.filters"]);
angular.module("gcApp").config(function ($stateProvider) {
    $stateProvider.state("edit", { //state for adding a new lyt_PT_Customer
        url: "/edit/2",
        templateUrl: "/modules/aboutus/partials/create.html",
        controller: "UpdateController"
    });
}).run(function ($state) {
    $state.go("edit"); //make a transition to lyt_PT_Customers state when app starts
});
