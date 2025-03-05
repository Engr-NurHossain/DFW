var dayValue = "monthly";
var lblval = "lead";
var date = new Date();
var upsoldDateFilter = "All";
var OpenTicketsDateFilter = "All";

/*Lead Daily Filter*/
var dailydate = date.getMonth() + "/" + date.getDate() + "/" + date.getFullYear();
var firstdate = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
var max = dailydate.split(/\D+/);
var maxdate = new Date(max[2], max[0], (parseInt(max[1]) + 1));
var lastdate = maxdate.getMonth() + 1 + "/" + maxdate.getDate() + "/" + maxdate.getFullYear();
var previousmindate = new Date(max[2], max[0], (parseInt(max[1]) - 1))
var previousfirstdate = previousmindate.getMonth() + 1 + "/" + previousmindate.getDate() + "/" + previousmindate.getFullYear();
/*Lead Daily Filter*/
/*Lead Weekly Filter*/
var weeklydate = dailydate.split(/\D+/);
var weeklymaxdate = new Date(weeklydate[2], weeklydate[0], (parseInt(weeklydate[1]) - 7));
var weeklylastdate = weeklymaxdate.getMonth() + 1 + "/" + weeklymaxdate.getDate() + "/" + weeklymaxdate.getFullYear();
/*Lead Weekly Filter*/
/*Lead Monthly Filter*/
var monthlydate = dailydate.split(/\D+/);
var monthlymaxdate = new Date(monthlydate[2], monthlydate[0], (parseInt(monthlydate[1]) - 31));
var monthlylastdate = monthlymaxdate.getMonth() + 1 + "/" + 1 + "/" + monthlymaxdate.getFullYear();
/*Lead Monthly Filter*/
/*Lead Yearly Filter*/
var yearlydate = dailydate.split(/\D+/);
var yearlymaxdate = new Date(yearlydate[2], yearlydate[0], (parseInt(yearlydate[1]) - 365));
var yearlylastdate = yearlymaxdate.getMonth() + 1 + "/" + yearlymaxdate.getDate() + "/" + yearlymaxdate.getFullYear();
/*Lead Yearly Filter*/
/*This Week Date*/
var first = date.getDate() - date.getDay();
var last = first + 6;
var firstday = new Date(date.setDate(first));
var lastday = new Date(date.setDate(last));
thisfirstdate = firstday.getMonth() + 1 + "/" + firstday.getDate() + "/" + firstday.getFullYear();
thislastdate = lastday.getMonth() + 1 + "/" + lastday.getDate() + "/" + lastday.getFullYear();
/*This Week Date*/
/*This Month Date*/
var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
thisfirstmonthdate = firstDay.getMonth() + 1 + "/" + firstDay.getDate() + "/" + firstDay.getFullYear();
thislastmonthdate = lastDay.getMonth() + 1 + "/" + lastDay.getDate() + "/" + lastDay.getFullYear();
/*This Month Date*/
/*This Year Date*/
var firstyearday = new Date("1/1/" + date.getFullYear());
var lastyearday = new Date("12/31/" + date.getFullYear());
thisfirstyeardate = firstyearday.getMonth() + 1 + "/" + firstyearday.getDate() + "/" + firstyearday.getFullYear();
thislastyeardate = lastyearday.getMonth() + 1 + "/" + lastyearday.getDate() + "/" + lastyearday.getFullYear();
/*This Year Date*/
var CustomerLeadGraphInitLoad = function (start, end, lblval) {
    $(".Customer_Lead_Graph").addClass('hidden');
    $(".Customer_Lead_Graph_loader").removeClass('hidden');
    setTimeout(function () {
        var url = encodeURI(AppConfigDomainSitePath + "/App/DashBoardCustomerLeadGraph/?StartDateTime=" + start + "&&EndDateTime=" + end + "&keyvalue=" + dayValue + "&labelvalue=" + lblval)
        $(".Customer_Lead_Graph").load(url);
    }, 4000);
}
var DashBoardServiceBoardListInitLoad = function (start, end) {
    $(".dashboard-service-board-list").addClass('hidden');
    $(".service-board-loader").removeClass('hidden');
    setTimeout(function () {
        var url = encodeURI("/App/DashBoardServiceBoardList/?StartDateRange=" + start + "&&EndDateRange=" + end)
        $(".dashboard-service-board-list").load(url);
    }, 4000);
}

