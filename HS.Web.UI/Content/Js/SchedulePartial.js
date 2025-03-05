$(document).ready(function () {
    console.log("hii");
    $(".ticket_color_btn").click(function () {
        var options = { direction: "right" };
        $('#ticket_color_div').toggle('slide', options, 1);
    });
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".ScheduleMapPopUp", type: 'iframe', width: Popupwidth, height: 650 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".LoaderWorkingDiv").hide();
    if (TicketDate != "" && TicketType != "") {
        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?viewstartdate=" + TicketDate + "&viewname=" + TicketType);
    }
    else {
        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar/");
    }
    $("#show_schedule_map").click(function () {
        if (typeof (myCalendar) != "undefined") {
            var moment = myCalendar.fullCalendar('getDate');
            var tickettype = $("#ListTicketType").val();
            var ticketuser = $("#ListEmployee").val();
            $(".ScheduleMapPopUp").attr('href', domainurl + "/Schedule/ScheduleGoogleMap?date=" + moment.format() + "&type=" + tickettype + "&user=" + ticketuser);
            $(".ScheduleMapPopUp").click();
        }
    })
    $("#btn_ticket_status_setup").click(function () {
        OpenRightToLeftModal(domainurl + "/Schedule/TicketStatusImageSettingPartial")
    })
});
