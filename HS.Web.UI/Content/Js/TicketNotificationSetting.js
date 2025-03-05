var statusticket = "";
var notifyid = "";
var notiemail = "";
var notisms = "";
var typeticket = "";
var NotifyArr = [];
var typeticketid = "";
var statusReport = false;
var count = 0;
var SaveTicketNotification = function (value) {
    var url = "/Ticket/SaveTicketNotification";
    $(".tik_type_checkbox").each(function () {
        if ($(this).prop('checked') == true) {
            var valueid = $(this).attr('data-id');
            NotifyArr.push({
                Id: $("#notify_id").val(),
                TicketStatus: value,
                Text: notisms,
                Email: notiemail,
                TicketType: $("#tik_type_" + valueid).attr('data-status')
            });
        }
    })
    var param = JSON.stringify({
        'notify': NotifyArr
    })
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data) {
                parent.ClosePopup();
                parent.OpenSuccessMessageNew("Success", "Notification template saved successfully.");
            }
            else {
                parent.OpenErrorMessageNew("Error", "Please check ticket type");
            }
        }
    });
}
$(document).ready(function () {
    $(".tik_status").click(function () {
        $(".tik_type_checkbox").prop("checked", false);
        $(".notify_middle_data").removeClass('hidden');
        $(".tik_status").each(function () {
            $(this).css({ "background-color": "white" });
            $(this).removeClass('status_active');
        })
        $(".tik_type").each(function () {
            $(this).css({ "background-color": "white" });
        })
        $(".notify_right_data_model").each(function () {
            $(this).addClass('hidden');
        })
        $(".notify_right_data").each(function () {
            $(this).addClass('hidden');
        })
        var idval = $(this).attr('data-id');
        $("#tik_status_" + idval).css({ "background-color": "#ccc" });
        $("#tik_status_" + idval).addClass('status_active');
       
        statusticket = $("#tik_status_" + idval).attr('data-status');
        typeticket = "";
        //if ($("#notify_status_data_" + statusticket).attr('data-status') == statusticket) {
        //    $("#notify_status_data_" + statusticket).removeClass('hidden');
        //    notifyid = $("#notify_id_" + statusticket).val();
        //}
        //else {
        //    $(".notify_right_data").removeClass('hidden');
        //    notifyid = "0";
        //}
    })
    $(".tik_type").click(function () {
        $(".tik_type_checkbox").prop('checked', false);
        
        $(".tik_type").each(function () {
            $(this).css({ "background-color": "white" });
        })
        $(".notify_right_data_model").each(function () {
            $(this).addClass('hidden');
        })
        $(".notify_right_data").each(function () {
            $(this).addClass('hidden');
        })
        var idval = $(this).attr('data-id');
        $("#tik_type_" + idval).css({ "background-color": "#ccc" });
        $("#tik_type_checkbox_" + idval).prop('checked', true);
        typeticket = $("#tik_type_" + idval).attr('data-status');
        typeticketid = idval;
        $.ajax({
            type: "POST",
            url: "/Ticket/GetTicketNotificationData",
            data: JSON.stringify({status: statusticket, type: typeticket}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data) {
                    $(".notify_right_data").removeClass('hidden');
                    tinyMCE.activeEditor.setContent(data.email);
                    $("#sms_text_notify").val(data.text);
                    $("#notify_id").val(data.notifyid);
                }
            }
        });
    })
    
    $("#SaveTicketNotification").click(function () {
        if (typeof (statusticket) != "undefined" && statusticket != null && statusticket != "") {
            notiemail = tinyMCE.get('email_text_notify').getContent();
            notisms = $("#sms_text_notify").val();
            SaveTicketNotification(statusticket);
        }
        else if (statusticket == "") {
            parent.OpenErrorMessageNew("Error", "Please select status");
        }
    })
})