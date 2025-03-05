var reminderDateUI;
var reminderEndDateUI;
var reminderCreatedDateUI;
var RemainderDatetime;
var complete = false;
var empid = $("#EmpId").val();
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
        if ($("#RemainderTime").val() != "-1") {
            var remDate = $("#CustomerNoteReminderDate").val();
            var FremDate = remDate.split('/');
            var remTime = $("#RemainderTime").val();

            /*var FremTime = remTime.split(':');
            var TimeString = remDate + ' ' + FremTime[0] + ':' + FremTime[1] + ':00';
    
            RemainderDatetime = TimeString;*/

            RemainderDatetime = remDate + ' ' + remTime;
        }
        console.log("error");
        if ($("#CustomerNoteNewNote").val() != "" && $('#CustomerNoteReminderDate').val() != "" && empid != "" && $("#RemainderTime").val() != "-1" && $('#EmpId').val() != null && $('#EmpId').val() != "-1") {
            if ($("#IsEmailReminder").is(':checked') != false || $("#IsTextReminder").is(':checked') != false) {
                var url = domainurl + "/Customer/AddNewReminderNote/";
                var param = {
                    Id: $("#Id").val(),
                    Notes: $("#CustomerNoteNewNote").val(),
                    ReminderDate: $("#CustomerNoteReminderDate").val(),
                    ReminderEndDate: $("#CustomerNoteReminderEndDate").val(),
                    RemainderTime: $("#RemainderTime").val(),
                    CustomerId: $("#btn-AddFollowUpReminder").attr("id-val"),
                    TeamSettingId: $("#TeamSettingId").val(),
                    IsInstantNotification: $("#IsInstantNotification").is(':checked'),
                    IsEmail: $("#IsEmailReminder").is(':checked'),
                    IsText: $("#IsTextReminder").is(':checked'),
                    cusIdValFollowup: $("#cusIdValFollowup").val(),
                    IsPin: $("#IsPinned").is(":checked"),
                    IsClose: complete,
                    reminderdatetimeforlog: RemainderDatetime
                };
                var assign = {
                    AssignEmployeeIdArray: $("#EmpId").val(),
                }
                var passparam = JSON.stringify({
                    'CustomerNote': param, 'assign': assign
                });
                console.log(param);
                $.ajax({
                    type: "POST",
                    ajaxStart: $(".loader-div").show(),
                    url: url,
                    data: passparam,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    success: function (data) {
                        if (data.result == true && data.message1 == "") {
                            OpenRightToLeftModal(false);
                            var customerId = $("#btn-AddFollowUpReminder").attr("id-val");
                            var empid = $("#btn-AddFollowUpReminder").attr("idval1");
                            console.log("fdssd");
                            //if (($("#Id").val()) > 0 && $("#TaskNote").val() != "undefined") {
                            //    if ($("#TaskNote").val().trim != "") {
                            //        TaskNoteSave();
                            //    }
                            //}

                            OpenSuccessMessageNew("Success!", "");
                            //$(".FollowUpTabContent").load("/Leads/LoadLeadFollowUpTabPartial/?CustomerId=" + cusid);
                            /*$(".NotesTab_Load").load(domainurl + "/Notes/NotesPartial?customerid=" + data.cusid);*/
                            OpenNotesTab();
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
var notTemplate = "";
if (taskreplyedirdeletepermit.toLowerCase() == "true") {
    console.log("withedit");
    notTemplate += "<div class='NoteTemplate'>"
        + "<input class='TaskNote_{3}' type='text' value='{2}' />"
        + "<button id='btn_edittaskreply_{3}' onclick='taskeditReply({3})' id-val='{3}' type='button' class='btn btn_edittaskreply_{3} note_reply_save' title='Save'>"
        + "Save</button >"
        + "<div class='TaskNoteOptions clearfix'>"
        + "<div class='NoteAddedByDiv'>"
        + "<div>"
        + "<span><b>Added By</b>:<span class='NoteAddedBy note_addedby_change_{3}'>{0}</span></span>"
        + "</div>"
        + "<div>"
        + "<span><b>Added Date</b>:<span class='NoteAddedDate note_addeddate_change_{3}'>{1}</span></span>"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</div>";
}
else {
    console.log("withoutedit");
    notTemplate += "<div class='NoteTemplate'>"
        + "<div class='TaskNote'>{2}"
        + "</div>"
        + "<div class='TaskNoteOptions clearfix'>"
        + "<div class='NoteAddedByDiv'>"
        + "<div>"
        + "<span><b>Added By</b>:<span class='NoteAddedBy'>{0}</span></span>"
        + "</div>"
        + "<div>"
        + "<span><b>Added Date</b>:<span class='NoteAddedDate'>{1}</span></span>"
        + "</div>"
        + "</div>"
        + "</div>"
        + "</div>";
}

var TaskNoteSave = function (id, nt) {
    var url = domainurl + "/Customer/AddTaskNote";
    var note = "";
    if (id > 0) {
        note = nt;
    }
    else {
        note = $("#TaskNote").val();
    }
    var param = JSON.stringify({
        Id: id,
        TaskId: $("#Id").val(),
        Note: note,
        AddedBy: "0000000b-0004-0000-0000-000000050000",
        CompanyId: "0000000b-0004-0000-0000-000000050000",
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
            if (data.result) {
                if (id > 0) {

                }
                else {
                    var tempTemplate = String.format(notTemplate, data.AddedBy, data.AddedDate, data.Note, data.Id);
                    $(".TaskNotesList").append(tempTemplate);
                    $("#TaskNote").val("");
                }
                
            }
            EnableElement("#AddNewTaskNotBtn");
            EnableElement(".btn_edittaskreply_" + id);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var taskeditReply = function (id) {
    console.log("replyedit");
    if ($(".TaskNote_" + id).val().trim() == "") {
        return;
    }
    //var idVal = $(this).attr("id-val");
    var note = $(".TaskNote_" + id).val();
    DisableElement(".btn_edittaskreply_" + id);
    TaskNoteSave(id, note);
}
$(document).ready(function () {
    $(".add_followup_reminder_body").height(window.innerHeight - 102)
    $("#EmpId").selectpicker('val', EmpArray);
    $("#RemainderTime option:contains('" + reminderTime + "')").attr('selected', true);
    if (complete == "true") {
        $("#btn-IsComplete1").removeClass('hidden');
    }

    if (timeval != "") {
        $("#RemainderTime").val(timeval);
    }
    else {
        $("#RemainderTime").val("-1");
    }
    $("#TeamSettingId").change(function () {
        if ($("#TeamSettingId").val() == "-1") {
            $("#EmpId").selectpicker('val', '');
        }
        else {
            var UsersId = $("#TeamSettingId option:selected").attr('userid');
            if (typeof (UsersId) != "undefined" && UsersId != "") {
                var temArr = UsersId.split(',');
                $("#EmpId").selectpicker('val', temArr);
            }
        }
    });
    
    parent.$('.close').click(function () {
        /*parent.$(".modal-body").html('');*/
    })
    reminderDateUI = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerNoteReminderDate')[0],
        trigger: $('#CustomerNoteReminderDateCustom')[0],
        firstDay: 1
    });
    reminderEndDateUI = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerNoteReminderEndDate')[0],
        trigger: $('#CustomerNoteReminderEndDateCustom')[0],
        firstDay: 1
    });
    reminderCreatedDateUI = new Pikaday({
        format: 'MM/DD/YYYY',
        field: $('#CustomerNoteCreatedDate')[0],
        trigger: $('#CustomerNoteCreatedDateCustom')[0],
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
    $("#btn-IsComplete1").click(function () {
        complete = true;
        empid = "00000000-0000-0000-0000-000000000000";
        AddFollowUpReminder();
    })
    $("#IsEmailReminder").is(':checked', true);
    $("#IsTextReminder").is(':checked', true);

    $(".AddNewTaskNotBtn").click(function () {
        if ($("#TaskNote").val().trim() == "") {
            return;
        }
        DisableElement("#AddNewTaskNotBtn");
        TaskNoteSave();
    });
});