$(document).ready(function () {
    var tbl_Customer = $("#tbl_Shipping").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Customer/GetShipping",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
       
            { "data": "ShippingMethod", "name": "ShippingMethod", "autoWidth": true },
            { "data": "ShippingCharge", "name": "ShippingCharge", "autoWidth": true },
            { "data": "StateName", "name": "StateName", "autoWidth": true },
            { "data": "DistricttName", "name": "DistricttName", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditShiping?Id=" + row.Id + "' class='btn btn-info'>Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'   >Delete</button>";
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
                url: "/Customer/DeleteShiping",
                data: customer,
                success: function (response) {
                    if (response.Status == 1) {
                        tbl_Customer.ajax.reload();
                        showMessage("Success", "Product deleted successfully");
                    } else {
                        showMessage("Failed", "Product not deleted successfully",);
                    }
                },
                error: function () {
                    showMessage("Failed", "Product not deleted successfully",);
                }
            });
        }
    });
});