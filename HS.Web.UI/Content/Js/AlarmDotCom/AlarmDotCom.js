var selectedDeleteId = 0;
var DataTablePageSize = 50;
var AlarmDelete = function () {
    var delitem = selectedDeleteId;
    $.ajax({
        url: domainurl + "/Setup/DeleteAlarmSetting",
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
var AlarmTable = $("#tblAlarm").DataTable({
    "pageLength": DataTablePageSize,
    "destroy": true,
    "language": {
        "emptyTable": "No data available"
    }
});
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    })
    
    $(".AddAlarm").click(function () {
        OpenRightToLeftModal(domainurl + "/Setup/AddAlarmDotComSetting/")
    });
    $(".item-edit-Alarm").click(function (item) {
        var valueid = $(item.target).attr('data-id');
        OpenRightToLeftModal(domainurl + "/Setup/AddAlarmDotComSetting?id=" + valueid);
    });
    $(".item-delete-Alarm").click(function (item) {
        selectedDeleteId = $(item.target).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", AlarmDelete);
    });

})