var FilterPayrollPaging = function (pageno, order) {
    var StartDayeVal = $('#PayrollFilterStartDate').val();
    var EndDayval = $('#PayrollFilterEndDate').val();
    var PtoStatus = encodeURI($("#PTOStutas").val());
    var LoadUrl = domainurl + String.format("/TimeClockPto/PTOListPartial/?StrStartDate={0}&StrEndDate={1}&PtoStatus={2}&order={3}&pageno1={4}&pagesize1={5}", StartDayeVal, EndDayval, PtoStatus, order, pageno, 10);
    $(".PTO_Main_Container .pto_partial_info_table_container").load(LoadUrl);
}
$(document).ready(function () {
    if (liststatus != null) {
        if (liststatus.length != 0) {
            $(".PTOStutas").selectpicker('val', liststatus);
        }
        else {
            $(".PTOStutas").selectpicker('val', '');
        }
    }
    else {
        $(".PTOStutas").selectpicker('val', '');
    }
    $("#btnAddPto").click(function () {
        OpenRightToLeftModal(domainurl + "/Calendar/AddPtoPartial");
    });    
    if (HourRemaining < 0) {
        $(".remaining_hour").css("color", "red");
    }
});