var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
    var OpenBkById = function (bkId) {
        if (typeof (bkId) != "undefined" && bkId > 0) {
            if (typeof (customerId) == "undefined") {
                customerId = 0;
            }
            OpenTopToBottomModal("/Booking/AddLeadBooking/?customerid=" + customerId + "&Id=" + bkId);
        }
    }
    var OpenTicketInvoice = function (invoiceid) {
        if (typeof (invoiceid) != "undefined") {
            OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?id=" + invoiceid);
        }
    }
var ResetFilterList = function () {
    $("#BookingSource").selectpicker('val', '');
    FilterSalesReport1(1);
}
    var FilterSalesReport = function (pageno, order) {
        var datemin = $(".min-date").val();
        var datemax = $(".max-date").val();
        //pagesize = parseInt(CurrentNumber) + 50;
        pagesize = 50;

        $(".Load_Sales_Report").load(domainurl + "/Reports/LoadBookingSalesReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order);

    }

    var FilterSalesReport1 = function (pageno, order) {
        var datemin = $(".min-date").val();
        var datemax = $(".max-date").val();
        pagesize = 50;

        $(".Load_Sales_Report").load(domainurl + "/Reports/LoadBookingSalesReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&source=" + encodeURI($("#BookingSource").val()) + "&order=" + order);
    }

$(document).ready(function () {
    $("#BookingSource").selectpicker('val', bsource);
    $(".convert_cus_report_filter").hide();
    $("#convert_cus_filterbtn").click(function () {
        console.log("filter");
        if ($(".convert_cus_report_filter").is(":visible")) {
            $(".convert_cus_report_filter").hide();

        } else {
            $(".convert_cus_report_filter").show();

        }
    });
    $(document).click(function (e) {
        if ($($(e.target).parent().parent()).hasClass('convert_cus_div')
            || $($(e.target).parent()).hasClass('convert_cus_div')
            || $($(e.target).parent().parent()).hasClass('convert_cus_div')) {
            return;
        }
        else if ($(".convert_cus_report_filter").is(":visible")) {
            $(".convert_cus_report_filter").hide();
        }
    });
        var pageno = PageNumber;
        $(".icon_sort_timeclock").click(function () {
            orderval = $(this).attr('data-val');
            console.log(orderval)
            FilterSalesReport1(pageno, orderval);
        })
        $("#sales_txt_search").val(SearchText);
        
        
        //$(".page-wrapper-contents").scroll(function () {
        //    if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {

        //        console.log("hit TotalBookingIDCount:" + TotalBookingIDCount );
        //        console.log("hit CurrentNumber:" + Totalpagesize);
        //        if (count == 1 && parseInt(TotalBookingIDCount) > parseInt(Totalpagesize)) {
        //            console.log("hit" + (TotalBookingIDCount > Totalpagesize));
        //            FilterSalesReport(1,orderval);
        //            count = count + 1;
        //        }
        //    }
        //})
        $("#btnDownloadSalesReport").click(function () {
            var StartDate = $(".min-date").val();
            var EndDate = $(".max-date").val();
            var Source = encodeURI($("#BookingSource").val());
            window.location.href = domainurl + "/Reports/LoadBookingSalesReportPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&GetReport=true" + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + orderval + "&source=" + Source;
        })
        $("#sales_inv_status").selectpicker('val', invstatus);
    })