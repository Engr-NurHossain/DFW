var DataTablePageSize = 50;
var selectedDeleteId = 0;
var TechDelete = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/TechScheduleSetting/DeleteTechSetting",
        data: { id: delitem },
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                parent.LoadTechSetting(true);
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
    var AlarmTable = $("#tblTech").DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });
    $(".AddTech").click(function () {
        OpenRightToLeftModal(domainurl + "/TechScheduleSetting/AddTechScheduleSetting/")
    });
    $(".item-edit-Tech").click(function (item) {
        var valueid = $(item.target).attr('data-id');
        OpenRightToLeftModal(domainurl + "/TechScheduleSetting/AddTechScheduleSetting?id=" + valueid);
    });
    $(".item-delete-Tech").click(function (item) {
        selectedDeleteId = $(item.target).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", TechDelete);
    })
})