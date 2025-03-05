var FilterPayrollPaging = function (pageno, order) {
    var StartDate = $("#PayrollFilterStartDate").val();
    var EndDate = $("#PayrollFilterEndDate").val();
    var FilterWeek = $("#FilterWeeks").val();
    var CurrentEmployee = $("#CurrentEmployee").val();
    //var LoadUrl = String.format("/TimeClockPto/PayrollReport/?StrStartDate={0}&StrEndDate={1}&PageNo={2}&StrSearchWeek={3}", 
    //    StartDate, EndDate, PageNo, $("#FilterWeeks").val());

    //var LoadUrl = String.format("/TimeClockPto/GetAllEmploployeePayrollReport/?FilterWeek={0}&order={1}&pageno={2}&pagesize={3}",
    var LoadUrl = domainurl + String.format("/TimeClockPto/GetAllEmploployeePayrollReport/?StrStartDate={0}&StrEndDate={1}&order={2}&pageno={3}&pagesize={4}&CurrentEmployee={5}",
        StartDate, EndDate, order, pageno, 10, CurrentEmployee);
    $(".payrollReportContainer").html(LoaderDom);
    $(".payrollReportContainer").load(LoadUrl);
}

var DownloadPayrollReport = function () {
    var CurrentEmployee = $("#CurrentEmployee").val();
    window.open(domainurl + "/Reports/GetAllEmploployeeTimeClockReport/?StrStartDate=" + $("#PayrollFilterStartDate").val()
        + "&StrEndDate=" + $("#PayrollFilterEndDate").val() + "&CurrentEmployee=" + CurrentEmployee);
}