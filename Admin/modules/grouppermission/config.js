var config = angular.module("gcApp.config", []).factory("Config", function () {
    var obj = {};
    obj.defaultConfig = {
        datePickerFormat: "dd/mm/yyyy",
        dateFormat: "DD/MM/YYYY",
        dateTimeFormat: "DD/MM/YYYY HH:mm:ss",
        isoDateTimeFormat: "YYYY-MM-DDTHH:mm:ss.000",
        timeFormat: "HH:mm"
    };

    obj.ApiKey = "";
    obj.currentTable = "";

    obj.fields = [
        { field: "GroupId", name: "#", create: false, edit: true, list: true },
        { field: "Module", name: "Mô đun", create: true, edit: true, list: true },
        { field: "PermissionId", name: "Mô tả", create: true, edit: true, list: true }
    ];

    obj.getFields = function (tblname) {
        var result = [];
        var match = false;
        for (var i = 0; i < this.models.length && !match; i++) {
            if (this.models[i].name === tblname) {
                result = this.models[i].fields;
                match = true;
            }
        }
        return result;
    };

    obj.getCacheKey = function (tblname, field, option) {
        var result = "";
        switch (typeof option) {
            case "string":
                //result = btoa("{0}_options_{1}_{2}".format(tblname, field, option));
                result = "{0}_options_{1}_{2}".format(tblname, field, option);
                break;
            case "object":
                result = "{0}_options_{1}_{2}".format(tblname, field, "array");
                //result = btoa("{0}_options_{1}_{2}".format(tblname, field, "array"));
                break;
        }
        return result;
    }

    //obj.setTable = function(tblname) {
    //    this.currentTable = tblname;
    //}
    //obj.getCurrentTable = function() {
    //    return this.currentTable;
    //}

    return obj;
});