var DataTablePageSize = 50;
var deleteId;
var complete = "false";
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$("#Right-To-Left-Modal-Body .modal-body").html('');
    })
    var InvoiceTable = $('#tblFollowUpNotes').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        },
        "order": []
    });

    $("#AddReminder").click(function () {
        var customerId = $("#AddReminderLeadId").val();
        OpenRightToLeftModal(domainurl + "/Leads/AddNewReminderNote?CustomerId=" + customerId + "&IsComplete=" + complete)
    });
    $("#AddNotes").click(function () {
        var customerId = $("#AddReminderLeadId").val();
        OpenRightToLeftModal(domainurl + "/Leads/AddLeadNotes?CustomerId=" + customerId)
    });
    $("#AddMessage").click(function () {
        var customerId = $("#AddReminderLeadId").val();
        OpenRightToLeftModal(domainurl + "/Notes/AddMessage?Id=" + customerId)
    });

    $(".follow-up-edit").click(function () {
        var EditId = $(this).attr("data-id");
        var customerId = $("#AddReminderLeadId").val();
        var Timeval = $(this).attr('data-val');
        var completevalue = $(this).attr('data-val1');
        complete = "true";
        OpenRightToLeftModal(domainurl + "/Leads/AddNewReminderNote?Id=" + EditId + "&CustomerId=" + customerId + "&Timeval=" + Timeval + "&IsComplete=" + complete);
    });

    $(".note-edit").click(function () {
        var EditId = $(this).attr("data-id");
        var customerId = $("#AddReminderLeadId").val();
        OpenRightToLeftModal(domainurl + "/Leads/AddLeadNotes?Id=" + EditId + "&CustomerId=" + customerId+"&From="+"");
    });
    $(".message-edit").click(function () {
        var EditId = $(this).attr("data-id");
        OpenRightToLeftModal(domainurl + "/Notes/AddMessage?Id=" + EditId);
    });

    var DeleteFollowUp = function () {

        var FollowUpDeleteId = deleteId;
        var customerId = $("#AddReminderLeadId").val();
        $.ajax({
            url: domainurl + "/Leads/DeleteLeadReminder",
            data: { id: FollowUpDeleteId, CustomerId: customerId },
            type: "Post",
            dataType: "Json",
            success: function (result) {
                if (result) {
                    openLeadNoteTab();

                   // $(".FollowUpTabContent").load(domainurl + "/Leads/LoadLeadFollowUpTabPartial?CustomerId=" + customerId);
                }
            },
            error: function () {
            }
        });
    }

    $(".follow-up-delete").click(function () {
        deleteId = $(this).attr("data-id")
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteFollowUp);
    });

    $(".item-edit").click(function () {
        var idval = $(this).attr('data-id');
        OpenRightToLeftModal(domainurl + "/Leads/AddLeadNotes?id=" + idval)
    })
});