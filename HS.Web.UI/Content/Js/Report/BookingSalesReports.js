String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";

var DataTablePageSize = 50;
var PayrollStartDatepicker;
var PayrollEndDatepicker;

var LoadSalesReportTab = function () {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    $(".Load_Sales_Report").html(TabsLoaderText);
    $(".Load_Sales_Report").load(domainurl + "/Reports/LoadBookingSalesReportPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50");
}

var LoadFinishedJobTab = function () {
    $(".FinishedJobTab_Report").html(TabsLoaderText);
    $(".FinishedJobTab_Report").load(domainurl + "/Reports/FinishdJob");
}

var PackageSummaryTab = function () {
    $(".PackageSummaryTab_Report").html(TabsLoaderText);
    $(".PackageSummaryTab_Report").load(domainurl + "/Reports/PackageSummary");
}

var my_date_format = function (input) {
    var d = new Date(Date.parse(input.replace(/-/g, "/")));
    var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var date = d.getDate() + " " + month[d.getMonth()] + ", " + d.getFullYear();

    return (date);
};

var FilterSalesReport2 = function (pageno, order) {
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    pagesize = 50;
    $(".Load_Sales_Report").load(domainurl + "/Reports/LoadBookingSalesReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order);
}

$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    LoadSalesReportTab();


    $(".SalesReportTab").click(function () {
        LoadSalesReportTab();
    });
    $(".FinishedJobTab").click(function () {
        LoadFinishedJobTab();
    });
    $(".PackageSummaryTab").click(function () {
        PackageSummaryTab();
    });
    $(".DateFilterContents .btn-apply-Datefilter").click(function () {
        console.log("date filter aise");
        var DateFrom = $(".min-date").val();
        var DateTo = $(".max-date").val();
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
        UpdatePtoCookie();
        FilterSalesReport2(1, null);
    });




});