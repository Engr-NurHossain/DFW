

var FilterLeadSourceReport = function (pageno, order) {
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    pagesize = parseInt(CurrentNumber) + 50;

    $(".LeadSource_Report").load(domainurl + "/Reports/LoadLeadSourceReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order + "&empGIDList=" + GetSelectedUserId());

}

var FilterLeadSourceReport1 = function (pageno, order) {
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    pagesize = 50;
    

    $(".LeadSource_Report").load(domainurl + "/Reports/LoadLeadSourceReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order + "&empGIDList=" + GetSelectedUserId());
}

$(document).ready(function () {
    var pageno = PageNumber;
    $(".icon_sort_timeclock").click(function () {
        orderval = $(this).attr('data-val');
        console.log(orderval)
        FilterLeadSourceReport1(pageno, orderval);
    })
    $("#sales_txt_search").val(SearchText);


    $(".page-wrapper-contents").scroll(function () {
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {

            console.log("hit TotalBookingIDCount:" + TotalBookingIDCount);
            console.log("hit CurrentNumber:" + Totalpagesize);
            if (count == 1 && parseInt(TotalBookingIDCount) > parseInt(Totalpagesize)) {
                console.log("hit" + (TotalBookingIDCount > Totalpagesize));
                FilterLeadSourceReport(1, orderval);
                count = count + 1;
            }
        }
    })
    $("#btnDownloadSalesReport").click(function () {
        var StartDate = $(".min-date").val();
        var EndDate = $(".max-date").val();
        window.location.href = domainurl + "/Reports/LoadLeadSourceReportPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&GetReport=true" + "&searchtxt=" + encodeURI($("#sales_txt_search").val() + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + orderval + "&empGIDList=" + GetSelectedUserId());
    })
    $("#sales_inv_status").selectpicker('val', invstatus);
    $('.partner_report_load_container').height(window.innerHeight - 188);

    $("#sales_txt_search").keyup(function (event) {
        event.preventDefault();
        if (event.keyCode == 13) {
            FilterLeadSourceReport1(1)
        }
    })


})
$(window).resize(function () {
    $('.partner_report_load_container').height(window.innerHeight - 188);

});