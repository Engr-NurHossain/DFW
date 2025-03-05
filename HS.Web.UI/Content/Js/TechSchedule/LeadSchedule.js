var LeadCalander = function () {
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: domainurl + "/Leads/LeadTechScheduleCalendarDisplay/",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {
                var Leadschedulelist = data.result;
                for (var i = 0; i < Leadschedulelist.length ; i++) {
                    //console.log("hi");
                    var myLeadEvent = {
                        title: "EventName: " + Leadschedulelist[i].EventName + "\n" + "EventStartTime: " + Leadschedulelist[i].EventStartTime + "\n" + "EventEndTime: " + Leadschedulelist[i].EventEndTime,
                        allDay: true,
                        start: Leadschedulelist[i].EventDate,
                        id: Leadschedulelist[i].EventId,
                        customerId: Leadschedulelist[i].EventCustomer,
                        EstTime: Leadschedulelist[i].EventTime
                    };
                   
                    myLeadCalendar.fullCalendar('renderEvent', myLeadEvent);
                }
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}
var myLeadCalendar;
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    myLeadCalendar = $('#LeadTechScheduleCalender');
    myLeadCalendar.fullCalendar({
        eventClick: function (calEvent, jsEvent, view) {
            OpenRightToLeftModal(domainurl + "/Leads/AddLeadTechSchedule?id=" + calEvent.id + "&Leadid=" + calEvent.customerId);
        }
    });
    LeadCalander();
});