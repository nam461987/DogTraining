var config = angular.module('ngBus.config', []);

config.factory('Config', function () {
    var obj = {};
    obj.defaultConfig = {
        datePickerFormat: "dd/mm/yyyy",
        dateFormat: "DD/MM/YYYY",
        dateTimeFormat: "DD/MM/YYYY HH:mm:ss",
        isoDateTimeFormat: "YYYY-MM-DDTHH:mm:ss.000",
        timeFormat: "HH:mm"
    };
    obj.fields = [
        { field: "Id", name: "#", create: false, edit: true, list: true, type: "hidden" },
        { field: "Name", name: "Tên", create: true, edit: true, list: true },
        { field: "Code", name: "module", create: true, edit: true, list: true },
        { field: "Description", name: "Mô tả", create: true, edit: true, list: true, type: "textarea" },
        { field: "CreatedDate", name: "Ngày tạo", create: false, edit: false, list: true, type: "datetime" }
    ];

    return obj;
});
