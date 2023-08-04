$(document).ready(function () {
    var tbl_Customer = $("#tbl_Admin").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Customer/Admindata",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs":
            [{
                "targets": [0],
                "searchable": false,
                "orderable": false
            },
            {
                "targets": [6],
                "searchable": false,
                "orderable": false
            },
            {
                "targets": [7],
                "searchable": false,
                "orderable": false
            }],

        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            { "data": "DateCreated", "name": "DateCreated", "autoWidth": true },
            { "data": "MobileNo", "name": "MobileNo", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-primary open111' data-toggle='modal' data-id='" + row.Id + "'>Assign Menu</button>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='SubAdminDetail?customerId=" + row.Id + "' class='btn btn-info' >Edit</a>";
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
            var customerId = $(this).attr("data-id");
            var customer =
            {
                Id: customerId
            }
            $.ajax({
                type: "POST",
                url: "/Customer/DeleteSubadminDetais",
                data: customer,
                success: function (response) {
                    if (response.Status == 1) {
                        tbl_Customer.ajax.reload();
                        showMessage("Success", "Customer deleted successfully");
                    } else {
                        showMessage("Failed", "Customer not deleted successfully",);
                    }
                },
                error: function () {
                    showMessage("Failed", "Customer not deleted successfully",);
                }
            });
        }
    });
    $(document).on("click", ".open111", function () {
        var SubAdminId = $(this).attr("data-id");
        $('#SubAdminId').val(SubAdminId);
        data = {"SubAdminId":SubAdminId}
        $.ajax({
            url: "/Customer/MenuData",
            type: "Post",
            data: data,
            success: function (responce) {
                //console.log(responce);
                $(".menus").html("");
                var abc = "";
                for (var i = 0; i < responce.MenuList.length; i++) {
                    var a = (responce.MenuList[i].Menuname).toString();
                    if (responce.MenuList[i].check) {
                        abc = abc + "<input  id='MenuList1_" + i + "__Menuname' name='MenuList1[" + i + "].Menuname' type='hidden' value='" + a + "' /><input data-val='true'  data-val-required='The Id field is required.' id='MenuList1_" + i + "__check' name='MenuList1[" + i + "].check' type='checkbox' value="+responce.MenuList[i].check+" checked /><label for='MenuList1_" + i + "__check'>" + responce.MenuList[i].Menuname + "</label></br>";
                    } else {
                        abc = abc + "<input value='" + a + "' id='MenuList1_" + i + "__Menuname' name='MenuList1[" + i + "].Menuname' type='hidden' /><input data-val='true'  data-val-required='The Id field is required.' id='MenuList1_" + i + "__check' name='MenuList1[" + i + "].check' type='checkbox' value='true' /><input name='MenuList1[" + i + "].check' type='hidden' value='false'><label for='MenuList1_" + i + "__check'>" + responce.MenuList[i].Menuname + "</label></br>";
                    }
                }
                $('.menus').append(abc);
                $("body").addClass("open");
            },
            error: function () {

            }
        });
       
    });
    $("button.close").on("click", function () {
        $("body").removeClass("open");
    });
});