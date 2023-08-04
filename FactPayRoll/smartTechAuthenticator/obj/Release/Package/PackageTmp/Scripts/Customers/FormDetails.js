$(document).ready(function () {
    var tbl_Detail = $("#tbl_formdetail").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "pageLength": 5,

        "ajax": {
            "url": "/Home/FormDetails1",
            "type": "Post",
            "datatype": "json"
        },
        
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "DateCreated", "name": "DateCreated", "autoWidth": true },
            { "data": "CustomerId", "name": "CustomerId", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Home/ViewFormDetail?FormIds=" + row.FormIds + "' class='btn btn-info' >View</a>";
                }
            },
          
        ]
    });

});