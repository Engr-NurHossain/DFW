var DataTablePageSize = 50;
var selectedDeleteId = 0;
var DeleteProduct = function () {
}
var initFileNames = function () {
    $(".fileNames").each(function () {
        $(this).text($(this).text().split("-___")[1])
    });
}
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
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
        OpenRightToLeftModal(domainurl + "/HrDoc/AddHrDoc?user=" + valueuser);
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
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", DeleteFile);
        
        //LoadProductCategory(true);
    })
    $(".item-edit").click(function () {
        selectedEditId = $(this).attr("data-id");
        OpenRightToLeftModal(domainurl + "/HrDoc/AddHrDoc?user=" + valueuser + "&id=" + selectedEditId);
        //LoadProductCategory(true);
    })
    $("#srch-term").keyup(function () {
        $("#tblinfo_filter input").val($("#srch-term").val());
        $("#tblinfo_filter input").trigger('keyup');
    });
    initFileNames();
});
var DeleteFile = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/HrDoc/DeleteUserFile",
        data: {
            id: delitem,
        },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                $(".LoadDocInfo").load(domainurl + "/HrDoc/HrDocPartial?usernum=" + valueuser);
            }
        },

        error: function () {
        }
    });
}


