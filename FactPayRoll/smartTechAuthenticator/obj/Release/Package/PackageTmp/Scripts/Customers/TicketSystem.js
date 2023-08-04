$(document).ready(function () {
    var tbl_Customer = $("#tbl_TicketSystem").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/TicketSystem/GetTickets",
            "type": "POST",
            "datatype": "json"
        },
        'columnDefs': [{
            'targets': [0, 1, 6, 7, 8,], // column index (start from 0)
            'orderable': false, // set orderable false for selected columns
        }],
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<img class='img-responsive' src='/Content/Tickets/" + row.Photos + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                }
            },
            /*   { "data": "Photos", "name": "Photos", "autoWidth": true },*/
            { "data": "Title", "name": "Title", "autoWidth": true },
            { "data": "Description", "name": "Description", "autoWidth": true },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "Answer", "name": "Answer", "autoWidth": true },
            { "data": "Status", "name": "Status", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/TicketSystem/TicketEdit?Id=" + row.Id + "' class='btn btn-info' >Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/TicketSystem/ViewTicketDetails?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'  data-isActive='" + row.IsActive + "'  >Delete</button>";
                }
            },

        ]
    });

    $(document).on("click", ".btn-delete", function () {
        if (confirm("Are you sure, you want to delete?")) {
            var Id = $(this).attr("data-id");
            var Category =
            {
                Id: Id
            }
            $.ajax({
                type: "POST",
                url: "/TicketSystem/DeleteTicket",
                data: Category,
                success: function (response) {
                    if (response.Status == 1) {
                        tbl_Customer.ajax.reload();
                        showMessage("Success", "Ticket deleted successfully");
                    } else {
                        showMessage("Failed", "Ticket not deleted successfully,Please delete all references.",);
                    }
                },
                error: function () {
                    showMessage("Failed", "Ticket not deleted successfully",);
                }
            });
        }
    });

    var tbl_Customer1 = $("#tbl_Tickets").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Customer/Tickets",
            "type": "POST",
            "datatype": "json"
        },
        'columnDefs': [{
            'targets': [0, 1,6,7,8,], // column index (start from 0)
            'orderable': false, // set orderable false for selected columns
        }],
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (row.Photos != null) {
                        return "<img class='img-responsive' src='/Content/Tickets/" + row.Photos + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                    } else {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },
            { "data": "Title", "name": "Title", "autoWidth": true },
            { "data": "Description", "name": "Description", "autoWidth": true },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "CreatedDate", "name": "CreatedDate", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    if (row.LabelStatus == 1) {
                        return "<label  class='text-primary'>Common</label>";
                    }
                    else if (row.LabelStatus == 2) {
                        return "<label  class='text-secondary'>Normal</label>";
                    }
                    else if (row.LabelStatus == 3) {
                        return "<label  class='text-danger'>Urgent</label>";
                    }
                    else if (row.LabelStatus == 4) {
                        return "<label class='text-danger'>Critical</label>";
                    }
                    else if (row.LabelStatus == 5) {
                        return "<label  class='text-warning'>Extend</label>";
                    }
                    else if (row.LabelStatus == 6) {
                        return "<label  class='text-success'>Complete</label>";
                    }
                    else {
                        return "<label  class=''></label>";
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (data.Status == "Solved") {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Solved</button>";
                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>UnSolved</button>";
                    }

                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Customer/ViewTicket?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },
        ]
    });

    $('.lblStatus').on('change', function () {
        var data = $(this).children("option:selected").val();
        var newdata = { "date": null, "status": data }
        $("#tbl_Tickets").DataTable({
            "bServerSide": true,
            "processing": true, // for show progress bar  
            "serverSide": true, // for process server side  
            "filter": true, // this is for disable filter (search box)  
            "orderMulti": false, // for disable multiple column at once  
            "pageLength": 5,
            "destroy": true,
            "ajax": {
                "type": "POST",
                "url": "/Customer/filterTickets",
                "datatype": "json",
                'data': newdata
            },
            'columnDefs': [{
                'targets': [0, 1, 6,7, 8,], // column index (start from 0)
                'orderable': false, // set orderable false for selected columns
            }],
            "columns": [
                {
                    "render": function (row, data, index, meta) {
                        return meta.row + 1;
                    }
                },
                {
                    data: null, render: function (data, type, row) {
                        if (row.Photos != null) {
                            return "<img class='img-responsive' src='/Content/Tickets/" + row.Photos + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                        } else {
                            return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                        }
                    }
                },
                { "data": "Title", "name": "Title", "autoWidth": true },
                { "data": "Description", "name": "Description", "autoWidth": true },
                { "data": "Name", "name": "Name", "autoWidth": true },
                { "data": "CreatedDate", "name": "CreatedDate", "autoWidth": true },
                {
                    data: null, render: function (data, type, row) {
                        if (row.LabelStatus == 1) {
                            return "<label  class='text-primary'>Common</label>";
                        }
                        else if (row.LabelStatus == 2) {
                            return "<label  class='text-secondary'>Normal</label>";
                        }
                        else if (row.LabelStatus == 3) {
                            return "<label  class='text-danger'>Urgent</label>";
                        }
                        else if (row.LabelStatus == 4) {
                            return "<label class='text-danger'>Critical</label>";
                        }
                        else if (row.LabelStatus == 5) {
                            return "<label  class='text-warning'>Extend</label>";
                        }
                        else if (row.LabelStatus == 6) {
                            return "<label  class='text-success'>Complete</label>";
                        }
                        else {
                            return "<label  class=''></label>";
                        }
                    }
                },
                {
                    data: null, render: function (data, type, row) {
                        if (data.Status == "Solved") {

                            return "<button class='btn' style='background-color:green; color:white; height:30px;' >Solved</button>";
                        }
                        else {
                            return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>UnSolved</button>";
                        }

                    }
                },
                {
                    data: null, render: function (data, type, row) {
                        return "<a href='/Customer/ViewTicket?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                    }
                },
            ]
        });
    });

    $(function () {

        $('input[name="datefilter"]').daterangepicker({
            autoUpdateInput: false,
            locale: {
                cancelLabel: 'Clear'
            }
        });

        $('input[name="datefilter"]').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
            var startDate = picker.startDate.format('DD/MM/YYYY');
            var endDate = picker.endDate.format('DD/MM/YYYY');
            //alert(startDate);
            //alert(endDate);
            var newdata = { "startDate": startDate, "endDate": endDate,"status": 0 }
            $("#tbl_Tickets").DataTable({
                "bServerSide": true,
                "processing": true, // for show progress bar  
                "serverSide": true, // for process server side  
                "filter": true, // this is for disable filter (search box)  
                "orderMulti": false, // for disable multiple column at once  
                "pageLength": 5,
                "destroy": true,
                "ajax": {
                    "type": "POST",
                    "url": "/Customer/filterTickets",
                    "datatype": "json",
                    'data': newdata
                }, 'columnDefs': [{
                    'targets': [0, 1, 6, 7, 8,], // column index (start from 0)
                    'orderable': false, // set orderable false for selected columns
                }],
                "columns": [
                    {
                        "render": function (row, data, index, meta) {
                            return meta.row + 1;
                        }
                    },
                    {
                        data: null, render: function (data, type, row) {
                            if (row.Photos != null) {
                                return "<img class='img-responsive' src='/Content/Tickets/" + row.Photos + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                            } else {
                                return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                            }
                        }
                    },
                    { "data": "Title", "name": "Title", "autoWidth": true },
                    { "data": "Description", "name": "Description", "autoWidth": true },
                    { "data": "Name", "name": "Name", "autoWidth": true },
                    { "data": "CreatedDate", "name": "CreatedDate", "autoWidth": true },
                    {
                        data: null, render: function (data, type, row) {
                            if (row.LabelStatus == 1) {
                                return "<label  class='text-primary'>Common</label>";
                            }
                            else if (row.LabelStatus == 2) {
                                return "<label  class='text-secondary'>Normal</label>";
                            }
                            else if (row.LabelStatus == 3) {
                                return "<label  class='text-danger'>Urgent</label>";
                            }
                            else if (row.LabelStatus == 4) {
                                return "<label class='text-danger'>Critical</label>";
                            }
                            else if (row.LabelStatus == 5) {
                                return "<label  class='text-warning'>Extend</label>";
                            }
                            else if (row.LabelStatus == 6) {
                                return "<label  class='text-success'>Complete</label>";
                            }
                            else {
                                return "<label  class=''></label>";
                            }
                        }
                    },
                    {
                        data: null, render: function (data, type, row) {
                            if (data.Status == "Solved") {

                                return "<button class='btn' style='background-color:green; color:white; height:30px;' >Solved</button>";
                            }
                            else {
                                return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>UnSolved</button>";
                            }

                        }
                    },
                    {
                        data: null, render: function (data, type, row) {
                            return "<a href='/Customer/ViewTicket?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                        }
                    },
                ]
            });
        });

        $('input[name="datefilter"]').on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });

    });






    //$('#startdate').on('change', function () {
    //    var date = $("#startdate").val();
    //    var newdata = { "date": date, "status": 0 }
    //    $("#tbl_Tickets").DataTable({
    //        "bServerSide": true,
    //        "processing": true, // for show progress bar  
    //        "serverSide": true, // for process server side  
    //        "filter": true, // this is for disable filter (search box)  
    //        "orderMulti": false, // for disable multiple column at once  
    //        "pageLength": 5,
    //        "destroy": true,
    //        "ajax": {
    //            "type": "POST",
    //            "url": "/Customer/filterTickets",
    //            "datatype": "json",
    //            'data': newdata
    //        },
    //        "columns": [
    //            {
    //                "render": function (row, data, index, meta) {
    //                    return meta.row + 1;
    //                }
    //            },
    //            {
    //                data: null, render: function (data, type, row) {
    //                    if (row.Photos != null) {
    //                        return "<img class='img-responsive' src='/Content/Tickets/" + row.Photos + "' alt='tbl_StaffImage' height='50px' width='50px'>";
    //                    } else {
    //                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
    //                    }
    //                }
    //            },
    //            { "data": "Title", "name": "Title", "autoWidth": true },
    //            { "data": "Description", "name": "Description", "autoWidth": true },
    //            { "data": "CreatedDate", "name": "CreatedDate", "autoWidth": true },
    //            {
    //                data: null, render: function (data, type, row) {
    //                    if (row.LabelStatus == 1) {
    //                        return "<label  class='text-primary'>Common</label>";
    //                    }
    //                    else if (row.LabelStatus == 2) {
    //                        return "<label  class='text-secondary'>Normal</label>";
    //                    }
    //                    else if (row.LabelStatus == 3) {
    //                        return "<label  class='text-danger'>Urgent</label>";
    //                    }
    //                    else if (row.LabelStatus == 4) {
    //                        return "<label class='text-danger'>Critical</label>";
    //                    }
    //                    else if (row.LabelStatus == 5) {
    //                        return "<label  class='text-warning'>Extend</label>";
    //                    }
    //                    else if (row.LabelStatus == 6) {
    //                        return "<label  class='text-success'>Complete</label>";
    //                    }
    //                    else {
    //                        return "<label  class=''></label>";
    //                    }
    //                }
    //            },
    //            {
    //                data: null, render: function (data, type, row) {
    //                    if (data.Status == "Solved") {

    //                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Solved</button>";
    //                    }
    //                    else {
    //                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>UnSolved</button>";
    //                    }

    //                }
    //            },
    //            {
    //                data: null, render: function (data, type, row) {
    //                    return "<a href='/Customer/ViewTicket?Id=" + row.Id + "' class='btn btn-info' >View</a>";
    //                }
    //            },
    //        ]
    //    });
    //});

});