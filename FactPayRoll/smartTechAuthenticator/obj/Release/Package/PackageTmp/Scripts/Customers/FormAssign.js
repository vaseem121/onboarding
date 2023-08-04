$(document).ready(function () {
    var tbl_FormAssign = $("#tbl_formassign").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "pageLength": 5,
        "ajax": {
            "url": "/Home/GetForm",
            "type": "POST",
            "datatype": "json"
        },
        //"columnDefs":
        //    [{
        //        "targets": [0],
        //        "searchable": false,
        //        "orderable": false
        //    },
        //    {
        //        "targets": [1],
        //        "searchable": false,
        //        "orderable": false
        //    },
        //    {
        //        "targets": [2],
        //        "searchable": false,
        //        "orderable": false
        //        },
        //        {
        //            "targets": [3],
        //            "searchable": false,
        //            "orderable": false
        //        }],
        "columns": [
            {
                "render": function (row, data, index, meta) {
                return meta.row + 1;
                }
            },
            { "data": "Name", "name": "FormName", "autoWidth": true },
            { "data": "CreatedDate", "name": "CreatedDate", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'data-isActive='" + row.IsActive + "'  >Delete</button>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Home/ViewForm?Id=" + row.Id + "' class='btn btn-info' >View</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-success open111' data-toggle='modal' data-id='" + row.Id + "'>Assign Form</button>";
                }
            },
            //{ "data": "Product", "name": "Product", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='/Home/EditForm?Id=" + row.Id + "' class='btn btn-info' >Edit</a>";
                }
            },
        ]
    });
    $(document).on("click", ".btn-delete", function () {
        if (confirm("Are you sure, you want to delete?")) {
            var FormId = $(this).attr("data-id");
            var Data =
            {
                Id: FormId
            }
            $.ajax({
                type: "POST",
                url: "/Home/DeleteForm",
                data: Data,
                success: function (response) {
                    if (response.Status == 1) {
                        tbl_FormAssign.ajax.reload();
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

    $(document).on("click", ".open111", function () {
        var FormId = $(this).attr("data-id");
        $('#formyhn').val(FormId);
        data = { "formid": FormId }
        $.ajax({
            url: "/Home/FormAssign1",
            type: "Post",
            data: data,
            success: function (responce) {
                console.log(responce);
                $(".prolist").html("");
                var abc = "";
                for (var i = 0; i < responce.ListPro.length; i++) {
                    var a = (responce.ListPro[i].Name).toString();
                    if (responce.ListPro[i].check) {
                        abc = abc + "<input  id='ListPro1_" + i + "__Id' name='ListPro1[" + i + "].Id' type='hidden' value='" + responce.ListPro[i].Id + "' /><input data-val='true'  data-val-required='The Id field is required.' id='ListPro1_" + i + "__check' name='ListPro1[" + i + "].check' type='checkbox' value=" + responce.ListPro[i].check + " checked /><label for='ListPro1_" + i + "__check'>" + responce.ListPro[i].Name + "</label></br>";
                    }
                    else {
                        abc = abc + "<input value='" + responce.ListPro[i].Id + "' id='ListPro1_" + i + "__Id' name='ListPro1[" + i + "].Id' type='hidden' /><input data-val='true'  data-val-required='The Id field is required.' id='ListPro1_" + i + "__check' name='ListPro1[" + i + "].check' type='checkbox' value='true' /><input name='ListPro1[" + i + "].check' type='hidden' value='false'><label for='ListPro1_" + i + "__check'>" + responce.ListPro[i].Name + "</label></br>";
                    }
                }
                $('.prolist').append(abc);
                $("body").addClass("open");
            },
            error: function () {
            }
        });

    });
    $("button.close").on("click", function () {
        $("body").removeClass("open");
    });

    //$(document).on("click", ".open111", function () {
    //    var formid = $(this).attr("data-id");
    //    $('#formyhn').val(formid);
    //    $("body").addclass("open");
    //});
    //$("button.close").on("click", function () {
    //    $("body").removeclass("open");
    //});
});