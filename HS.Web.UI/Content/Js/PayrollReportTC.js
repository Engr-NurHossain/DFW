var LoadUserTimeClock = function (pageno, order, id, userid) {
    if (typeof (pageno) == "undefined" && pageno == null && pageno == "") {
        pageno = 1;
    }
    pagesize = 10;
    var LoadUrl = domainurl + String.format("/TimeClockPto/EmployeeTimeClockListPayroll/?StrStartDate={0}&StrEndDate={1}&order={2}&pageno={3}&pagesize={4}&UserId={5}",
    startdate, endate, order, pageno, pagesize, userid);
    $(".Loadpayrollrep_" + id).html(LoaderDom);
    $(".Loadpayrollrep_" + id).load(LoadUrl);
}
$(document).ready(function () {
    var idlist = [{ id: ".OpenMapPopup", type: 'iframe', width: 500, height: 500 }];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".icon_sort_pto").click(function () {
        var orderval = $(this).attr('data-val');
        LoadUserTimeClock(pageno, orderval, empid, userid);
    })
});