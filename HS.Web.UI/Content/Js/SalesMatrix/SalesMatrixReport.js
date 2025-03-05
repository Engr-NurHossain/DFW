var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var date = new Date();
var OpenFirstCallCloseTab = function () {
    $(".sales_matrix_report_nav ul li").removeClass("active");
    $(".sales_matrix_report_nav .tab-content .tab-pane").removeClass("active");
    $(".FirstCallCloseNav").addClass("active");
    $("#FirstCallCloseTab").addClass("active");
    $("#FirstCallCloseTab").html(TabsLoaderText);
    $(".default_date_filter").show();
    $(".SoldToFunded_date_filter").hide();
    $("#FirstCallCloseTab").load(domainurl + "/Reports/FirstCallClose");
    tabreloadcookie();
    window.history.pushState({ urlPath: window.location.pathname }, "", $(".FirstCallCloseNav a").attr('href'))
}
var OpenOverAllCloseTab = function () {
    $(".sales_matrix_report_nav ul li").removeClass("active");
    $(".sales_matrix_report_nav .tab-content .tab-pane").removeClass("active");
    $(".OverAllCloseNav").addClass("active");
    $("#OverAllCloseTab").addClass("active");
    $("#OverAllCloseTab").html(TabsLoaderText);
    $(".default_date_filter").show();
    $(".SoldToFunded_date_filter").hide();
    $("#OverAllCloseTab").load(domainurl + "/Reports/OverAllClose");
    tabreloadcookie();
    window.history.pushState({ urlPath: window.location.pathname }, "", $(".OverAllCloseNav a").attr('href'))
}
var OpenSoldToFundedTab = function () {
    $(".sales_matrix_report_nav ul li").removeClass("active");
    $(".sales_matrix_report_nav .tab-content .tab-pane").removeClass("active");
    $(".SoldToFundedNav").addClass("active");
    $("#SoldToFundedTab").addClass("active");
    $("#SoldToFundedTab").html(TabsLoaderText);
    $(".default_date_filter").hide();
    $(".SoldToFunded_date_filter").show();
    $("#SoldToFundedTab").load(domainurl + "/Reports/SoldToFunded");
    tabreloadcookie();
    window.history.pushState({ urlPath: window.location.pathname }, "", $(".SoldToFundedNav a").attr('href'))
}
var OpenNumberOfSalesTab = function () {
    $(".sales_matrix_report_nav ul li").removeClass("active");
    $(".sales_matrix_report_nav .tab-content .tab-pane").removeClass("active");
    $(".NumberOfSalesNav").addClass("active");
    $("#NumberOfSalesTab").addClass("active");
    $("#NumberOfSalesTab").html(TabsLoaderText);
    $(".default_date_filter").show();
    $(".SoldToFunded_date_filter").hide();
    $("#NumberOfSalesTab").load(domainurl + "/Reports/NumberOfSales");
    tabreloadcookie();
    window.history.pushState({ urlPath: window.location.pathname }, "", $(".NumberOfSalesNav a").attr('href'))
}
var OpenAppointmentSetTab = function () {
    $(".sales_matrix_report_nav ul li").removeClass("active");
    $(".sales_matrix_report_nav .tab-content .tab-pane").removeClass("active");
    $(".AppointmentSetNav").addClass("active");
    $("#AppointmentSetTab").addClass("active");
    $("#AppointmentSetTab").html(TabsLoaderText);
    $(".default_date_filter").show();
    $(".SoldToFunded_date_filter").hide();
    $("#AppointmentSetTab").load(domainurl + "/Reports/AppointmentSet");
    tabreloadcookie();
    window.history.pushState({ urlPath: window.location.pathname }, "", $(".AppointmentSetNav a").attr('href'))
}
var tabreloadcookie = function () {
    if (typeof $.cookie("___SalesMatrixReportDate") != 'undefined' && $.cookie("___SalesMatrixReportDate") != null && $.cookie("___SalesMatrixReportDate") != "" && $.cookie("___SalesMatrixReportDate") != "~") {
        var cookieval = ($.cookie("___SalesMatrixReportDate")).split('~');
        console.log(cookieval);
        var tempstartdate = cookieval[0];
        SoldToFundedEndDateDatepicker.setDate(cookieval[1]);
        SoldToFundedStartDateDatepicker.setDate(tempstartdate);
    }
}
var updatesalesmatrixreportcookie = function () {
    
    var dateforreloadtabsalesmatrix = $("#WeeklyStartDate").val() + "~" + $("#WeeklyEndDate").val();
    $.cookie("___SalesMatrixReportDate", dateforreloadtabsalesmatrix, { expires: 2, path: "/" });
    console.log(dateforreloadtabsalesmatrix);
    
}

