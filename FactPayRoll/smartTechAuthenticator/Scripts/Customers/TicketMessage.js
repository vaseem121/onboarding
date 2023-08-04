$(document).ready(function () {
    var tbl_Customer = $(".TicketMessage").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/TicketSystem/TicketsMessageShow",
            "type": "POST",
            "datatype": "json"
        },
       
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Description", "name": "Description", "autoWidth": true },
            { "data": "CreatedDate", "name": "CreatedDate", "autoWidth": true },
           
        ]
    });

});