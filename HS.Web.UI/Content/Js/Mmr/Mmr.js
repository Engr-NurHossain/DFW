var DataTablePageSize = 50;
var selectedDeleteIdmmr = 0;
var MMRDelete = function () {
    var delitem = selectedDeleteIdmmr;
    $.ajax({
        url: domainurl + "/Setup/DeleteMMR",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                $(".LoadMMR").load(domainurl + "/Setup/MMRS/");
            }
        },

        error: function () {
        }

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
        
    $(".page-content").show();
    $("#LoadProductCategory").addClass("active");

    $("#AddNewMMR").click(function () {
        setTimeout(function () {
            OpenRightToLeftModalMMR(domainurl + "/Setup/AddMmr/");
        }, 344);
    });
    $(".item-edit-mmr").click(function () {
        var itemId = $(this).attr('data-id');
        OpenRightToLeftModalMMR(domainurl + "/Setup/AddMmr/" + itemId);
    });
    var table = $('#tblMmr').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });

    $('#tblMmr tbody').on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
        
    $(".item-delete-MMR").click(function () {
        selectedDeleteIdmmr = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", MMRDelete);
    });
    $("#srch-term").keyup(function () {
        $("#tblMmr_filter input").val($("#srch-term").val());
        $("#tblMmr_filter input").trigger('keyup');
    });
})