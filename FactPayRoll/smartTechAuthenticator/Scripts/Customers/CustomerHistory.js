$(document).ready(function () {
    var tbl_Customer = $(".zero_config1").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 7,

        "ajax": {
            "url": "/Customer/GetCustomersHistory",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "ProductName", "name": "ProductName", "autoWidth": true },
            { "data": "AntigenType", "name": "AntigenType", "autoWidth": true },
            { "data": "AuthentiCode", "name": "AuthentiCode", "autoWidth": true },
            { "data": "TestResults", "name": "TestResults", "autoWidth": true },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='ViewTestCertificate?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },
        ]
    });

})