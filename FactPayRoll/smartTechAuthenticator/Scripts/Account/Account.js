$(document).ready(function () {
    $("#StateId").change(function () {
        var stateId = $(this).val()      
        $.ajax({
            type: "GET",
            url: "/Account/GetDistrictts",
            data: { stateId },
            success: function (data) {             
                var ddlDistricttId = $("#DistricttId");
                ddlDistricttId.empty();
                ddlDistricttId.append("<option>Select State</option>");
                $.each(data, function (index, item) {
                    ddlDistricttId.append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                });
            },
            error: function (error) {
                console.log(error);
            }
        })
    });
});

