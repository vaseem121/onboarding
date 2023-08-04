$(document).ready(function () {
    var tbl_order = $("#tbl_Order").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Customer/GetOrder",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Order_Id", "name": "Order_Id", "autoWidth": true },
            { "data": "UserName", "name": "UserName", "autoWidth": true },
            {
                data: null, render: function (data,type,row) {
                    return row.Paymentstatus == "complete" ? "<button class='btn btn-success'>" + row.Paymentstatus + "</button>" : "<button class='btn btn-danger'>" + row.Paymentstatus +"</button>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (row.TrackingId == "1") {
                        return "<span class='text-danger text-lg' style='font-weight: bolder' >Orderd</span>";
                    }
                   else if (row.TrackingId == "2") {
                        return "<span class='text-warning text-lg' style='font-weight: bolder'>Shipped</span>";
                    }
                   else if (row.TrackingId == "3") {
                        return "<span class='text-primary text-lg' style='font-weight: bolder'>Out for Delivery</span>";
                    }
                   else if (row.TrackingId == "4") {
                        return "<span class='text-success text-lg' style='font-weight: bolder'>Deliverd</span>";
                    }
                }
            },
            { "data": "OrderDate", "name": "OrderDate", "autoWidth": true },
            { "data": "TotalAmount", "name": "TotalAmount", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Customer/TarckingDetails?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },


        ]
    });

    $('.lblStatus').on('change', function () {
        var data = $(this).children("option:selected").val();
        var newdata = { "date": null, "status": data }
        $("#tbl_Order").DataTable({
            "bServerSide": true,
            "processing": true, // for show progress bar  
            "serverSide": true, // for process server side  
            "filter": true, // this is for disable filter (search box)  
            "orderMulti": false, // for disable multiple column at once  
            "pageLength": 5,
            "destroy": true,
            "ajax": {
                "type": "POST",
                "url": "/Customer/GetFilterOrder",
                "datatype": "json",
                'data': newdata
            },
            "columns": [
                {
                    "render": function (row, data, index, meta) {
                        return meta.row + 1;
                    }
                },
                { "data": "Order_Id", "name": "Order_Id", "autoWidth": true },
                { "data": "UserName", "name": "UserName", "autoWidth": true },
                {
                    data: null, render: function (data, type, row) {
                        return row.Paymentstatus == "complete" ? "<button class='btn btn-success'>" + row.Paymentstatus + "</button>" : "<button class='btn btn-danger'>" + row.Paymentstatus + "</button>";
                    }
                },
                {
                    data: null, render: function (data, type, row) {
                        if (row.TrackingId == "1") {
                            return "<span class='text-danger text-lg' style='font-weight: bolder' >Orderd</span>";
                        }
                        else if (row.TrackingId == "2") {
                            return "<span class='text-warning text-lg' style='font-weight: bolder'>Shipped</span>";
                        }
                        else if (row.TrackingId == "3") {
                            return "<span class='text-primary text-lg' style='font-weight: bolder'>Out for Delivery</span>";
                        }
                        else if (row.TrackingId == "4") {
                            return "<span class='text-success text-lg' style='font-weight: bolder'>Deliverd</span>";
                        }
                    }
                },
                { "data": "OrderDate", "name": "OrderDate", "autoWidth": true },
                { "data": "TotalAmount", "name": "TotalAmount", "autoWidth": true },
                {
                    data: null, render: function (data, type, row) {
                        return "<a href='/Customer/TarckingDetails?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                    }
                },


            ]
        });
    });

    $('#startdate').on('change', function () {
        var date = $("#startdate").val();
        var newdata = { "date": date, "status": 0 }
        $("#tbl_Order").DataTable({
            "bServerSide": true,
            "processing": true, // for show progress bar  
            "serverSide": true, // for process server side  
            "filter": true, // this is for disable filter (search box)  
            "orderMulti": false, // for disable multiple column at once  
            "pageLength": 5,
            "destroy": true,
            "ajax": {
                "type": "POST",
                "url": "/Customer/GetFilterOrder",
                "datatype": "json",
                'data': newdata
            },
            "columns": [
                {
                    "render": function (row, data, index, meta) {
                        return meta.row + 1;
                    }
                },
                { "data": "Order_Id", "name": "Order_Id", "autoWidth": true },
                { "data": "UserName", "name": "UserName", "autoWidth": true },
                {
                    data: null, render: function (data, type, row) {
                        return row.Paymentstatus == "complete" ? "<button class='btn btn-success'>" + row.Paymentstatus + "</button>" : "<button class='btn btn-danger'>" + row.Paymentstatus + "</button>";
                    }
                },
                {
                    data: null, render: function (data, type, row) {
                        if (row.TrackingId == "1") {
                            return "<span class='text-danger text-lg' style='font-weight: bolder' >Orderd</span>";
                        }
                        else if (row.TrackingId == "2") {
                            return "<span class='text-warning text-lg' style='font-weight: bolder'>Shipped</span>";
                        }
                        else if (row.TrackingId == "3") {
                            return "<span class='text-primary text-lg' style='font-weight: bolder'>Out for Delivery</span>";
                        }
                        else if (row.TrackingId == "4") {
                            return "<span class='text-success text-lg' style='font-weight: bolder'>Deliverd</span>";
                        }
                    }
                },
                { "data": "OrderDate", "name": "OrderDate", "autoWidth": true },
                { "data": "TotalAmount", "name": "TotalAmount", "autoWidth": true },
                {
                    data: null, render: function (data, type, row) {
                        return "<a href='/Customer/TarckingDetails?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                    }
                },


            ]
        });
    });


});