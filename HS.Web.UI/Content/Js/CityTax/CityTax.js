DataTablePageSize = 50;
var selectedDeleteId = 0;

var CityTaxDelete = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/CityTax/DeleteCityTax",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                parent.LoadCityTax(true);
            }
        },

        error: function () {
        }

    });

}
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    })
    $(".LoaderWorkingDiv").hide()
    var table = $('#tblCitytax').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });
    $('#tblCitytax tbody').on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $("#srch-tax").keyup(function () {
        $("#tblCitytax_filter input").val($("#srch-tax").val());
        $("#tblCitytax_filter input").trigger('keyup');
    });
    $(".Addcitytax").click(function () {
        OpenRightToLeftModal(domainurl + "/CityTax/AddCityTax/");
    });
    $(".item-edit-tax").click(function () {
        var val = $(".item-edit-tax").attr('data-id');
        OpenRightToLeftModal(domainurl + "/CityTax/AddCityTax?id=" + val);
    })
    $(".item-delete-tax").click(function () {
        selectedDeleteId = $(this).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", CityTaxDelete);
    })
})