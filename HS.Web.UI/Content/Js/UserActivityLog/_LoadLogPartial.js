  var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
    //var OpenBkById = function (bkId) {
    //    if (typeof (bkId) != "undefined" && bkId > 0) {
    //        if (typeof (customerId) == "undefined") {
    //            customerId = 0;
    //        }
    //        OpenTopToBottomModal("/Booking/AddLeadBooking/?customerid=" + customerId + "&Id=" + bkId);
    //    }
    //}
    //var OpenTicketInvoice = function (invoiceid) {
    //    if (typeof (invoiceid) != "undefined") {
    //        OpenTopToBottomModal(domainurl + "/Invoice/AddInvoice/?id=" + invoiceid);
    //    }
    //}

    var FilterLog = function (pageno, order) {
        var datemin = $(".min-date").val();
        var datemax = $(".max-date").val();
        pagesize = parseInt(CurrentNumber) + 50;
        var LoadCustomerDiv = "#customer_tab_" + CustomerId + " ";


        $(".LogTab_Load").load(domainurl + "/Booking/LoadLogPartial?pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#Log_txt_search").val()) + "&CustomerId=" + CustomerId + "&order=" + order);
        //$(LoadCustomerDiv + ".LogTab_Load").load(domainurl + "/Booking/LoadLogPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#Log_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order);

    }


var FilterLog2 = function (pageno, order) {
    var datemin = $("#log_startdate").val();
    var datemax = $("#log_enddate").val();
    pagesize = 50;
 
    $(".lodtest").html(TabsLoaderText);
    setTimeout(function () {
        $(".LogTab_Load").load(domainurl + "/Booking/LoadLogPartial?pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#Log_txt_search").val()) + "&CustomerId=" + CustomerId + "&order=" + order + "&logstartdate=" + encodeURI(datemin) + "&logenddate=" + encodeURI(datemax));
    }, 2000);



 

 
}

    $(document).ready(function () {
        var pageno = PageNumber;
        $(".icon_sort_timeclock").click(function () {
            orderval = $(this).attr('data-val');
            console.log(orderval)
            FilterLog2(pageno, orderval);
        })
        $("#Log_txt_search").val(SearchText);
        
        
        //$(".page-wrapper-contents").scroll(function () {
        //    if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {

        //        console.log("hit TotalBookingIDCount:" + TotalBookingIDCount );
        //        console.log("hit CurrentNumber:" + Totalpagesize);
        //        if ( parseInt(TotalBookingIDCount) > parseInt(Totalpagesize)) {
        //            console.log("hit" + (TotalBookingIDCount > Totalpagesize));
        //            FilterLog(1,orderval);
        //            count3 = count3 + 1;
        //        }
        //    }
        //})
        $("#btnDownloadLog").click(function () {
            var StartDate = $(".min-date").val();
            var EndDate = $(".max-date").val();
            window.location.href = domainurl + "/Booking/LoadLogPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&GetReport=true" + "&searchtxt=" + encodeURI($("#Log_txt_search").val() + "&CustomerId=" + CustomerId + "&order=" + orderval);
        })
        $("#sales_inv_status").selectpicker('val', invstatus);


        $("#Log_txt_search").keydown(function (e) {
            if (e.which == 13) {

                FilterLog2(1, null);

               }
        });

    })