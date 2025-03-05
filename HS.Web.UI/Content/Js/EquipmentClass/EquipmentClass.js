var DataTablePageSize = 50;
    var selectedDeleteId = 0;
var DeleteProductClass = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Inventory/DeleteProductClass",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                parent.LoadProductClass(true);
            }
        },

        error: function () {
        }

    });
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
    $("#LoadProductClass").addClass("active");

    $("#AddNewProductClass").click(function () {
        $(".content-text").val("");
        $("#NewProductClass").load(domainurl + "/Inventory/AddProductClass");
    });
    $(".item-edit").click(function () {
        var itemId = $(this).attr('data-id');
        $("#NewProductClass").load(domainurl + "/Inventory/AddProductClass/" + itemId);
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
            
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteProductClass);
    });
    $("#srch-term").keyup(function () {
        $("#tblinfo_filter input").val($("#srch-term").val());
        $("#tblinfo_filter input").trigger('keyup');
    });
})