

$(document).ready(function () {
    var tbl_Customer = $("#tbl_Category").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Category/GetCategorys",
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
            { "data": "Location", "name": "Location", "autoWidth": true },
            { "data": "Website", "name": "Website", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            { "data": "MobileNo", "name": "MobileNo", "autoWidth": true },
            { "data": "UserPass", "name": "UserPass", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Category/CategoryDetails?Id=" + row.Id + "' class='btn btn-info' >Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                   // return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'  data-isActive='" + row.IsActive + "'  >Delete</button>";

                    return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'>Delete</button>";

                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='ViewEmployee?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },

        ]
    });

    $(document).on("click", ".btn-delete", function () {
        if (confirm("Are you sure, you want to delete?")) {
            var CategoryId = $(this).attr("data-id");
            var Category =
            {
               Id: CategoryId
            }
            $.ajax({
                type: "POST",
                url: "/Category/DeleteCategory",
                data: Category,
                success: function (response) {
                    if (response.Status == 1) {
                        tbl_Customer.ajax.reload();
                        showMessage("Success", "Employee deleted successfully");
                    } else {
                        showMessage("Failed", "Employee not deleted successfully,Please delete all references.",);
                    }
                },
                error: function () {
                    showMessage("Failed", "Employee not deleted successfully",);
                }
            });
        }
    });
});