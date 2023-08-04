

$(document).ready(function () {

    var newdata = { "data": 1 }
    $("#tbl_Certificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,


        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "ProductName", "name": "ProductName", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {

                    if (data != null)
                    {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else
                    {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },
           
            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified== 1)
                    {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0)
                    {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else
                    {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }
                    
                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },
            
        ]
    });

    var newdata2 = { "data": 2 }
    $("#tbl_RejectCertificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,

        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata2
        },


        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "ProductName", "name": "ProductName", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {
                    if (data != null)
                    {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else
                    {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                   
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }

                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },
           


        ]
    });

    var newdata0 = { "data": 0 }
    $("#tbl_UnverifiedCertificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,

        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata0
        },


        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "ProductName", "name": "ProductName", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {

                    if (data != null)
                    {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else
                    {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }

                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },

            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },
           


        ]
    });



});

$('#select').on('change', function () {
    var data = $(this).children("option:selected").val();

    var newdata = { "data": data }
    $("#tbl_Certificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,


        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata
        },


        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {
                    return '<img src="' + data + '" alt="' + data + '"height="16" width="16"/>';
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;'>Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>UnVerified</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }

                }
            },

            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },


        ]
    });
});

$('#startdate').on('change', function () {
    debugger;
    var date = $("#startdate").val();
    var newdata = { "data": 1, "date": date.toString() }
    /* alert(date);*/
    var MydataTable = $("#tbl_Certificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,


        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {

                    if (data != null) {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }

                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },



        ]
    });

});



$('#startdate0').on('change', function () {
    var date = $("#startdate0").val();
    var newdata = { "data": 0, "date": date.toString() }
    /*alert(date);*/
    var MydataTable = $("#tbl_UnverifiedCertificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,


        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {

                    if (data != null) {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }

                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },
        ]
    });

});


$('#startdate2').on('change', function () {
    var date = $("#startdate2").val();
    var newdata = { "data": 2, "date": date.toString() }
    /*alert(date);*/
    var MydataTable = $("#tbl_RejectCertificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,


        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {

                    if (data != null) {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }

                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },
        ]
    });

}); $('#startdate').on('change', function () {
    var date = $("#startdate").val();
    var newdata = { "data": 1, "date": date.toString() }
    /* alert(date);*/
    var MydataTable = $("#tbl_Certificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,


        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {

                    if (data != null) {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }

                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },



        ]
    });

});
$('#startdate0').on('change', function () {
    var date = $("#startdate0").val();
    var newdata = { "data": 0, "date": date.toString() }
    /*alert(date);*/
    var MydataTable = $("#tbl_UnverifiedCertificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,


        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {

                    if (data != null) {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }

                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },
        ]
    });

});


$('#startdate2').on('change', function () {
    var date = $("#startdate2").val();
    var newdata = { "data": 2, "date": date.toString() }
    /*alert(date);*/
    var MydataTable = $("#tbl_RejectCertificate").DataTable({
        "bServerSide": true,
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,
        "destroy": true,


        "ajax": {
            "type": "POST",
            "url": "/Customer/ViewCertificateVerifiedList",
            "datatype": "json",
            'data': newdata
        },

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            {
                "data": "CertificateImage", render: function (data, type, row) {

                    if (data != null) {
                        return '<img src=/Content/Certificate/' + data + ' alt=' + data + ' height="50px" width="50px"/>';
                    } else {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                }
            },

            {
                data: null, render: function (data, type, row) {
                    if (data.IsVerified == 1) {

                        return "<button class='btn' style='background-color:green; color:white; height:30px;' >Verified</button>";
                    }
                    else if (data.IsVerified == 0) {
                        return "<button class='btn' style='background-color: #ff4444; color:white; height:30px;'>Pending</button>";

                    }
                    else {
                        return "<button class='btn' style='background-color:#CC0000; color:white; height:30px;'>Rejected</button>";
                    }
                }
            },
            { "data": "Date", "name": "Date", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditVeryfied?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },
        ]
    });

}); 