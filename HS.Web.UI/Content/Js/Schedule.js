var LoadDate;
var myResource = [];
var FinalResource = [];
var myEvent = [];
var strTime;
var j = 0;
var DefaultView;
var view;
var moment;
var viewname;
var CountUser;
var viewstartdate;
var myCalendar;
var valtype;
var datacount;
var valUser;
var eventlength = 0;
var userlength = 0;
var containersize = 0;
var todaydate = new Date();
var month = todaydate.getMonth() + 1;
var day = todaydate.getDate();
var date = todaydate.getFullYear() + "-" + (month < 10 ? '0' : '') + month + "-" + (day < 10 ? '0' : '') + day + "T00:00:00Z";
var todaysdate = (month < 10 ? '0' : '') + month + "/" + (day < 10 ? '0' : '') + day + "/" + todaydate.getFullYear();
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var CompareTicketServiceArr;
var prevX = -1;
var Calander = function (Default) {
    var IsAll = $("#IsAllTechnicianList").val();
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: "Schedule/AllScheduleCalendar/",
        data: JSON.stringify({
            Default: UserVal, ResourceLim: resourceLimit,
            pageno: pageno,
            ReminderResource: ReminderResource,
            pageno1: pageno1,
            startdate: startdate,
            defaultView: defview,
            typeval: typeval,
            EventUserId: EventUserId,
            TicketId: ticketId,
            NoneEmp: IsAll
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {            
            if (data) {
                if (IsAllSelectdShow.toLowerCase() == 'true') {
                    $(".LoadSchedule").load(domainurl + "/Schedule/ScheduleUserListPartial?CurrentDate=" + startdate + "&CurrentView=" + defview + "&UserVal=" + UserVal + "&typeval=" + typeval + "&status=" + IsAll);
                }
                var schedulelist = data.model;
                var ReminderScheduleList = data.model1;
                var DefaultUser = data.Default;
                var ListEmpCalendar = data.modelEmp;
                var eventDate = data.startdate;
                viewstartdate = data.startdate;
                var View = data.View;
                datacount = data.datacount;
                CompareTicketServiceArr = data.CompareTicketId;
                $("#SheduleView").val(View);
                $("#ScheduleDateTime").val(eventDate);
                for (var f = 0; f < ListEmpCalendar.length; f++) {
                    if (ListEmpCalendar[f].UserId == "22222222-2222-2222-2222-222222222222" && myResource.length<5) {
                        myResource.push({
                            id: ListEmpCalendar[f].Id,
                            title: ListEmpCalendar[f].EMPName
                        });
                    }
                }
                for (var s = 0; s < ListEmpCalendar.length; s++) {
                    if (ListEmpCalendar[s].UserId != "22222222-2222-2222-2222-222222222222") {
                        myResource.push({
                            id: ListEmpCalendar[s].Id,
                            title: ListEmpCalendar[s].EMPName
                        });
                    }
                }
                var empcount = 0;
                for (var i = 0; i < ReminderScheduleList.length ; i++) {
                    var startTimeStamp = ReminderScheduleList[i].EventStartDate;
                    var StartdateVal = startTimeStamp.split(' ');
                    var finalStartTime = StartdateVal[1].split(':');
                    var Starthours = finalStartTime[0];
                    var Startminutes = finalStartTime[1];
                    var ampm = finalStartTime[0] >= 12 ? 'PM' : 'AM';
                    Starthours = Starthours % 12;
                    Starthours = Starthours ? Starthours : 12; // the hour '0' should be '12'
                    Startminutes = Startminutes < 10 ? '0' + Startminutes : Startminutes;
                    var strStartTime = Starthours + ':' + Startminutes + ' ' + ampm;
                    strTime = Starthours + ':' + Startminutes + ampm;
                    console.log("end");
                    var EndTimeStamp = ReminderScheduleList[i].EventEndDate;
                    var EndDateVal = EndTimeStamp.split(' ');
                    var finalEndTime = EndDateVal[1].split(':');
                    var Endhours = finalEndTime[0];
                    var Endminutes = finalEndTime[1];
                    var ampm = finalEndTime[0] >= 12 ? 'PM' : 'AM';
                    Endhours = Endhours % 12;
                    Endhours = Endhours ? Endhours : 12; // the hour '0' should be '12'
                    Endminutes = Endminutes < 10 ? '0' + Endminutes : Endminutes;
                    var strEndTime = Endhours + ':' + Endminutes + ' ' + ampm;
                    if (ReminderScheduleList[i].EventCustomId != "22222222-2222-2222-2222-222222222222" || (ReminderScheduleList[i].EventCustomId == "22222222-2222-2222-2222-222222222222" && empcount < 5)) {
                        if (ReminderScheduleList[i].EventCustomId == "22222222-2222-2222-2222-222222222222")
                            empcount++;
                        if (ReminderScheduleList[i].EventType == "Reminder") {
                        if (ReminderScheduleList[i].EventIsCalendar == true && ReminderScheduleList[i].EventAllDay == true) {
                            myEvent.push({
                                title: ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventType,
                                allDay: true,
                                start: StartdateVal[0],
                                end: StartdateVal[0],
                                Leadid: ReminderScheduleList[i].EventLeadId,
                                Customerid: ReminderScheduleList[i].EventCusId,
                                Appointmentid: ReminderScheduleList[i].EventAppid,
                                TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                EventPermission: ReminderScheduleList[i].EventPermissionName,
                                EventIsLead: ReminderScheduleList[i].EventCustomId,
                                resourceId: ReminderScheduleList[i].EventId,
                                backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                ticketid: ReminderScheduleList[i].EventTicketId
                            });
                        }
                        else if (ReminderScheduleList[i].EventIsCalendar == true && ReminderScheduleList[i].EventAllDay == false) {
                            myEvent.push({
                                title: finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventType + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                //allDay: true,
                                start: startTimeStamp,
                                end: EndTimeStamp,
                                Leadid: ReminderScheduleList[i].EventLeadId,
                                Customerid: ReminderScheduleList[i].EventCusId,
                                Appointmentid: ReminderScheduleList[i].EventAppid,
                                TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                EventPermission: ReminderScheduleList[i].EventPermissionName,
                                EventIsLead: ReminderScheduleList[i].EventCustomId,
                                resourceId: ReminderScheduleList[i].EventId,
                                backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                ticketid: ReminderScheduleList[i].EventTicketId
                            });
                        }
                        }
                        else {
                            if (ReminderScheduleList[i].EventIsCalendar == true && ReminderScheduleList[i].EventAllDay == true) {
                                if (ReminderScheduleList[i].EventBookingId != null && ReminderScheduleList[i].EventBookingId != "") {
                                    myEvent.push({
                                        title: "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventDisplayType,
                                        allDay: true,
                                        hovertitle: "Ticket#" + ReminderScheduleList[i].EventLeadId + "\n" + "Booking#" + ReminderScheduleList[i].EventBookingId + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventDisplayType + "\n" + "All day - Yes",
                                        start: StartdateVal[0],
                                        end: StartdateVal[0],
                                        Leadid: ReminderScheduleList[i].EventLeadId,
                                        Customerid: ReminderScheduleList[i].EventCusId,
                                        Appointmentid: ReminderScheduleList[i].EventAppid,
                                        TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                        EventPermission: ReminderScheduleList[i].EventPermissionName,
                                        EventUserId: ReminderScheduleList[i].EventCustomId,
                                        resourceId: ReminderScheduleList[i].EventId,
                                        backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                        EventUserDate: StartdateVal[0],
                                        ticketid: ReminderScheduleList[i].EventTicketId,
                                        status: ReminderScheduleList[i].EventStatus,
                                        additionalmem: ReminderScheduleList[i].EventAdditionalMember,
                                        rescheduleid: ReminderScheduleList[i].EventRescheduleId
                                    });
                                }
                                else {
                                    myEvent.push({
                                        title: "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventDisplayType,
                                        allDay: true,
                                        hovertitle: "Ticket#" + ReminderScheduleList[i].EventLeadId + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventDisplayType + "\n" + "All day - Yes",
                                        start: StartdateVal[0],
                                        end: StartdateVal[0],
                                        Leadid: ReminderScheduleList[i].EventLeadId,
                                        Customerid: ReminderScheduleList[i].EventCusId,
                                        Appointmentid: ReminderScheduleList[i].EventAppid,
                                        TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                        EventPermission: ReminderScheduleList[i].EventPermissionName,
                                        EventUserId: ReminderScheduleList[i].EventCustomId,
                                        resourceId: ReminderScheduleList[i].EventId,
                                        backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                        EventUserDate: StartdateVal[0],
                                        ticketid: ReminderScheduleList[i].EventTicketId,
                                        status: ReminderScheduleList[i].EventStatus,
                                        additionalmem: ReminderScheduleList[i].EventAdditionalMember,
                                        rescheduleid: ReminderScheduleList[i].EventRescheduleId
                                    });
                                }
                            }
                            else if (ReminderScheduleList[i].EventIsCalendar == true && ReminderScheduleList[i].EventAllDay == false) {
                                if (View == "Monthly") {
                                    myEvent.push({
                                        //title: finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventType + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                        //allDay: true,
                                        title: ReminderScheduleList[i].EventDisplayType + " (" + ReminderScheduleList[i].EventCalendarCount + ")",
                                        hovertitle: "<div style='width:100%;float:left;'><div style='width:100%;float:left;padding: 5px 15px;background-color: #f4f5f8;border-bottom: 1px solid #ccc;'><strong>" + ReminderScheduleList[i].EventType + "</strong>" + "<br>" + "Assigned - " + ReminderScheduleList[i].EventResourceName + "</div><div style='width:100%;float:left;padding:10px;overflow-y:auto;overflow-x:hidden;'>" + ReminderScheduleList[i].HoverTitle + "</div></div>",
                                        start: ReminderScheduleList[i].EventDate,
                                        end: ReminderScheduleList[i].EventDate,
                                        Leadid: ReminderScheduleList[i].EventLeadId,
                                        Customerid: ReminderScheduleList[i].EventCusId,
                                        Appointmentid: ReminderScheduleList[i].EventAppid,
                                        TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                        EventPermission: ReminderScheduleList[i].EventPermissionName,
                                        EventUserId: ReminderScheduleList[i].EventCustomId,
                                        resourceId: ReminderScheduleList[i].EventId,
                                        backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                        EventUserDate: ReminderScheduleList[i].EventDate,
                                        ticketid: ReminderScheduleList[i].EventTicketId,
                                        status: ReminderScheduleList[i].EventStatus,
                                        EventResource: ReminderScheduleList[i].EventResourceName,
                                        TypeDisplayEvent: ReminderScheduleList[i].EventDisplayType,
                                        additionalmem: ReminderScheduleList[i].EventAdditionalMember,
                                    });
                                }
                                else {
                                    if (ReminderScheduleList[i].EventBusinessName != null && ReminderScheduleList[i].EventBusinessName != "") {
                                        if (ReminderScheduleList[i].EventBookingId != null && ReminderScheduleList[i].EventBookingId != "") {
                                            myEvent.push({
                                                title: ReminderScheduleList[i].EventDisplayType + "\n" + finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " [" + ReminderScheduleList[i].EventBusinessName + "]" + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                                //allDay: true,
                                                //title: CountUser + "Assigned - " + ReminderScheduleList[i].EventTitle,
                                                hovertitle: "<strong>" + ReminderScheduleList[i].EventDisplayType + "</strong>" + "\n" + finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Ticket#" + ReminderScheduleList[i].EventLeadId + "\n" + "Booking#" + ReminderScheduleList[i].EventBookingId + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " [" + ReminderScheduleList[i].EventBusinessName + "]" + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                                start: startTimeStamp,
                                                end: EndTimeStamp,
                                                Leadid: ReminderScheduleList[i].EventLeadId,
                                                Customerid: ReminderScheduleList[i].EventCusId,
                                                Appointmentid: ReminderScheduleList[i].EventAppid,
                                                TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                                EventPermission: ReminderScheduleList[i].EventPermissionName,
                                                EventUserId: ReminderScheduleList[i].EventCustomId,
                                                resourceId: ReminderScheduleList[i].EventId,
                                                backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                                EventUserDate: StartdateVal[0],
                                                ticketid: ReminderScheduleList[i].EventTicketId,
                                                status: ReminderScheduleList[i].EventStatus,
                                                TypeDisplayEvent: ReminderScheduleList[i].EventDisplayType,
                                                EventResource: ReminderScheduleList[i].EventResourceName,
                                                eventendtime: EndTimeStamp,
                                                additionalmem: ReminderScheduleList[i].EventAdditionalMember,
                                                rescheduleid: ReminderScheduleList[i].EventRescheduleId
                                            });
                                        }
                                        else {
                                            myEvent.push({
                                                title: ReminderScheduleList[i].EventDisplayType + "\n" + finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " [" + ReminderScheduleList[i].EventBusinessName + "]" + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                                //allDay: true,
                                                //title: CountUser + "Assigned - " + ReminderScheduleList[i].EventTitle,
                                                hovertitle: "<strong>" + ReminderScheduleList[i].EventDisplayType + "</strong>" + "\n" + finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Ticket#" + ReminderScheduleList[i].EventLeadId + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " [" + ReminderScheduleList[i].EventBusinessName + "]" + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                                start: startTimeStamp,
                                                end: EndTimeStamp,
                                                Leadid: ReminderScheduleList[i].EventLeadId,
                                                Customerid: ReminderScheduleList[i].EventCusId,
                                                Appointmentid: ReminderScheduleList[i].EventAppid,
                                                TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                                EventPermission: ReminderScheduleList[i].EventPermissionName,
                                                EventUserId: ReminderScheduleList[i].EventCustomId,
                                                resourceId: ReminderScheduleList[i].EventId,
                                                backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                                EventUserDate: StartdateVal[0],
                                                ticketid: ReminderScheduleList[i].EventTicketId,
                                                status: ReminderScheduleList[i].EventStatus,
                                                TypeDisplayEvent: ReminderScheduleList[i].EventDisplayType,
                                                EventResource: ReminderScheduleList[i].EventResourceName,
                                                eventendtime: EndTimeStamp,
                                                additionalmem: ReminderScheduleList[i].EventAdditionalMember,
                                                rescheduleid: ReminderScheduleList[i].EventRescheduleId
                                            });
                                        }
                                    }
                                    else {
                                        if (ReminderScheduleList[i].EventBookingId != null && ReminderScheduleList[i].EventBookingId != "") {
                                            myEvent.push({
                                                title: finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventDisplayType + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                                //allDay: true,
                                                //title: CountUser + "Assigned - " + ReminderScheduleList[i].EventTitle,
                                                hovertitle: finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Ticket#" + ReminderScheduleList[i].EventLeadId + "\n" + "Booking#" + ReminderScheduleList[i].EventBookingId + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventDisplayType + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                                start: startTimeStamp,
                                                end: EndTimeStamp,
                                                Leadid: ReminderScheduleList[i].EventLeadId,
                                                Customerid: ReminderScheduleList[i].EventCusId,
                                                Appointmentid: ReminderScheduleList[i].EventAppid,
                                                TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                                EventPermission: ReminderScheduleList[i].EventPermissionName,
                                                EventUserId: ReminderScheduleList[i].EventCustomId,
                                                resourceId: ReminderScheduleList[i].EventId,
                                                backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                                EventUserDate: StartdateVal[0],
                                                ticketid: ReminderScheduleList[i].EventTicketId,
                                                status: ReminderScheduleList[i].EventStatus,
                                                TypeDisplayEvent: ReminderScheduleList[i].EventDisplayType,
                                                EventResource: ReminderScheduleList[i].EventResourceName,
                                                eventendtime: EndTimeStamp,
                                                additionalmem: ReminderScheduleList[i].EventAdditionalMember,
                                                rescheduleid: ReminderScheduleList[i].EventRescheduleId
                                            });
                                        }
                                        else {
                                            myEvent.push({
                                                title: finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventDisplayType + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                                //allDay: true,
                                                //title: CountUser + "Assigned - " + ReminderScheduleList[i].EventTitle,
                                                hovertitle: finalStartTime[0] + ":" + finalStartTime[1] + " " + StartdateVal[2] + " - " + finalEndTime[0] + ":" + finalEndTime[1] + " " + EndDateVal[2] + "\n" + "Ticket#" + ReminderScheduleList[i].EventLeadId + "\n" + "Assigned - " + ReminderScheduleList[i].EventTitle + "\n" + ReminderScheduleList[i].EventCustomerName + " - " + ReminderScheduleList[i].EventDisplayType + "\n" + ReminderScheduleList[i].EventStreet + "\n" + ReminderScheduleList[i].EventLocate,
                                                start: startTimeStamp,
                                                end: EndTimeStamp,
                                                Leadid: ReminderScheduleList[i].EventLeadId,
                                                Customerid: ReminderScheduleList[i].EventCusId,
                                                Appointmentid: ReminderScheduleList[i].EventAppid,
                                                TypeEvent: encodeURI(ReminderScheduleList[i].EventType),
                                                EventPermission: ReminderScheduleList[i].EventPermissionName,
                                                EventUserId: ReminderScheduleList[i].EventCustomId,
                                                resourceId: ReminderScheduleList[i].EventId,
                                                backgroundColor: "#" + ReminderScheduleList[i].EventColor,
                                                EventUserDate: StartdateVal[0],
                                                ticketid: ReminderScheduleList[i].EventTicketId,
                                                status: ReminderScheduleList[i].EventStatus,
                                                TypeDisplayEvent: ReminderScheduleList[i].EventDisplayType,
                                                EventResource: ReminderScheduleList[i].EventResourceName,
                                                eventendtime: EndTimeStamp,
                                                additionalmem: ReminderScheduleList[i].EventAdditionalMember,
                                                rescheduleid: ReminderScheduleList[i].EventRescheduleId
                                            });
                                        }
                                    }
                                }
                            }
                            }
                        }
                }
                daydiffrentborder(data.modelEmp, data.View);
                MyCalenderInit();
                
            }
            
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
    
}
var EventDetails = function (userid, type, date, resource, displaytype) {
    var hoverstr = "<div style='width:100%;float:left;'><div style='width:100%;float:left;padding: 5px 15px;background-color: #f4f5f8;border-bottom: 1px solid #ccc;'>" + "Date - " + date + "<br>" + "<strong>" + "Type - " + displaytype + "</strong>" + "<br>" + "Assigned - " + resource + "</div><div style='width:100%;float:left;padding:10px;'></div></div>";
    var url = "/Schedule/ScheduleEventDetails";
    var param = JSON.stringify({
        userid: userid,
        type: type,
        date: date
    })
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
                var ulstr = "<ul>{0}</ul>";
                var datamodel = data.model;
                if (datamodel.length > 0) {
                    var ULLists = "";
                    for (var i = 0; i < datamodel.length; i++) {
                        if (datamodel[i].BookingId != null && datamodel[i].BookingId != "") {
                            if (datamodel[i].IsAllDay == true) {
                                ULLists += "<li>" + "All day - Yes" + "<br>" + "Ticket#" + datamodel[i].TicketIntId + "<br>" + "Booking#" + datamodel[i].BookingId + "<br>" + "User - " + datamodel[i].EMPNAME + "<br>" + "Customer - " + datamodel[i].CustomerName + "<br>" + datamodel[i].Street + "<br>" + datamodel[i].City + "," + datamodel[i].State + " " + datamodel[i].ZipCode + "</li>";
                            }
                            else {
                                ULLists += "<li>" + datamodel[i].StartDate + " - " + datamodel[i].Enddate + "<br>" + "Ticket#" + datamodel[i].TicketIntId + "<br>" + "Booking#" + datamodel[i].BookingId + "<br>" + "User - " + datamodel[i].EMPNAME + "<br>" + "Customer - " + datamodel[i].CustomerName + "<br>" + datamodel[i].Street + "<br>" + datamodel[i].City + "," + datamodel[i].State + " " + datamodel[i].ZipCode + "</li>";
                            }
                        }
                        else {
                            if (datamodel[i].IsAllDay == true) {
                                ULLists += "<li>" + "All day - Yes" + "<br>" + "Ticket#" + datamodel[i].TicketIntId + "<br>" + "User - " + datamodel[i].EMPNAME + "<br>" + "Customer - " + datamodel[i].CustomerName + "<br>" + datamodel[i].Street + "<br>" + datamodel[i].City + "," + datamodel[i].State + " " + datamodel[i].ZipCode + "</li>";
                            }
                            else {
                                ULLists += "<li>" + datamodel[i].StartDate + " - " + datamodel[i].Enddate + "<br>" + "Ticket#" + datamodel[i].TicketIntId + "<br>" + "User - " + datamodel[i].EMPNAME + "<br>" + "Customer - " + datamodel[i].CustomerName + "<br>" + datamodel[i].Street + "<br>" + datamodel[i].City + "," + datamodel[i].State + " " + datamodel[i].ZipCode + "</li>";
                            }
                        }
                    }
                    hoverstr = hoverstr + String.format(ulstr, ULLists);
                }
                $(".event_hover_container").html(hoverstr);
            }
            else {
                $(".event_hover_container").html(hoverstr);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var EventDropHandler = function (Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional) {
    var url = domainurl + "/Schedule/DroppingPermissionScheduleCalendar";
    var param = JSON.stringify({
        eventType: Type,
        eventId: Id,
        eventDate: Date,
        eventAppid: appid,
        eventAllDay: allday,
        eventEndDate: endDate,
        Eventresid: Eventresid,
        dragresid: dropid,
        ViewName: ViewName,
        CustomId: CustomId,
        eventticketid: eventticketid,
        additional: additional
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
            if (data.ExistUserAssign) {
                EventResizeHandler(Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, true, true);
            }
            else if (data.ExistUserAdditional) {
                EventResizeHandler(Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, false, true);
            }
            else {
                if (data.result) {
                    if (typeof (additional) != "undefined" && additional != null && additional != "" && additional == "Additional") {
                        EventResizeHandler(Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, false, true);
                    }
                    else {
                        EventResizeHandler(Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, true, true);
                    }
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var EventResizeHandler = function (Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, chkassign, isexist) {
    var url = domainurl + "/Schedule/DraggingScheduleCalendar";
    var param = JSON.stringify({
        eventType: Type,
        eventId: Id,
        eventDate: Date,
        eventAppid: appid,
        eventAllDay: allday,
        eventEndDate: endDate,
        Eventresid: Eventresid,
        dragresid: dropid,
        ViewName: ViewName,
        CustomId: CustomId,
        eventticketid: eventticketid,
        additional: additional,
        chkassign: chkassign,
        isexist: isexist
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
            
            if (typeof (data.message) == "undefined" || data.message == null || data.message == "") {
                view = myCalendar.fullCalendar('getView');
                moment = myCalendar.fullCalendar('getDate');
                if (view.name == "agendaDay") {
                    viewname = "Daily";
                }
                if (view.name == "agendaFourDay") {
                    viewname = "Weekly";
                }
                if (view.name == "month") {
                    viewname = "Monthly";
                }
                if (view.name == "listWeek") {
                    viewname = "List";
                }
                $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + FinalPageno + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + moment.format() + "&typeval=" + encodeURIComponent(typeval));
            }
            else {
                view = myCalendar.fullCalendar('getView');
                moment = myCalendar.fullCalendar('getDate');
                if (view.name == "agendaDay") {
                    viewname = "Daily";
                }
                if (view.name == "agendaFourDay") {
                    viewname = "Weekly";
                }
                if (view.name == "month") {
                    viewname = "Monthly";
                }
                if (view.name == "listWeek") {
                    viewname = "List";
                }
                $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + FinalPageno + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + moment.format() + "&typeval=" + encodeURIComponent(typeval));
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var OnlyEventResizeHandler = function (Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional) {
    var url = domainurl + "/Schedule/OnlyEventResizeHandlerCalendar";
    var param = JSON.stringify({
        eventType: Type,
        eventId: Id,
        eventDate: Date,
        eventAppid: appid,
        eventAllDay: allday,
        eventEndDate: endDate,
        Eventresid: Eventresid,
        dragresid: dropid,
        ViewName: ViewName,
        CustomId: CustomId,
        eventticketid: eventticketid,
        additional: additional
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

            if (typeof (data.message) == "undefined" || data.message == null || data.message == "") {
                view = myCalendar.fullCalendar('getView');
                moment = myCalendar.fullCalendar('getDate');
                if (view.name == "agendaDay") {
                    viewname = "Daily";
                }
                if (view.name == "agendaFourDay") {
                    viewname = "Weekly";
                }
                if (view.name == "month") {
                    viewname = "Monthly";
                }
                if (view.name == "listWeek") {
                    viewname = "List";
                }
                $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + FinalPageno + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + moment.format() + "&typeval=" + encodeURIComponent(typeval));
            }
            else {
                view = myCalendar.fullCalendar('getView');
                moment = myCalendar.fullCalendar('getDate');
                if (view.name == "agendaDay") {
                    viewname = "Daily";
                }
                if (view.name == "agendaFourDay") {
                    viewname = "Weekly";
                }
                if (view.name == "month") {
                    viewname = "Monthly";
                }
                if (view.name == "listWeek") {
                    viewname = "List";
                }
                $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + FinalPageno + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + moment.format() + "&typeval=" + encodeURIComponent(typeval));
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var UserPermissionForCreateTicket = function (Eventresid, eventloaddate, customerid, ticketid) {
    var url = domainurl + "/Schedule/UserPermissionForCreateTicket";
    var param = JSON.stringify({
        Eventresid: Eventresid,
        eventloaddate: eventloaddate,
        customerid: customerid,
        ticketid: ticketid
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
                var splitLoadDate = eventloaddate.split('T');
                if (eventloaddate.indexOf(':') == -1) {
                    if (IsPreviousDate == "True") {
                        OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?loadDate=" + splitLoadDate[0] + "&CustomerId=" + customerid + "&Id=" + ticketid);
                    }
                    else {
                        if (eventloaddate >= date) {

                            OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?loadDate=" + splitLoadDate[0] + "&CustomerId=" + customerid + "&Id=" + ticketid);
                        }
                        else {
                            OpenErrorMessageNew("Error!", "Schedule date should be greater than today's date.", "");
                        }
                    }
                }
                else {
                    var splitLoadTime = splitLoadDate[1].split(':');

                    var LoadTime = splitLoadTime[0] + ":" + splitLoadTime[1];
                    if (IsPreviousDate == "True") {
                        OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?loadDate=" + splitLoadDate[0] + "&CustomerId=" + customerid + "&startTime=" + LoadTime + "&UserId=" + Eventresid + "&Id=" + ticketid);
                    }
                    else {
                        if (eventloaddate >= date) {
                            console.log("asi");
                            OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?loadDate=" + splitLoadDate[0] + "&CustomerId=" + customerid + "&startTime=" + LoadTime + "&UserId=" + Eventresid + "&Id=" + ticketid);
                        }
                        else {
                            OpenErrorMessageNew("Error!", "Schedule date should be greater than today's date.", "");
                        }
                    }
                }
            }
            else {
                OpenErrorMessageNew("Error", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}

var MyCalenderInit = function () {
    if (defview == "Weekly") {
        if (typeof (ispermit) != "undefined" && ispermit != null && ispermit != "" && ispermit == "False") {
            myCalendar = $('#ScheduleCalender').fullCalendar({
                groupByDateAndResource: true,
                height: 'parent',
                header: {
                    left: 'prev,next today',
                    //center: 'title',
                    right: 'myCustomButtonPrev,myCustomButtonNext, agendaDay,agendaFourDay,month,listWeek precustombuttons'
                },
                views: {
                    agendaFourDay: {
                        titleRangeSeparator: ' - ',
                        type: 'agenda',
                        duration: { days: 7 },
                        buttonText: 'Week',
                        eventLimit: 1
                    },
                    listWeek: {
                        titleRangeSeparator: ' - '
                    },
                    agendaDay: {
                        eventLimit: 1
                    }
                },
                defaultView: DefaultView,
                allDaySlot: true,
                //editable: true,
                //droppable: true,
                isUserCreated: true,
                eventLimit: true,
                maxTime: maxtime,
                minTime: mintime,
                resources: myResource,
                events: myEvent,
                eventClick: function (calEvent, jsEvent, view) {
                    if (calEvent.TypeEvent == "WorkOrder") {
                        if (calEvent.EventPermission == null) {
                            OpenTopToBottomModal(domainurl + "/WorkOrder/TopToBottomWorkOrder/?AppointmentId=" + calEvent.Appointmentid + "&CustomerId=" + calEvent.Customerid);
                        }
                        if (calEvent.EventPermission == "Technician") {
                            OpenTopToBottomModal(domainurl + "/Customer/LeadTechCallPartial/?id=" + calEvent.Leadid);
                        }
                    }
                    if (calEvent.TypeEvent == "ServiceOrder") {
                        OpenTopToBottomModal(domainurl + "/ServiceOrder/TopToBottomModalServiceOrder/?AppointmentId=" + calEvent.Appointmentid + "&CustomerId=" + calEvent.Customerid);
                    }
                    if (calEvent.TypeEvent == "QA1") {
                        OpenTopToBottomModal(domainurl + "/Customer/QA1QuestionariePartial/?id=" + calEvent.Leadid);
                    }
                    if (calEvent.TypeEvent == "QA2") {
                        OpenTopToBottomModal(domainurl + "/Customer/QA2QuestionariePartial/?id=" + calEvent.Leadid);
                    }
                    if (calEvent.TypeEvent == "Note") {
                        OpenRightToLeftModal(domainurl + "/Notes/AddNotes/?id=" + calEvent.leadId + "&customerid=" + calEvent.appid);
                    }
                    if (calEvent.TypeEvent == "Reminder") {
                        if (calEvent.EventIsLead == "False") {
                            OpenRightToLeftModal(domainurl + "/Customer/AddNewReminderNote/?id=" + calEvent.Leadid + "&CustomerId=" + calEvent.Customerid + "&Timeval=" + strTime + "&IsComplete=" + "false");
                        }
                        else {
                            OpenRightToLeftModal(domainurl + "/Leads/AddNewReminderNote/?id=" + calEvent.Leadid + "&CustomerId=" + calEvent.Customerid + "&Timeval=" + strTime + "&IsComplete=" + "false");
                        }
                    }
                    else {
                        if (view.name == "month") {
                            viewname = "Daily";
                            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + calEvent.EventUserId + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + calEvent.EventUserDate + "&typeval=" + encodeURIComponent(calEvent.TypeEvent) + "&TicketId=" + calEvent.ticketid);
                            //$(".fc-precustombuttons-button").css("display", "block");
                            //$(".fc-custombuttons-button").css("display", "block");
                            valtype = calEvent.TypeEvent;
                            valUser = calEvent.EventUserId;
                        }
                        else {
                            if (typeof (TechUserId) != "undefined" && TechUserId != null && TechUserId != "" && TechUserId == calEvent.EventUserId) {
                                OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?Id=" + calEvent.Leadid);
                            }
                        }
                    }
                    if (calEvent.TypeEvent == "Schedule") {
                        OpenRightToLeftModal(domainurl + "/TechSchedule/AddTechSchedule/?id=" + calEvent.Leadid + "&customerid=" + calEvent.Customerid + "&IsSchedule=" + "true");
                    }
                },
                dragScroll: true,
                displayEventTime: false,
                eventOverlap: true,
                eventRender: function (event, element, view) {
                    $(".popover").remove();
                    if (view.name == "month") {
                        element.popover({
                            html: 'true',
                            container: 'body',
                            animation: false,
                            delay: 300,
                            content: function () {
                                setTimeout(function () {
                                    EventDetails(event.ticketid, event.TypeEvent, event.EventUserDate, event.EventResource, event.TypeDisplayEvent);
                                }, 1500);
                                return "<div class='event_hover_container'></div>";
                            },
                            placement: 'right',
                            trigger: 'manual'
                        }).on("mouseenter", function () {
                            var _this = this;
                            $(this).popover("show");
                            $(".popover").on("mouseleave", function () {
                                $(_this).popover('hide');
                            });
                        }).on("mouseleave", function () {
                            var _this = this;
                            setTimeout(function () {
                                if (!$(".popover:hover").length) {
                                    $(_this).popover("hide");
                                }
                            }, 300);
                        }).on("click", function () {
                            $('.popover').not(this).popover('hide');
                        });
                    }
                    else {
                        if (view.name == "agendaDay") {
                            element.popover({
                                html: 'true',
                                container: 'body',
                                animation: true,
                                delay: 300,
                                content: event.hovertitle,
                                placement: 'top',
                                trigger: 'hover'
                            });
                        }
                        else if (view.name == "agendaFourDay") {
                            element.popover({
                                html: 'true',
                                container: 'body',
                                animation: true,
                                delay: 300,
                                content: event.hovertitle,
                                placement: 'top',
                                trigger: 'hover'
                            });
                        }
                    }
                    for (var s = 0; s < TicketStatusArr.length; s++) {
                        if (typeof (TicketStatusArr[s].image) != "undefined" && TicketStatusArr[s].image != null && TicketStatusArr[s].image != "" && event.status == TicketStatusArr[s].status) {
                            element.find(".fc-title").prepend('<img src = "/TicketStatusImageShow/W20H20X' + TicketStatusArr[s].status + '" style = "max-width:20px;height:20px;float:right;border-radius:50px;width:auto;" />');
                        }
                    }

                    if (CompareTicketServiceArr.length > 0) {
                        for (var comp = 0; comp < CompareTicketServiceArr.length; comp++) {
                            if (typeof (CompareTicketServiceArr[comp]) != "undefined" && CompareTicketServiceArr[comp] != null && CompareTicketServiceArr[comp] != "" && CompareTicketServiceArr[comp] == event.ticketid) {
                                if (event.status != "Lost" && event.status != "Pending") {
                                    element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                                    setInterval(function () {
                                        $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                        $(".fc-title i.fa.fa-bolt").fadeOut(100)
                                    }, 600);
                                }
                            }
                        }
                    }
                    if ((event.status != "Completed" && event.status != "Lost" && event.status != "Pending") && event.TypeEvent == "Drop%20Off") {
                        var dt = new Date(event.eventendtime);
                        dt = dt.setMinutes(dt.getMinutes() - 30);
                        var dt2 = new Date();
                        if (dt2.getTime() > dt) {
                            element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                            setInterval(function () {
                                $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                $(".fc-title i.fa.fa-bolt").fadeOut(100)
                            }, 600);
                        }
                    }
                    if ((event.status != "Completed" && event.status != "Lost" && event.status != "Pending") && event.TypeEvent == "Pick%20Up") {
                        var dt = new Date(event.eventendtime);
                        dt = dt.setMinutes(dt.getMinutes() - 30);
                        var dt2 = new Date();
                        if (dt2.getTime() > dt) {
                            element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                            setInterval(function () {
                                $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                $(".fc-title i.fa.fa-bolt").fadeOut(100)
                            }, 600);
                        }
                    }
                    if (parseInt(event.rescheduleid) > 0) {
                        element.find(".fc-title").prepend('<img src = "/Content/img/reschedule_new.png" style = "max-width:20px;height:20px;float:right;border-radius:50px;width:auto;margin-right:25px;" />');
                    }
                },
                dayRender: function (date, cell, view) {
                    var cellYear = date.year();
                    var cellMonth = (date.month() + 1 < 10) ? '0' + (date.month() + 1) : (date.month() + 1);
                    var cellDay = (date.date() < 10) ? '0' + (date.date()) : (date.date());
                    var newDate = cellYear + "-" + cellMonth + "-" + cellDay;
                    if (newDate >= getDates() && view.name == "month") {
                        cell.append('<span><i class="fa fa-plus" aria-hidden="true" style="color:#ccc;"></i></span>');
                    }
                },
                customButtons: {
                    myCustomButtonNext: {
                        icon: 'right-double-arrow',
                        click: function () {
                            event.preventDefault();
                            $(".fc-view-container .fc-view").animate({
                                scrollLeft: "+=200px"
                            }, "slow");
                        },
                        title: 'Scroll to right'
                    },
                    myCustomButtonPrev: {
                        icon: 'left-double-arrow',
                        click: function () {
                            event.preventDefault();
                            $(".fc-view-container .fc-view").animate({
                                scrollLeft: "-=200px"
                            }, "slow");
                        },
                        title: 'Scroll to right'
                    }
                },
            });
        }
        else {
            myCalendar = $('#ScheduleCalender').fullCalendar({
                groupByDateAndResource: true,
                height: 'parent',
                header: {
                    left: 'prev,next today',
                    //center: 'title',
                    right: 'myCustomButtonPrev,myCustomButtonNext, agendaDay,agendaFourDay,month,listWeek precustombuttons'
                },
                views: {
                    agendaFourDay: {
                        titleRangeSeparator: ' - ',
                        type: 'agenda',
                        duration: { days: 7 },
                        buttonText: 'Week',
                        eventLimit: 1
                        
                    },
                    listWeek: {
                        titleRangeSeparator: ' - '
                    },
                    agendaDay: {
                        eventLimit: 1
                    }
                },
                defaultView: DefaultView,
                allDaySlot: true,
                editable: true,
                droppable: true,
                isUserCreated: true,
                eventLimit: true,
                maxTime: maxtime,
                minTime: mintime,
                resources: myResource,
                events: myEvent,                
                dragScroll: true,
                dayClick: function (start, jsEvent, view, resourceObj) {
                    LoadDate = moment(start).format();
                    if (view.name == "month") {
                        viewname = "Daily";
                        $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + LoadDate + "&typeval=" + encodeURIComponent(typeval));
                        //$(".fc-precustombuttons-button").css("display", "block");
                        //$(".fc-custombuttons-button").css("display", "block");
                    }
                    else {
                        UserPermissionForCreateTicket(resourceObj.id, LoadDate, ticketCustomerId, TicketId);
                    }
                },
                eventClick: function (calEvent, jsEvent, view) {
                    if (calEvent.TypeEvent == "WorkOrder") {
                        if (calEvent.EventPermission == null) {
                            OpenTopToBottomModal(domainurl + "/WorkOrder/TopToBottomWorkOrder/?AppointmentId=" + calEvent.Appointmentid + "&CustomerId=" + calEvent.Customerid);
                        }
                        if (calEvent.EventPermission == "Technician") {
                            OpenTopToBottomModal(domainurl + "/Customer/LeadTechCallPartial/?id=" + calEvent.Leadid);
                        }
                    }
                    if (calEvent.TypeEvent == "ServiceOrder") {
                        OpenTopToBottomModal(domainurl + "/ServiceOrder/TopToBottomModalServiceOrder/?AppointmentId=" + calEvent.Appointmentid + "&CustomerId=" + calEvent.Customerid);
                    }
                    if (calEvent.TypeEvent == "QA1") {
                        OpenTopToBottomModal(domainurl + "/Customer/QA1QuestionariePartial/?id=" + calEvent.Leadid);
                    }
                    if (calEvent.TypeEvent == "QA2") {
                        OpenTopToBottomModal(domainurl + "/Customer/QA2QuestionariePartial/?id=" + calEvent.Leadid);
                    }
                    if (calEvent.TypeEvent == "Note") {
                        OpenRightToLeftModal(domainurl + "/Notes/AddNotes/?id=" + calEvent.leadId + "&customerid=" + calEvent.appid);
                    }
                    if (calEvent.TypeEvent == "Reminder") {
                        if (calEvent.EventIsLead == "False") {
                            OpenRightToLeftModal(domainurl + "/Customer/AddNewReminderNote/?id=" + calEvent.Leadid + "&CustomerId=" + calEvent.Customerid + "&Timeval=" + strTime + "&IsComplete=" + "false");
                        }
                        else {
                            OpenRightToLeftModal(domainurl + "/Leads/AddNewReminderNote/?id=" + calEvent.Leadid + "&CustomerId=" + calEvent.Customerid + "&Timeval=" + strTime + "&IsComplete=" + "false");
                        }
                    }
                    else {
                        if (view.name == "month") {
                            viewname = "Daily";
                            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + calEvent.EventUserId + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + calEvent.EventUserDate + "&typeval=" + encodeURIComponent(calEvent.TypeEvent) + "&TicketId=" + calEvent.ticketid);
                            //$(".fc-precustombuttons-button").css("display", "block");
                            //$(".fc-custombuttons-button").css("display", "block");
                            valtype = calEvent.TypeEvent;
                            valUser = calEvent.EventUserId;
                        }
                        else {
                            OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?Id=" + calEvent.Leadid);
                        }
                    }
                    if (calEvent.TypeEvent == "Schedule") {
                        OpenRightToLeftModal(domainurl + "/TechSchedule/AddTechSchedule/?id=" + calEvent.Leadid + "&customerid=" + calEvent.Customerid + "&IsSchedule=" + "true");
                    }
                },
                eventDrop: function (event, delta, minuteDelta) {
                    var view = myCalendar.fullCalendar('getView');
                    moment = myCalendar.fullCalendar('getDate');
                    if (view.name == "month") {
                        EventResizeHandler(event.TypeEvent, event.Leadid, event.start, event.Appointmentid, event.allDay, event.end, event.resourceId, "", view.name, event.EventUserId, event.ticketid, event.additionalmem, true, true);
                    }
                    else {
                        EventDropHandler(event.TypeEvent, event.Leadid, event.start.format(), event.Appointmentid, event.allDay, event.end, event.resourceId, "", view.name, event.EventUserId, event.ticketid, event.additionalmem);
                    }
                    $(".popover").remove();
                },
                eventResize: function (event, delta, minuteDelta) {
                    var view = myCalendar.fullCalendar('getView');
                    moment = myCalendar.fullCalendar('getDate');
                    OnlyEventResizeHandler(event.TypeEvent, event.Leadid, event.start, event.Appointmentid, event.allDay, event.end.format(), event.resourceId, "", view.name, event.EventUserId, event.ticketid, event.additionalmem);
                    $(".popover").remove();
                },
                displayEventTime: false,
                eventOverlap: true,
                eventRender: function (event, element, view) {
                    $(".popover").remove();
                    if (view.name == "month") {
                        element.popover({
                            html: 'true',
                            container: 'body',
                            animation: false,
                            delay: 300,
                            content: function () {
                                setTimeout(function () {
                                    EventDetails(event.ticketid, event.TypeEvent, event.EventUserDate, event.EventResource, event.TypeDisplayEvent);
                                }, 1500);
                                return "<div class='event_hover_container'></div>";
                            },
                            placement: 'right',
                            trigger: 'manual'
                        }).on("mouseenter", function () {
                            var _this = this;
                            $(this).popover("show");
                            $(".popover").on("mouseleave", function () {
                                $(_this).popover('hide');
                            });
                        }).on("mouseleave", function () {
                            var _this = this;
                            setTimeout(function () {
                                if (!$(".popover:hover").length) {
                                    $(_this).popover("hide");
                                }
                            }, 300);
                        }).on("click", function () {
                            $('.popover').not(this).popover('hide');
                        });
                    }
                    else {
                        if (view.name == "agendaDay") {
                            element.popover({
                                html: 'true',
                                container: 'body',
                                animation: true,
                                delay: 300,
                                content: event.hovertitle,
                                placement: 'top',
                                trigger: 'hover'
                            });
                        }
                        else if (view.name == "agendaFourDay") {
                            element.popover({
                                html: 'true',
                                container: 'body',
                                animation: true,
                                delay: 300,
                                content: event.hovertitle,
                                placement: 'top',
                                trigger: 'hover'
                            });
                        }
                    }
                    for (var s = 0; s < TicketStatusArr.length; s++) {
                        if (typeof (TicketStatusArr[s].image) != "undefined" && TicketStatusArr[s].image != null && TicketStatusArr[s].image != "" && event.status == TicketStatusArr[s].status) {
                            element.find(".fc-title").prepend('<img src = "/TicketStatusImageShow/W20H20X' + TicketStatusArr[s].status + '" style = "max-width:20px;height:20px;float:right;border-radius:50px;width:auto;" />');
                        }
                    }

                    if (CompareTicketServiceArr.length > 0) {
                        for (var comp = 0; comp < CompareTicketServiceArr.length; comp++) {
                            if (typeof (CompareTicketServiceArr[comp]) != "undefined" && CompareTicketServiceArr[comp] != null && CompareTicketServiceArr[comp] != "" && CompareTicketServiceArr[comp] == event.ticketid) {
                                if (event.status != "Lost" && event.status != "Pending") {
                                    element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                                    setInterval(function () {
                                        $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                        $(".fc-title i.fa.fa-bolt").fadeOut(100)
                                    }, 600);
                                }
                            }
                        }
                    }
                    if ((event.status != "Completed" && event.status != "Lost" && event.status != "Pending") && event.TypeEvent == "Drop%20Off") {
                        var dt = new Date(event.eventendtime);
                        dt = dt.setMinutes(dt.getMinutes() - 30);
                        var dt2 = new Date();
                        if (dt2.getTime() > dt) {
                            element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                            setInterval(function () {
                                $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                $(".fc-title i.fa.fa-bolt").fadeOut(100)
                            }, 600);
                        }
                    }
                    if ((event.status != "Completed" && event.status != "Lost" && event.status != "Pending") && event.TypeEvent == "Pick%20Up") {
                        var dt = new Date(event.eventendtime);
                        dt = dt.setMinutes(dt.getMinutes() - 30);
                        var dt2 = new Date();
                        if (dt2.getTime() > dt) {
                            element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                            setInterval(function () {
                                $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                $(".fc-title i.fa.fa-bolt").fadeOut(100)
                            }, 600);
                        }
                    }
                    if (parseInt(event.rescheduleid) > 0) {
                        element.find(".fc-title").prepend('<img src = "/Content/img/reschedule_new.png" style = "max-width:20px;height:20px;float:right;border-radius:50px;width:auto;margin-right:25px;" />');
                    }
                },
                dayRender: function (date, cell, view) {
                    var cellYear = date.year();
                    var cellMonth = (date.month() + 1 < 10) ? '0' + (date.month() + 1) : (date.month() + 1);
                    var cellDay = (date.date() < 10) ? '0' + (date.date()) : (date.date());
                    var newDate = cellYear + "-" + cellMonth + "-" + cellDay;
                    if (newDate >= getDates() && view.name == "month") {
                        cell.append('<span><i class="fa fa-plus" aria-hidden="true" style="color:#ccc;"></i></span>');
                    }
                },
                customButtons: {
                    myCustomButtonNext: {
                        icon: 'right-double-arrow',
                        click: function () {
                            event.preventDefault();
                            $(".fc-view-container .fc-view").animate({
                                scrollLeft: "+=300px"
                            }, "slow");
                        },
                        title: 'Scroll to right'
                    },
                    myCustomButtonPrev: {
                        icon: 'left-double-arrow',
                        click: function () {
                            event.preventDefault();
                            $(".fc-view-container .fc-view").animate({
                                scrollLeft: "-=300px"
                            }, "slow");
                        },
                        title: 'Scroll to right'
                    }
                },
            });
        }
    }
    else {
        if (typeof (ispermit) != "undefined" && ispermit != null && ispermit != "" && ispermit == "False") {
            myCalendar = $('#ScheduleCalender').fullCalendar({
                height: 'parent',
                header: {
                    left: 'prev,next today',
                    //center: 'title',
                    right: 'myCustomButtonPrev,myCustomButtonNext, agendaDay,agendaFourDay,month,listWeek precustombuttons'
                },
                views: {
                    agendaFourDay: {
                        titleRangeSeparator: ' - ',
                        type: 'agenda',
                        duration: { days: 7 },
                        buttonText: 'Week',
                        eventLimit: 1
                    },
                    listWeek: {
                        titleRangeSeparator: ' - '
                    },
                    agendaDay: {
                        eventLimit: 1
                    }
                },
                defaultView: DefaultView,
                allDaySlot: true,
                //editable: true,
                //droppable: true,
                isUserCreated: true,
                eventLimit: true,
                maxTime: maxtime,
                minTime: mintime,
                resources: myResource,
                events: myEvent,
                eventClick: function (calEvent, jsEvent, view) {
                    if (calEvent.TypeEvent == "WorkOrder") {
                        if (calEvent.EventPermission == null) {
                            OpenTopToBottomModal(domainurl + "/WorkOrder/TopToBottomWorkOrder/?AppointmentId=" + calEvent.Appointmentid + "&CustomerId=" + calEvent.Customerid);
                        }
                        if (calEvent.EventPermission == "Technician") {
                            OpenTopToBottomModal(domainurl + "/Customer/LeadTechCallPartial/?id=" + calEvent.Leadid);
                        }
                    }
                    if (calEvent.TypeEvent == "ServiceOrder") {
                        OpenTopToBottomModal(domainurl + "/ServiceOrder/TopToBottomModalServiceOrder/?AppointmentId=" + calEvent.Appointmentid + "&CustomerId=" + calEvent.Customerid);
                    }
                    if (calEvent.TypeEvent == "QA1") {
                        OpenTopToBottomModal(domainurl + "/Customer/QA1QuestionariePartial/?id=" + calEvent.Leadid);
                    }
                    if (calEvent.TypeEvent == "QA2") {
                        OpenTopToBottomModal(domainurl + "/Customer/QA2QuestionariePartial/?id=" + calEvent.Leadid);
                    }
                    if (calEvent.TypeEvent == "Note") {
                        OpenRightToLeftModal(domainurl + "/Notes/AddNotes/?id=" + calEvent.leadId + "&customerid=" + calEvent.appid);
                    }
                    if (calEvent.TypeEvent == "Reminder") {
                        if (calEvent.EventIsLead == "False") {
                            OpenRightToLeftModal(domainurl + "/Customer/AddNewReminderNote/?id=" + calEvent.Leadid + "&CustomerId=" + calEvent.Customerid + "&Timeval=" + strTime + "&IsComplete=" + "false");
                        }
                        else {
                            OpenRightToLeftModal(domainurl + "/Leads/AddNewReminderNote/?id=" + calEvent.Leadid + "&CustomerId=" + calEvent.Customerid + "&Timeval=" + strTime + "&IsComplete=" + "false");
                        }
                    }
                    else {
                        if (view.name == "month") {
                            viewname = "Daily";
                            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + calEvent.EventUserId + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + calEvent.EventUserDate + "&typeval=" + encodeURIComponent(calEvent.TypeEvent) + "&TicketId=" + calEvent.ticketid);
                            //$(".fc-precustombuttons-button").css("display", "block");
                            //$(".fc-custombuttons-button").css("display", "block");
                            valtype = calEvent.TypeEvent;
                            valUser = calEvent.EventUserId;
                        }
                        else {
                            if (typeof (TechUserId) != "undefined" && TechUserId != null && TechUserId != "" && TechUserId == calEvent.EventUserId) {
                                OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?Id=" + calEvent.Leadid);
                            }
                        }
                    }
                    if (calEvent.TypeEvent == "Schedule") {
                        OpenRightToLeftModal(domainurl + "/TechSchedule/AddTechSchedule/?id=" + calEvent.Leadid + "&customerid=" + calEvent.Customerid + "&IsSchedule=" + "true");
                    }
                },
                dragScroll: true,
                displayEventTime: false,
                eventOverlap: true,
                eventRender: function (event, element, view) {
                    $(".popover").remove();
                    if (view.name == "month") {
                        element.popover({
                            html: 'true',
                            container: 'body',
                            animation: false,
                            delay: 300,
                            content: function () {
                                setTimeout(function () {
                                    EventDetails(event.ticketid, event.TypeEvent, event.EventUserDate, event.EventResource, event.TypeDisplayEvent);
                                }, 1500);
                                return "<div class='event_hover_container'></div>";
                            },
                            placement: 'right',
                            trigger: 'manual'
                        }).on("mouseenter", function () {
                            var _this = this;
                            $(this).popover("show");
                            $(".popover").on("mouseleave", function () {
                                $(_this).popover('hide');
                            });
                        }).on("mouseleave", function () {
                            var _this = this;
                            setTimeout(function () {
                                if (!$(".popover:hover").length) {
                                    $(_this).popover("hide");
                                }
                            }, 300);
                        }).on("click", function () {
                            $('.popover').not(this).popover('hide');
                        });
                    }
                    else {
                        if (view.name == "agendaDay") {
                            element.popover({
                                html: 'true',
                                container: 'body',
                                animation: true,
                                delay: 300,
                                content: event.hovertitle,
                                placement: 'top',
                                trigger: 'hover'
                            });
                        }
                        else if (view.name == "agendaFourDay") {
                            element.popover({
                                html: 'true',
                                container: 'body',
                                animation: true,
                                delay: 300,
                                content: event.hovertitle,
                                placement: 'top',
                                trigger: 'hover'
                            });
                        }
                    }
                    for (var s = 0; s < TicketStatusArr.length; s++) {
                        if (typeof (TicketStatusArr[s].image) != "undefined" && TicketStatusArr[s].image != null && TicketStatusArr[s].image != "" && event.status == TicketStatusArr[s].status) {
                            element.find(".fc-title").prepend('<img src = "/TicketStatusImageShow/W20H20X' + TicketStatusArr[s].status + '"" style = "max-width:20px;height:20px;float:right;border-radius:50px;width:auto;" />');
                        }
                    }

                    if (CompareTicketServiceArr.length > 0) {
                        for (var comp = 0; comp < CompareTicketServiceArr.length; comp++) {
                            if (typeof (CompareTicketServiceArr[comp]) != "undefined" && CompareTicketServiceArr[comp] != null && CompareTicketServiceArr[comp] != "" && CompareTicketServiceArr[comp] == event.ticketid) {
                                if (event.status != "Lost" && event.status != "Pending") {
                                    element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                                    setInterval(function () {
                                        $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                        $(".fc-title i.fa.fa-bolt").fadeOut(100)
                                    }, 600);
                                }
                            }
                        }
                    }
                    if ((event.status != "Completed" && event.status != "Lost" && event.status != "Pending") && event.TypeEvent == "Drop%20Off") {
                        var dt = new Date(event.eventendtime);
                        dt = dt.setMinutes(dt.getMinutes() - 30);
                        var dt2 = new Date();
                        if (dt2.getTime() > dt) {
                            element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                            setInterval(function () {
                                $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                $(".fc-title i.fa.fa-bolt").fadeOut(100)
                            }, 600);
                        }
                    }
                    if ((event.status != "Completed" && event.status != "Lost" && event.status != "Pending") && event.TypeEvent == "Pick%20Up") {
                        var dt = new Date(event.eventendtime);
                        dt = dt.setMinutes(dt.getMinutes() - 30);
                        var dt2 = new Date();
                        if (dt2.getTime() > dt) {
                            element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                            setInterval(function () {
                                $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                $(".fc-title i.fa.fa-bolt").fadeOut(100)
                            }, 600);
                        }
                    }
                    if (parseInt(event.rescheduleid) > 0) {
                        element.find(".fc-title").prepend('<img src = "/Content/img/reschedule_new.png" style = "max-width:20px;height:20px;float:right;border-radius:50px;width:auto;margin-right:25px;" />');
                    }
                },
                dayRender: function (date, cell, view) {
                    var cellYear = date.year();
                    var cellMonth = (date.month() + 1 < 10) ? '0' + (date.month() + 1) : (date.month() + 1);
                    var cellDay = (date.date() < 10) ? '0' + (date.date()) : (date.date());
                    var newDate = cellYear + "-" + cellMonth + "-" + cellDay;
                    if (newDate >= getDates() && view.name == "month") {
                        cell.append('<span><i class="fa fa-plus" aria-hidden="true" style="color:#ccc;"></i></span>');
                    }
                },
                customButtons: {
                    myCustomButtonNext: {
                        icon: 'right-double-arrow',
                        click: function () {
                            event.preventDefault();
                            $(".fc-view-container .fc-view").animate({
                                scrollLeft: "+=200px"
                            }, "slow");
                        },
                        title: 'Scroll to right'
                    },
                    myCustomButtonPrev: {
                        icon: 'left-double-arrow',
                        click: function () {
                            event.preventDefault();
                            $(".fc-view-container .fc-view").animate({
                                scrollLeft: "-=200px"
                            }, "slow");
                        },
                        title: 'Scroll to right'
                    }
                },
            });
        }
        else {
            myCalendar = $('#ScheduleCalender').fullCalendar({
                height: 'parent',
                header: {
                    left: 'prev,next today',
                    //center: 'title',
                    right: 'myCustomButtonPrev,myCustomButtonNext, agendaDay,agendaFourDay,month,listWeek precustombuttons'
                },
                views: {
                    agendaFourDay: {
                        titleRangeSeparator: ' - ',
                        type: 'agenda',
                        duration: { days: 7 },
                        buttonText: 'Week',
                        eventLimit: 1
                    },
                    listWeek: {
                        titleRangeSeparator: ' - '
                    },
                    agendaDay: {
                        eventLimit: 1
                    }
                },
                defaultView: DefaultView,
                allDaySlot: true,
                editable: true,
                droppable: true,
                isUserCreated: true,
                eventLimit: true,
                maxTime: maxtime,
                minTime: mintime,
                resources: myResource,
                events: myEvent,
                //dragScroll: true,
                dayClick: function (start, jsEvent, view, resourceObj) {
                    LoadDate = moment(start).format();
                    if (view.name == "month") {
                        viewname = "Daily";
                        $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + LoadDate + "&typeval=" + encodeURIComponent(typeval));
                        //$(".fc-precustombuttons-button").css("display", "block");
                        //$(".fc-custombuttons-button").css("display", "block");
                    }
                    else {
                        UserPermissionForCreateTicket(resourceObj.id, LoadDate, ticketCustomerId, TicketId);
                    }
                },
                eventClick: function (calEvent, jsEvent, view) {
                    if (calEvent.TypeEvent == "WorkOrder") {
                        if (calEvent.EventPermission == null) {
                            OpenTopToBottomModal(domainurl + "/WorkOrder/TopToBottomWorkOrder/?AppointmentId=" + calEvent.Appointmentid + "&CustomerId=" + calEvent.Customerid);
                        }
                        if (calEvent.EventPermission == "Technician") {
                            OpenTopToBottomModal(domainurl + "/Customer/LeadTechCallPartial/?id=" + calEvent.Leadid);
                        }
                    }
                    if (calEvent.TypeEvent == "ServiceOrder") {
                        OpenTopToBottomModal(domainurl + "/ServiceOrder/TopToBottomModalServiceOrder/?AppointmentId=" + calEvent.Appointmentid + "&CustomerId=" + calEvent.Customerid);
                    }
                    if (calEvent.TypeEvent == "QA1") {
                        OpenTopToBottomModal(domainurl + "/Customer/QA1QuestionariePartial/?id=" + calEvent.Leadid);
                    }
                    if (calEvent.TypeEvent == "QA2") {
                        OpenTopToBottomModal(domainurl + "/Customer/QA2QuestionariePartial/?id=" + calEvent.Leadid);
                    }
                    if (calEvent.TypeEvent == "Note") {
                        OpenRightToLeftModal(domainurl + "/Notes/AddNotes/?id=" + calEvent.leadId + "&customerid=" + calEvent.appid);
                    }
                    if (calEvent.TypeEvent == "Reminder") {
                        if (calEvent.EventIsLead == "False") {
                            OpenRightToLeftModal(domainurl + "/Customer/AddNewReminderNote/?id=" + calEvent.Leadid + "&CustomerId=" + calEvent.Customerid + "&Timeval=" + strTime + "&IsComplete=" + "false");
                        }
                        else {
                            OpenRightToLeftModal(domainurl + "/Leads/AddNewReminderNote/?id=" + calEvent.Leadid + "&CustomerId=" + calEvent.Customerid + "&Timeval=" + strTime + "&IsComplete=" + "false");
                        }
                    }
                    else {
                        if (view.name == "month") {
                            viewname = "Daily";
                            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
                            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + calEvent.EventUserId + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + calEvent.EventUserDate + "&typeval=" + encodeURIComponent(calEvent.TypeEvent) + "&TicketId=" + calEvent.ticketid);
                            //$(".fc-precustombuttons-button").css("display", "block");
                            //$(".fc-custombuttons-button").css("display", "block");
                            valtype = calEvent.TypeEvent;
                            valUser = calEvent.EventUserId;
                        }
                        else {
                            OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?Id=" + calEvent.Leadid);
                        }
                    }
                    if (calEvent.TypeEvent == "Schedule") {
                        OpenRightToLeftModal(domainurl + "/TechSchedule/AddTechSchedule/?id=" + calEvent.Leadid + "&customerid=" + calEvent.Customerid + "&IsSchedule=" + "true");
                    }
                },
                eventDrop: function (event, delta, minuteDelta) {
                    var view = myCalendar.fullCalendar('getView');
                    moment = myCalendar.fullCalendar('getDate');
                    if (view.name == "month") {
                        EventResizeHandler(event.TypeEvent, event.Leadid, event.start, event.Appointmentid, event.allDay, event.end, event.resourceId, "", view.name, event.EventUserId, event.ticketid, event.additionalmem, true, true);
                    }
                    else {
                        EventDropHandler(event.TypeEvent, event.Leadid, event.start.format(), event.Appointmentid, event.allDay, event.end, event.resourceId, "", view.name, event.EventUserId, event.ticketid, event.additionalmem);
                    }
                    $(".popover").remove();
                },
                eventResize: function (event, delta, minuteDelta) {
                    var view = myCalendar.fullCalendar('getView');
                    moment = myCalendar.fullCalendar('getDate');
                    OnlyEventResizeHandler(event.TypeEvent, event.Leadid, event.start, event.Appointmentid, event.allDay, event.end.format(), event.resourceId, "", view.name, event.EventUserId, event.ticketid, event.additionalmem);
                    $(".popover").remove();
                },
                displayEventTime: false,
                eventOverlap: true,
                eventRender: function (event, element, view) {
                    $(".popover").remove();
                    if (view.name == "month") {
                        element.popover({
                            html: 'true',
                            container: 'body',
                            animation: false,
                            delay: 300,
                            content: function () {
                                setTimeout(function () {
                                    EventDetails(event.ticketid, event.TypeEvent, event.EventUserDate, event.EventResource, event.TypeDisplayEvent);
                                }, 1500);
                                return "<div class='event_hover_container'></div>";
                            },
                            placement: 'right',
                            trigger: 'manual'
                        }).on("mouseenter", function () {
                            var _this = this;
                            $(this).popover("show");
                            $(".popover").on("mouseleave", function () {
                                $(_this).popover('hide');
                            });
                        }).on("mouseleave", function () {
                            var _this = this;
                            setTimeout(function () {
                                if (!$(".popover:hover").length) {
                                    $(_this).popover("hide");
                                }
                            }, 300);
                        }).on("click", function () {
                            $('.popover').not(this).popover('hide');
                        });
                    }
                    else {
                        if (view.name == "agendaDay") {
                            element.popover({
                                html: 'true',
                                container: 'body',
                                animation: true,
                                delay: 300,
                                content: event.hovertitle,
                                placement: 'top',
                                trigger: 'hover'
                            });
                        }
                        else if (view.name == "agendaFourDay") {
                            element.popover({
                                html: 'true',
                                container: 'body',
                                animation: true,
                                delay: 300,
                                content: event.hovertitle,
                                placement: 'top',
                                trigger: 'hover'
                            });
                        }
                    }
                    for (var s = 0; s < TicketStatusArr.length; s++) {
                        if (typeof (TicketStatusArr[s].image) != "undefined" && TicketStatusArr[s].image != null && TicketStatusArr[s].image != "" && event.status == TicketStatusArr[s].status) {
                            element.find(".fc-title").prepend('<img src = "/TicketStatusImageShow/W20H20X' + TicketStatusArr[s].status + '"" style = "max-width:20px;height:20px;float:right;border-radius:50px;width:auto;" />');
                        }
                    }

                    if (CompareTicketServiceArr.length > 0) {
                        for (var comp = 0; comp < CompareTicketServiceArr.length; comp++) {
                            if (typeof (CompareTicketServiceArr[comp]) != "undefined" && CompareTicketServiceArr[comp] != null && CompareTicketServiceArr[comp] != "" && CompareTicketServiceArr[comp] == event.ticketid) {
                                if (event.status != "Lost" && event.status != "Pending") {
                                    element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                                    setInterval(function () {
                                        $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                        $(".fc-title i.fa.fa-bolt").fadeOut(100)
                                    }, 600);
                                }
                            }
                        }
                    }
                    if ((event.status != "Completed" && event.status != "Lost" && event.status != "Pending") && event.TypeEvent == "Drop%20Off") {
                        var dt = new Date(event.eventendtime);
                        dt = dt.setMinutes(dt.getMinutes() - 30);
                        var dt2 = new Date();
                        if (dt2.getTime() > dt) {
                            element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                            setInterval(function () {
                                $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                $(".fc-title i.fa.fa-bolt").fadeOut(100)
                            }, 600);
                        }
                    }
                    if ((event.status != "Completed" && event.status != "Lost" && event.status != "Pending") && event.TypeEvent == "Pick%20Up") {
                        var dt = new Date(event.eventendtime);
                        dt = dt.setMinutes(dt.getMinutes() - 30);
                        var dt2 = new Date();
                        if (dt2.getTime() > dt) {
                            element.find(".fc-title").prepend('<i class = "fa fa-bolt" style="float:right;font-size:30px;padding-left:5px;color:black;"></i>');
                            setInterval(function () {
                                $(".fc-title i.fa.fa-bolt").fadeIn(1000);
                                $(".fc-title i.fa.fa-bolt").fadeOut(100)
                            }, 600);
                        }
                    }
                    if (parseInt(event.rescheduleid) > 0) {
                        element.find(".fc-title").prepend('<img src = "/Content/img/reschedule_new.png" style = "max-width:20px;height:20px;float:right;border-radius:50px;width:auto;margin-right:25px;" />');
                    }
                },
                dayRender: function (date, cell, view) {
                    var cellYear = date.year();
                    var cellMonth = (date.month() + 1 < 10) ? '0' + (date.month() + 1) : (date.month() + 1);
                    var cellDay = (date.date() < 10) ? '0' + (date.date()) : (date.date());
                    var newDate = cellYear + "-" + cellMonth + "-" + cellDay;
                    if (newDate >= getDates() && view.name == "month") {
                        cell.append('<span><i class="fa fa-plus" aria-hidden="true" style="color:#ccc;"></i></span>');
                    }
                },
                customButtons: {
                    myCustomButtonNext: {
                        icon: 'right-double-arrow',
                        click: function () {
                            event.preventDefault();
                            $(".fc-view-container .fc-view").animate({
                                scrollLeft: "+=300px"
                            }, "slow");
                        },
                        title: 'Scroll to right'
                    },
                    myCustomButtonPrev: {
                        icon: 'left-double-arrow',
                        click: function () {
                            event.preventDefault();
                            $(".fc-view-container .fc-view").animate({
                                scrollLeft: "-=300px"
                            }, "slow");
                        },
                        title: 'Scroll to right'
                    }
                },
            });
        }
    }
    
    
    if (viewstartdate != null && viewstartdate != "") {
        $('#ScheduleCalender').fullCalendar('gotoDate', viewstartdate);
    }
    $(".fc-agendaDay-button").click(function () {
        console.log("agenda");
        view = myCalendar.fullCalendar('getView');
        moment = myCalendar.fullCalendar('getDate');
        if (view.name == "agendaDay") {
            viewname = "Daily";
        }

        $(".AppointmentScheduleCalendar").html(TabsLoaderText);
        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + moment.format() + "&typeval=" + encodeURIComponent(typeval));
    })
    $(".fc-month-button").click(function () {
        view = myCalendar.fullCalendar('getView');
        moment = myCalendar.fullCalendar('getDate');
        $(".AppointmentScheduleCalendar").html(TabsLoaderText);
        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + "Monthly" + "&viewstartdate=" + moment.format() + "&typeval=" + encodeURIComponent(typeval));
    })
    $(".fc-listWeek-button").click(function () {
        view = myCalendar.fullCalendar('getView');
        moment = myCalendar.fullCalendar('getDate');
        $(".AppointmentScheduleCalendar").html(TabsLoaderText);
        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + "List" + "&viewstartdate=" + moment.format() + "&typeval=" + encodeURIComponent(typeval));
    })
    $(".fc-agendaFourDay-button").click(function () {
        view = myCalendar.fullCalendar('getView');
        moment = myCalendar.fullCalendar('getDate');
        $(".AppointmentScheduleCalendar").html(TabsLoaderText);
        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + "Weekly" + "&viewstartdate=" + moment.format() + "&typeval=" + encodeURIComponent(typeval));

    })
    $(".fc-prev-button").click(function () {
        var moment = myCalendar.fullCalendar('getDate');
        view = myCalendar.fullCalendar('getView');
        var splitLoadDate = moment.format().split('T');
        if (view.name == "agendaDay") {
            viewname = "Daily";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + splitLoadDate[0] + "&typeval=" + encodeURIComponent(typeval));
            
        }
        if (view.name == "agendaFourDay") {
            viewname = "Weekly";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + splitLoadDate[0] + "&typeval=" + encodeURIComponent(typeval));
        }
        if (view.name == "month") {
            viewname = "Monthly";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + splitLoadDate[0] + "&typeval=" + encodeURIComponent(typeval));
        }
        if (view.name == "listWeek") {
            viewname = "List";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + splitLoadDate[0] + "&typeval=" + encodeURIComponent(typeval));
        }
    })
    $(".fc-next-button").click(function () {
        var moment = myCalendar.fullCalendar('getDate');
        view = myCalendar.fullCalendar('getView');
        var splitLoadDate = moment.format().split('T');
        if (view.name == "agendaDay") {
            viewname = "Daily";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + splitLoadDate[0] + "&typeval=" + encodeURIComponent(typeval));
            
        }
        if (view.name == "agendaFourDay") {
            viewname = "Weekly";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + splitLoadDate[0] + "&typeval=" + encodeURIComponent(typeval));
        }
        if (view.name == "month") {
            viewname = "Monthly";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + splitLoadDate[0] + "&typeval=" + encodeURIComponent(typeval));
        }
        if (view.name == "listWeek") {
            viewname = "List";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + splitLoadDate[0] + "&typeval=" + encodeURIComponent(typeval));
        }

    })
    $(".fc-today-button").click(function () {
        var moment = myCalendar.fullCalendar('getDate');
        view = myCalendar.fullCalendar('getView');
        if (view.name == "agendaDay") {
            viewname = "Daily";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=&typeval=" + encodeURIComponent(typeval));
            
        }
        if (view.name == "agendaFourDay") {
            viewname = "Weekly";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=&typeval=" + encodeURIComponent(typeval));
        }
        if (view.name == "month") {
            viewname = "Monthly";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=&typeval=" + encodeURIComponent(typeval));
        }
        if (view.name == "listWeek") {
            viewname = "List";
            $(".AppointmentScheduleCalendar").html(TabsLoaderText);
            $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + UserVal + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=&typeval=" + encodeURIComponent(typeval));
        }

    })
    $("#cal_search_btn").click(function () {
        var value = $("#ListEmployee").val();
        var view = myCalendar.fullCalendar('getView');
        var moment = myCalendar.fullCalendar('getDate');
        var typeval = encodeURIComponent($("#ListTicketType").val());
        if (view.name == "agendaDay") {
            viewname = "Daily";
        }
        if (view.name == "agendaFourDay") {
            viewname = "Weekly";
        }
        if (view.name == "month") {
            viewname = "Monthly";
        }
        if (view.name == "listWeek") {
            viewname = "List";
        }
        var IsSelected = $("#IsAllTechnicianList").val();
        $(".AppointmentScheduleCalendar").html(TabsLoaderText);
        $(".AppointmentScheduleCalendar").load(domainurl + "/Schedule/ScheduleCalendar?parent=null" + "&UserVal=" + value + "&pageno=" + 1 + "&ReminderResource=" + ReminderResource + "&pageno2=" + pageno1 + "&viewname=" + viewname + "&viewstartdate=" + moment.format() + "&typeval=" + typeval + "&FilterWithSearch=true&SelectedEmpOnly=" + IsSelected);
        
    })
    var viewdefault = myCalendar.fullCalendar('getView');
    if (IsMobileDevice == "False") {
        if (viewdefault.name == "agendaDay") {
            LoadDayViewCalendarContainer();
        }
        else if (viewdefault.name == "agendaFourDay") {
            LoadWeekViewCalendarContainer();
        }
        else {
            $(".fc-scroller").attr("style", "overflow-y: hidden !important; height: 100% !important;");
        }
    }
    else {
        if (viewdefault.name == "agendaDay") {
            LoadDayMobileViewCalendarContainer();
        }
        else if (viewdefault.name == "agendaFourDay") {
            LoadWeekMobileViewCalendarContainer();
        }
        else {
            $(".fc-scroller").attr("style", "overflow-y: hidden !important; height: 100% !important;");
        }
    }
}
function getDates() {
    var date = new Date();
    var cellYear = date.getFullYear();
    var cellMonth = (date.getMonth() + 1 < 10) ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1);
    var cellDay = (date.getDate() < 10) ? '0' + (date.getDate()) : (date.getDate());
    var newDate = cellYear + "-" + cellMonth + "-" + cellDay;
    return newDate;
}
var LoadDayViewCalendarContainer = function () {
    $(".fc-view-container").attr("style", "width: auto !important;");
    $(".fc-view-container .fc-view").attr("style", "overflow-x: scroll !important;");
    
    eventlength = $(".fc-content-skeleton table tbody tr td .fc-event-container a").length;
    userlength = $(".fc-resource-cell").length;
    var eventcontainer = $(".fc-content-skeleton table tbody tr td .fc-event-container");
    var usercontainer = $(".fc-resource-cell");
    if (userlength > 5) {
        if (eventlength == 0) {
            containersize = userlength * 20;
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "% !important;");
        }
        else if (eventlength == 1) {
            containersize = ((eventlength * 200) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength == 2) {
            containersize = ((eventlength * 150) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength == 3) {
            containersize = ((eventlength * 100) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength > 3 && eventlength <10) {
            containersize = ((eventlength * 50) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else {
            containersize = ((eventlength * 20) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
    }
    else {
        containersize = 100;
        $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "% !important;");
    }
    $(".fc-scroller").attr("style", "overflow-y: scroll !important; height: 380px !important;");
    $(".fc-scroller").scrollTop(0);
}
var LoadDayMobileViewCalendarContainer = function () {
    $(".fc-view-container").attr("style", "width: auto !important;");
    $(".fc-view-container .fc-view").attr("style", "overflow-x: scroll !important;");

    eventlength = $(".fc-content-skeleton table tbody tr td .fc-event-container a").length;
    userlength = $(".fc-resource-cell").length;
    var eventcontainer = $(".fc-content-skeleton table tbody tr td .fc-event-container");
    var usercontainer = $(".fc-resource-cell");
    if (userlength > 4) {
        if (eventlength == 0) {
            containersize = userlength * 30;
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "% !important;");
        }
        else if (eventlength == 1) {
            containersize = ((eventlength * 200) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength == 2) {
            containersize = ((eventlength * 150) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength == 3) {
            containersize = ((eventlength * 100) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength > 3 && eventlength < 10) {
            containersize = ((eventlength * 50) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else {
            containersize = ((eventlength * 20) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
    }
    else {
        containersize = 100;
        $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "% !important;");
    }
    $(".fc-scroller").attr("style", "overflow-y: scroll !important;");
    $(".fc-scroller").scrollTop(0);
}
var LoadWeekViewCalendarContainer = function () {
    $(".fc-view-container").attr("style", "width: auto !important;");
    $(".fc-view-container .fc-view").attr("style", "overflow-x: scroll !important;");
    eventlength = $(".fc-content-skeleton table tbody tr td .fc-event-container a").length;
    userlength = $(".fc-resource-cell").length;
    if (userlength > 1) {
        if (eventlength == 0) {
            containersize = userlength * 20;
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "% !important;");
        }
        else if (eventlength == 1) {
            containersize = ((eventlength * 200) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength == 2) {
            containersize = ((eventlength * 150) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength == 3) {
            containersize = ((eventlength * 100) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength > 3 && eventlength < 10) {
            containersize = ((eventlength * 50) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else {
            containersize = ((eventlength * 20) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
    }
    else {
        containersize = 100;
        $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "% !important;");
    }
    $(".fc-scroller").attr("style", "overflow-y: scroll !important; height: 360px !important;");
    $(".fc-scroller").scrollTop(0);
}
var LoadWeekMobileViewCalendarContainer = function () {
    $(".fc-view-container").attr("style", "width: 100% !important;");
    $(".fc-view-container .fc-view").attr("style", "overflow-x: scroll !important;");
    eventlength = $(".fc-content-skeleton table tbody tr td .fc-event-container a").length;
    userlength = $(".fc-resource-cell").length;
    if (userlength > 1) {
        if (eventlength == 0) {
            containersize = userlength * 30;
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "% !important;");
        }
        else if (eventlength == 1) {
            containersize = ((eventlength * 200) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength == 2) {
            containersize = ((eventlength * 150) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength == 3) {
            containersize = ((eventlength * 100) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else if (eventlength > 3 && eventlength < 10) {
            containersize = ((eventlength * 50) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
        else {
            containersize = ((eventlength * 20) * userlength);
            $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "px !important;");
        }
    }
    else {
        containersize = 100;
        $(".fc-view-container .fc-view > table").attr("style", "width: " + containersize + "% !important;");
    }
    $(".fc-scroller").attr("style", "overflow-y: scroll !important;");
    $(".fc-scroller").scrollTop(0);
}

$(document).ready(function () {
    
    if (defview == "Weekly") {
        DefaultView = "agendaFourDay";
    }
    else if (defview == "Monthly") {
        DefaultView = "month";
    }
    else if (defview == "Daily") {
        DefaultView = "agendaDay";
    }
    else if (defview == "List") {
        DefaultView = "listWeek";
    }
    
    Calander(null);
    $('.popover').not(this).popover('hide');
    $(".popover").remove();

    
})

var daydiffrentborder = function (data, View) {
    var nodelist = document.getElementsByTagName("style").length;
    if (nodelist > 0) {
        $("style").remove();
    }
    if (View == "Weekly") {
    var usercount = data.length;
    var number = 1 + usercount;
    var style = document.createElement('style');
    document.head.appendChild(style);   
        for (var i = 0; i < 7; i++) {
            var classname = ".fc-unthemed td.fc-widget-content:nth-child(" + number + ")";          
            style.sheet.insertRule(classname + "{border-right: 2px solid black !important}");            
            number += usercount;
        }    
    }
}