$(document).ready(function () {
    SoldToFundedStartDateDatepicker = new Pikaday({
        field: $('#WeeklyStartDate')[0],
        format: 'MM/DD/YYYY'
    });
    SoldToFundedEndDateDatepicker = new Pikaday({
        field: $('#WeeklyEndDate')[0],
        format: 'MM/DD/YYYY'
    });
    
    $("#WeeklyEndDate").change(function () {
        console.log("FireEndDate");
        var dayEffective = 6;
        console.log("beforeEffectiveDate");
        if (typeof (effectiveweektoday) != "undefined" && effectiveweektoday != null) {
            dayEffective = Number(effectiveweektoday)-1;
        }
        var convertEndDate = new Date($('#WeeklyEndDate').val());
        //StartDateDatepicker.setDate(convertEndDate.addDays(-6));
        var first1 = convertEndDate.getDate();
        var last1 = first1 - dayEffective;
        console.log(last1);
        var firstday1 = new Date(convertEndDate.setDate(last1));
        var thisweek1 = firstday1.getMonth() + 1 + "/" + firstday1.getDate() + "/" + firstday1.getFullYear();
        var lastday1 = new Date(convertEndDate.setDate(first1));
        var lastweek1 = lastday1.getMonth() + 1 + "/" + lastday1.getDate() + "/" + lastday1.getFullYear();
        SoldToFundedStartDateDatepicker.setDate(thisweek1);
        updatesalesmatrixreportcookie();
    });
    $("#WeeklyStartDate").change(function () {
        updatesalesmatrixreportcookie();
    });
    $(".LoaderWorkingDiv").hide();


    if (top.location.hash != "") {
        if (top.location.hash == "#tabFirstCallClose") {
            OpenFirstCallCloseTab();
        }
        else if (top.location.hash == "#tabOverAllClose") {
            OpenOverAllCloseTab();
        }
        else if (top.location.hash == "#tabSoldToFunded") {
            OpenSoldToFundedTab();
        }
        else if (top.location.hash == "#tabNumberOfSales") {
            OpenNumberOfSalesTab();
        }
        else if (top.location.hash == "#tabAppointmentSet") {
            OpenAppointmentSetTab();
        }
    }
    else {
        OpenFirstCallCloseTab();
    }
    

    $(".FirstCallCloseNav").click(function () {
        OpenFirstCallCloseTab();
    });

    $(".OverAllCloseNav").click(function () {
        OpenOverAllCloseTab();
        
    });

    $(".SoldToFundedNav").click(function () {
        OpenSoldToFundedTab();
    });

    $(".NumberOfSalesNav").click(function () {
        OpenNumberOfSalesTab();
    });

    $(".AppointmentSetNav").click(function () {
        OpenAppointmentSetTab();
    });

    $(".DateFilterContents .btn-apply-Datefilter").unbind().click(function () {
        var DateFrom = $(".min-date").val();
        var DateTo = $(".max-date").val();
        UpdatePtoCookie();

        if (top.location.hash != "") {
            if (top.location.hash == "#tabFirstCallClose") {
                OpenFirstCallCloseTab();
            }
            else if (top.location.hash == "#tabOverAllClose") {
                OpenOverAllCloseTab();
            }
            else if (top.location.hash == "#tabSoldToFunded") {
                OpenSoldToFundedTab();
            }
            else if (top.location.hash == "#tabNumberOfSales") {
                OpenNumberOfSalesTab();
            }
            else if (top.location.hash == "#tabAppointmentSet") {
                OpenAppointmentSetTab();
            }
        }
        else {
            OpenFirstCallCloseTab();
        }
        var StartDate = my_date_format($(".DateFilterContents .min-date").val());
        var EndDate = my_date_format($(".DateFilterContents .max-date").val())
        if (StartDate == "NaN undefined, NaN") {
            StartDate = "All Time";
            EndDate = "";
        }

        $(".DateFilterContents .date-start").html("");
        $(".DateFilterContents .date-end").html("");
        $(".DateFilterContents .date-start").html(StartDate);
        $(".DateFilterContents .date-end").html(EndDate);
        $(".DateFilterContents .dropdown-filter").hide();

    });
});