$(document).ready(function () {
    var tbl_Customer = $(".zero_config3").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 7,

        "ajax": {
            "url": "/Customer/GetCustomersIP_History",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "CustomerUserId", "name": "CustomerUserId", "autoWidth": true },
            { "data": "UserName", "name": "UserName", "autoWidth": true },
            { "data": "IP_Address", "name": "IP_Address", "autoWidth": true },
            { "data": "EventDateTime", "name": "EventDateTime", "autoWidth": true },
        ]
    });

})