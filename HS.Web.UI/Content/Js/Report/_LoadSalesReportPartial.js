var FilterSalesReport = function (pageno, order) {
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    $(".Load_Sales_Report").load(domainurl + "/Reports/LoadSalesReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&Order=" + order + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()));
}

var FilterSalesReport1 = function (pageno) {
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    pagesize = 50;
    $(".Load_Sales_Report").load(domainurl + "/Reports/LoadSalesReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()));
}

$(document).ready(function () {




    $(".icon_sort_timeclock").click(function () {
        var datemin = $(".min-date").val();
        var datemax = $(".max-date").val();
        console.log("sorting");
        var orderval = $(this).attr('data-val');

        $(".Load_Sales_Report").load(domainurl + "/Reports/LoadSalesReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&Order=" + orderval + "&pageno=" + pageno + "&pagesize=" + pagesize);
    })
    $("#btnDownloadSalesReport").click(function () {
        var StartDate = $(".min-date").val();
        var EndDate = $(".max-date").val();
        window.location.href = domainurl + "/Reports/LoadSalesReportPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&GetReport=true" + "&searchtxt=" + encodeURI($("#sales_txt_search").val() + "&invostatus=" + encodeURI($("#sales_inv_status").val()));
    })
    $("#sales_inv_status").selectpicker('val', invstatus);
    $("#sales_txt_search").keydown(function (e) {

        if (e.which == 13) {
            FilterSalesReport1(1);
        }
    });


})