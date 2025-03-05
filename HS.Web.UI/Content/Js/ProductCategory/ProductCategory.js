var DataTablePageSize = 50;
var selectedDeleteId = 0;
var LoadProductCategoryPage = function (itemId) {
    $("#NewProductCategory").html("");
    OpenRightToLeftModal(domainurl + "/Customer/AddProductCategory/?id=" + itemId);
    //$("#NewProductCategory").load(domainurl + "/Customer/AddProductCategory/" + itemId);
}
var DeleteProduct = function (delId) {
    //var delitem = delId;
    //$.ajax({
    //    url: domainurl + "/Customer/DeleteProductCategory",
    //    data: { id: delitem },
    //    type: "Post",
    //    dataType: "Json",
    //    success: function (result) {
    //        if (result) {
    //            parent.LoadProductCategory(true);
    //        }
    //    },

    //    error: function () {
    //    }
    //});
    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", function () {
        $.ajax({
            url: domainurl + "/Customer/DeleteProductCategory",
            data: { id: delId },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                if (result) {
                    parent.LoadProductCategory(true);
                }
            },

            error: function () {
            }
        });
    });
}
$(document).ready(function () {
    console.log("Test");
    $(".collapsible").click(function (e) {
        e.preventDefault();
        $(this).toggleClass("collapse expand");
        $(this).closest('li').children('ul').slideToggle();
    });

    $(".LoaderWorkingDiv").hide();
    $("#LoadServiceProduct").click(function () {
        LoadServiceProduct(false);
    });
    $("#LoadBack").click(function () {
        LoadInventory(false);
        ActiveClassCustomColor();
    });
    $(".page-content").show();
    $("#LoadProductCategory").addClass("active");
    $(".thead-th-style").click(function () {

    });
    $("#AddNewProduct").click(function () {
        $(".content-text").val("");
        $("#NewProductCategory").load(domainurl + "/Customer/AddProductCategory");
    });
    //$(".thead-th-style").click(function () {
    //    $("ul.category-ul-list").children("li").sort(sort_li).appendTo('ul.category-ul-list');
    //    function sort_li(a, b) {
    //        var A = $(a).data('position');
    //        var B = $(b).data('position');
    //        return (A < B) ? -1 : (A > B) ? 1 : 0; 
    //    }
    //});
    var table = $('#tblCategory').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });

    $('#tblCategory tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    //$(".item-delete").click(function () {
    //    selectedDeleteId = $(this).attr("id");
    //    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteProduct);
    //})
    $("#srch-term").keyup(function () {
        $("#tblCategory_filter input").val($("#srch-term").val());
        $("#tblCategory_filter input").trigger('keyup');
    });
});
