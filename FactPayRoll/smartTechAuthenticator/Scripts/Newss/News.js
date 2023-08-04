$(document).ready(function () {
    var tbl_News = $("#tbl_AllNews").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "pageLength": 5,

        "ajax": {
            "url": "/News/GetNewsDetails",
            "type": "Post",
            "datatype": "json"
        },
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "Title", "name": "Title", "autoWidth": true },
            { "data": "Description", "name": "Description", "autoWidth": true },
           
   
            {
                data: null, render: function (data, type, row) {
                    return "<img class='img-responsive' src='/Content/NewsFile/" + row.Photos + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                }
            },
            { "data": "Createddate", "name": "Createddate", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='EditNews?Id=" + row.Id + "' class='btn btn-info' >Edit</a>";
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
                url: "/News/DeleteNews",
                data: customer,
                success: function () {
                    showMessage("Success", "News deleted successfully",);
                    tbl_News.ajax.reload();
                }
            });
        }
    });
});