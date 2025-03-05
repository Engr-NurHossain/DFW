var DataTablePageSize = 50;
var selectedDeleteId = 0;
var DeleteProduct = function () {


}
$(document).ready(function () {

    $(".LoaderWorkingDiv").hide();
    $("#LoadServiceProduct").click(function () {
        LoadServiceProduct(false);
    });
    $("#LoadBack").click(function () {
        LoadInventory(false);
    });
    $(".page-content").show();
    $("#LoadProductCategory").addClass("active");

    $("#AddNewProduct").click(function () {
        $(".content-text").val("");
        $("#NewProductCategory").load(domainurl + "/Matrix/AddSalesMatrix");
    });
    $(".item-edit").click(function () {
        var itemId = $(this).attr('data-id');
        OpenRightToLeftModal(domainurl + "/Matrix/AddSalesMatrix?Id=" + itemId);
    });
    var table = $('#tblinfo').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });

    $('#tblinfo tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $(".item-delete").click(function () {
        selectedDeleteId = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this sales matrix ?", DeleteSalesMatrix);
        //LoadProductCategory(true);
    })
    $("#srch-term").keyup(function () {
        $("#tblinfo_filter input").val($("#srch-term").val());
        $("#tblinfo_filter input").trigger('keyup');
    });
});
var DeleteSalesMatrix = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Matrix/DeleteSalesMatrix",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                parent.LoadSalesMatrix(true);
            }
        },

        error: function () {
        }
    });
}

//var EditSalesMatrix = function () {
//    var itemId = $(this).attr('data-id');
//    console.log(itemId);
//    OpenRightToLeftModal(domainurl + "/Matrix/AddSalesMatrix?Id=" + itemId);
//}