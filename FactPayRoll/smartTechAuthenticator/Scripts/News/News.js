$(document).ready(function () {
    var tbl_News = $("#tbl_AllNews").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "pageLength": 5,

        "ajax": {
            "url": "/News/InsertNews",
            "type": "Post",
            "datatype":"json"
        },
        "columns": [
            {
                "render": function (row, data, index, meta) {
                    return meta.row + 1;
                }
                }
                     { "data": "NewsTitle", "name": "NewsTitle", "autoWidth": true },
            { "data": "Description", "name": "Description", "autoWidth": true },
            {
                data: null, render: function (data, type, row) {
                    if (row != null)
                    {
                        return "<img class='img-responsive' src='/Content/NewsFile/" + row.Photo + "' alt='tbl_StaffImage' height='50px' width='50px'>";
                    } else
                    {
                        return '<img src=/Content/Certificate/image1.png height="50px" width="50px"/>';
                    }
                    
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='NewsDetails?Id=" + row.Id + "' class='btn btn-info' >Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<button class='btn btn-danger btn-delete'   data-id='" + row.Id + "'  data-isActive='" + row.IsActive + "'  >Delete</button>";
                }
            },
                 ]
    })

})