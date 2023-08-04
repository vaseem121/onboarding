$(document).ready(function () {
    var tbl_order = $("#tbl_Order").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Mall/GetOrder",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Id", "name": "Id", "autoWidth": true },
            { "data": "UserName", "name": "UserName", "autoWidth": true },
            { "data": "Paymentstatus", "name": "Paymentstatus", "autoWidth": true },
            { "data": "OrderDate", "name": "OrderDate", "autoWidth": true },
            { "data": "TotalAmount", "name": "TotalAmount", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Mall/OrderDetail?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },
         

        ]
    });

});