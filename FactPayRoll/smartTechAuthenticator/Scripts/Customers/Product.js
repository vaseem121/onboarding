

$(document).ready(function () {
    var tbl_Customer = $("#tbl_Products").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Customer/GetAllProducts",
            "type": "POST",
            "datatype": "json"
        },     
    //    "columns": [
    //        {
    //            "render": function (row, data, index, meta) {
    //                return meta.row + 1;
    //            }
    //        }, 
    //        //{
    //        //    data: null, render: function (data, type, row) {
    //        //        if (row!=null)
    //        //        {
    //        //            return "<img class='img-responsive' src='/Content/Product/" + row.Photo + "' alt='tbl_StaffImage' height='50px' width='50px'>";
    //        //        } else
    //        //        {
    //        //            return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
    //        //        }
                   
    //        //    }
    //        //},
    //        { "data": "Company Name", "name": "CompanyName", "autoWidth": true },
    //        { "data": "Location", "name": "Location", "autoWidth": true },
    //        { "data": "Website", "name": "Website", "autoWidth": true },
    //        { "data": "Email", "name": "Email", "autoWidth": true },
    //        { "data": "Phone Number", "name": "PhoneNumber", "autoWidth": true },
    //        { "data": "Password", "name": "Password", "autoWidth": true },
    //        {
    //            data: null, render: function (data, type, row) {
    //                return "<a href='ProductDetailsProductDetails?ProductId=" + row.Id + "' class='btn btn-info' >Edit</a>";
    //            }
    //        },
    //        {
    //            data: null, render: function (data, type, row) {
    //                return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'  data-isActive='" + row.IsActive + "'  >Delete</button>";
    //            }
    //        },
    //        {
    //            data: null, render: function (data, type, row) {
    //                return "<a href='ViewProduct?ProductId=" + row.Id + "' class='btn btn-info' >View</a>";
    //            }
    //        },
    //        {
    //            data: null, render: function (data, type, row) {
    //                return "<a href='Gallery?ProductId=" + row.Id + "' class='btn btn-success' > Gallery</a>";
    //            }
    //        },

    //    ]
    //});
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
                    return "<a href='ProductDetails?Id=" + row.Id + "' class='btn btn-info' >Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'>Delete</button>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='ViewProduct?Id=" + row.Id + "' class='btn btn-info' >View</a>";
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
                url: "/Customer/DeleteProduct",
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