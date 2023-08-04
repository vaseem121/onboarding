$(document).ready(function () {
    var tbl_Customer = $("#zero_config2").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 7,

        "ajax": {
            "url": "/Customer/GetCustomersOrderHistory",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "UserName", "name": "UserName", "autoWidth": true },
            { "data": "Order_Id", "name": "Order_Id", "autoWidth": true },
            { "data": "OrderDate", "name": "OrderDate", "autoWidth": true },
            { "data": "Shippingstatus", "name": "Shippingstatus", "autoWidth": true },
            { "data": "Qty", "name": "Qty", "autoWidth": true },
            { "data": "TotalAmount", "name": "TotalAmount", "autoWidth": true },
           {
               data: null, render: function (data, type, row) {
                   return "<a href='/Mall/OrderCutomerDetail?Id=" + row.Id + "' class='btn btn-info' >View</a>";
               }
            },
           
        ]
    });

})