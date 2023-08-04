

$(document).ready(function () {
    var id = $("#id").value();
    var tbl_Customer = $("#tbl_CustomersView").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "data": id,
            "url": "/Customer/GetCustomersTestHistory",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            { "data": "Nric", "name": "NRIC", "autoWidth": true },
            { "data": "MobileNo", "name": "MobileNo", "autoWidth": true },
            { "data": "DateCreated", "name": "DateCreated", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='CustomerDetails?customerId=" + row.Id + "' class='btn btn-info' >Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'  data-isActive='" + row.IsActive + "'  >Delete</button>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='Customer/ViewCustomer?customerId=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },
        ]
    });
});