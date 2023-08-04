
$(document).ready(function () {
    var tbl_Customer = $("#tbl_BannerCarousal").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 5,

        "ajax": {
            "url": "/Customer/GetBannerCarousals",
            "type": "POST",
            "datatype": "json"
        },

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
                        return "<img class='img-responsive' src='/Content/BannerUploadedFiles/" + row.Photos + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                    }
                    else
                    {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                   
                }
            },
            { "data": "Title", "name": "Title", "autoWidth": true },
            { "data": "Description", "name": "Description", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Customer/BannerCarousalDetails?Id=" + row.Id + "' class='btn btn-info' >Edit</a>";
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
                url: "/Customer/DeleteBannerCarousal",
                data: Category,
                success: function (response) {
                    if (response.Status == 1) {
                        tbl_Customer.ajax.reload();
                        showMessage("Success", "Banner deleted successfully");
                    } else {
                        showMessage("Failed", "Banner not deleted successfully,Please delete all references.",);
                    }
                },
                error: function () {
                    showMessage("Failed", "Banner not deleted successfully",);
                }
            });
        }
    });
});