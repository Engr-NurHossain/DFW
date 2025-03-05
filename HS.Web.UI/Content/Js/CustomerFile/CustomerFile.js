var DataTablePageSize = 50;
var selectedDeleteId = 0;
var DeleteProduct = function () { 
}
var initFileNames = function () {
    $(".fileNames").each(function () {
        $(this).text($(this).text().split("-___")[1])
    });
}
var AddCustomerFile = function () {
    OpenRightToLeftModal(domainurl + "/File/Addfile/" + CustomerLoadId);
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addFile");
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

    //$("#AddNewProduct").click(function () {
    //    $(".content-text").val("");
    //    $("#NewProductCategory").load("/File/Addfile/" + CustomerLoadId);
    //});
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";

    var Filetable = $(LoadCustomerDiv + '.tblFileCustomer').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        },
        "order": [[2, 'desc']]
    });
    Filetable.order([0, 'desc']).draw();
    $(LoadCustomerDiv + '.tblFileCustomer').on('click', 'tr', function () {
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
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file ?", DeleteFile);
        //LoadProductCategory(true);
    });
    $("#srch-term").keyup(function () {
        $("#tblFileCustomer_filter input").val($("#srch-term").val());
        $("#tblFileCustomer_filter input").trigger('keyup');
    });
    initFileNames();
}); 
var DeleteFile = function () {
    console.log("Delete file1");
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/File/DeleteCustomerFile",
        data: {
            id: delitem,
            CustomerId: CustomerLoadId
        },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                /*$("#FilesTab").load("/File/CustomerFiles/" + CustomerLoadId);*/
                OpenFilesTab();
            }
        },

        error: function () {
        }
    });
}
