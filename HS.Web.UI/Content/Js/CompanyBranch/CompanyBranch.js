var DataTablePageSize = 50;
var selectedDeleteId = 0;
var CompanyBranchDelete = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/CompanyBranch/DeleteCompanyBranch",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                parent.LoadCompanyBranch(true);
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
    $(".Addbranch").click(function () {
        OpenRightToLeftModal(domainurl + "/CompanyBranch/AddCompanyBranch/");
    });
    var table = $('#tblBranch').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });
    $('#tblBranch tbody').on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $("#srch-branch").keyup(function () {
        $("#tblBranch_filter input").val($("#srch-branch").val());
        $("#tblBranch_filter input").trigger('keyup');
    });
    $(".item-edit-branch").click(function (item) {
        var valueid = $(item.target).attr('data-id');
        OpenRightToLeftModal(domainurl + "/CompanyBranch/AddCompanyBranch?id=" + valueid);
    });
    $(".item-delete-branch").click(function () {
        selectedDeleteId = $(this).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", CompanyBranchDelete);
    })
})