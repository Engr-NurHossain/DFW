$(document).ready(function () {
    var DateFrom = $(".min-date").val();
    var DateTo = $(".max-date").val();
    var SearchText = $("#searchtext_datereference").val();
    $(".icon_sort_timeclock").click(function () {

        var orderval = $(this).attr('data-val');
        $(".date_reference_table").html(TabsLoaderText);
        $(".date_reference_table").load("/Reports/DateReferenceReportList", { PageNo: pagenumber, SearchText: SearchText, order: orderval, StartDate: DateFrom, EndDate: DateTo, viewtype: "webview" });
    })
});