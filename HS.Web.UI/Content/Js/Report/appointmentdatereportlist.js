$(document).ready(function () {
    var DateFrom = $(".min-date").val();
    var DateTo = $(".max-date").val();
    var SearchText = $("#searchtext_reference").val();
    $(".icon_sort_timeclock").click(function () {
        var orderval = $(this).attr('data-val');
        $(".appointment_date_table").html(TabsLoaderText);
        $(".appointment_date_table").load("/Reports/AppointmentDateReportList", { PageNo: pagenumber, SearchText: SearchText, order: orderval, StartDate: DateFrom, EndDate: DateTo, viewtype: "webview" });
    })
});