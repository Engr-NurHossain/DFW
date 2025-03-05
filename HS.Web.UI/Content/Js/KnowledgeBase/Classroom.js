String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";

//var DataTablePageSize = 50;
//var PayrollStartDatepicker;
//var PayrollEndDatepicker;
var DataTablePageSize = 50;
var PayrollStartDatepicker;
var PayrollEndDatepicker;
var IsAdmin = false;
var IsAdminAssigned = false;
//var LoadAssignedList = function () {
//    $("#Assigned_Tab").addClass('active');
//    $("#Load_Completed_List").removeClass('active');
//    $(".Load_Assigned_List").html(TabsLoaderText);
//    $(".Load_Assigned_List").load(domainurl + "/Sales/AssignedToClassroom");
//}
var LoadAssignedList = function () {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    $("#Assigned_Tab").addClass('active');
    $("#Load_Completed_List").removeClass('active');
    $(".Load_Assigned_List").html(TabsLoaderText);
    $(".Load_Assigned_List").load(domainurl + "/Sales/AssignedToClassroom?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&IsAdminAssigned=" + IsAdminAssigned);
    //$(".Load_Assigned_List").load(domainurl + "/QtiManage/AssignedToClassroom");
}
//var LoadCompletedList = function () {
//    $("#Assigned_Tab").removeClass('active');
//    $("#Load_Completed_List").addClass('active');
//    $("#Load_Completed_List").html(TabsLoaderText);
//    $("#Load_Completed_List").load(domainurl + "/Sales/CompletedToClassroom");
//}
var LoadCompletedList = function () {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    $("#Assigned_Tab").removeClass('active');
    $("#Load_Completed_List").addClass('active');
    $("#Load_Completed_List").html(TabsLoaderText);
    $("#Load_Completed_List").load(domainurl + "/Sales/CompletedToClassroom?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&IsAdmin=" + IsAdmin);
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
    if (startTab == "AssignedTab") {
        //ShowDateFilter();
        LoadAssignedList();
    }
    else if (startTab == "AccessedReportTab") {
        $("#CompletedList").addClass('active');
        $(".CompletedList").addClass('active');
        ShowDateFilter();
        LoadCompletedList();
    }

    $(".AssignedList").click(function () {
        console.log("Search");
        ShowDateFilter();
        LoadAssignedList();
    });
    $(".CompletedList").click(function () {
        console.log("CompletedList");
        ShowDateFilter();
        LoadCompletedList();
    });
    $(".DateFilterContents .btn-apply-Datefilter").unbind().click(function () {
        var DateFrom = $(".min-date").val();
        var DateTo = $(".max-date").val();
        UpdatePtoCookie();

        if ($(".AssignedList").hasClass("active")) {
            LoadAssignedList(1);
        }
        else if ($(".CompletedList").hasClass("active")) {
            LoadCompletedList(1);
        }
        //else if ($(".LoadAccountabilityReportTab").hasClass("active")) {
        //    LoadAccountabilityReport(1);
        //}
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