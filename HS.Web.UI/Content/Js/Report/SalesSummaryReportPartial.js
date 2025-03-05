var FilterSalesSummary = function (pageno) {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    var searchtext = $("#sales_summary_search_text").val();
    $("#LoadSalesSummaryTab").html(TabsLoaderText);
    $("#LoadSalesSummaryTab").load(domainurl + "/Reports/SalesSummaryReportPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&searchtext=" + encodeURI(searchtext) + "&pageno=" + pageno + "&pagesize=20");
}
$(document).ready(function () {
    var pagenumber = '@ViewBag.PageNumber';
    var DateFrom = $(".min-date").val();
    var DateTo = $(".max-date").val();
    var searchtext = $("#sales_summary_search_text").val();
    var pagesize = 20;

    $(".icon_sort_timeclock").click(function () {

        var orderval = $(this).attr('data-val');
        $("#LoadSalesSummaryTab").html(TabsLoaderText);
        $("#LoadSalesSummaryTab").load("/Reports/SalesSummaryReportPartial", { PageNo: pagenumber, pagesize: pagesize, SearchText: searchtext, order: orderval, Start: DateFrom, End: DateTo });
    })

    $("#btnSalesSummary").click(function () {
        var StartDate = $(".min-date").val();
        var EndDate = $(".max-date").val();
        var searchtext = $("#sales_summary_search_text").val();
        window.location.href = domainurl + "/Reports/SalesSummaryReportPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&GetReport=true" + "&searchtext=" + encodeURI(searchtext) + "&pageno=" + pageno + "&pagesize=20";
    })
    $("#sales_summary_search_text").keydown(function (e) {

        if (e.which == 13) {
            FilterSalesSummary(1);
        }
    });
})