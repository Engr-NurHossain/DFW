String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";

var DataTablePageSize = 50;
var PayrollStartDatepicker;
var PayrollEndDatepicker;

var LoadSearchesReport = function () {
    $("#SalesReportTab").addClass('active');
    $("#Load_Accessed_Report").removeClass('active');
    $(".Load_Searches_Report").html(TabsLoaderText);
    $(".Load_Searches_Report").load(domainurl + "/Reports/SearchedReport");
}
var LoadAccessedReport = function () {
    $("#SalesReportTab").removeClass('active');
    $("#Load_Accessed_Report").addClass('active');
    $("#Load_Accessed_Report").html(TabsLoaderText);
    $("#Load_Accessed_Report").load(domainurl + "/Reports/AccessedReport");
}
var LoadAccountabilityReport = function () {
    $("#Load_Accountability_Report").html(TabsLoaderText);
    $("#Load_Accountability_Report").load(domainurl + "/Reports/AccountabilityReport");
}
var my_date_format = function (input) {
    var d = new Date(Date.parse(input.replace(/-/g, "/")));
    var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var date = d.getDate() + " " + month[d.getMonth()] + ", " + d.getFullYear();

    return (date);
};
var ShowDateFilter = function () {
    $(".DateFilterContents").show();
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    if (startTab == "SearchesReportTab") {
        ShowDateFilter();
        LoadSearchesReport();
    }
    else if (startTab == "AccessedReportTab") {
        $("#LoadAccessedReportTab").addClass('active');
        $(".LoadAccessedReportTab").addClass('active');
        ShowDateFilter();
        LoadAccessedReport();
    }
    else if (startTab == "AccountabilityReportTab") {
        $("#LoadAccountabilityReportTab").addClass('active');
        $(".LoadAccountabilityReportTab").addClass('active');
        ShowDateFilter();
        LoadAccountabilityReport();
    }
    
    $(".SearchesReportTab").click(function () {
        console.log("Search"); 
        ShowDateFilter();
        LoadSearchesReport();
    });
    $(".LoadAccessedReportTab").click(function () {
        console.log("LoadAccessedReportTab");
        ShowDateFilter();
        LoadAccessedReport();
    });
    $(".LoadAccountabilityReportTab").click(function () {
        ShowDateFilter();
        LoadAccountabilityReport();
    });
    $(".DateFilterContents .btn-apply-Datefilter").unbind().click(function () {
        var DateFrom = $(".min-date").val();
        var DateTo = $(".max-date").val();
        UpdatePtoCookie();

        if ($(".SearchesReportTab").hasClass("active")) {
            LoadSearchesReport(1);
        }
        else if ($(".LoadAccessedReportTab").hasClass("active")) {
            LoadAccessedReport(1);
        }
        else if ($(".LoadAccountabilityReportTab").hasClass("active")) {
            LoadAccountabilityReport(1);
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
    $('ul.payroll_report_top_nav li a').click(function (e) {
        if (!TabPopStateCheck) {
            window.history.pushState({ urlPath: window.location.pathname }, "", $(e.target).attr('tabname'));
        }
        TabPopStateCheck = false;
    });
    if (top.location.hash != '') {
        if ($("[tabname='" + top.location.hash + "']").length > 0) {
            TabPopStateCheck = true;
            $("[tabname='" + top.location.hash + "']").click();
        }
    }
});