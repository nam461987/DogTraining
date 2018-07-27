var config = angular.module('ngService.config', []);

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
        { field: "Id", name: "#", create: false, edit: true, list: true, type: "hidden", order: 0 },
        { field: "TypeId", name: "Nhóm", create: true, edit: true, list: true, type: "select", option: "/Admin/GetGroupOption", order: 3 },
        //{ field: "UserName", name: "Tên đăng nhập", create: true, edit: true, list: true, edisabled: true },
        { field: "PasswordHash", name: "Mật khẩu mới", create: true, edit: true, list: false, type: "password", order: 2 },
        { field: "FullName", name: "Họ tên", create: true, edit: true, list: true, order: 4 },
        { field: "UserName", name: "Tên đăng nhập", create: true, edit: true, list: true, type: "username", order: 1, readonly: true },
        { field: "Mobile", name: "SĐT", create: true, edit: true, list: true, order: 5 },
        { field: "Email", name: "Email", create: true, edit: true, list: true, order: 6 },
        { field: "Address", name: "Địa chỉ", create: true, edit: true, list: true, type: "textarea", order: 7 },
        { field: "Active", name: "Trạng thái", create: false, edit: false, list: true, type: "active" },
        { field: "CreatedDate", name: "Ngày cập nhật", create: false, edit: false, list: true, type: "datetime" }
    ];

    return obj;
});
