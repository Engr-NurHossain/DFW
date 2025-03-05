var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var TicketResizeing = function (ev) {
    if ($("#EventMovePermission").val() == 'True' || $("#IsSupervisorPermission").val() == 'True') {
        $("#CalendarResizing").val("yes");
        var ResizingId = ev.target.id,
            handle = $('#' + ResizingId),
            FinalWidth = 0,
            parentDiv = handle.parent(),
            divPosition = parentDiv.position(),
            divwidth = parentDiv.width(),
            divheight = parentDiv.height(),
            x = ev.clientX,
            y = ev.clientY,
            setWidth = $("#EventTicketResize").val(),
            CalView = $("#CalendarViewPostion").val(),
            setHeight = parseInt($("#CalendarHeight").val()),
            divideValue = 0;
        handle.parent();
        if (CalView == 'vertical') {
            divideValue = setHeight;//25;
            if (setWidth == '200') { setWidth = '150'; }
            else if (setWidth == '160') { setWidth = '120'; }
            else if (setWidth == '120') { setWidth = '100'; }
            else if (setWidth == '80') { setWidth = '70' }
        }
        else {
            divideValue = 50;
            if (setWidth == '200') { divideValue = 50; }
            else if (setWidth == '160') { divideValue = 40; }
            else if (setWidth == '120') { divideValue = 30; }
            else if (setWidth == '80') { divideValue = 20; }
        }
        var ticketid = ev.target.dataset.ticketid,
            userid = ev.target.dataset.userid,
            empid = ev.target.dataset.empid,
            tickettype = ev.target.dataset.tickettype,
            appdate = ev.target.dataset.appdate,
            appointmentid = ev.target.dataset.appointmentid,
            appstarttime = ev.target.dataset.appstarttime,
            appendtime = ev.target.dataset.appendtime,
            additional = ev.target.dataset.additional;

        $(document).on('mousemove.ticketsizing', function (e) {
            if (CalView == 'vertical') {
                var my = e.clientY;
                FinalWidth = my - y;
                parentDiv.css({
                    'bottom': (0 - FinalWidth) + 'px',
                    'width': setWidth + 'px',
                    'height': (divheight + FinalWidth) + 'px'
                });
            }
            else {
                var mx = e.clientX;
                FinalWidth = mx - x;
                parentDiv.css({
                    'width': (divwidth + FinalWidth) + 'px'
                });
            }

        }).on('mouseup.ticketsizing', function (e) {
            $(document).off('.ticketsizing');
            e.stopPropagation();
            e.cancelBubble = true;
            var starttime = appstarttime.split(' ')[0];
            if (appstarttime.split(' ')[1] == 'PM') {
                var starthour = parseInt(starttime.split(':')[0]);
                var startminute = starttime.split(':')[1];
                if (starthour != 12) {
                    starthour = 12 + starthour;
                }
                starttime = starthour + ':' + startminute + ':00';
            }
            else {
                starttime = starttime + ':00';
            }

            var startDateTime = appdate + 'T' + starttime,
                AppTicketStartDateTime = new Date(appdate + " " + appstarttime);
            var mydate = appdate + " " + appendtime;
            var endtime = new Date(mydate);
            var minutesCount = '0.00',
                minutes = 0,
                flag = false;

            if (FinalWidth < 0) {
                minutesCount = parseFloat(((FinalWidth * -1) / divideValue).toFixed(2));
            }
            else {
                minutesCount = parseFloat((FinalWidth / divideValue).toFixed(2));
                flag = true;
            }
            var Totalminutes = parseInt(String(minutesCount).split('.')[0])
            var pointvalue = parseInt(String(minutesCount).split('.')[1]);
            if (isNaN(pointvalue)) { pointvalue = 0; }
            if (pointvalue < divideValue) {
                minutes = parseInt(15 * Totalminutes);
            }
            else {
                minutes = parseInt(15 * (Totalminutes + 1));
            }
            if (flag) {
                endtime.setMinutes(endtime.getMinutes() + minutes);
            }
            else {
                endtime.setMinutes(endtime.getMinutes() - minutes);
            }

            var endhours = endtime.getHours();
            var endminutes = endtime.getMinutes();
            endhours = endhours < 10 ? '0' + endhours : endhours;
            endminutes = endminutes < 10 ? '0' + endminutes : endminutes;
            var eTime = endhours + ':' + endminutes + ':00';
            var AppTicketEndDateTime = new Date(appdate + " " + eTime);
            if (AppTicketStartDateTime >= AppTicketEndDateTime) {
                AppTicketStartDateTime.setMinutes(AppTicketStartDateTime.getMinutes() + 15);
                endhours = AppTicketStartDateTime.getHours();
                endminutes = AppTicketStartDateTime.getMinutes();
                endhours = endhours < 10 ? '0' + endhours : endhours;
                endminutes = endminutes < 10 ? '0' + endminutes : endminutes;
                eTime = endhours + ':' + endminutes + ':00';
            }
            var endDateTime = appdate + 'T' + eTime;

            $(".LoaderWorkingDiv").show();
            if (additional == "Additional") {
                CalendarEventResizeHandler(tickettype, ticketid, startDateTime, appointmentid, false, endDateTime, empid, "", "agendaDay", userid, appointmentid, additional, null, null, true);
            }
            else {
                CalendarEventResizeHandler(tickettype, ticketid, startDateTime, appointmentid, false, endDateTime, empid, "", "agendaDay", userid, appointmentid, "", true, true, true);
            }
        });
    }
    ev.stopPropagation();
    ev.cancelBubble = true;
}
var CalendarSearchFilter = function () {
    var pd = $('#eventDate').val();
    $('#eventDate').val(pd);
    var node = document.getElementById("tablebing");
    node.innerHTML = "";
    var value = $("#ListEmployee").val();
    var srctxt = encodeURI($("#calendarSearchId").val());
    var typeval = $("#ListTicketType").val();
    var skill = $("#EmployeeSkills").val();
    var viewname = $("#dailyShedule").val();
    $(".LoaderWorkingDiv").show();
    GetAllCalendarData(pd, viewname, typeval, value, skill, srctxt);
}
var CalendarEventDragStart = function (tdId, ev) {
    if ($("#EventMovePermission").val() == 'True' || $("#IsSupervisorPermission").val() == 'True') {
        ev.dataTransfer.setData("srcid", ev.target.id);
        ev.dataTransfer.setData("etype", ev.target.dataset.etype);
        ev.dataTransfer.setData("tid", ev.target.dataset.tid);
        ev.dataTransfer.setData("edate", ev.target.dataset.edate);
        ev.dataTransfer.setData("eguid", ev.target.dataset.eguid);
        ev.dataTransfer.setData("stime", ev.target.dataset.stime);
        ev.dataTransfer.setData("etime", ev.target.dataset.etime);
        ev.dataTransfer.setData("aid", ev.target.dataset.aid);
        ev.dataTransfer.setData("duration", ev.target.dataset.duration);
        ev.dataTransfer.setData("additional", ev.target.dataset.additional);
    }

}
var CalendarEventAllowDrop = function (id, ev) {
    ev.preventDefault();
}
var CalendarEventDropOff = function drop(tdId, ev) {
    if ($("#EventMovePermission").val() == 'True' || $("#IsSupervisorPermission").val() == 'True') {
        var resizing = $("#CalendarResizing").val();
        if (resizing != "yes") {
            ev.preventDefault();
            var setWidth = $("#EventTicketResize").val();
            var CalView = $("#CalendarViewPostion").val();
            var setHeight = parseInt($("#CalendarHeight").val());
            var target = ev.target;
            var userid = ev.target.id;

            var startMin = '00';
            if (CalView == 'vertical') {
                var divHeight25 = setHeight / 4, divHeight50 = divHeight25 * 2, divHeight75 = divHeight25 * 3, divHeight100 = setHeight; 
                var dropFrom = ev.offsetY;
                if (dropFrom < divHeight50 && dropFrom > (divHeight25 - 1)) { startMin = '15'; }
                else if (dropFrom < divHeight75 && dropFrom > (divHeight50 - 1)) { startMin = '30'; }
                else if (dropFrom < divHeight100 && dropFrom > (divHeight75 - 1)) { startMin = '45'; }
            }
            else {
                var dropFrom = ev.offsetX;
                if (setWidth == '200') {
                    if (dropFrom < 100 && dropFrom > 49) { startMin = '15'; }
                    else if (dropFrom < 150 && dropFrom > 99) { startMin = '30'; }
                    else if (dropFrom < 200 && dropFrom > 149) { startMin = '45'; }
                }
                else if (setWidth == '160') {
                    if (dropFrom < 80 && dropFrom > 39) { startMin = '15'; }
                    else if (dropFrom < 120 && dropFrom > 79) { startMin = '30'; }
                    else if (dropFrom < 160 && dropFrom > 119) { startMin = '45'; }
                }
                else if (setWidth == '120') {
                    if (dropFrom < 60 && dropFrom > 29) { startMin = '15'; }
                    else if (dropFrom < 90 && dropFrom > 59) { startMin = '30'; }
                    else if (dropFrom < 120 && dropFrom > 89) { startMin = '45'; }
                }
                else if (setWidth == '80') {
                    if (dropFrom < 40 && dropFrom > 19) { startMin = '15'; }
                    else if (dropFrom < 60 && dropFrom > 39) { startMin = '30'; }
                    else if (dropFrom < 80 && dropFrom > 59) { startMin = '45'; }
                }
            }

            var mydate, formDate, st, sTime, allday = false;
            if (userid.toString().split('_')[2] == "full") { allday = true; }
            var srcId = ev.dataTransfer.getData("srcid");
            var eType = ev.dataTransfer.getData("etype");
            var st = ev.dataTransfer.getData("stime");
            var eDate = ev.dataTransfer.getData("edate");
            var additional = ev.dataTransfer.getData("additional");
            var tId = parseInt(ev.dataTransfer.getData("tid"));
            var eId = parseInt(ev.currentTarget.dataset.empintid);
            var eGuid = ev.dataTransfer.getData("eguid");
            var et = ev.dataTransfer.getData("etime");
            var aid = ev.dataTransfer.getData("aid");
            var du = parseInt(ev.dataTransfer.getData("duration"));
            if (srcId.toString().split('_')[2] == "full") {
                st = ev.currentTarget.dataset.mytime;
                var sttime = st.toString().split(':')[0];
                var tt = st.toString().split(' ')[1];
                sTime = sttime + ':' + startMin + ' ' + tt;
                var mydate = eDate + " " + sTime;
                var std = new Date(mydate);
                var Shours = std.getHours();
                var Sminutes = std.getMinutes();
                Shours = Shours < 10 ? '0' + Shours : Shours;
                Sminutes = Sminutes < 10 ? '0' + Sminutes : Sminutes;
                mydate = eDate + "T" + Shours + ':' + Sminutes + ':00';
                formDate = null;
                if (allday == true) {
                    mydate = eDate;
                }
            }
            else {
                st = ev.currentTarget.dataset.mytime;
                if (allday == true || st == "FullDay") {
                    mydate = eDate;
                    formDate = null;
                    allday = true;
                }
                else {
                    
                    var sttime = st.toString().split(':')[0];
                    var tt = st.toString().split(' ')[1];
                    sTime = sttime + ':' + startMin + ' ' + tt;
                    var mydate = eDate + " " + sTime;
                    var std = new Date(mydate);
                    var Shours = std.getHours();
                    var Sminutes = std.getMinutes();
                    Shours = Shours < 10 ? '0' + Shours : Shours;
                    Sminutes = Sminutes < 10 ? '0' + Sminutes : Sminutes;
                    mydate = eDate + "T" + Shours + ':' + Sminutes + ':00';
                    var formDate = new Date(std);
                    formDate.setMinutes(formDate.getMinutes() + (15 * du));
                    formDate = new Date(formDate);
                    var hours = formDate.getHours();
                    var minutes = formDate.getMinutes();
                    hours = hours < 10 ? '0' + hours : hours;
                    minutes = minutes < 10 ? '0' + minutes : minutes;
                    var eTime = hours + ':' + minutes + ':00';
                    formDate = eDate + "T" + eTime;
                }
            }

            var ViewName = $("#dailyShedule").val();
            if (ViewName == "Daily") { ViewName = "agendaDay"; }
            else if (ViewName == "Weekly") { ViewName = "agendaFourDay"; }
            else if (ViewName == "Monthly") { ViewName = "month"; }
            else if (ViewName == "List") { ViewName = "listWeek"; }
            else { ViewName = "agendaDay"; }

            if (userid != '' && userid != srcId) {
                target.appendChild(document.getElementById(srcId));
            }
            $(".LoaderWorkingDiv").show();
            CalendarEventDropHandler(eType, tId, mydate, aid, allday, formDate, eId, "", ViewName, eGuid, aid, additional);
        }
    }
}
var CalendarEventDropHandler = function (Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional) {
    var url = domainurl + "/Calendar/DroppingPermissionScheduleCalendar";
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
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.ExistUserAssign) {
                CalendarEventResizeHandler(Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, true, true, false);
            }
            else if (data.ExistUserAdditional) {
                CalendarEventResizeHandler(Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, false, true, false);
            }
            else {
                if (data.result) {
                    if (typeof (additional) != "undefined" && additional != null && additional != "" && additional == "Additional") {
                        CalendarEventResizeHandler(Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, false, true, false);
                    }
                    else {
                        CalendarEventResizeHandler(Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, true, true, false);
                    }
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var CalendarEventResizeHandler = function (Type, Id, Date, appid, allday, endDate, Eventresid, dropid, ViewName, CustomId, eventticketid, additional, chkassign, isexist, isresizeing) {
    var url = "";
    if (isresizeing) {
        url = domainurl + "/Calendar/OnlyEventResizeHandlerCalendar";
    }
    else {
        url = domainurl + "/Calendar/DraggingScheduleCalendar";
    }
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
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            var viewname = $("#dailyShedule").val();
            var value = $("#ListEmployee").val();
            var skill = $("#EmployeeSkills").val();
            var pd = $('#eventDate').val();
            GetAllCalendarData(pd, viewname, typeval, value, skill);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
//var GetAllCalendarData = function (sd, viewname, typeval, value, skill, srctxt) {
//    var ScheduleCalendarMinTimeRange = $("#ScheduleCalendarMinTimeRange").val();
//    var ScheduleCalendarMaxTimeRange = $("#ScheduleCalendarMaxTimeRange").val();
//    var FirstDayOfWeek = $("#FirstDayOfWeek").val();
//    var CustomCalendarTopRowEmployee = $("#CustomCalendarTopRowEmployee").val();
//    var holiday = $("#CustomCalendarColumnHourDuration").val();
//    var IsAll = $("#IsAllTechnicianList").val();
//    console.log(IsAll);
//    if (IsAll == 'all') {
//        value = 'all';
//    }
//    else if (IsAll == 'none') {
//        value = 'none';
//    }
//    $.ajax({
//        type: "POST",
//        url: domainurl + "/Calendar/AllScheduleCalendar",
//        data: JSON.stringify({
//            Default: value,
//            startdate: sd,
//            defaultView: viewname,
//            typeval: typeval,
//            skills: skill,
//            CalendarStartTime: ScheduleCalendarMinTimeRange,
//            CalendarEndTime: ScheduleCalendarMaxTimeRange,
//            FirstDayOfWeek: FirstDayOfWeek,
//            TopRowEmployee: CustomCalendarTopRowEmployee,
//            HolidayCount: holiday,
//            SearchText: srctxt
//        }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            console.log("Calendar obayed", result);
//            if (result.Active.IsActive) {
//                var act = 'Inactive <span class="ticket-count-circle-green">' + result.Active.TotalCount+'</span>';
//                $(".toggle-off").empty();
//                $(".toggle-off").append(act);
//            }
//            else {
//                var act = 'Active <span class="ticket-count-circle-red">' + result.Active.TotalCount + '</span>';
//                $(".toggle-on").empty();
//                $(".toggle-on").append(act);
//            }
//            if (IsAllShow.toLowerCase() == 'true') {
//                $(".LoadSchedule").load(domainurl + "/Calendar/SchedulePartial?CurrentDate=" + sd + "&CurrentView=" + viewname + "&UserVal=" + value + "&typeval=" + typeval + "&status=" + IsAll);
//            }
//            $("#CalendarResizing").val("");
//            $(".LoaderWorkingDiv").hide();
//            $('#titleDate').html(result.DateTitle);
//            var EmpList = $("#ListEmployee").val();
//            var CalView = $("#CalendarViewPostion").val();
//            if (result.View == "Daily") {
//                if (CalView == 'vertical') {
//                    var abc = VerticalTableViewCreate(result.Datalist.EmpList, result.Datalist.Timelist, result.View, result.Datalist.SystemUser);
//                    if (abc == true) {
//                        VerticalCalendarDataBind(result.Datalist.AppList, result.View, result.Datalist.SystemUser, result.Datalist.EmpList.length);
//                        document.getElementById("current-bar").style.marginTop = pixle + "px";
//                        if (pixle > 30) {
//                            if (Scheduler != null && Scheduler != '' && Scheduler != 'undefined') {
//                                clearInterval(Scheduler);
//                            }


//                            Scheduler = setInterval(function () {
//                                var currentvalue = parseFloat($("#CurrentBar").val());
//                                var totalPixle = parseFloat($("#TotalPix").val());
//                                if (!isNaN(currentvalue) && !isNaN(totalPixle)) {
//                                    pixle = currentvalue + marginTop;
//                                    if (totalPixle >= pixle) {
//                                        document.getElementById("current-bar").style.marginTop = pixle + "px";
//                                        $("#CurrentBar").val(pixle);
//                                    }
//                                }
//                            }, 1000 * 60);
//                        }
//                    }
//                }
//                else {
//                    var abc = TableViewCreate(result.Datalist.EmpList, result.Datalist.Timelist, result.View, result.Datalist.SystemUser);
//                    if (abc == true) {
//                        CalendarDataBind(result.Datalist.AppList, result.View, result.Datalist.SystemUser);
//                    }
//                }


//            }
//            else if (result.View == "Weekly") {
//                var abc = TableViewCreate(result.Datalist.EmpList, result.Datalist.Timelist, result.View, null);
//                if (abc == true) {
//                    CalendarDataBind(result.Datalist, result.View);
//                }
//            }
//            else if (result.View == "Monthly") {
//                var abc = TableViewCreate(null, result.Datalist.DayForCalendar, result.View, null);
//                if (abc == true) {
//                    CalendarDataBind(result.Datalist, result.View);
//                }
//            }

//        },
//        error: function (response) {
//            console.log(response.responseText);
//        }
//    });
//}
// Table show time in top and employee in left
var GetAllCalendarData = function (sd, viewname, typeval, value, skill, srctxt) {
    var ScheduleCalendarMinTimeRange = $("#ScheduleCalendarMinTimeRange").val();
    var ScheduleCalendarMaxTimeRange = $("#ScheduleCalendarMaxTimeRange").val();
    var FirstDayOfWeek = $("#FirstDayOfWeek").val();
    var CustomCalendarTopRowEmployee = $("#CustomCalendarTopRowEmployee").val();
    var holiday = $("#CustomCalendarColumnHourDuration").val();
    var IsAll = $("#IsAllTechnicianList").val();
    if (IsAll == 'all') {
        value = 'all';
    }
    else if (IsAll == 'none') {
        value = 'none';
    }
    $.ajax({
        type: "POST",
        url: domainurl + "/Calendar/AllScheduleCalendar",
        data: JSON.stringify({
            Default: value,
            startdate: sd,
            defaultView: viewname,
            typeval: typeval,
            skills: skill,
            CalendarStartTime: ScheduleCalendarMinTimeRange,
            CalendarEndTime: ScheduleCalendarMaxTimeRange,
            FirstDayOfWeek: FirstDayOfWeek,
            TopRowEmployee: CustomCalendarTopRowEmployee,
            HolidayCount: holiday,
            SearchText: srctxt
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log("Test 01", result);
            if (IsAllShow.toLowerCase() == 'true') {
                $(".LoadSchedule").load(domainurl + "/Calendar/SchedulePartial?CurrentDate=" + sd + "&CurrentView=" + viewname + "&UserVal=" + value + "&typeval=" + typeval + "&status=" + IsAll + "&skills=" + skill);
            }
            if (result.Calstatus.length > 0) {
                for (var i = 0; i < result.Calstatus.length; i++) {
                    var htmlcount = '.html-' + result.Calstatus[i].Type;
                    if (result.Calstatus[i].CustomerBillID > 0) {
                        $(htmlcount).html('(' + result.Calstatus[i].CustomerBillID + ')');
                    }
                    else {
                        $(htmlcount).html('');
                    }
                }
            }
            $("#CalendarResizing").val("");
            $(".LoaderWorkingDiv").hide();
            $('#titleDate').html(result.DateTitle);
            var EmpList = $("#ListEmployee").val();
            var CalView = $("#CalendarViewPostion").val();
            if (result.View == "Daily") {
                if (CalView == 'vertical') {
                    var abc = VerticalTableViewCreate(result.Datalist.EmpList, result.Datalist.Timelist, result.View, result.Datalist.SystemUser);
                    if (abc == true) {
                        VerticalCalendarDataBind(result.Datalist.AppList, result.View, result.Datalist.SystemUser, result.Datalist.EmpList.length);

                        document.getElementById("current-bar").style.marginTop = pixle + "px";
                        if (pixle > 0) {
                            if (Scheduler != null && Scheduler != '' && Scheduler != 'undefined') {
                                clearInterval(Scheduler);
                            }


                            Scheduler = setInterval(function () {
                                var currentvalue = parseFloat($("#CurrentBar").val());
                                var totalPixle = parseFloat($("#TotalPix").val());
                                if (!isNaN(currentvalue) && !isNaN(totalPixle)) {
                                    pixle = currentvalue + marginTop;
                                    if (totalPixle >= pixle) {
                                        document.getElementById("current-bar").style.marginTop = pixle + "px";
                                        $("#CurrentBar").val(pixle);
                                    }
                                }
                            }, 1000 * 60);
                        }
                    }
                }
                else {
                    var abc = TableViewCreate(result.Datalist.EmpList, result.Datalist.Timelist, result.View, result.Datalist.SystemUser);
                    if (abc == true) {
                        CalendarDataBind(result.Datalist.AppList, result.View, result.Datalist.SystemUser);
                    }
                }


            }
            else if (result.View == "Weekly") {
                var abc = TableViewCreate(result.Datalist.EmpList, result.Datalist.Timelist, result.View, null);
                if (abc == true) {
                    CalendarDataBind(result.Datalist, result.View);
                }
            }
            else if (result.View == "Monthly") {
                var abc = TableViewCreate(null, result.Datalist.DayForCalendar, result.View, null);
                if (abc == true) {
                    CalendarDataBind(result.Datalist, result.View);
                }
            }

        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
}
var TableViewCreate = function (emp, time, view, user) {
    //daily Table
    console.log("Test 02",emp)
    var fontsize = $("#CustomCalendarFontSize").val();
    if (fontsize == null || fontsize == "") {
        fontsize = '10';
    }
    if (view == "Daily") {
        console.log("cac");
        var PositionView = $("#SysUserViewPostion").val();
        var html = '<table class="responstable my-daily"><thead><tr><th>Name</th><th>Full Day</th>';
        for (var i = 0; i < time.length; i++) {
            var hh = parseInt(time[i].TimeName.split(':')[0]);
            var mmtt = time[i].TimeName.split(':')[1];
            var mm = parseInt(mmtt.split(' ')[0]);
            var tt = time[i].TimeName.split(' ')[1];
            if (!isNaN(mm) && mm > 0) {
                html += '<th>' + time[i].TimeName + '</th>';
            }
            else {
                html += '<th>' + hh + ' ' + tt + '</th>';
            }
            
        }
        html += '</tr></thead><tbody class="my-tbody">';
        var colspancount = time.length + 1;
        if (IsHideFullDay.toLowerCase() == 'true') {
            colspancount = time.length;
        }
        if (PositionView == 'topleft' && user.EmpGuidId != null && bottomvalue == 'No') {            
            html += '<tr data-empintId="' + user.EmpIntId + '"  id = "tr_' + user.EmpGuidId + '"><td  draggable="false"  title="' + user.EmpName + ' (' + user.GroupName + ')">' + user.EmpName + '</td><td colspan = "' + colspancount + '" class="topuserscroll fullday event-container" data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" id="td_' + user.EmpGuidId + '_full" data-empintId="' + user.EmpIntId + '"> <div class="topuserscroll_container">';
                     
            for (var i = 1; i <= user.TaskCount; i++) {
                html += '<div data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="my-td event-container" id="div_' + user.EmpGuidId + '_SystemUser_' + i + '" data-empintId="' + user.EmpIntId + '"data-EmpGuidId="' + user.EmpGuidId + '"></div>';
            }
            html += '</div></td></tr>';
        }
        for (var i = 0; i < emp.length; i++) {
            var Availstartnumber = 0, Availendnumber = 0, Titlemsg = 'Daily starting leave';
            var comholiday = emp[i].IsCompanyHoliday;
            var status = emp[i].HolidayStatus.split(',')[0];
            if (emp[i].EmpGuidId != '22222222-2222-2222-2222-222222222222') {
                Availstartnumber = parseInt(emp[i].AvailablityTime.split(',')[1]);
                Availstartnumber = Availstartnumber - 2;
                Availendnumber = parseInt(emp[i].AvailablityTime.split(',')[2]);
                if (Availstartnumber < Availendnumber) {
                    Availendnumber = Availendnumber - 1;
                }
                if (Availendnumber == 0 && Availendnumber < Availstartnumber) {
                    Titlemsg = 'Day off';
                    Availstartnumber = Availstartnumber + 1;
                }
            }
            if (comholiday) {
                html += '<tr data-empintId="' + emp[i].EmpIntId + '"  id = "tr_' + emp[i].EmpGuidId + '">';

                if (emp[i].ErrorCount > 0) {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + ' <i title="' + emp[i].ErrorTitleStatus + '" class="fa fa-exclamation-circle" onclick="calendarerrorshow(this)" id="' + emp[i].EmpGuidId + '" aria-hidden="true" style="color:red; font-size: 20px;"></i>(' + emp[i].ErrorCount + ')<div class="calerrorpopup" id="errpopup_' + emp[i].EmpGuidId + '"><div class="calerrorpopup-content"><span class="calerrorpopupclose" onclick="calendarerrorclose(this)" id="close_' + emp[i].EmpGuidId + '">&times;</span>' + emp[i].ErrorTicketIdEditList + '</div></div></td>';
                }
                else {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</td>';
                }
                if (IsHideFullDay.toLowerCase() != 'true') {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Company Holiday">Company Holiday</td>';
                }
                for (var k = 0; k < time.length; k++) {
                    html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Company Holiday"></td>';
                }
            }
           else if (status == "FullDay") {
                html += '<tr data-empintId="' + emp[i].EmpIntId + '"  id = "tr_' + emp[i].EmpGuidId + '">';

                if (emp[i].ErrorCount > 0) {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + ' <i title="' + emp[i].ErrorTitleStatus + '" class="fa fa-exclamation-circle" onclick="calendarerrorshow(this)" id="' + emp[i].EmpGuidId + '" aria-hidden="true" style="color:red; font-size: 20px;"></i>(' + emp[i].ErrorCount + ')<div class="calerrorpopup" id="errpopup_' + emp[i].EmpGuidId + '"><div class="calerrorpopup-content"><span class="calerrorpopupclose" onclick="calendarerrorclose(this)" id="close_' + emp[i].EmpGuidId + '">&times;</span>' + emp[i].ErrorTicketIdEditList + '</div></div></td>';
                }
                else {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</td>';
                }
                if (IsHideFullDay.toLowerCase() != 'true') {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Full day leave">Full day leave</td>';
                }
                for (var k = 0; k < time.length; k++) {
                    html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Full day leave"></td>';
                }
            }
            else if (status == "WeekEnd") {
                html += '<tr data-empintId="' + emp[i].EmpIntId + '"  id = "tr_' + emp[i].EmpGuidId + '">';

                if (emp[i].ErrorCount > 0) {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + ' <i title="' + emp[i].ErrorTitleStatus + '" class="fa fa-exclamation-circle" onclick="calendarerrorshow(this)" id="' + emp[i].EmpGuidId + '" aria-hidden="true" style="color:red; font-size: 20px;"></i>(' + emp[i].ErrorCount + ')<div class="calerrorpopup" id="errpopup_' + emp[i].EmpGuidId + '"><div class="calerrorpopup-content"><span class="calerrorpopupclose" onclick="calendarerrorclose(this)" id="close_' + emp[i].EmpGuidId + '">&times;</span>' + emp[i].ErrorTicketIdEditList + '</div></div></td>';
                }
                else {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</td>';
                }
                if (IsHideFullDay.toLowerCase() != 'true') {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="WeekEnd">WeekEnd</td>';
                }
                for (var k = 0; k < time.length; k++) {
                    html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="WeekEnd"></td>';
                }
            }
            else if (status == "HalfDay") {
                var timenumber = parseInt(emp[i].HolidayStatus.split(',')[1]);
                html += '<tr data-empintId="' + emp[i].EmpIntId + '"  id = "tr_' + emp[i].EmpGuidId + '">';
                if (emp[i].ErrorCount > 0) {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + ' <i title="' + emp[i].ErrorTitleStatus + '" class="fa fa-exclamation-circle" onclick="calendarerrorshow(this)" id="' + emp[i].EmpGuidId + '" aria-hidden="true" style="color:red; font-size: 20px;"></i>(' + emp[i].ErrorCount + ')<div class="calerrorpopup" id="errpopup_' + emp[i].EmpGuidId + '"><div class="calerrorpopup-content"><span class="calerrorpopupclose" onclick="calendarerrorclose(this)" id="close_' + emp[i].EmpGuidId + '">&times;</span>' + emp[i].ErrorTicketIdEditList + '</div></div></td>';
                }
                else {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</td>';
                }
                if (IsHideFullDay.toLowerCase() != 'true') {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Half day leave">Half day leave</td>';
                }
                for (var k = 0; k < time.length; k++) {
                    if (k <= Availstartnumber || k < timenumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Half day leave" ></td>';
                    }
                    else if (Availendnumber > 0 && k >= Availendnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="Daily ending leave" ></td>';
                    }
                    else {
                        html += '<td data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" ondrop = "CalendarEventDropOff(this, event)" ondragover = "CalendarEventAllowDrop(this, event)" class="my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" onclick = "CalendarTicketCreate(this, event)" ></td>';
                    }
                }
            }
            else if (status == "CustomTime") {
                var startnumber = parseInt(emp[i].HolidayStatus.split(',')[1]);
                startnumber = startnumber - 1;
                var endnumber = parseInt(emp[i].HolidayStatus.split(',')[2]);
                if (startnumber < endnumber) {
                    endnumber = endnumber - 2;
                }
                html += '<tr data-empintId="' + emp[i].EmpIntId + '"  id = "tr_' + emp[i].EmpGuidId + '">';
                if (emp[i].ErrorCount > 0) {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + ' <i  title="' + emp[i].ErrorTitleStatus + '" class="fa fa-exclamation-circle" onclick="calendarerrorshow(this)" id="' + emp[i].EmpGuidId + '" aria-hidden="true" style="color:red; font-size: 20px;"></i>(' + emp[i].ErrorCount + ')<div class="calerrorpopup" id="errpopup_' + emp[i].EmpGuidId + '"><div class="calerrorpopup-content"><span class="calerrorpopupclose" onclick="calendarerrorclose(this)" id="close_' + emp[i].EmpGuidId + '">&times;</span>' + emp[i].ErrorTicketIdEditList + '</div></div></td>';
                }
                else {
                    html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</td>';
                }
                if (IsHideFullDay.toLowerCase() != 'true') {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Few hours leave">Few hours leave</td>';
                }
                for (var k = 0; k < time.length; k++) {
                    if (k <= Availstartnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="' + Titlemsg + '" ></td>';
                    }
                    else if (Availendnumber > 0 && k >= Availendnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="Daily ending leave" ></td>';
                    }
                    else if (k >= startnumber && k <= endnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="Few hours leave" ></td>';
                    }
                    else {
                        html += '<td data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" ondrop = "CalendarEventDropOff(this, event)" ondragover = "CalendarEventAllowDrop(this, event)" class="my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" onclick = "CalendarTicketCreate(this, event)" ></td>';
                    }
                }
            }
            //else if (status == "WeekEnd") {
                
            //    html += '<tr data-empintId="' + emp[i].EmpIntId + '"  id = "tr_' + emp[i].EmpGuidId + '">';
            //    if (emp[i].ErrorCount > 0) {
            //        html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + ' <i  title="' + emp[i].ErrorTitleStatus + '" class="fa fa-exclamation-circle" onclick="calendarerrorshow(this)" id="' + emp[i].EmpGuidId + '" aria-hidden="true" style="color:red; font-size: 20px;"></i>(' + emp[i].ErrorCount + ')<div class="calerrorpopup" id="errpopup_' + emp[i].EmpGuidId + '"><div class="calerrorpopup-content"><span class="calerrorpopupclose" onclick="calendarerrorclose(this)" id="close_' + emp[i].EmpGuidId + '">&times;</span>' + emp[i].ErrorTicketIdEditList + '</div></div></td>';
            //    }
            //    else {
            //        html += '<td  draggable="false"  title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</td>';
            //    }
            //    if (IsHideFullDay.toLowerCase() != 'true') {
            //        html += '<td data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="my-td event-container weekendcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '"  title="Weekend off">Weekend</td>';
            //    }
            //    for (var k = 0; k < time.length; k++) {
            //        if (k <= Availstartnumber) {
            //            html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="' + Titlemsg + '" ></td>';
            //        }
            //        else if (Availendnumber > 0 && k >= Availendnumber) {
            //            html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="Daily ending leave" ></td>';
            //        }
            //        else {
            //            html += '<td data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" ondrop = "CalendarEventDropOff(this, event)" ondragover = "CalendarEventAllowDrop(this, event)" class="my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" onclick = "CalendarTicketCreate(this, event)"  title="Weekend off"></td>';
            //        }
            //    }
            //}
            else {
                html += '<tr data-empintId="' + emp[i].EmpIntId + '"  id = "tr_' + emp[i].EmpGuidId + '">';
                if (emp[i].ErrorCount > 0) {
                    html += '<td  draggable="false" title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + ' <i  title="' + emp[i].ErrorTitleStatus + '" class="fa fa-exclamation-circle" onclick="calendarerrorshow(this)" id="' + emp[i].EmpGuidId + '" aria-hidden="true" style="color:red; font-size: 20px;"></i>(' + emp[i].ErrorCount + ')<div class="calerrorpopup" id="errpopup_' + emp[i].EmpGuidId + '"><div class="calerrorpopup-content"><span class="calerrorpopupclose" onclick="calendarerrorclose(this)" id="close_' + emp[i].EmpGuidId + '">&times;</span>' + emp[i].ErrorTicketIdEditList + '</div></div></td>';
                }
                else {
                    html += '<td  draggable="false" title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</td>';
                }
                if (IsHideFullDay.toLowerCase() == 'true') {
                    html += '<td data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="my-td event-container hide-class" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '"></td>';
                }
                else {
                    html += '<td data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="my-td event-container" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '"></td>';
                }
                for (var k = 0; k < time.length; k++) {
                    if (k <= Availstartnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="' + Titlemsg + '" ></td>';
                    }
                    else if (Availendnumber > 0 && k >= Availendnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="Daily ending leave" ></td>';
                    }
                    else {
                        html += '<td data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" ondrop = "CalendarEventDropOff(this, event)" ondragover = "CalendarEventAllowDrop(this, event)" class="my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" onclick = "CalendarTicketCreate(this, event)" ></td>';
                    }
                }
            }
            html += '</tr>';
        }
        if (PositionView == 'bottomright' && user.EmpGuidId != null && bottomvalue == 'No') {
            var colspancount = time.length;
            html += '<tr data-empintId="' + user.EmpIntId + '"  id = "tr_' + user.EmpGuidId + '"><td  draggable="false">' + user.EmpName + '</td><td colspan = "' + colspancount + '" class="topuserscroll fullday event-container" data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" id="td_' + user.EmpGuidId + '_full" data-empintId="' + user.EmpIntId + '"> <div class="topuserscroll_container">';
            for (var i = 1; i <= user.TaskCount; i++) {
                html += '<div data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="my-td event-container" id="div_' + user.EmpGuidId + '_SystemUser_' + i + '" data-empintId="' + user.EmpIntId + '"data-EmpGuidId="' + user.EmpGuidId + '"></div>';
            }
            html += '</div></td></tr>';
        }
        html += '</tbody>';
        // System User Bottom fixed 
        if (bottomvalue == 'Yes' && user.EmpGuidId != null) {
            html += '</table><table class="footer_table_style"><tr data-empintId="' + user.EmpIntId + '"  id = "tr_' + user.EmpGuidId + '"><td  draggable="false" title="' + user.EmpName + ' (' + user.GroupName + ')" >' + user.EmpName + '</td><td class="topuserscroll fullday event-container" data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" id="td_' + user.EmpGuidId + '_full" data-empintId="' + user.EmpIntId + '"> <div class="topuserscroll_container">';
            
            for (var i = 1; i <= user.TaskCount; i++) {
                html += '<div data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="hr_my-td event-container" id="div_' + user.EmpGuidId + '_SystemUser_' + i + '" data-empintId="' + user.EmpIntId + '"data-EmpGuidId="' + user.EmpGuidId + '"></div>';
            }
            html += '</div></td></tr>';
        }
        html += '</table>';
    }
    //weekly Table
    else if (view == "Weekly") {
        var html = '<table class="responstable my-weekly"><thead><tr><th>Name</th>';
        for (var i = 0; i <= time.length; i++) {
            if (i == time.length) {
                html += '<th></th>';
            }
            else {
                html += '<th>' + time[i].TimeName + '<br />' + time[i].DateName + '</th>';
            }            
        }
        html += '</tr></thead><tbody class="my-tbody">';
        html += '<tr class="trtoptotal"><td class="trtoptotal">Daily Total</td>';
        for (var k = 0; k < time.length; k++) {
            var tid = 'td_top_' + time[k].DateName;
            html += '<td id="' + tid + '" class="trtoptotal"></td>';
        }
        html += '<td class="trtoptotal">Weekly Total</td></tr>';

        for (var i = 0; i < emp.length; i++) {
            html += '<tr data-empintId="' + emp[i].EmpIntId + '"  id = "tr_' + emp[i].EmpGuidId + '"><td class="weektd" draggable="false" title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</td>';
            for (var k = 0; k < time.length; k++) {
                html += '<td data-mytime="' + time[k].DateName + '" data-empintId="' + emp[i].EmpIntId + '" class="weektd my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].DateName + '" onclick="GoToSelectedDate(this)" ></td>';
            }
            html += '<td class="weektd my-td event-container" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" onclick="eventCreate(this, event)"></td>';
            html += '</tr>';
        }
        html += '<tr id="weeklyBottomId"><td class=""></td>';
        for (var k = 0; k < time.length; k++) {
            var tid = 'td_bottom_' + time[k].DateName;
            html += '<td id="' + tid + '" class="weektd"></td>';
        }
        html += '<td id="TotalWeeklyBottomInfoCount" class="weektd"></td></tr>';
        html += '</tbody></table>';
    }
    else if (view == "Monthly") {
        var html = '<table class="responstable my-monthly"><thead><tr>';
        for (var i = 0; i < time[1].WeekList.length; i++) {
            html += '<th><b>' + time[1].WeekList[i].TimeName + '</b></th>';
        }
        html += '</tr></thead><tbody class="my-tbody">';
        for (var k = 0; k < time.length; k++) {
            html += '<tr>';
            for (let l = 0; l < time[k].WeekList.length; l++) {
                var tid = 'td_' + time[k].WeekList[l].DateName;
                var did = 'div_' + time[k].WeekList[l].DateName;

                if (time[k].WeekList[l].DayNumber > 0) {
                    html += '<td id="' + tid + '"><div class="monthlydate">' + time[k].WeekList[l].DayNumber + '</div><div id="' + did + '" onclick="GoToSelectedDate(this)" class ="monthlyinfo"></div></td>';
                }
                else {
                    html += '<td id="' + tid + '"><div class="nextmonthlydate">' + time[k].WeekList[l].DateName + '</div><div id="' + did + '" class ="nextmonthlyinfo"></div></td>';
                }
                if (l == 6) {
                    html += '<td id="td_' + time[k].WeekCount + '_week"><div class="monthlydatereport"> Week ' + time[k].WeekCount + '</div><div id="div_' + time[k].WeekCount + '_week" class ="monthlyinforeport"></div></td>';
                }

            }
            html += '</tr>';
        }
        html += '</tbody></table>';
    }
    document.getElementById("tablebing").innerHTML = html;
    var color = $("#CustomCalendarTableHeaderColor").val();
    if (color != null && color != 'undefined' && color != "") {
        $(".responstable th").css('background-color', '#' + color);
    }
    return true;
}
var CalendarDataBind = function (data, view, topuser) {
    var table = document.querySelector(".my-tbody");
    var fontsize = $("#CustomCalendarFontSize").val();
    if (fontsize == null || fontsize == "") {
        fontsize = '10';
    }
    var setWidth = parseInt($("#EventTicketResize").val()),
        divideValue = 50;
    if (setWidth == 200) { divideValue = 50; }
    else if (setWidth == 160) { divideValue = 40; }
    else if (setWidth == 120) { divideValue = 30; }
    else if (setWidth == 80) { divideValue = 20; }

    var setHeight = parseInt($("#CalendarHeight").val()),
        tdHeightValue = 90;
    if (isNaN(setHeight)) { setHeight = 120; }
    //if (setHeight > 100) { tdHeightValue = setHeight - 30; }
    //else if (setHeight > 60) { tdHeightValue = setHeight - 20; }
    //else { tdHeightValue = setHeight - 10; }
    $(".my-daily td").css("height", setHeight + "px");
    //For Daily View
    if (view == "Daily") {
        var usertaskcount = 1;
        for (let i = 0; i < data.length; i++) {
            var uid = data[i].UserId;
            var fulladdress = '';
            if (data[i].TicketAddress != '' && data[i].TicketAddress != null && data[i].TicketAddress != 'undefined') {
                fulladdress = '<br /><span>' + data[i].TicketAddress + '</span>';
            }
            var CustomCalendarTopRowEmployee = $("#CustomCalendarTopRowEmployee").val();
            if (uid == CustomCalendarTopRowEmployee && topuser.TaskCount > 0) {
                var divId = 'field_' + CustomCalendarTopRowEmployee + '_SystemUser_' + i + '_' + usertaskcount;
                var tdId = 'div_' + CustomCalendarTopRowEmployee + '_SystemUser_' + usertaskcount;
                var exdiv = document.getElementById(tdId);
                var divhtml = '<div class ="topclass"><div title="' + data[i].TitleString +'" class="my-td event" id="' + divId + '" data-tId="' + data[i].TicketId + '" data-eGuid="' + data[i].UserId + '" data-EmpName="' + data[i].EmployeeName + '" data-TypeName="' + data[i].TicketTypeDisplayText + '" data-eType="' + data[i].AppointmentType + '" data-eDate="' + data[i].AppointmentDate + '" data-duration="' + data[i].EndDivCount + '" data-aId="' + data[i].AppointmentId + '" data-stime="' + data[i].AppointmentStartTime + '" data-etime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" draggable="true" ondragstart="CalendarEventDragStart(this, event)" onclick="CalendarTicketUpdate(this, event)"><div class="col-sm-2">';
                if (data[i].LeftIcon != '' && data[i].LeftIcon != null && data[i].LeftIcon != 'undefined') {
                    divhtml += '<img style = "border: 3px solid ' + data[i].BGColor + '; border-radius: 50%;" src = "' + data[i].LeftIcon + '" onerror = "ImageHide(event)" id="img-' + data[i].TicketId + '-' + data[i].EmpId + '">';
                }
                divhtml += '</div><div class="col-sm-10">';
                divhtml += '<span>' + data[i].TicketId + '</span><span><a href="javascript:void(0)" class="bothmargin-4px" data-details="' + data[i].PopupDetails + '" onclick = "ShowDetailsPopup(event)"  title="Details Information" id="' + data[i].TicketId + '"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
                if (data[i].IsCalled && data[i].CellNo != '' && data[i].CellNo != null) {
                    divhtml += '<a href="tel:' + data[i].CellNo +'" onclick = "StopAllClickEvent(event)" title="Call Ahead" class="call-cls cus-anchor"><i style="color: #2ca01c" class="fa fa-phone-square" aria-hidden="true"></i></a>'
                }
                divhtml += '</span><br /><span><a class="cus-anchor" onclick="StopAllClickEvent(event)" href="/Customer/CustomerDetail/?id=' + data[i].CustomerIntId + '" target="_blank">' + data[i].Name + '</a></span>' + fulladdress + '<br /><span>' + data[i].TicketTypeDisplayText + '</span><br /><span>' + data[i].Status + '</span></div></div></div>';
                if (exdiv != null) {
                    divhtml += exdiv.innerHTML;
                }
                document.getElementById(tdId).innerHTML = divhtml;
                document.getElementById(divId).style.cssText = 'font-size:' + fontsize + 'px!important';
                usertaskcount += 1;
                var trId = 'tr_' + uid;
                if ($("#" + trId).height() < 120) {
                    document.getElementById(trId).style.cssText = 'height: 120px !important';
                }
            }
            else {
                for (let j = 0; j < table.childElementCount; j++) {
                    var id = table.rows[j].id,
                        shadowvalue = "", bordervalue = "",
                        idnumber = id.toString().split('_')[1],
                        shadow = $("#CustomCalendarScheduleShadowShow").val(),
                        border = $("#CustomCalendarScheduleBorderShow").val(),
                        iconSize = parseInt($("#EventIconResizer").val());
                    if (isNaN(iconSize)) { iconSize = 25; }
                    iconSize = iconSize + 'px';

                    if (shadow != null && shadow != 'undefined' && shadow != "") {
                        shadowvalue = "0px 0px " + shadow + "px " + data[i].StatusColor;
                    }
                    if (border != null && border != 'undefined' && border != "") {
                        bordervalue = border + "px solid " + data[i].StatusColor;
                    }
                    if (idnumber == uid) {
                        if (data[i].IsAllDay == false) {
                            var sc = parseInt(data[i].StartDivCount);
                            var floatnumber = parseFloat(sc / 4).toFixed(2);
                            var innernum = parseInt(floatnumber.toString().split('.')[0]) + 1;
                            var marginnum = parseInt(floatnumber.toString().split('.')[1]);
                            if (marginnum == 25) { marginnum = divideValue; }
                            else if (marginnum == 50) { marginnum = divideValue * 2; }
                            else if (marginnum == 75) { marginnum = divideValue * 3; }
                            var widthnum = parseInt(data[i].EndDivCount * divideValue);
                            var divId = 'field_' + uid + '_' + innernum;
                            var tdId = 'td_' + uid + '_' + innernum;
                            var divcount, exdiv = document.getElementById(tdId);
                            if (exdiv != null) { divcount = exdiv.childElementCount; }
                            var overlodding = data[i].OverLodding;
                            var divhtml = '<div  title="' + data[i].TitleString +'" id="' + divId + '" data-width="' + widthnum + '" data-tId="' + data[i].TicketId + '" data-eGuid="' + data[i].UserId + '" data-EmpName="' + data[i].EmployeeName + '" data-TypeName="' + data[i].TicketTypeDisplayText + '"  data-eType="' + data[i].AppointmentType + '" data-eDate="' + data[i].AppointmentDate + '" data-duration="' + data[i].EndDivCount + '" data-aId="' + data[i].AppointmentId + '" data-stime="' + data[i].AppointmentStartTime + '" data-etime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" class="my-td event colorfull_div task_Count ResizingIcon" draggable="true" ondragstart="CalendarEventDragStart(this, event)" onclick="CalendarTicketUpdate(this, event)" ><div class="col-sm-2">';
                            if (data[i].LeftIcon != '' && data[i].LeftIcon != null && data[i].LeftIcon != 'undefined') {
                                divhtml += '<img style="border-radius: 50%;" src="' + data[i].LeftIcon + '" onerror="ImageHide(event)" id="img-' + data[i].TicketId + '-' + data[i].EmpId + '">';
                            }
                            divhtml += '</div><div class="col-sm-10">';
                            divhtml += '<span>' + data[i].TicketId + '</span><span><a href="javascript:void(0)" class="bothmargin-4px" data-details="' + data[i].PopupDetails + '" onclick = "ShowDetailsPopup(event)"  title="Details Information" id="' + data[i].TicketId + '"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
                            if (data[i].IsCalled && data[i].CellNo != '' && data[i].CellNo != null) {
                                divhtml += '<a href="tel:' + data[i].CellNo + '" onclick = "StopAllClickEvent(event)" title="Call Ahead" class="call-cls cus-anchor"><i style="color: #2ca01c" class="fa fa-phone-square" aria-hidden="true"></i></a>'
                            }
                           divhtml += '</span><br /><span><a onclick="StopAllClickEvent(event)" class="cus-anchor" href="/Customer/CustomerDetail/?id=' + data[i].CustomerIntId + '" target="_blank">' + data[i].Name + '</a></span>' + fulladdress + '<br /><span>' + data[i].TicketTypeDisplayText + '</span><br /><span>' + data[i].Status + '</span></div><div class="ResizingBorder" style="cursor: col-resize!important;" id="Re_' + divId + '" onmousedown="TicketResizeing(event)" onclick="StopAllClickEvent(event)" data-ticketid="' + data[i].TicketId + '" data-userid="' + data[i].UserId + '" data-empid="' + data[i].EmpId + '"  data-tickettype="' + data[i].AppointmentType + '" data-appdate="' + data[i].AppointmentDate + '" data-appointmentid="' + data[i].AppointmentId + '" data-appstarttime="' + data[i].AppointmentStartTime + '"data-appendtime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" ></div ></div>';
                            if (overlodding > 0) {
                                var toppx = tdHeightValue * overlodding;
                                divhtml += exdiv.innerHTML;
                                document.getElementById(tdId).innerHTML = divhtml;
                                $(".colorfull_div .col-sm-2 img").height(iconSize);
                                $(".colorfull_div .col-sm-2 img").width(iconSize);
                                document.getElementById(divId).style.cssText = 'width:' + widthnum + 'px;    margin-left:' + marginnum + 'px; margin-top: ' + toppx + 'px; text-align: center; background-color: ' + data[i].BGColor + '; height: ' + tdHeightValue +'px !important; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; box-shadow: ' + shadowvalue + '; border-radius: 6px; border: ' + bordervalue + ';';
                                var trId = 'tr_' + uid;
                                toppx = toppx + 120;
                                document.getElementById(trId).style.cssText = 'height:' + toppx + 'px !important';
                            }
                            else {
                                if (divcount > 0) {
                                    divhtml += exdiv.innerHTML;
                                }
                                document.getElementById(tdId).innerHTML = divhtml;
                                $(".colorfull_div .col-sm-2 img").height(iconSize);
                                $(".colorfull_div .col-sm-2 img").width(iconSize);
                                document.getElementById(divId).style.cssText = 'width:' + widthnum + 'px;    margin-left:' + marginnum + 'px;  text-align: center; background-color: ' + data[i].BGColor + '; height: ' + tdHeightValue + 'px !important; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; box-shadow: ' + shadowvalue + '; border-radius: 6px; border: ' + bordervalue + ';';
                                var trId = 'tr_' + uid;
                                if ($("#" + trId).height() < 120) {
                                    document.getElementById(trId).style.cssText = 'height: 120px !important';
                                }
                            }

                        }
                        else {
                            var divId = 'field_' + uid + '_full';
                            var tdId = 'td_' + uid + '_full';
                            var exdiv = document.getElementById(tdId);
                            var divhtml = '<div  id="' + divId + '" data-tId="' + data[i].TicketId + '" data-eGuid="' + data[i].UserId + '" data-EmpName="' + data[i].EmployeeName + '" data-TypeName="' + data[i].TicketTypeDisplayText + '"  data-eType="' + data[i].AppointmentType + '" data-eDate="' + data[i].AppointmentDate + '" data-duration="' + data[i].EndDivCount + '" data-aId="' + data[i].AppointmentId + '" data-stime="' + data[i].AppointmentStartTime + '" data-etime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" class="my-td event colorfull_div task_Count" draggable="true" ondragstart="CalendarEventDragStart(this, event)" onclick="CalendarTicketUpdate(this, event)" ><div class="col-sm-2">';
                            if (data[i].LeftIcon != '' && data[i].LeftIcon != null && data[i].LeftIcon != 'undefined') {
                                divhtml += '<img style="border-radius: 50%;" src="' + data[i].LeftIcon + '" onerror="ImageHide(event)" id="img-' + data[i].TicketId + '-' + data[i].EmpId + '">';
                            }
                            divhtml += '</div><div class="col-sm-10">';
                            divhtml += '<span>' + data[i].TicketId + '</span><span><a href="javascript:void(0)" class="bothmargin-4px" data-details="' + data[i].PopupDetails + '" onclick = "ShowDetailsPopup(event)"  title="Details Information" id="' + data[i].TicketId + '"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
                            if (data[i].IsCalled && data[i].CellNo != '' && data[i].CellNo != null) {
                                divhtml += '<a href="tel:' + data[i].CellNo + '" onclick = "StopAllClickEvent(event)" title="Call Ahead" class="call-cls cus-anchor"><i style="color: #2ca01c" class="fa fa-phone-square" aria-hidden="true"></i></a>'
                            }
                            divhtml += '</span><br /><span><a onclick="StopAllClickEvent(event)" class="cus-anchor" href="/Customer/CustomerDetail/?id=' + data[i].CustomerIntId + '" target="_blank">' + data[i].Name + '</a></span>' + fulladdress + '<br /><span>' + data[i].TicketTypeDisplayText + '</span><br /><span>' + data[i].Status + '</span></div></div>';
                            if (exdiv != null) {
                                divhtml += exdiv.innerHTML;
                            }
                            document.getElementById(tdId).innerHTML = divhtml;
                            $(".colorfull_div .col-sm-2 img").height(iconSize);
                            $(".colorfull_div .col-sm-2 img").width(iconSize);
                            document.getElementById(divId).style.cssText = 'width:' + setWidth + 'px !important;  margin-left: 0px; text-align: center; background-color: ' + data[i].BGColor + '; height: ' + tdHeightValue + 'px !important; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; box-shadow: ' + shadowvalue + '; border-radius: 6px; border: ' + bordervalue + ';';
                            var trId = 'tr_' + uid;
                            if ($("#" + trId).height() < 120) {
                                document.getElementById(trId).style.cssText = 'height: 120px !important';
                            }
                        }
                    }
                }
            }
        }

    }
    //For Weekly View
    else if (view == "Weekly") {
        //daily total count
        for (var k = 0; k < data.Timelist.length; k++) {
            var curDate = data.Timelist[k].DateName;
            var tdtid = 'td_top_' + curDate;
            var tdbid = 'td_bottom_' + curDate;
            var daylist = data.ServiceCount.DailyServiceTotalList;
            for (let i = 0; i < daylist.length; i++) {
                var appDate = daylist[i].AppDate;
                var total = parseInt(daylist[i].DailyTotal);
                var empCount = parseInt(daylist[i].DailyAvgCount);
                var avg = 0.00;
                if (total > 0) {
                    avg = parseFloat(total / empCount).toFixed(2);
                }
                if (curDate == appDate) {
                    var tophtml = '<div class="row"><span>' + total + '</span></div>';
                    document.getElementById(tdtid).innerHTML = tophtml;
                }
                if (curDate == appDate) {
                    var buttomhtml = '<div class="row bottomweekly">';
                    var type = data.ServiceCount.TicketTypeList;
                    for (let n = 0; n < type.length; n++) {
                        var typeService = type[n];
                        var f = false;
                        for (let j = 0; j < daylist[i].DailyServiceTotal.length; j++) {
                            var service = daylist[i].DailyServiceTotal[j].Service;
                            if (service == typeService) {
                                var scount = daylist[i].DailyServiceTotal[j].SCount;
                                buttomhtml += '<span>Total ' + service + ':' + scount + '</span><br />';
                                f = true;
                            }
                        }
                        if (!f) {
                            buttomhtml += '<span>Total ' + typeService + ':0</span><br />';
                        }
                    }
                    var rmrshow = $("#CustomCalendarRMRReportShow").val();
                    if (rmrshow == 'true') {
                        buttomhtml += '<span>Total Daily RMR:' + total + '</span><br />';
                        buttomhtml += '<span>Avg. Daily RMR:' + avg + '</span>';
                    }
                    buttomhtml += '</div>';
                    document.getElementById(tdbid).innerHTML = buttomhtml;
                    document.getElementById(tdbid).style.cssText = 'width: 100%; text-align: center; height: ' + setHeight +'px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif;';
                }
            }
        }

        //weekly total count
        for (var k = 0; k < data.EmpList.length; k++) {
            var curId = data.EmpList[k].EmpGuidId;
            var tdid = 'td_' + curId + '_full';
            var weeklist = data.ServiceCount.WeeklyServiceTotalList;
            for (let i = 0; i < weeklist.length; i++) {
                var appEid = weeklist[i].Empid;
                var total = parseInt(weeklist[i].WeeklyTotal);
                var empCount = parseInt(weeklist[i].WeeklyAvgCount);
                var avg = 0.00;
                if (total > 0) {
                    avg = parseFloat(total / empCount).toFixed(2);
                }
                if (curId == appEid) {
                    var weekhtml = '<div class="row lastweekly">';
                    var type = data.ServiceCount.TicketTypeList;
                    for (let n = 0; n < type.length; n++) {
                        var typeService = type[n];
                        var f = false;
                        for (let j = 0; j < weeklist[i].WeeklyServiceTotal.length; j++) {
                            var service = weeklist[i].WeeklyServiceTotal[j].Service;
                            if (service == typeService) {
                                var scount = weeklist[i].WeeklyServiceTotal[j].SCount;
                                weekhtml += '<span>Total ' + service + ':' + scount + '</span><br />';
                                f = true;
                            }
                        }
                        if (!f) {
                            weekhtml += '<span>Total ' + typeService + ':0</span><br />';
                        }
                    }
                    var rmrshow = $("#CustomCalendarRMRReportShow").val();
                    if (rmrshow == 'true') {
                        weekhtml += '<span>Total Weekly RMR:' + total + '</span><br />';
                        weekhtml += '<span>Avg. Weekly RMR:' + avg + '</span>';
                    }
                    weekhtml += '</div>';
                    document.getElementById(tdid).innerHTML = weekhtml;
                    document.getElementById(tdid).style.cssText = 'width: 100%; text-align: center; height: ' + setHeight +'px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif;';
                }
            }
        }
        var Totalweekhtml = '<div class="row bottomweekly">';
        var TicketTypeList = data.ServiceCount.TicketTypeList;
        for (let ttl = 0; ttl < TicketTypeList.length; ttl++) {
            var WeekTotalflag = false;
            var Totalweeklist = data.WeeklyTotalInfo;
            for (let twl = 0; twl < Totalweeklist.length; twl++) {
                if (Totalweeklist[twl].Service == TicketTypeList[ttl]) {
                    Totalweekhtml += '<span>Weekly Total ' + Totalweeklist[twl].Service + ':' + Totalweeklist[twl].SCount + '</span><br />';
                    WeekTotalflag = true;
                }
            }
            if (!WeekTotalflag) {
                Totalweekhtml += '<span>Weekly Total ' + TicketTypeList[ttl] + ':0</span><br />';
            }
            
        }
        Totalweekhtml += '</div>';
        document.getElementById("TotalWeeklyBottomInfoCount").innerHTML = Totalweekhtml;
        document.getElementById("TotalWeeklyBottomInfoCount").style.cssText = 'width: 100%; text-align: center; height: ' + setHeight + 'px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif;';

        //OFF Day Load
        for (let n = 0; n < data.EmpList.length; n++) {
            var trid = table.childNodes[n + 1].id;
            var tid = trid.toString().split('_')[1];
            for (let o = 0; o < data.Timelist.length; o++) {
                var html, htmldate = data.WeeklyInfo[o].AppointmentDate;
                var dayname = data.WeeklyInfo[o].DayName;
                var font = 'fa fa-map-o';
                var looptdid = 'td_' + tid + '_' + htmldate;
                if (data.Timelist[o].WeekEnd != "") {
                    html = '<div class="row">';
                    html += '<span>' + data.Timelist[o].WeekEnd + '</span>';
                    html += '</div>';
                    document.getElementById(looptdid).innerHTML = html;
                    document.getElementById(looptdid).style.cssText = 'width: 100%; text-align: center; height: ' + setHeight +'px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif;';
                }
                else {
                    var flag = false;
                    for (let p = 0; p < data.Timelist[o].Holidays.length; p++) {
                        if (data.Timelist[o].Holidays[p].UserId == tid) {
                            var Leave, status = data.Timelist[o].Holidays[p].Type;
                            if (status == "FullDay") { Leave = "Full Day Off"; }
                            if (status == "HalfDay") { Leave = "Half Day Off"; }
                            if (status == "CustomTime") { Leave = "From " + data.Timelist[o].Holidays[p].StartTime + " To " + data.Timelist[o].Holidays[p].EndTime + " Off"; }
                            html = '<div class="row">';
                            html += '<span>' + Leave + '</span>';
                            html += '</div>';
                            document.getElementById(looptdid).innerHTML = html;
                            document.getElementById(looptdid).style.cssText = 'width: 100%; text-align: center; height: ' + setHeight +'px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif;';
                            flag = true;
                        }
                    }
                    if (!flag) {
                        html = '<div class="row"><div class="col-sm-2"><i class="' + font + '" aria-hidden="true"></i></div><div class="col-sm-10">';
                        html += '<span></span>';
                        html += '</div></div>';
                        document.getElementById(looptdid).innerHTML = html;
                        document.getElementById(looptdid).style.cssText = 'width: 100%; text-align: center; background-color: #ccccff; height: ' + setHeight +'px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; border: 2px solid #cc5200';
                    }
                }

            }
        }
        for (let i = 0; i < data.EmpList.length; i++) {
            var id = table.childNodes[i + 1].id;
            var idnumber = id.toString().split('_')[1];
            for (let j = 0; j < data.Timelist.length; j++) {
                var s = data.WeeklyInfo[j].dailyInfolist;
                var innerdate = data.WeeklyInfo[j].AppointmentDate;
                if (s != null) {
                    for (var k = 0; k < data.WeeklyInfo[j].dailyInfolist.length; k++) {
                        var ss = data.WeeklyInfo[j].dailyInfolist[k].dailyInfos;
                        if (ss != null) {
                            for (let l = 0; l < data.WeeklyInfo[j].dailyInfolist[k].dailyInfos.length; l++) {
                                if (data.WeeklyInfo[j].dailyInfolist[k].dailyInfos[l].EmpId == idnumber && data.WeeklyInfo[j].dailyInfolist[k].dailyInfos[l].AppDate == innerdate) {
                                    var tdId = 'td_' + idnumber + '_' + innerdate;
                                    var divId = 'div_' + idnumber + '_' + innerdate;
                                    var divhtml = '<div id="' + divId + '" class="row" data-aid="' + data.WeeklyInfo[j].dailyInfolist[k].dailyInfos[l].AppointmentId + '" ><div class="col-sm-2"><i class="' + data.WeeklyInfo[j].LeftIcon + '" aria-hidden="true"></i></div><div class="col-sm-10">';
                                    if (ss.length > 0) {
                                        for (let m = 0; m < ss.length; m++) {
                                            var spanId = 'span_' + idnumber + '_' + innerdate + '_' + m;
                                            divhtml += '<span title="' + data.WeeklyInfo[j].dailyInfolist[k].dailyInfos[m].PopUpString + '" id="' + spanId + '" >' + data.WeeklyInfo[j].dailyInfolist[k].dailyInfos[m].NameInfo + ' </span><br />';
                                        }
                                    }

                                    divhtml += '</div></div>';
                                    document.getElementById(tdId).innerHTML = "";
                                    document.getElementById(tdId).innerHTML = divhtml;
                                    document.getElementById(tdId).style.cssText = 'width: 100%; text-align: center; background-color: ' + data.WeeklyInfo[j].BGColor + '; height: ' + setHeight +'px; margin-left: 0px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; border: ' + data.WeeklyInfo[j].Border + ';';
                                }
                            }
                        }
                    }
                }
            }
        }

        let monthNames = ["Jan", "Feb", "Mar", "Apr",
            "May", "Jun", "Jul", "Aug",
            "Sep", "Oct", "Nov", "Dec"];
        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = monthNames[today.getMonth()];
        var dd = today.getDate();
        if (dd < 10) { dd = '0' + dd; }
        var fullday = dd + '-' + mm + '-' + yyyy;
        var tdclass = '.class-' + fullday;
        $(tdclass).css({ "background-color": "#48f58d !important" });
    }
    else if (view == "Monthly") {
        //daily
        for (var i = 0; i < data.DayDataShow.length; i++) {
            var did = 'div_' + data.DayDataShow[i].AppDate;
            var html = '';
            for (let j = 0; j < data.TicketTypeList.length; j++) {
                if (data.DayDataShow[i].DailyServiceTotal.length > 0) {
                    var l = 0, m = false;
                    for (let k = 0; k < data.DayDataShow[i].DailyServiceTotal.length; k++) {
                        if (data.TicketTypeList[j].Service == data.DayDataShow[i].DailyServiceTotal[k].Service) {
                            l = k;
                            m = true;
                        }
                    }
                    var ServiceCount = parseInt(data.DayDataShow[i].DailyServiceTotal[l].SCount);
                    if (isNaN(ServiceCount)) {
                        ServiceCount = 0;
                    }
                    if (m && ServiceCount > 0) {
                        html += '<span>' + data.DayDataShow[i].DailyServiceTotal[l].ServiceName + ': ' + data.DayDataShow[i].DailyServiceTotal[l].SCount + '</span><br />';
                    }
                    //else {
                    //    html += '<span>' + data.TicketTypeList[j].ServiceName + ': 0</span><br />';
                    //}

                }
                //else {
                //    html += '<span>' + data.TicketTypeList[j].ServiceName + ': 0</span><br />';
                //}
            }
            document.getElementById(did).innerHTML = html;
            document.getElementById(did).style.cssText = 'font-size:' + fontsize + 'px!important';
        }
        for (var i = 0; i < data.WeeklyTotalDataShow.length; i++) {
            var total = 0, avg = 0, html = '', did = 'div_' + data.WeeklyTotalDataShow[i].keyValue + '_week';
            for (let j = 0; j < data.TicketTypeList.length; j++) {
                var l = 0, m = false;
                for (let k = 0; k < data.WeeklyTotalDataShow[i].MonthInfo.length; k++) {
                    if (data.TicketTypeList[j].Service == data.WeeklyTotalDataShow[i].MonthInfo[k].ServiceValue) {
                        l = k;
                        m = true;
                        total += data.WeeklyTotalDataShow[i].MonthInfo[k].ServiceTotal;
                        avg += data.WeeklyTotalDataShow[i].MonthInfo[k].AvgCount;
                    }
                }
                var ServiceTotalCount = parseInt(data.WeeklyTotalDataShow[i].MonthInfo[l].ServiceTotal);
                if (isNaN(ServiceTotalCount)) {
                    ServiceTotalCount = 0;
                }
                if (m && ServiceTotalCount > 0) {
                    html += '<span>Total ' + data.WeeklyTotalDataShow[i].MonthInfo[l].ServiceName + ': ' + data.WeeklyTotalDataShow[i].MonthInfo[l].ServiceTotal + '</span><br />';
                }
                //else {
                //    html += '<span>Total ' + data.TicketTypeList[j].ServiceName + ': 0</span><br />';
                //}
            }
            if (total > 0 && avg > 0) {
                avg = parseFloat(total / avg).toFixed(2);
            }
            var rmrshow = $("#CustomCalendarRMRReportShow").val();
            if (rmrshow == 'true') {
                html += '<span>Total Weekly RMR: ' + total + '</span><br />';
                html += '<span>Total Weekly RMR Avg: ' + avg + '</span>';
            }

            document.getElementById(did).innerHTML = html;
            document.getElementById(did).style.cssText = 'font-size:' + fontsize + 'px!important';
        }
        var today = new Date();
        var yyyy = today.getFullYear();
        var mm = today.getMonth() + 1;
        if (mm < 10) { mm = '0' + mm; }
        var dd = today.getDate();
        if (dd < 10) { dd = '0' + dd; }
        var fullday = yyyy + '-' + mm + '-' + dd;
        var divid = '#div_' + fullday;
        var tdid = '#td_' + fullday;
        $(divid).css({ "background-color": "#48f58d" });
        $(tdid).css({ "background-color": "#48f58d" });
        $(divid).attr('title', 'Today');
        $(tdid).attr('title', 'Today');
    }
    //Add Dynamic Css
    var minW = setWidth + 'px';
    var fSize = fontsize + 'px';
    $(".responstable").css({ "table-layout": "fixed" });
    if (setWidth < 100) {
        $(".topclass").css({ "width": "100px", "font-size": fSize });
    }
    else {
        $(".topclass").css({ "width": minW, "font-size": fSize });
    }
    $(".my-daily th").css({ "width": minW, "font-size": fSize });
    $(".my-daily td").css({ "width": minW, "font-size": fSize});
    $(".my-weekly th").css({ "width": minW, "font-size": fSize });
    $(".my-weekly .weektd").css({ "width": minW, "font-size": fSize, "height": setHeight + "px !important" });
    $(".lastweekly").css({ "overflow": "auto", "height": setHeight + "px" });
    $(".bottomweekly").css({ "overflow-y": "auto", "height": setHeight + "px" });

    var containerwidth = $('.tablebing_container').width();
    var calendartablewidth = $('.responstable').width();
    if (containerwidth >= calendartablewidth) {
        $(".responstable").css({ "width": "unset" });
    }
    ticketclosescroll();
}
// Table show employee in top and top in left
var VerticalTableViewCreate = function (emp, time, view, user) {
    //daily Table
    var html = '';
    var fontsize = $("#CustomCalendarFontSize").val();
    if (fontsize == null || fontsize == "") {
        fontsize = '10';
    }
    if (view == "Daily") {
        var PositionView = $("#SysUserViewPostion").val();
        var setWidth = parseInt($("#EventTicketResize").val());
        
        html = '<table class="vertical_calendar responstable vertical-my-daily"><thead><tr><th>Name</th>';
        if (setWidth == 0) {
            html = '<table class="uncheck_width responstable vertical-my-daily"><thead><tr><th>Name</th>';
        }
        // Hearder Section
        if (PositionView == 'topleft' && user.EmpGuidId != null && bottomvalue == 'No' && bottomvalue == 'No') {
            html += '<th id="th_' + user.EmpGuidId + '" draggable="false" title="' + user.EmpName + ' (' + user.GroupName + ')" >' + user.EmpName + '</th>';
        }
        for (var i = 0; i < emp.length; i++) {
            if (emp[i].ErrorCount > 0) {
                html += '<th id="th_' + emp[i].EmpGuidId + '" draggable="false" title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + ' <i title="' + emp[i].ErrorTitleStatus + '" class="fa fa-exclamation-circle" onclick="calendarerrorshow(this)" id="' + emp[i].EmpGuidId + '" aria-hidden="true" style="color:red; font-size: 20px;"></i>(' + emp[i].ErrorCount + ')<div class="calerrorpopup" id="errpopup_' + emp[i].EmpGuidId + '"><div class="vertical-calerrorpopup-content"><span class="calerrorpopupclose" onclick="calendarerrorclose(this)" id="close_' + emp[i].EmpGuidId + '">&times;</span>' + emp[i].ErrorTicketIdEditList + '</div></div></th>';
            }
            else {
                html += '<th id="th_' + emp[i].EmpGuidId + '" draggable="false" title="' + emp[i].EmpName + ' (' + emp[i].GroupName + ')" >' + emp[i].EmpName + '</th>';
            }
        }
        if (PositionView == 'bottomright' && user.EmpGuidId != null && bottomvalue == 'No') {
            html += '<th id="th_' + user.EmpGuidId + '" draggable="false" title="' + user.EmpName + ' (' + user.GroupName + ')" >' + user.EmpName + '</th>';
        }
        html += '</tr></thead><tbody class="my-tbody ggg"><tr><td class="time_bar_container"><div  class="current-time-bar" id="current-bar"></div></td></tr>';
        if (IsHideFullDay.toLowerCase() == 'true') {
            html += '<tr id="tr_FullDay" class="hide-class">';
            html += '<td class="fullday">Full Day</td>';
        }
        else {
            html += '<tr id="tr_FullDay">';
            html += '<td class="fullday">Full Day</td>';
        }
        var rowspancount = time.length + 1;
        if (IsHideFullDay.toLowerCase() == 'true') {
            rowspancount = time.length;
        }
        if (PositionView == 'topleft' && user.EmpGuidId != null && bottomvalue == 'No' && IsHideFullDay.toLowerCase() != 'true') {
            html += '<td rowspan="' + rowspancount + '" class="topuserscroll fullday event-container" data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" id="td_' + user.EmpGuidId + '_full" data-empintId="' + user.EmpIntId + '"><div class="topuserscroll_container">';
            for (var i = 1; i <= user.TaskCount; i++) {
                html += '<div data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="fullday event-container" id="div_' + user.EmpGuidId + '_SystemUser_' + i + '" data-empintId="' + user.EmpIntId + '" data-EmpGuidId="' + user.EmpGuidId + '"></div>';
            }
            html += '</div></td>';
        }
        for (var i = 0; i < emp.length; i++) {
            var comholiday = emp[i].IsCompanyHoliday;
            var status = emp[i].HolidayStatus.split(',')[0];
            if (IsHideFullDay.toLowerCase() == 'true') {
                if (comholiday) {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor hide-class" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Company Holiday">Company Holiday</td>';
                }
               else if (status == "FullDay") {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor hide-class" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Full day leave">Full day leave</td>';
                }
                else if (status == "WeekEnd") {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor hide-class" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="WeekEnd">WeekEnd</td>';
                }
                else if (status == "HalfDay") {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor hide-class" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Half day leave">Half day leave</td>';
                }
                else if (status == "CustomTime") {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor hide-class" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Few hours leave">Few hours leave</td>';
                }
                //else if (status == "WeekEnd") {
                //    html += '<td data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="my-td event-container weekendcolor hide-class" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '"  title="Weekend off">Weekend</td>';
                //}
                else {
                    html += '<td data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="fullday event-container hide-class" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '"></td>';
                }
            }
            else {
                if (comholiday) {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Company Holiday">Company Holiday</td>';
                }
               else if (status == "FullDay") {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Full day leave">Full day leave</td>';
                }
                else if (status == "WeekEnd") {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="WeekEnd">WeekEnd</td>';
                }
                else if (status == "HalfDay") {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Half day leave">Half day leave</td>';
                }
                else if (status == "CustomTime") {
                    html += '<td data-mytime="FullDay"  draggable="false" class="my-td event-container fulltdcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '" title="Few hours leave">Few hours leave</td>';
                }
                //else if (status == "WeekEnd") {
                //    html += '<td data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="my-td event-container weekendcolor" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '"  title="Weekend off">Weekend</td>';
                //}
                else {
                    html += '<td data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="fullday event-container" id="td_' + emp[i].EmpGuidId + '_full" data-empintId="' + emp[i].EmpIntId + '"></td>';
                }
            }
        }
        if (PositionView == 'bottomright' && user.EmpGuidId != null && bottomvalue == 'No' && IsHideFullDay.toLowerCase() != 'true') {
            html += '<td rowspan="' + rowspancount + '" class="topuserscroll fullday event-container" data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" id="td_' + user.EmpGuidId + '_full" data-empintId="' + user.EmpIntId + '"><div class="topuserscroll_container">';
            for (var i = 1; i <= user.TaskCount; i++) {
                html += '<div data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="fullday event-container" id="div_' + user.EmpGuidId + '_SystemUser_' + i + '" data-empintId="' + user.EmpIntId + '" data-EmpGuidId="' + user.EmpGuidId + '"></div>';
            }
            html += '</div></td>';
        }
        html += '</tr>';

        //Body Section        
        for (var k = 0; k < time.length; k++) {
            var colspancount = time.length + 1;
            $("#TimeCount").val(colspancount);
            var hh = parseInt(time[k].TimeName.split(':')[0]);
            var mmtt = time[k].TimeName.split(':')[1];
            var mm = parseInt(mmtt.split(' ')[0]);
            var tt = time[k].TimeName.split(' ')[1];
            if (!isNaN(mm) && mm > 0) {
                html += '<tr id = "tr_' + time[k].TimeName + '_' + time[k].SL + '"><td>' + time[k].TimeName + '</td>';
            }
            else {
                html += '<tr id = "tr_' + time[k].TimeName + '_' + time[k].SL + '"><td>' + hh + ' ' + tt + '</td>';
            }
            if (PositionView == 'topleft' && user.EmpGuidId != null && bottomvalue == 'No' && IsHideFullDay.toLowerCase() == 'true' && k == 0) {
                html += '<td rowspan="' + rowspancount + '" class="topuserscroll fullday event-container" data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" id="td_' + user.EmpGuidId + '_full" data-empintId="' + user.EmpIntId + '"><div class="topuserscroll_container">';
                for (var i = 1; i <= user.TaskCount; i++) {
                    html += '<div data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="fullday event-container" id="div_' + user.EmpGuidId + '_SystemUser_' + i + '" data-empintId="' + user.EmpIntId + '" data-EmpGuidId="' + user.EmpGuidId + '"></div>';
                }
                html += '</div></td>';
            }
            for (var i = 0; i < emp.length; i++) {
                var comholiday = emp[i].IsCompanyHoliday;
                var status = emp[i].HolidayStatus.split(',')[0];
                var Availstartnumber = 0, Availendnumber = 0, Titlemsg = 'Daily starting leave';
                if (emp[i].EmpGuidId != '22222222-2222-2222-2222-222222222222') {
                    Availstartnumber = parseInt(emp[i].AvailablityTime.split(',')[1]);
                    Availstartnumber = Availstartnumber - 2;
                    Availendnumber = parseInt(emp[i].AvailablityTime.split(',')[2]);
                    if (Availstartnumber < Availendnumber) {
                        Availendnumber = Availendnumber - 1;
                    }
                    if (Availendnumber == 0 && Availendnumber < Availstartnumber) {
                        Titlemsg = 'Day off';
                        Availstartnumber = Availstartnumber + 1;
                    }
                }
                if (comholiday) {
                    html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Company Holiday"></td>';
                }
               else if (status == "FullDay") {
                    html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Full day leave"></td>';
                }
                else if (status == "WeekEnd") {
                    html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="WeekEnd"></td>';
                }
                else if (status == "HalfDay") {
                    var timenumber = parseInt(emp[i].HolidayStatus.split(',')[1]);
                    if (Availstartnumber >= k || k < timenumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Half day leave" ></td>';
                    }
                    else if (Availendnumber > 0 && k >= Availendnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Daily end leave" ></td>';
                    }
                    else {
                        html += '<td data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" ondrop = "CalendarEventDropOff(this, event)" ondragover = "CalendarEventAllowDrop(this, event)" class="vertical-my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" onclick = "CalendarTicketCreate(this, event)" ></td>';
                    }
                }
                else if (status == "CustomTime") {
                    var startnumber = parseInt(emp[i].HolidayStatus.split(',')[1]);
                    startnumber = startnumber - 1;
                    var endnumber = parseInt(emp[i].HolidayStatus.split(',')[2]);
                    if (startnumber < endnumber) {
                        endnumber = endnumber - 2;
                    }
                    if (Availstartnumber >= k) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="' + Titlemsg + '" ></td>';
                    }
                    else if (Availendnumber > 0 && k >= Availendnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Daily end leave" ></td>';
                    }
                    else if (k >= startnumber && k <= endnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '"  title="Few hours leave" ></td>';
                    }
                    else {
                        html += '<td data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" ondrop = "CalendarEventDropOff(this, event)" ondragover = "CalendarEventAllowDrop(this, event)" class="vertical-my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" onclick = "CalendarTicketCreate(this, event)" ></td>';
                    }
                }
                //else if (status == "WeekEnd") {
                //    if (Availstartnumber >= k) {
                //        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="' + Titlemsg + '" ></td>';
                //    }
                //    else if (Availendnumber > 0 && k >= Availendnumber) {
                //        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Daily end leave" ></td>';
                //    }
                //    else {
                //        html += '<td data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" ondrop = "CalendarEventDropOff(this, event)" ondragover = "CalendarEventAllowDrop(this, event)" class="vertical-my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" onclick = "CalendarTicketCreate(this, event)"  title="Weekend off"></td>';
                //    }
                //}
                else {
                    if (Availstartnumber >= k) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="' + Titlemsg + '" ></td>';
                    }
                    else if (Availendnumber > 0 && k >= Availendnumber) {
                        html += '<td draggable="false" data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" class="my-td vertical-my-td event-container fulltdcolor" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" title="Daily end leave" ></td>';
                    }
                    else {
                        html += '<td data-mytime="' + time[k].TimeName + '" data-empintId="' + emp[i].EmpIntId + '" ondrop = "CalendarEventDropOff(this, event)" ondragover = "CalendarEventAllowDrop(this, event)" class="vertical-my-td event-container" id = "td_' + emp[i].EmpGuidId + '_' + time[k].SL + '" onclick = "CalendarTicketCreate(this, event)" ></td>';
                    }
                }
            }
            if (PositionView == 'bottomright' && user.EmpGuidId != null && bottomvalue == 'No' && IsHideFullDay.toLowerCase() == 'true' && k == 0) {
                html += '<td rowspan="' + rowspancount + '" class="topuserscroll fullday event-container" data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" id="td_' + user.EmpGuidId + '_full" data-empintId="' + user.EmpIntId + '"><div class="topuserscroll_container">';
                for (var i = 1; i <= user.TaskCount; i++) {
                    html += '<div data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="fullday event-container" id="div_' + user.EmpGuidId + '_SystemUser_' + i + '" data-empintId="' + user.EmpIntId + '" data-EmpGuidId="' + user.EmpGuidId + '"></div>';
                }
                html += '</div></td>';
            }
            html += '</tr>';
        }
        html += '</tbody>';
        // System User Bottom fixed 
        if (bottomvalue == 'Yes' && user.EmpGuidId != null) {
            var colspans = emp.length;
            html += '</table><table class="footer_table_style"><tr data-empintId="' + user.EmpIntId + '"  id = "tr_' + user.EmpGuidId + '"><td  draggable="false" title="' + user.EmpName + ' (' + user.GroupName + ')" >' + user.EmpName + '</td><td class="topuserscroll fullday event-container" data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" id="td_' + user.EmpGuidId + '_full" data-empintId="' + user.EmpIntId + '"> <div class="topuserscroll_container">';
            for (var i = 1; i <= user.TaskCount; i++) {
                html += '<div data-mytime="FullDay" ondrop="CalendarEventDropOff(this, event)" ondragover="CalendarEventAllowDrop(this, event)" class="my-td event-container" id="div_' + user.EmpGuidId + '_SystemUser_' + i + '" data-empintId="' + user.EmpIntId + '"data-EmpGuidId="' + user.EmpGuidId + '"></div>';
            }
            html += '</div></td></tr>';
        }
        html += '</table>';
    }
    document.getElementById("tablebing").innerHTML = html;
    var color = $("#CustomCalendarTableHeaderColor").val();
    if (color != null && color != 'undefined' && color != "") {
        $(".responstable th").css('background-color', '#' + color);
    }
    return true;
}
var VerticalCalendarDataBind = function (data, view, topuser, usercount) {
    var table = document.querySelector(".my-tbody");
    var setWidth = parseInt($("#EventTicketResize").val()),
        fontsize = $("#CustomCalendarFontSize").val(),
        iconSize = parseInt($("#EventIconResizer").val()),
        totalwidth = 80, dynamicWidth = 120;
    if (isNaN(iconSize)) { iconSize = 25; }
    iconSize = iconSize + 'px';
    if (fontsize == null || fontsize == "") {
        fontsize = '10';
    }
    if (setWidth == 80) {
        dynamicWidth = 70;
        fontsize = 8;
        iconSize = '25px';
    }
    else if (setWidth == 120) {
        dynamicWidth = 100;
        fontsize = 10;
    }
    else if (setWidth == 200) {
        dynamicWidth = 150;
    }
    else if (setWidth == 0) {
        fontsize = 12;
    }
    var divideValue = 25;
    //Add Dynamic Css
    var fSize = fontsize + 'px';
    var minW = setWidth + 'px';
    if (setWidth > 0) {
        if (setWidth == 80) {
            $(".topclass").css({ "min-width": minW, "font-size": "8px" });
            $(".vertical-my-daily th").css({ "min-width": minW, "font-size": "8px" });
            $(".vertical-my-daily td").css({ "min-width": minW, "font-size": "8px" });
            $(".my-weekly th").css({ "min-width": minW, "font-size": "8px" });
            $(".my-weekly .weektd").css({ "min-width": minW, "font-size": "8px" });
        }
        else if (setWidth == 120) {
            $(".topclass").css({ "min-width": minW, "font-size": "12px" });
            $(".vertical-my-daily th").css({ "min-width": minW, "font-size": "12px" });
            $(".vertical-my-daily td").css({ "min-width": minW, "font-size": "12px" });
            $(".my-weekly th").css({ "min-width": minW, "font-size": "12px" });
            $(".my-weekly .weektd").css({ "min-width": minW, "font-size": "12px" });
        }
        else if (setWidth == 160) {
            $(".topclass").css({ "min-width": minW, "font-size": fSize });
            $(".vertical-my-daily th").css({ "min-width": minW, "font-size": fSize });
            $(".vertical-my-daily td").css({ "min-width": minW, "font-size": fSize });
            $(".my-weekly th").css({ "min-width": minW, "font-size": fSize });
            $(".my-weekly .weektd").css({ "min-width": minW, "font-size": fSize });
        }
        else if (setWidth == 200) {
            $(".topclass").css({ "min-width": minW, "font-size": fSize });
            $(".vertical-my-daily th").css({ "min-width": minW, "font-size": fSize });
            $(".vertical-my-daily td").css({ "min-width": minW, "font-size": fSize });
            $(".my-weekly th").css({ "min-width": minW, "font-size": fSize });
            $(".my-weekly .weektd").css({ "min-width": minW, "font-size": fSize });
        }    
    }
    else {
        $(".uncheck_width").css({ "width": "100%" });
        $(".topclass").css({ "font-size": "12px" });
        $(".vertical-my-daily th").css({ "font-size": "12px" });
        $(".vertical-my-daily td").css({ "font-size": "12px" });
        $(".my-weekly th").css({ "font-size": "12px" });
        $(".my-weekly .weektd").css({ "font-size": "12px" });
    }
    var setHeight = parseInt($("#CalendarHeight").val());
    $(".vertical-my-td").css("height", setHeight + "px");
    divideValue = setHeight / 4;
    //For Daily View
    if (view == "Daily") {
        var usertaskcount = 1;
        var runningid, toppxvalue = 0;
        for (let i = 0; i < data.length; i++) {
            var uid = data[i].UserId;
            var imagefile='', fulladdress = '';
            if (data[i].TicketAddress != '' && data[i].TicketAddress != null && data[i].TicketAddress != 'undefined') {
                fulladdress = '<br /><span>' + data[i].TicketAddress + '</span>';
            }
            if (data[i].LeftIcon != '' && data[i].LeftIcon != null && data[i].LeftIcon != 'undefined') {
                imagefile = '<span><img style="border: 3px solid ' + data[i].BGColor + '; border-radius: 50%; height: ' + iconSize + '; width: ' + iconSize + '; float: right; margin-right: -15px;" src = "' + data[i].LeftIcon + '" onerror = "ImageHide(event)" id="img-' + data[i].TicketId + '-' + data[i].EmpId + '"></span>';
            }
            var CustomCalendarTopRowEmployee = $("#CustomCalendarTopRowEmployee").val();
            if (uid == CustomCalendarTopRowEmployee && topuser.TaskCount > 0) {
                var divId = 'field_' + CustomCalendarTopRowEmployee + '_SystemUser_' + i + '_' + usertaskcount;
                var tdId = 'div_' + CustomCalendarTopRowEmployee + '_SystemUser_' + usertaskcount;
                var exdiv = document.getElementById(tdId);
                var divhtml = '<div class ="vertical-topclass" title="' + data[i].TitleString +'"><div class="vertical-event" id="' + divId + '" data-tId="' + data[i].TicketId + '" data-eGuid="' + data[i].UserId + '" data-EmpName="' + data[i].EmployeeName + '" data-TypeName="' + data[i].TicketTypeDisplayText + '" data-eType="' + data[i].AppointmentType + '" data-eDate="' + data[i].AppointmentDate + '" data-duration="' + data[i].EndDivCount + '" data-aId="' + data[i].AppointmentId + '" data-stime="' + data[i].AppointmentStartTime + '" data-etime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" draggable="true" ondragstart="CalendarEventDragStart(this, event)" onclick="CalendarTicketUpdate(this, event)"><div class="col-sm-12 text-left">';

                divhtml += '<span class="bothmargin-5px">' + data[i].TicketId + '</span><span><a href="javascript:void(0)" class="bothmargin-4px" data-details="' + data[i].PopupDetails + '" onclick = "ShowDetailsPopup(event)"  title="Details Information" id="' + data[i].TicketId + '"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
                if (data[i].IsCalled && data[i].CellNo != '' && data[i].CellNo != null) {
                    divhtml += '<a href="tel:' + data[i].CellNo + '" onclick = "StopAllClickEvent(event)" title="Call Ahead" class="call-cls cus-anchor"><i style="color: #2ca01c" class="fa fa-phone-square" aria-hidden="true"></i></a>'
                }
                divhtml += '</span>' + imagefile + '</div></div></div>';
                if (exdiv != null) {
                    divhtml += exdiv.innerHTML + '<br>';
                }
                document.getElementById(tdId).innerHTML = divhtml;
                document.getElementById(divId).style.cssText = 'width: ' + dynamicWidth + 'px; text-align: center; background-color: ' + data[i].BGColor + '; height: 30px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; border-radius: 6px; border: ' + bordervalue + ';';
                usertaskcount += 1;
            }
            else {
                // Edit Area
                var sc = parseInt(data[i].StartDivCount);
                var floatnumber = parseFloat(sc / 4).toFixed(2);
                var innernum = parseInt(floatnumber.toString().split('.')[0]) + 1;
                for (let j = 0; j < table.rows[innernum].cells.length; j++) {
                    var id = table.rows[innernum].cells[j].id,
                        shadowvalue = "", bordervalue = "",
                        idnumber = id.toString().split('_')[1],
                        shadow = $("#CustomCalendarScheduleShadowShow").val(),
                        border = $("#CustomCalendarScheduleBorderShow").val();

                    if (shadow != null && shadow != 'undefined' && shadow != "") {
                        shadowvalue = "0px 0px " + shadow + "px " + data[i].StatusColor;
                    }
                    if (border != null && border != 'undefined' && border != "") {
                        bordervalue = border + "px solid " + data[i].StatusColor;
                    }
                    if (idnumber == uid) {
                        if (data[i].IsAllDay == false) {
                            var marginnum = parseInt(floatnumber.toString().split('.')[1]);
                            if (marginnum == 25) { marginnum = divideValue; }
                            else if (marginnum == 50) { marginnum = divideValue * 2; }
                            else if (marginnum == 75) { marginnum = divideValue * 3; }
                            var hightnum = parseInt(data[i].EndDivCount * divideValue);
                            var divId = 'field_' + uid + '_' + innernum;
                            var tdId = 'td_' + uid + '_' + innernum;
                            var divcount, exdiv = document.getElementById(tdId);
                            if (exdiv != null) { divcount = exdiv.childElementCount; }
                            var overlodding = data[i].OverLodding;
                            var divhtml = '<div  id="' + divId + '" title="' + data[i].TitleString +'" data-width="' + hightnum + '" data-tId="' + data[i].TicketId + '" data-eGuid="' + data[i].UserId + '" data-EmpName="' + data[i].EmployeeName + '" data-TypeName="' + data[i].TicketTypeDisplayText + '"  data-eType="' + data[i].AppointmentType + '" data-eDate="' + data[i].AppointmentDate + '" data-duration="' + data[i].EndDivCount + '" data-aId="' + data[i].AppointmentId + '" data-stime="' + data[i].AppointmentStartTime + '" data-etime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" class="vertical-event task_Count vertical-ResizingIcon" draggable="true" ondragstart="CalendarEventDragStart(this, event)" onclick="CalendarTicketUpdate(this, event)" ><div class="col-sm-12 text-left" style="font-size: ' + fontsize + 'px">';

                            if (hightnum > 74) {
                                divhtml += '<span>' + data[i].TicketId + '</span><span><a href="javascript:void(0)" class="bothmargin-4px" data-details="' + data[i].PopupDetails + '" onclick = "ShowDetailsPopup(event)"  title="Details Information" id="' + data[i].TicketId + '"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
                                if (data[i].IsCalled && data[i].CellNo != '' && data[i].CellNo != null) {
                                    divhtml += '<a href="tel:' + data[i].CellNo + '" onclick = "StopAllClickEvent(event)" title="Call Ahead" class="call-cls cus-anchor"><i style="color: #2ca01c" class="fa fa-phone-square" aria-hidden="true"></i></a>'
                                }
                                divhtml += '</span><br /><span><a onclick="StopAllClickEvent(event)" class="cus-anchor" href="/Customer/CustomerDetail/?id=' + data[i].CustomerIntId + '" target="_blank">' + data[i].Name + '</a></span>' + imagefile + fulladdress + '<br /><span>' + data[i].TicketTypeDisplayText + '</span><br /><span>' + data[i].Status + '</span></div><div class="vertical-ResizingBorder" style="cursor: row-resize!important;" id="Re_' + divId + '" onmousedown="TicketResizeing(event)" onclick="StopAllClickEvent(event)" data-ticketid="' + data[i].TicketId + '" data-userid="' + data[i].UserId + '" data-empid="' + data[i].EmpId + '"  data-tickettype="' + data[i].AppointmentType + '" data-appdate="' + data[i].AppointmentDate + '" data-appointmentid="' + data[i].AppointmentId + '" data-appstarttime="' + data[i].AppointmentStartTime + '"data-appendtime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" ></div ></div>';
                            }
                            else if (hightnum > 49) {
                                divhtml += '<span>' + data[i].TicketId + '</span><span><a href="javascript:void(0)" class="bothmargin-4px" data-details="' + data[i].PopupDetails + '" onclick = "ShowDetailsPopup(event)"  title="Details Information" id="' + data[i].TicketId + '"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
                                if (data[i].IsCalled && data[i].CellNo != '' && data[i].CellNo != null) {
                                    divhtml += '<a href="tel:' + data[i].CellNo + '" onclick = "StopAllClickEvent(event)" title="Call Ahead" class="call-cls cus-anchor"><i style="color: #2ca01c" class="fa fa-phone-square" aria-hidden="true"></i></a>'
                                }
                                divhtml += '</span><br /><span><a onclick="StopAllClickEvent(event)" class="cus-anchor" href="/Customer/CustomerDetail/?id=' + data[i].CustomerIntId + '" target="_blank">' + data[i].Name + '</a></span>' + imagefile + '<br /><span>' + data[i].TicketTypeDisplayText + '</span><br /><span>' + data[i].Status + '</span></div><div class="vertical-ResizingBorder" style="cursor: row-resize!important;" id="Re_' + divId + '" onmousedown="TicketResizeing(event)" onclick="StopAllClickEvent(event)" data-ticketid="' + data[i].TicketId + '" data-userid="' + data[i].UserId + '" data-empid="' + data[i].EmpId + '"  data-tickettype="' + data[i].AppointmentType + '" data-appdate="' + data[i].AppointmentDate + '" data-appointmentid="' + data[i].AppointmentId + '" data-appstarttime="' + data[i].AppointmentStartTime + '"data-appendtime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" ></div ></div>';
                            }
                            else {
                                divhtml += '<span class="bothmargin-5px">' + data[i].TicketId + '</span><span><a href="javascript:void(0)" class="bothmargin-4px" data-details="' + data[i].PopupDetails + '" onclick = "ShowDetailsPopup(event)"  title="Details Information" id="' + data[i].TicketId + '"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
                                if (data[i].IsCalled && data[i].CellNo != '' && data[i].CellNo != null) {
                                    divhtml += '<a href="tel:' + data[i].CellNo + '" onclick = "StopAllClickEvent(event)" title="Call Ahead" class="call-cls cus-anchor"><i style="color: #2ca01c" class="fa fa-phone-square" aria-hidden="true"></i></a>'
                                }
                                divhtml += '</span>' + imagefile + '</div><div class="vertical-ResizingBorder" style="cursor: row-resize!important;" id="Re_' + divId + '" onmousedown="TicketResizeing(event)" onclick="StopAllClickEvent(event)" data-ticketid="' + data[i].TicketId + '" data-userid="' + data[i].UserId + '" data-empid="' + data[i].EmpId + '"  data-tickettype="' + data[i].AppointmentType + '" data-appdate="' + data[i].AppointmentDate + '" data-appointmentid="' + data[i].AppointmentId + '" data-appstarttime="' + data[i].AppointmentStartTime + '"data-appendtime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" ></div ></div>';
                            }
                            if (overlodding > 0) {
                                var TCount = parseInt($("#TimeCount").val());
                                var toppx = dynamicWidth * overlodding;                                
                                divhtml += exdiv.innerHTML;
                                document.getElementById(tdId).innerHTML = divhtml;
                                document.getElementById(divId).style.cssText = 'width: ' + dynamicWidth + 'px; margin-top:' + marginnum + 'px; margin-left: ' + toppx + 'px; text-align: center; background-color: ' + data[i].BGColor + '; height: ' + hightnum + 'px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; box-shadow: ' + shadowvalue + '; border-radius: 6px; border: ' + bordervalue + ';';
                                if (runningid == uid && toppx < toppxvalue) {
                                    toppx = toppxvalue;
                                }
                                if (runningid == uid) {
                                    toppxvalue = toppx;
                                }
                                else {
                                    runningid = uid;
                                    toppxvalue = toppx;
                                }
                                if (setWidth > 0) {
                                    toppx = toppx + setWidth;
                                }
                                else {
                                    toppx = toppx + 140;
                                }
                                for (let a = 0; a < TCount; a++) {
                                    var trId = 'td_' + uid + '_' + a;
                                    if (a == 0) {
                                        var trId = 'td_' + uid + '_full';
                                    }                                    
                                    document.getElementById(trId).style.cssText = 'min-width:' + toppx + 'px!important';
                                }
                                totalwidth += toppx;
                            }
                            else {
                                if (divcount > 0) {
                                    divhtml += exdiv.innerHTML;
                                }
                                document.getElementById(tdId).innerHTML = divhtml;
                                document.getElementById(divId).style.cssText = 'width: ' + dynamicWidth + 'px;    margin-top:' + marginnum + 'px;  text-align: center; background-color: ' + data[i].BGColor + '; height: ' + hightnum + 'px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; box-shadow: ' + shadowvalue + '; border-radius: 6px; border: ' + bordervalue + ';';
                                totalwidth += dynamicWidth;
                            }

                        }
                        else {
                            var divId = 'field_' + uid + '_full';
                            var tdId = 'td_' + uid + '_full';
                            var exdiv = document.getElementById(tdId);
                            var divhtml = '<div  id="' + divId + '" data-tId="' + data[i].TicketId + '" data-eGuid="' + data[i].UserId + '" data-EmpName="' + data[i].EmployeeName + '" data-TypeName="' + data[i].TicketTypeDisplayText + '"  data-eType="' + data[i].AppointmentType + '" data-eDate="' + data[i].AppointmentDate + '" data-duration="' + data[i].EndDivCount + '" data-aId="' + data[i].AppointmentId + '" data-stime="' + data[i].AppointmentStartTime + '" data-etime="' + data[i].AppointmentEndTime + '" data-additional="' + data[i].AdditionalMember + '" class="vertical-event task_Count" draggable="true" ondragstart="CalendarEventDragStart(this, event)" onclick="CalendarTicketUpdate(this, event)" ><div class="col-sm-12 text-left">';
                            divhtml += '<span class="bothmargin-5px">' + data[i].TicketId + '</span><span><a href="javascript:void(0)" class="bothmargin-4px" data-details="' + data[i].PopupDetails + '" onclick = "ShowDetailsPopup(event)"  title="Details Information" id="' + data[i].TicketId + '"><i class="fa fa-search-plus" aria-hidden="true"></i></a>';
                            if (data[i].IsCalled && data[i].CellNo != '' && data[i].CellNo != null) {
                                divhtml += '<a href="tel:' + data[i].CellNo + '" onclick = "StopAllClickEvent(event)" title="Call Ahead" class="call-cls cus-anchor"><i style="color: #2ca01c" class="fa fa-phone-square" aria-hidden="true"></i></a>'
                            }
                            divhtml += '</span></div></div>';
                            if (exdiv != null) {
                                divhtml += exdiv.innerHTML;
                            }
                            document.getElementById(tdId).innerHTML = divhtml;
                            document.getElementById(divId).style.cssText = 'width:' + dynamicWidth + 'px!important;  margin-top: 0px; text-align: center; background-color: ' + data[i].BGColor + '; height: 25px; font-size: ' + fontsize + 'px !important; font-family: Arial, Helvetica, sans - serif; box-shadow: ' + shadowvalue + '; border-radius: 6px; border: ' + bordervalue + ';';
                            totalwidth += dynamicWidth;
                        }
                    }
                }
            }
        }

    }
    if (setWidth > 0) {
        if (totalwidth < 80) {
            totalwidth =  usercount * setWidth + 80;
        }
        var currentwidth = $("div.calendar_color_container").innerWidth();
        if (currentwidth > totalwidth) {
            $(".vertical_calendar").css({ "width": "unset", "table-layout": "fixed" });
        }
        else {
            $(".vertical_calendar").css({ "width": "max-content" });
        }
    }
    ticketclosescroll();
}
var CalendarPreviousButton = function () {

    var viewname = $("#dailyShedule").val();
    var pd = $('#eventDate').val();
    var pred = new Date(pd);
    var zone = String(TimeZoneValue(pred));
    var firstChar = zone.charAt(0);
    if (firstChar == '-') {
        pred.setDate(pred.getDate() + 1);
    }
    if (viewname == "Daily") {
        pred.setDate(pred.getDate() - 1);
        var py = pred.getFullYear();
        var pm = (pred.getMonth() + 1).toString();
        if (pm.length == 1) { pm = 0 + pm; }
        var pday = (pred.getDate()).toString();
        if (pday.length == 1) { pday = 0 + pday; }
        pred = py + '-' + pm + '-' + pday;
    }
    if (viewname == "Weekly" || viewname == "List") {
        pred.setDate(pred.getDate() - 7);
        var py = pred.getFullYear();
        var pm = (pred.getMonth() + 1).toString();
        if (pm.length == 1) { pm = 0 + pm; }
        var pday = (pred.getDate()).toString();
        if (pday.length == 1) { pday = 0 + pday; }
        pred = py + '-' + pm + '-' + pday;
    }
    if (viewname == "Monthly") {
        pred.setMonth(pred.getMonth() - 1);
        var py = pred.getFullYear();
        var pm = (pred.getMonth() + 1).toString();
        if (pm.length == 1) { pm = 0 + pm; }
        pred = py + '-' + pm + '-01';
    }
    if ($("#SelectedDateReload").val() == "true" && viewname == "Daily") {
        window.location.href = domainurl + '/calendar?ticketdate=' + pred;
    }
    else {
        $('#eventDate').val(pred);
        var node = document.getElementById("tablebing");
        node.innerHTML = "";
        GetAllCalendarData(pred, viewname, typeval, UserVal, UserSkills);
        $(".LoaderWorkingDiv").show();
    }
}
var CalendarNextButton = function () {
    var viewname = $("#dailyShedule").val();
    var nd = $('#eventDate').val();
    var nxd = new Date(nd);
    var zone = String(TimeZoneValue(nxd));
    var firstChar = zone.charAt(0);
    if (firstChar == '-') {
        nxd.setDate(nxd.getDate() + 1);
    }
    if (viewname == "Daily") {
        nxd.setDate(nxd.getDate() + 1);
        var ny = nxd.getFullYear();
        var nm = (nxd.getMonth() + 1).toString();
        if (nm.length == 1) { nm = 0 + nm; }
        var nday = (nxd.getDate()).toString();
        if (nday.length == 1) { nday = 0 + nday; }
        nxd = ny + '-' + nm + '-' + nday;
    }
    if (viewname == "Weekly" || viewname == "List") {
        nxd.setDate(nxd.getDate() + 7);
        var ny = nxd.getFullYear();
        var nm = (nxd.getMonth() + 1).toString();
        if (nm.length == 1) { nm = 0 + nm; }
        var nday = (nxd.getDate()).toString();
        if (nday.length == 1) { nday = 0 + nday; }
        nxd = ny + '-' + nm + '-' + nday;
    }
    if (viewname == "Monthly") {
        nxd.setMonth(nxd.getMonth() + 1);
        var ny = nxd.getFullYear();
        var nm = (nxd.getMonth() + 1).toString();
        if (nm.length == 1) { nm = 0 + nm; }
        nxd = ny + '-' + nm + '-01';
    }

    $('#eventDate').val(nxd);
    if ($("#SelectedDateReload").val() == "true" && viewname == "Daily") {
        window.location.href = domainurl + '/calendar?ticketdate=' + nxd;
    }
    else {
        var node = document.getElementById("tablebing");
        node.innerHTML = "";
        GetAllCalendarData(nxd, viewname, typeval, UserVal, UserSkills);
        $(".LoaderWorkingDiv").show();
    }
}
var CalendarDateChange = function (date) {
    var node = document.getElementById("tablebing");
    node.innerHTML = "";
    var selecteddate = date.value;
    var viewname = $("#dailyShedule").val();
    if ($("#SelectedDateReload").val() == "true" && viewname == "Daily") {
        window.location.href = domainurl + '/calendar?ticketdate=' + selecteddate;
    }
    else {
        GetAllCalendarData(selecteddate, viewname, typeval, UserVal, UserSkills);
        $(".LoaderWorkingDiv").show();
    }
}
var CalendarDayViewButtonClick = function () {
    var viewname = "Daily";
    $("#dailyShedule").val(viewname);
    var pd = $('#eventDate').val();
    $('#eventDate').val(pd);
    var node = document.getElementById("tablebing");
    node.innerHTML = "";
    var value = $("#ListEmployee").val();
    var typeval = encodeURIComponent($("#ListTicketType").val());
    var skill = encodeURIComponent($("#EmployeeSkills").val());
    $("#dailyShedule").val(viewname);
    $("#btnDay").addClass("active");
    $("#btnWeek").removeClass("active");
    $("#btnMonth").removeClass("active");
    $("#btnList").removeClass("active");
    $(".LoaderWorkingDiv").show();
    GetAllCalendarData(pd, viewname, typeval, value);
}
var CalendarWeekViewButtonClick = function () {
    var viewname = "Weekly";
    $("#dailyShedule").val(viewname);
    var pd = $('#eventDate').val();
    $('#eventDate').val(pd);
    var node = document.getElementById("tablebing");
    node.innerHTML = "";
    var value = $("#ListEmployee").val();
    var typeval = encodeURIComponent($("#ListTicketType").val());
    var skill = encodeURIComponent($("#EmployeeSkills").val());
    $("#dailyShedule").val(viewname);
    $("#btnDay").removeClass("active");
    $("#btnWeek").addClass("active");
    $("#btnMonth").removeClass("active");
    $("#btnList").removeClass("active");
    $(".LoaderWorkingDiv").show();
    GetAllCalendarData(pd, viewname, typeval, value, skill);
}
var CalendarMonthViewButtonClick = function () {
    var viewname = "Monthly";
    $("#dailyShedule").val(viewname);
    var pd = $('#eventDate').val();
    $('#eventDate').val(pd);
    var node = document.getElementById("tablebing");
    node.innerHTML = "";
    var value = $("#ListEmployee").val();
    var typeval = encodeURIComponent($("#ListTicketType").val());
    var skill = encodeURIComponent($("#EmployeeSkills").val());
    $("#dailyShedule").val(viewname);
    $("#btnDay").removeClass("active");
    $("#btnWeek").removeClass("active");
    $("#btnMonth").addClass("active");
    $("#btnList").removeClass("active");
    $(".LoaderWorkingDiv").show();
    GetAllCalendarData(pd, viewname, typeval, value, skill);
}
var GoToSelectedDate = function (id) {
    var view = $("#dailyShedule").val();
    var nxd = id.id.toString().split('_')[1];
    if (view == "Weekly") {
        var day = id.dataset.mytime.split('-')[0];
        var year = id.dataset.mytime.split('-')[2];
        var month = id.dataset.mytime.split('-')[1];
        if (month == "Jan") { month = '01' } else if (month == "Feb") { month = '02' } else if (month == "Mar") { month = '03' } else if (month == "Apr") { month = '04' }
        else if (month == "May") { month = '05' } else if (month == "Jun") { month = '06' } else if (month == "Jul") { month = '07' } else if (month == "Aug") { month = '08' }
        else if (month == "Sep") { month = '09' } else if (month == "Oct") { month = '10' } else if (month == "Nov") { month = '11' } else if (month == "Dec") { month = '12' }
        nxd = year + '-' + month + '-' + day;
    }
    var viewname = "Daily";
    if ($("#SelectedDateReload").val() == "true" && viewname == "Daily") {
        window.location.href = domainurl + '/calendar?ticketdate=' + nxd;
    }
    else {
        $("#dailyShedule").val(viewname);
        $('#eventDate').val(nxd);
        var node = document.getElementById("tablebing");
        node.innerHTML = "";
        $("#dailyShedule").val(viewname);
        $("#btnDay").addClass("active");
        $("#btnWeek").removeClass("active");
        $("#btnMonth").removeClass("active");
        $("#btnList").removeClass("active");
        $(".LoaderWorkingDiv").show();
        GetAllCalendarData(nxd, viewname, typeval, UserVal, UserSkills);
    }

}
var CalendarTicketUpdate = function (id, ev) {
    var resizing = $("#CalendarResizing").val();
    if (resizing != "yes" && IsClickedPermition.toLowerCase() == 'true') {
        var Leadid = ev.currentTarget.dataset.tid;
        if (Leadid != null && Leadid != "") {
            OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?Id=" + Leadid);
        }
    }
    ev.stopPropagation();
    ev.cancelBubble = true;
}
var CalendarTicketCreate = function (id, ev) {
    if ($("#EventMovePermission").val() == 'True' || $("#IsSupervisorPermission").val() == 'True') {
        var resizing = $("#CalendarResizing").val();
        if (resizing != "yes" && IsClickedPermition.toLowerCase() == 'true') {
            var date = $('#eventDate').val();
            var viewname = $("#dailyShedule").val();
            var Eventresid = ev.currentTarget.dataset.empintid;
            var time = ev.currentTarget.dataset.mytime;
            var hour = time.split(":")[0];
            var minutes = time.split(":")[1].split(" ")[0];
            var tt = time.split(" ")[1];
            if (tt == "PM" && hour != '12') {
                hour = 12 + parseInt(hour);
                if (hour == "24") {
                    hour = "00";
                }
            }
            var customerid = "00000000-0000-0000-0000-000000000000";
            var TicketIntId = $("#CustomCalendarTictekIntId").val();
            var CustomerGuidId = $("#CustomCalendarCustomerGuidId").val();
            if (!isNaN(parseInt(TicketIntId)) && parseInt(TicketIntId) > 0 && CustomerGuidId != customerid) {
                OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?loadDate=" + date + "&CustomerId=" + CustomerGuidId + "&startTime=" + hour + ':' + minutes + "&UserId=" + Eventresid + "&Id=" + TicketIntId);
            }
            else {
                if (IsNewTicketCreated == 'True') {
                    var eventloaddate = date + "T" + hour + ':' + minutes + ':00Z';
                    UserPermissionForCreateTicket(Eventresid, eventloaddate, CustomerGuidId, TicketIntId);
                }
            }
        }
    }
}
var StopAllClickEvent = function (ev) {
    ev.stopPropagation();
    ev.cancelBubble = true;
}
var UserPermissionForCreateTicket = function (Eventresid, eventloaddate, customerid, ticketid) {
    var todaydate = new Date();
    var month = todaydate.getMonth() + 1;
    var day = todaydate.getDate();
    var date = todaydate.getFullYear() + "-" + (month < 10 ? '0' : '') + month + "-" + (day < 10 ? '0' : '') + day + "T00:00:00Z";
    var url = domainurl + "/Calendar/UserPermissionForCreateTicket";
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
                        OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?loadDate=" + splitLoadDate[0] + "&CustomerId=" + customerid + "&UserId=" + Eventresid + "&Id=" + ticketid);
                    }
                    else {
                        if (eventloaddate >= date) {

                            OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?loadDate=" + splitLoadDate[0] + "&CustomerId=" + customerid + "&UserId=" + Eventresid + "&Id=" + ticketid);
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
                            OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?loadDate=" + splitLoadDate[0] + "&CustomerId=" + customerid + "&startTime=" + LoadTime + "&UserId=" + Eventresid + "&Id=" + ticketid);
                        }
                        else {
                            OpenErrorMessageNew("Error!", "Schedule date should be greater than today's date.", "");
                        }
                    }
                }
            }
            else {
                OpenErrorMessageNew("Error", "Sorry, please try again later.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var calendarerrorshow = function (ev) {
    var errorid = ev.id;
    errorid = 'errpopup_' + errorid;
    var modal = document.getElementById(errorid);
    if (modal.style.display == 'block') {
        modal.style.display = "none";
    }
    else {
        modal.style.display = "block";
    }
}
var calendarerrorclose = function (ev) {
    var closeid = ev.id,
        errorid = closeid.split('_')[1];
    errorid = 'errpopup_' + errorid;
    var modal = document.getElementById(errorid);
    modal.style.display = "none";
}
var ShowDetailsPopup = function (ev) {
    var detailsData = ev.currentTarget.dataset.details;
    detailsData = '<span class="calerrorpopupclose" onclick="HidePopupDetails()">&times;</span>' + detailsData;
    var clientX = parseInt(ev.clientX);
    var clientY = parseInt(ev.clientY);
    clientX = clientX - 105;
    if (clientY > 319) {
        clientY = clientY - 265;
    }
    else {
        clientY = clientY + 10;
    }
    ev.stopPropagation();
    ev.cancelBubble = true;
    $('#detailsdata1').html('');
    $('#detailsdata1').html(detailsData);
    $("#detailspopup1").css({
        "left": clientX + "px", "top": clientY + "px"
    });
    var options = { direction: "top" };
    $('#detailspopup1').toggle('slide', options, 1);
}
var HidePopupDetails = function () {
    $('#detailsdata1').html('');
    $("#detailspopup1").hide();
}
var ShowCustomerDetailsPopup = function (ev) {
    var Name = ev.currentTarget.dataset.customername;
    var CId = ev.currentTarget.dataset.customerid;
    var detailsData = '<a target="_blank" href="/Customer/Customerdetail/?id=' + CId + '" onclick="StopAllClickEvent(event)"><p class="cus-anchor"><b>Customer Name: </b>' + Name + '</p></a>';
    detailsData = '<span class="calerrorpopupclose" onclick="HidePopupInfo()">&times;</span>' + detailsData;
    var clientY = parseInt(ev.clientY);
    var clientX = parseInt(ev.clientX);
    clientY = clientY + 7;
    clientX = clientX - 105;
    var result = window.innerHeight - clientY;
    if (result < 110) {
        clientY = clientY - 135;
    }
    ev.stopPropagation();
    ev.cancelBubble = true;
    $('#detailsdata2').html('');
    $('#detailsdata2').html(detailsData);
    $("#detailspopup2").css({
        "left": clientX + "px", "top": clientY + "px"
    });
    var options = { direction: "top" };
    $('#detailspopup2').toggle('slide', options, 1);
}
var ShowDirectionDetailsPopup = function (ev) {
    var Data = ev.currentTarget.dataset.address;
    DestinationCusAddress = Data;
    var street = ev.currentTarget.dataset.street;
    var city = ev.currentTarget.dataset.city;
    if (street != null && street != '' && street != 'undefined') { city = street + ', ' + city; }
    var detailsData = '<a onclick="GetDirection()"><p class="cus-anchor"><b>Address: </b>' + city + '</p></a>';
    detailsData = '<span class="calerrorpopupclose" onclick="HidePopupInfo()">&times;</span>' + detailsData;
    var clientY = parseInt(ev.clientY);
    var clientX = parseInt(ev.clientX);
    clientY = clientY + 7;
    clientX = clientX - 105;
    var result = window.innerHeight - clientY;
    if (result < 110) {
        clientY = clientY - 135;
    }
    ev.stopPropagation();
    ev.cancelBubble = true;
    $('#detailsdata2').html('');
    $('#detailsdata2').html(detailsData);
    $("#detailspopup2").css({
        "left": clientX + "px", "top": clientY + "px"
    });
    var options = { direction: "top" };
    $('#detailspopup2').toggle('slide', options, 1);
}
var HidePopupInfo = function () {
    $('#detailsdata2').html('');
    $("#detailspopup2").hide();
}
var calendardetailspopupclose = function (ev) {
    var closeid = ev.currentTarget.id;
    ev.stopPropagation();
    ev.cancelBubble = true;
    closeid = 'details' + closeid;
    var modal = document.getElementById(closeid);
    modal.style.display = "none";
}
var editErrorTicket = function (id) {
    if (id != null && id != "") {
        OpenTopToBottomModal(domainurl + "/Ticket/AddTicket/?Id=" + id);
    }
}
var geocoder, currentformatedAddress, DestinationCusAddress;
function successFunction(position) {
    var lat = position.coords.latitude;
    var lng = position.coords.longitude;
    codeLatLng(lat, lng);
}
function errorFunction() {
    alert("Geocoder failed");
}
function initialize() {
    $('.tt-menu').hide();
    geocoder = new google.maps.Geocoder();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successFunction, errorFunction);
    }
}
function codeLatLng(lat, lng) {

    var latlng = new google.maps.LatLng(lat, lng);
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                for (var i = 0; i < results[0].address_components.length; i++) {
                    for (var b = 0; b < results[0].address_components[i].types.length; b++) {

                        //there are different types that might hold a city admin_area_lvl_1 usually does in come cases looking for sublocality type will be more appropriate
                        if (results[0].address_components[i].types[b] == "locality") {
                            //this is the object you are looking for
                            currentformatedAddress = results[0].formatted_address;
                            window.open("https://www.google.com/maps?saddr=" + currentformatedAddress.replace(",", "").replace(" ", "+") + "&daddr=" + DestinationCusAddress.replace(",", "").replace(" ", "+"), "_blank");
                            break;
                        }
                    }
                }
            } else {
                alert("No results found");
            }
        } else {
            alert("Geocoder failed due to: " + status);
        }
    });
}
var GetDirection = function (ev) {
    var Data = ev.currentTarget.dataset.address;
    DestinationCusAddress = Data;
    initialize();
}
var ImageHide = function (ev) {
    var HideId = ev.target.id;
    var model = document.getElementById(HideId);
    model.style.opacity = 0;
    model.style.display = "none";
}
var TimeZoneValue = function (dt) {
    return (-dt.getTimezoneOffset() < 0 ? '-' : '+') + (Math.abs(dt.getTimezoneOffset() / 60) < 10 ? '0' : '') + (Math.abs(dt.getTimezoneOffset() / 60)) + '00';
}
var EditDateHeader = function (ev) {
    $('#DateTilteHide').hide();
    $('#DateTitleShow').show();
   //var evdt = new Pikaday({
   //     field: $('#eventDate')[0]
   // });
   // ev.stopPropagation();
   // ev.cancelBubble = true;
}
var CloseDateHeader = function () {
    $('#DateTitleShow').hide();
    $('#DateTilteHide').show();
}
var OpenBkById = function (bkId, cusId) {
    if (typeof (bkId) != "undefined" && bkId > 0) {
        if (typeof (cusId) == "undefined") {
            cusId = 0;
        }
        OpenTopToBottomModal("/Booking/AddLeadBooking/?customerid=" + cusId + "&Id=" + bkId);
    }
}

