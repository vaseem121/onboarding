
$(document).ready(function () {


    var tbl_Customer = $("#tbl_ManageQr1").DataTable({
        "processing": true, // for show progress bar  
        "serverSide": true, // for process server side  
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 10,

        "ajax": {
            "url": "/Customer/GetCheckQrAllData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "QrCode", "name": "QrCode", "autoWidth": true },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
            { "data": "CreatedDate", "name": "CreatedDate", "autoWidth": true },
            { "data": "IsExpire", "name": "IsExpire", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-danger btn-Reset'   data-id='" + row.Id + "'  data-isActive='" + row.IsExpire + "'>Reset</button>";
                }
            }

        ]
    });

    $(document).on("click", ".btn-Reset", function () {
        if (confirm("Are you sure, you want to Reset?")) {
            var customerId = $(this).attr("data-id");
            var customer =
            {
                Id: customerId
            }
            $.ajax({
                type: "POST",
                url: "/Customer/ResetQRData",
                data: customer,
                success: function (response) {
                    if (response.Status == 1) {
                        tbl_Customer.ajax.reload();
                        showMessage("Success", "QrCode Reset successfully");
                    } else {
                        showMessage("Failed", "QrCode not Reset successfully",);
                    }
                },
                error: function () {
                    showMessage("Failed", "QrCode not Reset successfully",);
                }
            });
        }
    });
});

$(document).on("change", ".ProductClass", function () {
    var QRId = $(this).attr("DataValue");
    var ProductId = this.value;
    var data = { "QRId": QRId, "ProductId": ProductId };
    $.ajax({
        url: '/Customer/UpdateProduct',
        type: "post",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        success: function (result) {
            if (result == "1") {
                location.reload();

            } else {
            }
        }
    });
})