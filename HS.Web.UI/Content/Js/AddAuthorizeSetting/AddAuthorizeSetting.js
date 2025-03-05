var selectedDeleteId = 0;
var DataTablePageSize = 50;
var AuthorizeDelete = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Setup/DeleteAuthorizeSetting",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                parent.LoadSettings(true);
            }
        },

        error: function () {
        }

    });

}
var AuthorizeTable = $("#tblAuthorize").DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    }
});
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    });
    
    $(".AddAuthorize").click(function () {
        OpenRightToLeftModal(domainurl + "/Setup/AddAuthorizeDotNetSetting/")
    });
    $(".item-edit-Authorize").click(function (item) {
        var valueid = $(item.target).attr('data-id');
        OpenRightToLeftModal(domainurl + "/Setup/AddAuthorizeDotNetSetting?id=" + valueid);
    });
    $(".item-delete-Authorize").click(function (item) {
        selectedDeleteId = $(item.target).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", AuthorizeDelete);
    })
})