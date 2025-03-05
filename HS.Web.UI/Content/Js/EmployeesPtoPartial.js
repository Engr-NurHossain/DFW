var AcceptRejectConfirm = function (ptoId, accept) {
    var Param = {
        PtoId: ptoId,
        Accept: accept
    };
    var url = domainurl + "/TimeClockPto/AcceptRejectPTO";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message);
                OpenEmployeesPtoTab();
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var AcceptPto = function (ptoId) {
    OpenConfirmationMessageNew("Confirm?", "Approve this PTO?", function () {
        AcceptRejectConfirm(ptoId, true);
    });
}
var RejectPto = function (ptoId) {
    //OpenSuccessMessage("Confirmation!","Reject this PTO?",function(){
    //    AcceptRejectConfirm(ptoId,false);
    //});
    console.log("hi");
    $(".RejectPtoPopUp").attr('href', domainurl + "/TimeClockPto/RejectPtoPopUp/?id=" + ptoId);
    $(".RejectPtoPopUp").click();
}
//var liststatus = '@Html.Raw(Json.Encode(@ViewBag.liststatus))';
//liststatus = JSON.parse(liststatus);
//var FilterPayrollPaging = function (pageno, order) {
//    var StartDayeVal = $('#PayrollFilterStartDate').val();
//    var EndDayval = $('#PayrollFilterEndDate').val();
//    var PtoStatus = $("#PTOStutas").val();
//    console.log("sdfsdf" + order);
//    var LoadUrl = domainurl + String.format("/TimeClockPto/EmployeesPtoListPartial/?StrStartDate={0}&StrEndDate={1}&PtoStatus={2}&order={3}&pageno1={4}&pagesize1={5}", StartDayeVal, EndDayval, PtoStatus, order, pageno, 10);
//    $(".PTO_Main_Container .employees_pto_info_table_container").load(LoadUrl);
//}

$(document).ready(function () {
    var idlist = [{ id: ".OpenMapPopup", type: 'iframe', width: 500, height: 500 },
     { id: ".RejectPtoPopUp", type: 'iframe', width: 400, height: 200 }
    ];

    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $("#btnAddEmployeesPto").click(function () {
        OpenRightToLeftModal(domainurl + "/TimeClockPto/AddEmployeesPto");
    });
});