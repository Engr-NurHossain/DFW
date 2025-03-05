var DataTablePageSize = 50;
var deleteCustomerId = $('.item-delete').attr('id');

var TechNewSheduleLoad = function () {
    OpenRightToLeftModal(domainurl + "/TechSchedule/AddTechSchedule/?id=0&customerid=" + CustomerGuid);
}
    
var DeleteTechSchedule = function (selectedDeleteId) {
    
    $.ajax({
        url: domainurl + "/TechSchedule/DeleteTechSchedule",
        data: { id: selectedDeleteId },
        type: "Post",
        dataType: "Json",
        success: function (result) {    
            if (result) {
                OpenScheduleTab();
            }
        },

        error: function () {
        }
            
    });
    var customerId = deleteCustomerId;
    console.log("from customer delete" + customerId);
    $("#ScheduleTab").load(domainurl + "/TechSchedule/TechSchedulePartial/?customerid=" + customerId);
    //LoadScheduleTech(true);
}

$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    }) 
    $(".item-edit").click(function () {
        var idvalue1 = $(this).attr('data-id');
        OpenRightToLeftModal(domainurl + "/TechSchedule/AddTechSchedule?id=" + idvalue1 + "&customerId=" + CustomerGuid);
    });
    var table = $('#tblSchedule').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });
    $(".item-delete").click(function () {
        var selectedDeleteId = $(this).attr("data-id");
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", function () {
            DeleteTechSchedule(selectedDeleteId);
        });

    });
        
})