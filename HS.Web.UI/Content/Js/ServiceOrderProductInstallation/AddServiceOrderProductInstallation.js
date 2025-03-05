var DataTablePageSize = 50;

$(document).ready(function () {
    var table = $('#tblinfo').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });

    $(".close-div").click(function () {
        parent.$(".add-invoice-div").hide();
    });
})
