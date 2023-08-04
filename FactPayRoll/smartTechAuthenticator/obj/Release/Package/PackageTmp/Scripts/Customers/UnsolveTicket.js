$(document).ready(function () {
    var tbl_Customer = $("#tbl_UnSolved_Tickets").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/TicketSystem/UnSolvedTickets",
            "type": "POST",
            "datatype": "json"
        },
        'columnDefs': [{
            'targets': [0, 1,6, 7, 8,], // column index (start from 0)
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
                    if (row != null)
                    {
                        return "<img class='img-responsive' src='/Content/Tickets/" + row.Photo + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                    } else
                    {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                    
                }
            },
        
            { "data": "Title", "name": "Title", "autoWidth": true },
            { "data": "Message", "name": "Message", "autoWidth": true },
            { "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true },
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
                    return "<button type='button' class='btn btn-danger'>" + row.Status + "</button>";
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
        var newdata = {"date":null,"status": data }
        $("#tbl_UnSolved_Tickets").DataTable({
            "bServerSide": true,
            "processing": true, // for show progress bar  
            "serverSide": true, // for process server side  
            "filter": true, // this is for disable filter (search box)  
            "orderMulti": false, // for disable multiple column at once  
            "pageLength": 5,
            "destroy": true,
            "ajax": {
                "type": "POST",
                "url": "/TicketSystem/filterUnSolvedTickets",
                "datatype": "json",
                'data': newdata
            }, 'columnDefs': [{
                'targets': [0, 1,6, 7, 8,], // column index (start from 0)
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
                        if (row.Photo != null) {
                            return "<img class='img-responsive' src='/Content/Tickets/" + row.Photo + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                        } else {
                            return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                        }

                    }
                },

                { "data": "Title", "name": "Title", "autoWidth": true },
                { "data": "Message", "name": "Message", "autoWidth": true },
                { "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true },
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
                        return "<button type='button' class='btn btn-danger'>" + row.Status + "</button>";
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
            var newdata = { "startDate": startDate, "endDate": endDate, "status": 0 }
            $("#tbl_UnSolved_Tickets").DataTable({
                "bServerSide": true,
                "processing": true, // for show progress bar  
                "serverSide": true, // for process server side  
                "filter": true, // this is for disable filter (search box)  
                "orderMulti": false, // for disable multiple column at once  
                "pageLength": 5,
                "destroy": true,
                "ajax": {
                    "type": "POST",
                    "url": "/TicketSystem/filterUnSolvedTickets",
                    "datatype": "json",
                    'data': newdata
                }, 'columnDefs': [{
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
                            if (row.Photo != null) {
                                return "<img class='img-responsive' src='/Content/Tickets/" + row.Photo + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                            } else {
                                return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                            }

                        }
                    },

                    { "data": "Title", "name": "Title", "autoWidth": true },
                    { "data": "Message", "name": "Message", "autoWidth": true },
                    { "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true },
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
                            return "<button type='button' class='btn btn-danger'>" + row.Status + "</button>";
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
    //    $("#tbl_UnSolved_Tickets").DataTable({
    //        "bServerSide": true,
    //        "processing": true, // for show progress bar  
    //        "serverSide": true, // for process server side  
    //        "filter": true, // this is for disable filter (search box)  
    //        "orderMulti": false, // for disable multiple column at once  
    //        "pageLength": 5,
    //        "destroy": true,
    //        "ajax": {
    //            "type": "POST",
    //            "url": "/TicketSystem/filterUnSolvedTickets",
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
    //                    if (row.Photo != null) {
    //                        return "<img class='img-responsive' src='/Content/Tickets/" + row.Photo + "' alt='tbl_StaffImage' height='50px' width='50px'>";
    //                    } else {
    //                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
    //                    }

    //                }
    //            },

    //            { "data": "Title", "name": "Title", "autoWidth": true },
    //            { "data": "Message", "name": "Message", "autoWidth": true },
    //            { "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true },
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
    //                    return "<button type='button' class='btn btn-danger'>" + row.Status + "</button>";
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