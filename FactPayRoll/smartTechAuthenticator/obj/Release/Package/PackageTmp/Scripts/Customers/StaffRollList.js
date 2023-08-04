

$(document).ready(function () {
    var tbl_Customer = $("#tbl_Customers_Roll").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Customer/GetStaffRoll",
            "type": "POST",
            "datatype": "json"
        },
        
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    if (data.UserType == 1)
                    {
                        return "<select class='select' data-id='" + row.Id + "'><option value='1' selected>Customer</option><option value='2'>SubAdmin</option></select>";
                    } else if (data.UserType == 2)
                    {
                        return "<select class='select' data-id='" + row.Id + "'><option value='1'>Customer</option><option value='2' selected>SubAdmin</option></select>";
                    }
                    
                }
            },
        ]
    });

    $(document).on("change", ".select", function () {
        if (confirm("Are you sure, you want to Role Change?")) {
            var conceptName = $(this).find(":selected").val();
            
            var Id = $(this).attr("data-id");
            var Category =
            {
                Id: Id,
                UserType: conceptName,
            }
            $.ajax({
                type: "POST",
                url: "/Customer/ChangeRole",
                data: Category,
                success: function (response) {
                    if (response.Status == 1) {
                        tbl_Customer.ajax.reload();
                        showMessage("Success", "Role Updated successfully");
                    } else {
                        showMessage("Failed", "Role not Updated successfully.",);
                    }
                },
                error: function () {
                    showMessage("Failed", "Role not Updated successfully",);
                }
            });
        }
    });
});