var DataTablePageSize = 50;
var deleteId;
var complete = "false";
var DeleteFollowUp = function () {

    var FollowUpDeleteId = deleteId;
    var customerId = $("#AddReminderLeadId").val();
    $.ajax({
        url: domainurl + "/Customer/DeleteCustomerNote",
        data: { id: FollowUpDeleteId, CustomerId: customerId},
        type: "Post",
        dataType: "Json",
        success: function (result) {
            if (result) {
                OpenNotesTab();
                //$(".FollowUpTabContent").load("/Leads/LoadLeadFollowUpTabPartial?CustomerId=" + customerId);
            }
        },
        error: function () {
        }
    });
}
var AddReminder = function (CustomerId) {
    OpenRightToLeftModal(domainurl + "/Customer/AddNewReminderNote?CustomerId=" + CustomerId + "&IsComplete=" + complete);
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addNote");
}

$(document).ready(function () {
    //parent.$('.close').click(function () {
    //    parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    //})
    var InvoiceTable = $('#tblFollowUpNotes').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        }
    });
    $(".AddReminder").click(function () {
        var customerId = $("#AddReminderLeadId").val();
        OpenRightToLeftModal(domainurl + "/Customer/AddNewReminderNote?CustomerId=" + customerId + "&IsComplete=" + complete);
    });
    $(".AddMessage").click(function () {
        //var customerId = $("#AddReminderLeadId").val();
        OpenRightToLeftModal(domainurl + "/Notes/AddMessage");
    });
    $(".follow-up-edit").click(function () {
        var EditId = $(this).attr("data-id");
        var customerId = $("#AddReminderLeadId").val();
        var time = $(this).attr('data-val');
        complete = "true";
        OpenRightToLeftModal(domainurl + "/Customer/AddNewReminderNote?Id=" + EditId + "&CustomerId=" + customerId + "&Timeval=" + time + "&IsComplete=" + complete);
    });
    $(".item-message-edit").click(function () {
        var EditId = $(this).attr("data-id");
        OpenRightToLeftModal(domainurl + "/Notes/AddMessage?Id=" + EditId);
    });
    //var openexistingreminder = function (id, taskid, time) {
     
    //    complete = "true";
    //    OpenRightToLeftModal(domainurl + "/Customer/AddNewReminderNote?Id=" + taskid + "&CustomerId=" + id + "&Timeval=" + time + "&IsComplete=" + complete);
    //}
    $(".follow-up-delete").click(function () {
        deleteId = $(this).attr("data-id")
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteFollowUp);
    });
});