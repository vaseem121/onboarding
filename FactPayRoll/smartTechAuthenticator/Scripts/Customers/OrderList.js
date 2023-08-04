$(document).ready(function () {
    var tbl_Customer = $("#tbl_OrderList").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Home/GetOrderList",
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
            {
                data: null, render: function (data, type, row) {
                    return row.Paymentstatus == "complete" ? "<button class='btn btn-success'>" + row.Paymentstatus + "</button>" : "<button class='btn btn-danger'>" + row.Paymentstatus + "</button>";
                }
            },
            { "data": "OrderDate", "name": "OrderDate", "autoWidth": true },
            { "data": "TotalAmount", "name": "TotalAmount", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Mall/OrderDetail1?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },
        ]
    });
})