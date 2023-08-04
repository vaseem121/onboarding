$(document).ready(function () {
    var tbl_News = $("#tbl_FAQNews").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "pageLength": 4,

        "ajax": {
            "url": "/Customer/GetFAQDetails",
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
                    return "<a href='EditFAQNews?Id=" + row.Id + "' class='btn btn-info' >Edit</a>";
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
                url: "/Customer/DeleteFAQNews",
                data: customer,
                success: function () {
                    showMessage("Success", "FAQNews deleted successfully",);
                    tbl_News.ajax.reload();
                }
            });
        }
    });

});