var TechnicianDashboardDataWithDateFiltering = function (firstdate, lastdate, previousfirstdate, SearchKey) {
    var url = domainurl + "/App/TechnicianDashboardDataWithDateFiltering";
    var param = JSON.stringify({
        firstdate: firstdate,
        lastdate: lastdate,
        previousfirstdate: previousfirstdate,
        SearchKey: SearchKey
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
            if (data.result == true) {
                console.log(data);
                if (data.SearchKey == "TotalOpenTicketDaily") {
                    $(".daily_lead").removeClass('hidden');
                    $(".monthly_lead").addClass('hidden');
                    $(".weekly_lead").addClass('hidden');
                    $(".yearly_lead").addClass('hidden');
                    $(".daily_lead_Total").text("Total Open Ticket Today: " + data.TotalOpenInstallationTicket);
                    $(".daily_lead_thisday_body").text("Installation Ticket: " + data.OpenInstallationTicket);
                    $(".daily_lead_lastday_body").text("Service Ticket: " + data.OpenServiceTicket);
                    //$(".lead_filter_list").click(function () {
                    //    window.location.href = "/Lead?firstdate=" + data.firstdate + "&lastdate=" + data.lastdate;
                    //})
                }
                else if (data.SearchKey == "TotalClosedTicketDaily") {
                    $(".daily_Total_Closed").removeClass('hidden');
                    $(".monthly_Total_Closed").addClass('hidden');
                    $(".weekly_Total_Closed").addClass('hidden');
                    $(".yearly_Total_Closed").addClass('hidden');
                    $(".daily_Total_Closed_Total").text("Total Closed Ticket Today: " + data.TotalClosedInstallationTicket);
                    $(".daily_Total_Closed_thisday_body").text("Installation Ticket: " + data.ClosedInstallationTicket);
                    $(".daily_Total_Closed_lastday_body").text("Service Ticket: " + data.ClosedServiceTicket);
                    //$(".Total_Closed_filter_list").click(function () {
                    //    window.location.href = "/Customer?firstdate=" + data.firstdate + "&lastdate=" + data.lastdate;
                    //})
                }
                else if (data.SearchKey == "TotalUpsoldDaily") {
                    $(".daily_TotalUpsold").removeClass('hidden');
                    $(".monthly_TotalUpsold").addClass('hidden');
                    $(".weekly_TotalUpsold").addClass('hidden');
                    $(".yearly_TotalUpsold").addClass('hidden');
                    $(".daily_TotalUpsold_Total").text("Total Upsold Cost Today: " + Currency + data.TotalUpsoldTotalPrice);
                    $(".daily_TotalUpsold_thisday_body").text("Services: " + Currency + data.UpsoldServicesTotalPrice);
                    $(".daily_TotalUpsold_lastday_body").text("Equipment: " + Currency + data.UpsoldEquipmentsTotalPrice);


                    //$(".Activities_filter_list").click(function () {
                    //    window.location.href = "/Customer?firstdate=" + data.firstdate + "&lastdate=" + data.lastdate;
                    //})
                }


                else if (data.SearchKey == "TotalOpenTicketWeekly") {
                    $(".daily_lead").addClass('hidden');
                    $(".monthly_lead").addClass('hidden');
                    $(".weekly_lead").removeClass('hidden');
                    $(".yearly_lead").addClass('hidden');
                    $(".weekly_lead_Total").text("Total Open Ticket This Week: " + +data.TotalOpenInstallationTicket);
                    $(".weekly_lead_thisday_body").text("Installation Ticket: " + data.OpenInstallationTicket);
                    $(".weekly_lead_lastday_body").text("Service Ticket: " + data.OpenServiceTicket);
                    //$(".lead_filter_list").click(function () {
                    //    window.location.href = "/Lead?firstdate=" + data.firstdate + "&lastdate=" + data.lastdate;
                    //})
                }
                else if (data.SearchKey == "TotalClosedTicketWeekly") {
                    $(".daily_Total_Closed").addClass('hidden');
                    $(".monthly_Total_Closed").addClass('hidden');
                    $(".weekly_Total_Closed").removeClass('hidden');
                    $(".yearly_Total_Closed").addClass('hidden');
                    $(".weekly_Total_Closed_Total").text("Total Closed Ticket This Week: " + data.TotalClosedInstallationTicket);
                    $(".weekly_Total_Closed_thisday_body").text("Installation Ticket: " + data.ClosedInstallationTicket);
                    $(".weekly_Total_Closed_lastday_body").text("Service Ticket: " + data.ClosedServiceTicket);
                    //$(".customer_filter_list").click(function () {
                    //    window.location.href = "/Customer?firstdate=" + data.firstdate + "&lastdate=" + data.lastdate;
                    //})
                }
                else if (data.SearchKey == "TotalUpsoldWeekly") {
                    $(".daily_TotalUpsold").addClass('hidden');
                    $(".monthly_TotalUpsold").addClass('hidden');
                    $(".weekly_TotalUpsold").removeClass('hidden');
                    $(".yearly_TotalUpsold").addClass('hidden');
                    $(".weekly_TotalUpsold_Total").text("Total Upsold Cost This Week: " + Currency + data.TotalUpsoldTotalPrice);
                    $(".weekly_TotalUpsold_thisday_body").text("Services: " + Currency + data.UpsoldServicesTotalPrice);
                    $(".weekly_TotalUpsold_lastday_body").text("Equipment: " + Currency + data.UpsoldEquipmentsTotalPrice);

                }


                else if (data.SearchKey == "TotalOpenTicketMonthly") {
                    $(".daily_lead").addClass('hidden');
                    $(".monthly_lead").removeClass('hidden');
                    $(".weekly_lead").addClass('hidden');
                    $(".yearly_lead").addClass('hidden');
                    $(".monthly_lead_Total").text("Total Open Ticket This Month: " + data.TotalOpenInstallationTicket);
                    $(".monthly_lead_thisday_body").text("Installation Ticket: " + data.OpenInstallationTicket);
                    $(".monthly_lead_lastday_body").text("Service Ticket: " + data.OpenServiceTicket);

                }
                else if (data.SearchKey == "TotalClosedTicketMonthly") {
                    $(".daily_Total_Closed").addClass('hidden');
                    $(".monthly_Total_Closed").removeClass('hidden');
                    $(".weekly_Total_Closed").addClass('hidden');
                    $(".yearly_Total_Closed").addClass('hidden');
                    $(".monthly_Total_Closed_Total").text("Total Closed Ticket This Month: " + data.TotalClosedInstallationTicket);
                    $(".monthly_Total_Closed_thisday_body").text("Installation Ticket: " + data.ClosedInstallationTicket);
                    $(".monthly_Total_Closed_lastday_body").text("Service Ticket: " + data.ClosedServiceTicket);

                }
                else if (data.SearchKey == "TotalUpsoldMonthly") {
                    $(".daily_TotalUpsold").addClass('hidden');
                    $(".monthly_TotalUpsold").removeClass('hidden');
                    $(".weekly_TotalUpsold").addClass('hidden');
                    $(".yearly_TotalUpsold").addClass('hidden');
                    $(".monthly_TotalUpsold_Total").text("Total Upsold Cost This Month: " + Currency + data.TotalUpsoldTotalPrice);
                    $(".monthly_TotalUpsold_thisday_body").text("Services: " + Currency + data.UpsoldServicesTotalPrice);
                    $(".monthly_TotalUpsold_lastday_body").text("Equipment: " + Currency + data.UpsoldEquipmentsTotalPrice);

                }


                else if (data.SearchKey == "TotalOpenTicketYearly") {
                    $(".daily_lead").addClass('hidden');
                    $(".monthly_lead").addClass('hidden');
                    $(".weekly_lead").addClass('hidden');
                    $(".yearly_lead").removeClass('hidden');
                    $(".yearly_lead_Total").text("Total Open Ticket This Year: " + data.TotalOpenInstallationTicket);
                    $(".yearly_lead_thisday_body").text("Installation Ticket:" + data.OpenInstallationTicket);
                    $(".yearly_lead_lastday_body").text("Service Ticket: " + data.OpenServiceTicket);

                }
                else if (data.SearchKey == "TotalClosedTicketYearly") {
                    $(".daily_Total_Closed").addClass('hidden');
                    $(".monthly_Total_Closed").addClass('hidden');
                    $(".weekly_Total_Closed").addClass('hidden');
                    $(".yearly_Total_Closed").removeClass('hidden');
                    $(".yearly_Total_Closed_Total").text("Total Closed Ticket This Year: " + data.TotalClosedInstallationTicket);
                    $(".yearly_Total_Closed_thisday_body").text("Installation Ticket: " + data.ClosedInstallationTicket);
                    $(".yearly_Total_Closed_lastday_body").text("Service Ticket: " + data.ClosedServiceTicket);

                }
                else if (data.SearchKey == "TotalUpsoldYearly") {
                    $(".daily_TotalUpsold").addClass('hidden');
                    $(".monthly_TotalUpsold").addClass('hidden');
                    $(".weekly_TotalUpsold").addClass('hidden');
                    $(".yearly_TotalUpsold").removeClass('hidden');
                    $(".yearly_TotalUpsold_Total").text("Total Upsold Cost This Year: " + Currency + data.TotalUpsoldTotalPrice);
                    $(".yearly_TotalUpsold_thisday_body").text("Services: " + Currency + data.UpsoldServicesTotalPrice);
                    $(".yearly_TotalUpsold_lastday_body").text("Equipment: " + Currency + data.UpsoldEquipmentsTotalPrice);

                }

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}


var DashboardInstallationListInitLoad = function (start, end) {
    $(".dashboard-Installation-list").addClass('hidden');
    $(".installer-board-loader").removeClass('hidden');
    setTimeout(function () {
        var url = encodeURI(AppConfigDomainSitePath + "/App/DashBoardInstallationList/?StartDateRange=" + start + "&&EndDateRange=" + end)
        $(".dashboard-Installation-list").load(url);
    }, 4000);

}
var ServiceBoardDataByDate = function (dayVal) {
    var StartDate;
    var EndDate;
    var CurrentDate = new Date().toISOString().slice(0, 10);
    if (dayVal == "today") {
        var EDate = new Date();
        EDate.setDate(EDate.getDate() + 1);
        StartDate = new Date().toISOString().slice(0, 10);
        EndDate = EDate.toISOString().slice(0, 10);
    }
    if (dayVal == "weekly") {
        var SDate = new Date();
        SDate.setDate(SDate.getDate() - 7);
        StartDate = SDate.toISOString().slice(0, 10);
        EndDate = CurrentDate;
    }
    if (dayVal == "monthly") {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        StartDate = firstDay.getFullYear() + '-' + (firstDay.getMonth() + 1) + '-' + (firstDay.getDate());
        EndDate = lastDay.getFullYear() + '-' + (lastDay.getMonth() + 1) + '-' + (lastDay.getDate());
    }
    var url = encodeURI(AppConfigDomainSitePath + "/App/DashBoardServiceBoardList/?StartDateRange=" + StartDate + "&&EndDateRange=" + EndDate);
    $(".dashboard-service-board-list").load(url);
}
var InstallationBoardDataByDate = function (dayVal) {
    var StartDate;
    var EndDate;
    var CurrentDate = new Date().toISOString().slice(0, 10);

    if (dayVal == "today") {
        var EDate = new Date();
        EDate.setDate(EDate.getDate() + 1);
        StartDate = new Date().toISOString().slice(0, 10);
        EndDate = EDate.toISOString().slice(0, 10);
    }
    if (dayVal == "weekly") {
        var SDate = new Date();
        SDate.setDate(SDate.getDate() - 7);
        StartDate = SDate.toISOString().slice(0, 10);
        EndDate = CurrentDate;
    }
    if (dayVal == "monthly") {
        var date = new Date();
        var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
        var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
        StartDate = firstDay.getFullYear() + '-' + (firstDay.getMonth() + 1) + '-' + (firstDay.getDate());
        EndDate = lastDay.getFullYear() + '-' + (lastDay.getMonth() + 1) + '-' + (lastDay.getDate());
    }
    var url = encodeURI(AppConfigDomainSitePath + "/App/DashBoardInstallationList/?StartDateRange=" + StartDate + "&&EndDateRange=" + EndDate)
    $(".dashboard-Installation-list").load(url);
}
var DataByDate = function (dayVal, labelvalue) {
    var StartDate;
    var EndDate;
    var CurrentDate = new Date().toISOString().slice(0, 10);

    if (dayVal == "today") {
        StartDate = firstdate;
        EndDate = lastdate;
    }
    if (dayVal == "weekly") {
        StartDate = thisfirstdate;
        EndDate = thislastdate;
    }
    if (dayVal == "monthly") {
        StartDate = thisfirstmonthdate;
        EndDate = thislastmonthdate;
    }
    if (dayVal == "yearly") {
        StartDate = thisfirstyeardate;
        EndDate = thislastyeardate;
    }

    var url = encodeURI(AppConfigDomainSitePath + "/App/DashBoardCustomerLeadGraph/?StartDateTime=" + StartDate + "&&EndDateTime=" + EndDate + "&keyvalue=" + dayVal + "&labelvalue=" + labelvalue);
    $(".Customer_Lead_Graph").load(url);
}
var ShowTechTicket = function () {
    window.open("/TechnesianTickets", '_blank');

}
var ShowTechClosedTicket = function () {
    window.open("/TechnesianCloseTickets", '_blank');

}
var ShowTechOpenTicket = function () {
    window.open("/TechnesianOpenTickets/" + OpenTicketsDateFilter, '_blank');

}

var LoadTechEquipmentList = function () {
    window.open("/Tech/EquipmentList", '_blank');

}
var LoadTechUpsoldtList = function () {

    //"LeadsVerificationDetail/?id=" + id;

    window.open("/Tech/UpsoldList/" + upsoldDateFilter, '_blank');

}


//$(".btnTotalOpenTicketdaily").click(function () {
//    TechnicianDashboardDataWithDateFiltering(firstdate, lastdate, previousfirstdate, "TotalOpenTicketDaily");
//});
//$(".btnTotalOpenTicketweekly").click(function () {
//    TechnicianDashboardDataWithDateFiltering(thisfirstdate, thislastdate, weeklylastdate, "TotalOpenTicketWeekly");
//});
//$(".btnTotalOpenTicketmonthly").click(function () {
//    TechnicianDashboardDataWithDateFiltering(thisfirstmonthdate, thislastmonthdate, monthlylastdate, "TotalOpenTicketMonthly");
//});
//$(".btnTotalOpenTicketyearly").click(function () {
//    TechnicianDashboardDataWithDateFiltering(thisfirstyeardate, thislastyeardate, yearlylastdate, "TotalOpenTicketYearly");
//});









var LoadTechServiceList = function () {
    window.open("/Tech/ServiceList", '_blank');

}

var LoadTotal90GoBackList = function () {
    console.log("LoadTotal90GoBackList is working");
    window.open("/GoBackList", '_blank');

}

var UpdateTicketCookie = function () {
    var TicketStartDate = $(".TicketStartDate").val();
    var TicketEndDate = $(".TicketEndDate").val();

    var NewCookie = String.format("{0},{1}", TicketStartDate, TicketEndDate);
    console.log(NewCookie);
    $.cookie("_TechTicketFilter", NewCookie, { expires: 1 * 60 * 24, path: '/' });
}

var UpdateJobTicketCookie = function () {
    var TicketStartDate = $(".OtherTicketStartDate").val();
    var TicketEndDate = $(".OtherTicketEndDate").val();

    var NewCookie = String.format("{0},{1}", TicketStartDate, TicketEndDate);
    console.log(NewCookie);
    $.cookie("_TechJobTicketFilter", NewCookie, { expires: 1 * 60 * 24, path: '/' });
}

$(document).ready(function () {
    var currentDate = new Date();
    var CurrentYear = currentDate.getFullYear();
    StartDate = CurrentYear + '-01-01';
    EndDate = CurrentYear + '-12-31';
    CustomerLeadGraphInitLoad(thisfirstmonthdate, thislastmonthdate, lblval);




    var EDate = new Date();
    EDate.setDate(currentDate.getDate() + 1);
    var startDateSB = new Date().toISOString().slice(0, 10);
    var endDateSB = EDate.toISOString().slice(0, 10);

    $(".TicketStartDate").val(StartDateTicket);
    $(".TicketEndDate").val(EndDateTicket);

    $(".OtherTicketStartDate").val(StartDateTicket);
    $(".OtherTicketEndDate").val(EndDateTicket);
    StartDateDatepicker = new Pikaday({
        field: $('.TicketEndDate')[0],
        format: 'MM/DD/YYYY'
    });
    EndtDateDatepicker = new Pikaday({
        field: $('.TicketStartDate')[0],
        format: 'MM/DD/YYYY'
    });

    OtherStartDateDatepicker = new Pikaday({
        field: $('.OtherTicketEndDate')[0],
        format: 'MM/DD/YYYY'
    });
    OtherEndtDateDatepicker = new Pikaday({
        field: $('.OtherTicketStartDate')[0],
        format: 'MM/DD/YYYY'
    });

    DashBoardServiceBoardListInitLoad(startDateSB, endDateSB);
    DashboardInstallationListInitLoad(startDateSB, endDateSB);
    $(".show-service-board-report").click(function () {
        $(".dashboard-service-board-list").addClass('hidden');
        $(".service-board-loader").removeClass('hidden');
        var DayValue = $(this).attr("id-val");
        setTimeout(function () {
            ServiceBoardDataByDate(DayValue);
        }, 2000);
    })
    $(".show-installation-board-report").click(function () {
        $(".dashboard-Installation-list").addClass('hidden');
        $(".installer-board-loader").removeClass('hidden');
        var DayValue = $(this).attr("id-val");
        setTimeout(function () {
            InstallationBoardDataByDate(DayValue);
        }, 2000);
    })
    $(".show-sales-report").click(function () {
        $(".Customer_Lead_Graph").addClass('hidden');
        $(".Customer_Lead_Graph_loader").removeClass('hidden');
        dayValue = $(this).attr("id-val");
        setTimeout(function () {
            DataByDate(dayValue, lblval);
        }, 2000);
    });
    $(".show-filter-label").click(function () {
        $(".Customer_Lead_Graph").addClass('hidden');
        $(".Customer_Lead_Graph_loader").removeClass('hidden');
        lblval = $(this).attr("id-val");
        setTimeout(function () {
            if (dayValue != "undefined" && dayValue != null) {
                DataByDate(dayValue, lblval);
            }
            else {
                $(".Customer_Lead_Graph_loader").addClass('hidden');
                OpenErrorMessageNew("Error!", "Please select an actions!", "");
            }
        }, 2000);
    });

    $('.Install-Ticket-list-panel').load(AppConfigDomainSitePath + "/App/DashBoardAssignedTicket/?DateFrom=" + $(".TicketStartDate").val()
      + "&DateTo=" + $(".TicketEndDate").val() + "&IsInstallation=true");

    $('.Other-Ticket-list-panel').load(AppConfigDomainSitePath + "/App/DashBoardAssignedTicket/?DateFrom=" + $(".OtherTicketStartDate").val()
   + "&DateTo=" + $(".OtherTicketEndDate").val() + "&IsInstallation=false");

    $("#AssignedStatus").change(function () {
        var Status = $("#AssignedStatus").val();
        var TicketType = $("#InstallationTicketType").val();
        $('.Install-Ticket-list-panel').load(AppConfigDomainSitePath + "/App/DashBoardAssignedTicket/?TicketStatus=" + encodeURI(Status)
            + "&TicketType=" + encodeURI(TicketType) + "&DateFrom=" + $(".TicketStartDate").val()
        + "&DateTo=" + $(".TicketEndDate").val() + "&IsInstallation=true");
    })
    $("#InstallDateFilter").click(function () {
        var Status = $("#AssignedStatus").val();
        var TicketType = $("#InstallationTicketType").val();
        $('.Install-Ticket-list-panel').load(AppConfigDomainSitePath + "/App/DashBoardAssignedTicket/?TicketStatus=" + encodeURI(Status)
            + "&TicketType=" + encodeURI(TicketType) + "&DateFrom=" + $(".TicketStartDate").val()
            + "&DateTo=" + $(".TicketEndDate").val() + "&IsInstallation=true");
        //UpdateTicketCookie();

    })

    $("#InstallationTicketType").change(function () {
        console.log("Hey");
        var Status = $("#AssignedStatus").val();
        var TicketType = $("#InstallationTicketType").val();
        $('.Install-Ticket-list-panel').load(AppConfigDomainSitePath + "/App/DashBoardAssignedTicket/?TicketStatus=" + encodeURI(Status)
            + "&TicketType=" + encodeURI(TicketType) + "&DateFrom=" + $(".TicketStartDate").val()
        + "&DateTo=" + $(".TicketEndDate").val() + "&IsInstallation=true");
   
    })




    $("#OtherAssignedStatus").change(function () {
        console.log("Hur");
        var Status = $("#OtherAssignedStatus").val();
        var TicketType = $("#OtherTicketType").val();
        $('.Other-Ticket-list-panel').load(AppConfigDomainSitePath + "/App/DashBoardAssignedTicket/?TicketStatus=" + encodeURI(Status)
            + "&TicketType=" + encodeURI(TicketType) + "&DateFrom=" + $(".OtherTicketStartDate").val()
        + "&DateTo=" + $(".OtherTicketEndDate").val() + "&IsInstallation=false");
    })
    $("#OtherDateFilter").click(function () {
        console.log("sdfsdf")
        var Status = $("#OtherAssignedStatus").val();
        var TicketType = $("#OtherTicketType").val();
        $('.Other-Ticket-list-panel').load(AppConfigDomainSitePath + "/App/DashBoardAssignedTicket/?TicketStatus=" + encodeURI(Status)
            + "&TicketType=" + encodeURI(TicketType) + "&DateFrom=" + $(".OtherTicketStartDate").val()
        + "&DateTo=" + $(".OtherTicketEndDate").val() + "&IsInstallation=false");
        UpdateJobTicketCookie();
    })

    $("#OtherTicketType").change(function () {
        var Status = $("#OtherAssignedStatus").val();
        var TicketType = $("#OtherTicketType").val();
        $('.Other-Ticket-list-panel').load(AppConfigDomainSitePath + "/App/DashBoardAssignedTicket/?TicketStatus=" + encodeURI(Status)
            + "&TicketType=" + encodeURI(TicketType) + "&DateFrom=" + $(".OtherTicketStartDate").val()
        + "&DateTo=" + $(".OtherTicketEndDate").val() + "&IsInstallation=false");
    })

    if ($(".Reminder-list-panel").height() <= 445) {
        $(".Install-Ticket-list-panel").css("overflow-y", "scroll");
        $(".Install-Ticket-list-panel").css("overflow-x", "hidden");
        $(".Install-Ticket-list-panel").css("height", "48vh");
    }


    $(".btnTotalOpenTicketdaily").click(function () {
        OpenTicketsDateFilter = "Daily";
        TechnicianDashboardDataWithDateFiltering(firstdate, lastdate, previousfirstdate, "TotalOpenTicketDaily");
    });
    $(".btnTotalOpenTicketweekly").click(function () {
        OpenTicketsDateFilter = "Weekly";
        TechnicianDashboardDataWithDateFiltering(thisfirstdate, thislastdate, weeklylastdate, "TotalOpenTicketWeekly");
    });
    $(".btnTotalOpenTicketmonthly").click(function () {
        OpenTicketsDateFilter = "Monthly";
        TechnicianDashboardDataWithDateFiltering(thisfirstmonthdate, thislastmonthdate, monthlylastdate, "TotalOpenTicketMonthly");
    });
    $(".btnTotalOpenTicketyearly").click(function () {
        OpenTicketsDateFilter = "Yearly";
        TechnicianDashboardDataWithDateFiltering(thisfirstyeardate, thislastyeardate, yearlylastdate, "TotalOpenTicketYearly");
    });
    $(".btnTotalClosedTicketdaily").click(function () {
        TechnicianDashboardDataWithDateFiltering(firstdate, lastdate, previousfirstdate, "TotalClosedTicketDaily");
    });
    $(".btnTotalClosedTicketweekly").click(function () {
        TechnicianDashboardDataWithDateFiltering(thisfirstdate, thislastdate, weeklylastdate, "TotalClosedTicketWeekly");
    });
    $(".btnTotalClosedTicketmonthly").click(function () {
        TechnicianDashboardDataWithDateFiltering(thisfirstmonthdate, thislastmonthdate, monthlylastdate, "TotalClosedTicketMonthly");
    });
    $(".btnTotalClosedTicketyearly").click(function () {
        TechnicianDashboardDataWithDateFiltering(thisfirstyeardate, thislastyeardate, yearlylastdate, "TotalClosedTicketYearly");
    });
    $(".btnTotalUpsolddaily").click(function () {
        TechnicianDashboardDataWithDateFiltering(firstdate, lastdate, previousfirstdate, "TotalUpsoldDaily");
        upsoldDateFilter = "Daily";
    });
    $(".btnTotalUpsoldweekly").click(function () {
        TechnicianDashboardDataWithDateFiltering(thisfirstdate, thislastdate, weeklylastdate, "TotalUpsoldWeekly");
        upsoldDateFilter = "Weekly";
    });
    $(".btnTotalUpsoldmonthly").click(function () {
        TechnicianDashboardDataWithDateFiltering(thisfirstmonthdate, thislastmonthdate, monthlylastdate, "TotalUpsoldMonthly");
        upsoldDateFilter = "Monthly";
    });
    $(".btnTotalUpsoldyearly").click(function () {
        TechnicianDashboardDataWithDateFiltering(thisfirstyeardate, thislastyeardate, yearlylastdate, "TotalUpsoldYearly");
        upsoldDateFilter = "Yearly";
    });

});