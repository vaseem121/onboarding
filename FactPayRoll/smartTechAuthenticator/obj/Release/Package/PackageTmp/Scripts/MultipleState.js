$(document).ready(function () {
    $("#StateId").change(function () {
        debugger;
        var data = $(this).val();
        var lastEl = data[data.length - 1];
        var StateId = lastEl.toString();
        //var StateId = {
        //    'data': data
        //}
        //console.log(StateId);
        //alert(data);
        $.ajax({
            type: "GET",
            url: "/Account/GetDistrictts",
            traditional: true,
            data: { StateId },
           /* data: { "data": data},*/
            success: function (data) {
                $("#DistricttId").empty();
                var ddlDistricttId = $("#DistricttId");
                console.log(ddlDistricttId)
               // ddlDistricttId.empty();
              //  ddlDistricttId.append("<option>Select State</option>");
                $.each(data, function (index, item) {
                    ddlDistricttId.append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                    console.log(item)
                });
                
            },
            error: function (error) {
                console.log(error);
            }
        })
    });
});


 
