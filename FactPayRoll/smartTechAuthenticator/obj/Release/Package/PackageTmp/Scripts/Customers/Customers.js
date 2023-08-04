

$(document).ready(function () {
    var tbl_Customer=$("#tbl_Customers").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Customer/GetCustomers",
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
            { "data": "Nric", "name": "NRIC", "autoWidth": true },
            { "data": "MobileNo", "name": "MobileNo", "autoWidth": true },
            { "data": "DateCreated", "name": "DateCreated", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='CustomerDetails?customerId=" + row.Id + "' class='btn btn-info' >Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'  data-isActive='" + row.IsActive + "'  >Delete</button>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='ViewCustomer?customerId=" + row.Id + "' class='btn btn-info' >View</a>";
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
                url: "/Customer/DeleteCustomerDetais",
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
                    showMessage("Failed", "Customer not deleted successfully", );
                }
            });
        }
    });
});