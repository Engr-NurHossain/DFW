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

    //var FilterCollectionReport = function (pageno, order) {
    //    var datemin = $(".min-date").val();
    //    var datemax = $(".max-date").val();
    //    var convertmindate = $("#collection_inv_min_date").val();
    //    var convertmaxdate = $("#collection_inv_max_date").val();
    //    var createmindate = $("#collection_due_min_date").val();
    //    var createmaxdate = $("#collection_due_max_date").val();
    //    pagesize = 20;
    //    $(".collection_invoice-table").html(TabsLoaderText);

    //    $(".collection_invoice-table").load(domainurl + "/Reports/LoadCollectionReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#Collection_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order + "&convertmaxdate=" + encodeURI(convertmaxdate) + "&convertmindate=" + encodeURI(convertmindate) + "&createmindate=" + encodeURI(createmindate) + "&createmaxdate=" + encodeURI(createmaxdate));

    //}

    var FilterCollectionReport1 = function (pageno, order) {
        var datemin = $(".min-date").val();
        var datemax = $(".max-date").val();
        var convertmindate = $("#collection_inv_min_date").val();
        var convertmaxdate = $("#collection_inv_max_date").val();
        var createmindate = $("#collection_due_min_date").val();
        var createmaxdate = $("#collection_due_max_date").val();
        var collectionmindate = $("#collection_min_date").val();
        var collectionmaxdate = $("#collection_max_date").val();
        var salesCommission = encodeURI($("#salesCommissionDropdown").val());
        pagesize = 20;
        $(".collection_invoice-table").html(TabsLoaderText);

        $(".collection_invoice-table").load(domainurl + "/Reports/CollectionReportList?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#Collection_txt_search").val()) + "&salesCommission=" + salesCommission + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order + "&convertmaxdate=" + encodeURI(convertmaxdate) + "&convertmindate=" + encodeURI(convertmindate) + "&createmindate=" + encodeURI(createmindate) + "&createmaxdate=" + encodeURI(createmaxdate) + "&collectionmindate=" + encodeURI(collectionmindate) + "&collectionmaxdate=" + encodeURI(collectionmaxdate));
    }
    var ResetFilterList = function () {
        $(".collection_cus_inp").val("");
        $("#Collection_txt_search").val("");
        $("#salesCommissionDropdown").val("-1");
        $(".collection_cus_inp_drp").val("-1");
        FilterCollectionReport1(1);
    }
    $(document).ready(function () {
        FilterCollectionReport1(1);
 
        var pageno = PageNumber;
        $(".icon_sort_timeclock").click(function () {
            orderval = $(this).attr('data-val');
            console.log(orderval)
            FilterCollectionReport1(pageno, orderval);
        })
        $("#Collection_txt_search").val(SearchText);
        
        $("#btnDownloadCollectionReport").click(function () {
            var datemin = $(".min-date").val();
            var datemax = $(".max-date").val();
            var convertmindate = $("#collection_inv_min_date").val();
            var convertmaxdate = $("#collection_inv_max_date").val();
            var createmindate = $("#collection_due_min_date").val();
            var createmaxdate = $("#collection_due_max_date").val();
            var collectionmindate = $("#collection_min_date").val();
            var collectionmaxdate = $("#collection_max_date").val();
            var salesCommission = encodeURI($("#salesCommissionDropdown").val());
            window.location.href = domainurl + "/Reports/CollectionReportList?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=1&pagesize=50" + "&GetReport=true" + "&searchtxt=" + encodeURI($("#Collection_txt_search").val() + "&salesCommission=" + salesCommission + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + orderval + "&convertmaxdate=" + encodeURI(convertmaxdate) + "&convertmindate=" + encodeURI(convertmindate) + "&createmindate=" + encodeURI(createmindate) + "&createmaxdate=" + encodeURI(createmaxdate) + "&collectionmindate=" + encodeURI(collectionmindate) + "&collectionmaxdate=" + encodeURI(collectionmaxdate));
        })
        /*$("#sales_inv_status").selectpicker('val', invstatus);*/

        $("#Collection_txt_search").keydown(function (e) {
            if (e.which == 13) {

                FilterCollectionReport1(1, null);

            }
        });

        new Pikaday({ format: 'MM/DD/YYYY', field: $('#collection_inv_min_date')[0] });
        new Pikaday({ format: 'MM/DD/YYYY', field: $('#collection_inv_max_date')[0] });
        new Pikaday({ format: 'MM/DD/YYYY', field: $('#collection_due_min_date')[0] });
        new Pikaday({ format: 'MM/DD/YYYY', field: $('#collection_due_max_date')[0] });
        new Pikaday({ format: 'MM/DD/YYYY', field: $('#collection_min_date')[0] });
        new Pikaday({ format: 'MM/DD/YYYY', field: $('#collection_max_date')[0] });
        $(".collection_cus_report_filter").hide();
        $("#collection_cus_filterbtn").click(function () {
            console.log("filter");
            if ($(".collection_cus_report_filter").is(":visible")) {
                $(".collection_cus_report_filter").hide();

            } else {
                $(".collection_cus_report_filter").show();

            }
        });
        //$(document).click(function (e) {
        //    if ($($(e.target).parent().parent()).hasClass('collection_cus_div')
        //                || $($(e.target).parent()).hasClass('collection_cus_div')
        //                || $($(e.target).parent().parent()).hasClass('collection_cus_div')) {
        //        return;
        //    }
        //    else if ($(".collection_cus_report_filter").is(":visible")) {
        //        $(".collection_cus_report_filter").hide();
        //    }
        //});

    
    })