$(document).ready(function () {
    var DateFrom = $(".min-date").val();
    var DateTo = $(".max-date").val();
    var SearchText = $("#searchtext_goback_report").val();

    $(".icon_sort_timeclock").click(function () {

        var orderval = $(this).attr('data-val');
        $(".go_back_report_table").html(TabsLoaderText);
        $(".go_back_report_table").load("/Reports/GoBackReportList", { PageNo: pagenumber, SearchText: SearchText, order: orderval, StartDate: DateFrom, EndDate: DateTo, viewtype: "webview" });
    })
})