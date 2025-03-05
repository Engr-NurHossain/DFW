var count2 = 1;
var pagesize;
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var FilterInvoiceReport2 = function (pageno, order) {
    console.log("report");
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    var convertmindate = $("#inv_min_date").val();
    var convertmaxdate = $("#inv_max_date").val();
    var createmindate = $("#due_min_date").val();
    var createmaxdate = $("#due_max_date").val();
    pagesize = parseInt($(".invoice_pagesize_val").val()) + 50;
    $(".invoice-table").load(domainurl + "/Reports/InvoiceListReportList?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#invoice_txt_search").val()) + "&order=" + order + "&convertmaxdate=" + encodeURI(convertmaxdate) + "&convertmindate=" + encodeURI(convertmindate) + "&createmindate=" + encodeURI(createmindate) + "&createmaxdate=" + encodeURI(createmaxdate));
}


var FilterInvoiceReport1 = function (pageno, order) {
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    var convertmindate = $("#inv_min_date").val();
    var convertmaxdate = $("#inv_max_date").val();
    var createmindate = $("#due_min_date").val();
    var createmaxdate = $("#due_max_date").val();
    pagesize = 50;
    $(".invoice-table").html(TabsLoaderText);
    $(".invoice-table").load(domainurl + "/Reports/InvoiceListReportList?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#invoice_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order + "&convertmaxdate=" + encodeURI(convertmaxdate) + "&convertmindate=" + encodeURI(convertmindate) + "&createmindate=" + encodeURI(createmindate) + "&createmaxdate=" + encodeURI(createmaxdate));
}

var OpenTicketInvoice = function (invoiceid) {
    if (typeof (invoiceid) != "undefined") {
        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?id=" + invoiceid);
    }
}
var ResetFilterList = function () {
    $(".convert_cus_inp").val("");
    $("#invoice_txt_search").val("");
    $(".convert_cus_inp_drp").val("-1");
    FilterInvoiceReport1(1);
}
$(document).ready(function () {
    FilterInvoiceReport1(1);
    var pageno = PageNumber;
    $(".icon_sort_timeclock").click(function () {
        orderval = $(this).attr('data-val');
        FilterInvoiceReport1(pageno, orderval);
    })

    $("#btnDownloadInvoiceReport").click(function () {
        var StartDate = $(".min-date").val();
        var EndDate = $(".max-date").val();
        var convertmindate = $("#inv_min_date").val();
        var convertmaxdate = $("#inv_max_date").val();
        var createmindate = $("#due_min_date").val();
        var createmaxdate = $("#due_max_date").val();
        window.location.href = domainurl + "/Reports/InvoiceListReportList?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&GetReport=true" + "&searchtxt=" + encodeURI($("#invoice_txt_search").val() + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&convertmaxdate=" + encodeURI(convertmaxdate) + "&convertmindate=" + encodeURI(convertmindate) + "&createmindate=" + encodeURI(createmindate) + "&createmaxdate=" + encodeURI(createmaxdate));
    })
    $("#sales_inv_status").selectpicker('val', invstatus);


    new Pikaday({ format: 'MM/DD/YYYY', field: $('#inv_min_date')[0] });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#inv_max_date')[0] });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#due_min_date')[0] });
    new Pikaday({ format: 'MM/DD/YYYY', field: $('#due_max_date')[0] });
    $(".convert_cus_report_filter").hide();
    $("#convert_cus_filterbtn").click(function () {
        console.log("filter");
        if ($(".convert_cus_report_filter").is(":visible")) {
            $(".convert_cus_report_filter").hide();

        } else {
            $(".convert_cus_report_filter").show();

        }
    });

    $("#invoice_txt_search").keydown(function (e) {

        if (e.which == 13) {
            FilterInvoiceReport1(1);
        }
    });
})