var ticketclosescroll = function () {
    if (scrolluserval != '' && scrolluserval != 'null' && scrolluserval != '00000000-0000-0000-0000-000000000000' && urlcount == 1) {
        var scrollid = 'tr_' + scrolluserval;
        if ($("#CalendarViewPostion").val() == 'vertical') {
            scrollid = 'th_' + scrolluserval;
        }
        document.getElementById(scrollid).scrollIntoView({
            behavior: 'smooth'
        });
        urlcount++;
    }
}
var ResetFilter = function () {
    $(".selectpicker_skills").selectpicker('val', null);
    $(".selectpicker_type").selectpicker('val', null);
    $(".selectpicker_user").selectpicker('val', null);
    CalendarSearchFilter();
}
$(document).ready(function () {
    var startTime, endTime, startHour, endHour, startMin, endMin, currentHour, currentMin, totalPixle, nowTime = new Date();

    var evdt = new Pikaday({
        field: $('#eventDate')[0]
    });


    $("#search-div").hide();
    var EventSizing = parseInt($("#EventTicketResize").val());
    if (window.innerWidth < 761) {

        $("#CustomCalendarFontSize").val('12');
        $("#EventIconResizer").val('25');
        if (EventSizing > 100) {
            $("#EventTicketResize").val('120');
        }
    }
    var TktHeight = parseInt($("#CalendarHeight").val());
    if (isNaN(TktHeight)) { TktHeight = 0; }
    marginTop = (TktHeight / 60);
    currentHour = parseInt(nowTime.getHours());
    currentMin = parseInt(nowTime.getMinutes());
    startTime = $("#ScheduleCalendarMinTimeRange").val();
    endTime = $("#ScheduleCalendarMaxTimeRange").val();
    startHour = parseInt(startTime.split(':')[0]);
    endHour = parseInt(endTime.split(':')[0]);
    startMin = parseInt(startTime.split(':')[1]);
    endMin = parseInt(endTime.split(':')[1]);
    var timediff = endHour - startHour;
    var mindiff = endMin - startMin;
    totalPixle = (timediff * TktHeight);
    console.log('hgf');
    if (IsHideFullDay.toLowerCase() == 'true') {
    }
    else {
        totalPixle += TktHeight;
        pixle = TktHeight;
    }
    if (startMin <= endMin) {        
        totalPixle += mindiff * marginTop;
    }
    else {
        totalPixle = totalPixle - (mindiff * marginTop * -1);
    }
    $("#TotalPix").val(totalPixle);

    if (startHour <= currentHour && endHour >= currentHour) {
        if (currentHour == endHour && currentMin >= endMin) { pixle = 0; }
        else {
            currentHour = currentHour - startHour;
            if (currentHour > 0) {
                currentMin = currentMin - startMin;
                if (currentMin >= 0) {
                    pixle += currentHour * TktHeight;
                    pixle += marginTop * currentMin;
                }
                else {
                    pixle += currentHour * TktHeight;
                    pixle = pixle - (marginTop * currentMin * -1);
                }


            }
            else {
                currentMin = currentMin - startMin;
                if (currentMin > 0) {
                    pixle += marginTop * currentMin;
                }
                else {
                    pixle = 0;
                }
            }
        }
        $("#CurrentBar").val(pixle);
    }
    else {
        $("#CurrentBar").val(0);
    }

    $(".filter-class").click(function () {
        var source = $(this).attr("data-filter");
        GetAllCalendarData($('#eventDate').val(), $("#dailyShedule").val(), source, $("#ListEmployee").val(), $("#EmployeeSkills").val());
    });

    $("#btn_filter_taggol").click(function () {
        $("#search-div").toggle();
    });

    $('#tablebing').height(window.innerHeight - 158 - 82);
    $('.page-wrapper-contents').css('overflow', 'scroll');
    $(".selectpicker_skills").selectpicker('val', UserSkills);
    $(".selectpicker_user").selectpicker('val', UserVal);
    $(".selectpicker_type").selectpicker('val', typeval);
    $('#eventDate').val(selectrdDate);
    if (defview == "List") { defview = "Weekly"; }
    if (IsCalendarShowPermission == 'False' && IsCalendarShow == 'False') {
        OpenErrorMessageNew("Access denied", "You do not have permission to view calendar", function () { window.location.href = domainurl + "/dashboard" });
    }
    else {
        GetAllCalendarData(selectrdDate, defview, typeval, UserVal, UserSkills);
    }
    $('#DateTilteHide').show();
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
    $("#show_schedule_map").click(function () {
        console.log("Map");
        var SelectedDate = $('#eventDate').val();
        var todaydate = new Date();
        var hour = todaydate.getHours();
        var minutes = todaydate.getMinutes();
        var date = SelectedDate + "T" + (hour < 10 ? '0' : '') + hour + ":" + (minutes < 10 ? '0' : '') + minutes + ":00";
        var tickettype = $("#ListTicketType").val();
        var ticketuser = $("#ListEmployee").val();
        var userskill = $("#EmployeeSkills").val();
        $(".ScheduleMapPopUp").attr('href', domainurl + "/Calendar/ScheduleGoogleMap?date=" + date + "&type=" + tickettype + "&user=" + ticketuser + "&skills=" + userskill);
        $(".ScheduleMapPopUp").click();
    })
    $("#btn_ticket_status_setup").click(function () {
        window.open(domainurl + "/calendarsetup", "_blank");
    })
    var setview = $("#dailyShedule").val();
    if (setview == "Monthly") {
        $("#btnDay").removeClass("active");
        $("#btnWeek").removeClass("active");
        $("#btnMonth").addClass("active");
    }
    else if (setview == "Daily") {
        $("#btnDay").addClass("active");
        $("#btnWeek").removeClass("active");
        $("#btnMonth").removeClass("active");
    }
    else {
        $("#btnDay").removeClass("active");
        $("#btnWeek").addClass("active");
        $("#btnMonth").removeClass("active");
    }
    $("#btnAddPto").click(function () {
        if (IsPermissionToAddDayOff.toLowerCase() == 'true') {
            OpenRightToLeftModal(domainurl + "/TimeClockPto/AddEmployeesPto");
        }
        else {
            OpenRightToLeftModal(domainurl + "/Calendar/AddPtoPartial");
        }
    });
    
});
$(window).resize(function () {
    $('#tablebing').height(window.innerHeight - 158)
});