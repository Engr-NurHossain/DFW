var reminderDateUI;
var reminderEndDateUI;
var reminderCreatedDateUI;
var RemainderDatetime;
var complete = false;
var empid = $("#AssignName").val();
var InstantNotification = function () {
    var StartByDate = new Date($("#CustomerNoteReminderDate").val());
    var StartByDateFormat = StartByDate.getMonth() + 1 + "/" + StartByDate.getDate() + "/" + StartByDate.getFullYear();
    var CurrentDateTime = new Date();

    var RemTime = $("#RemainderTime").val();
    var RemTimeFormat = new Date(StartByDateFormat + " " + RemTime);

    if (RemTimeFormat <= CurrentDateTime) {
        $(".instantNotificationDiv").removeClass('hidden');
    } else {
        $(".instantNotificationDiv").addClass('hidden');
    }
};
var AddFollowUpReminder = function () {
    if (completeddatePermit && $("#CustomerNoteReminderEndDate").val() < $("#CustomerNoteReminderDate").val()) {
        OpenErrorMessageNew("Error!", "Please select a valid completed date");
    }
    else {
        console.log("entered");
        if ($("#RemainderTime").val() != "-1") {
            var remDate = $("#CustomerNoteReminderDate").val();
            var FremDate = remDate.split('/');
            var remTime = $("#RemainderTime").val();
            RemainderDatetime = remDate + ' ' + remTime;
        }

        if ($("#CustomerNoteNewNote").val() != "" && $('#CustomerNoteReminderDate').val() != "" && $("#RemainderTime").val() != "-1" && empid != "" && $('#AssignName').val() != null) {
            if ($("#IsEmailReminder").is(':checked') != false || $("#IsTextReminder").is(':checked') != false) {
                var url = domainurl + "/Leads/AddNewReminderNote/";
                var param = JSON.stringify({
                    id: $("#Id").val(),
                    notes: $("#CustomerNoteNewNote").val(),
                    reminderDate: RemainderDatetime,
                    ReminderEndDate: $("#CustomerNoteReminderEndDate").val(),
                    customerId: $("#btn-AddFollowUpReminder").attr("id-val"),
                    TeamSettingId: $("#TeamSettingId").val(),
                    IsInstantNotification: $("#IsInstantNotification").is(':checked'),
                    IsEmail: $("#IsEmailReminder").is(':checked'),
                    IsText: $("#IsTextReminder").is(':checked'),
                    //EmpId: empid,
                    IsClose: complete,
                    EmpIdList: $("#AssignName").val(),
                    reminderdatetimeforlog: RemainderDatetime

                });

                $.ajax({
                    type: "POST",
                    ajaxStart: $(".loader-div").show(),
                    url: url,
                    data: param,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (data) {
                        if (data.UpdateResult == true && data.message1 == "") {
                            OpenRightToLeftModal(false);
                            var customerId = $("#btn-AddFollowUpReminder").attr("id-val");
                            OpenSuccessMessageNew("Success!", "Successfully added new reminder.");
                            openLeadNoteTab();
                            LeadDetailTabCount();
                        }
                        else if (data.result == true && data.message1 == "") {
                            OpenRightToLeftModal(false);
                            var customerId = $("#btn-AddFollowUpReminder").attr("id-val");
                            OpenSuccessMessageNew("Success!", "Successfully added new reminder.");
                            openLeadNoteTab();
                            LeadDetailTabCount();
                        }
                        else {
                            OpenErrorMessageNew("Error!", data.message1, "");
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $(".loader-div").hide();
                        console.log(errorThrown);
                    }
                });
            }
            else {
                OpenErrorMessageNew("Error!", "Please select any follow up method");
            }
        }
        else {
            OpenErrorMessageNew("Error!", "Please fill up all required fields.");
        }
    }
}
$(document).ready(function () {
    $("#AssignName").change(function () {
        empid = $(this).val();
    })
    reminderDateUI = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerNoteReminderDate')[0],
        trigger: $('#CustomerNoteReminderDate_custom')[0],
        firstDay: 1
    });
    reminderEndDateUI = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerNoteReminderEndDate')[0],
        trigger: $('#CustomerNoteReminderEndDate_custom')[0],
        firstDay: 1
    });
    reminderCreatedDateUI = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerNoteCreatedDate')[0],
        trigger: $('#CustomerNoteCreatedDate_custom')[0],
        firstDay: 1
    });
    $("#btn-AddFollowUpReminder").click(function () {
        complete = false;
        this.disabled = true;
        AddFollowUpReminder();
        setTimeout(function () {
            $('#btn-AddFollowUpReminder').attr('disabled', false);
        }, 10000);
    });
    $("#RemainderTime").change(function () {
        InstantNotification();
    });
    $("#CustomerNoteReminderDate").change(function () {
        InstantNotification();
    });
    InstantNotification();
    $("#btn-IsComplete").click(function () {
        complete = true;
        empid = "00000000-0000-0000-0000-000000000000";
        AddFollowUpReminder();
    });
    $('.selectpicker').selectpicker('val', Emparr);
    $("#RemainderTime option:contains('" + remindertime + "')").attr('selected', true);
    if (complete == "true") {
        $("#btn-IsComplete").removeClass('hidden');
    }
    if (timeval == '') {
        $("#RemainderTime").val("-1");
    }
    var height = window.innerHeight - 108;
    $(".add_followup_reminder_container").height(height);
    $("#TeamSettingId").change(function () {
        if ($("#TeamSettingId").val() == "-1") {
            $('.selectpicker').selectpicker('val', '');
        }
        else {
            var UsersId = $("#TeamSettingId option:selected").attr('userid');
            if (typeof (UsersId) != "undefined" && UsersId != "") {
                var temArr = UsersId.split(',');
                $('.selectpicker').selectpicker('val', temArr);
            }
        }
    });
});