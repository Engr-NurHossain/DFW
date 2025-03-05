var IsLead = true;
 
var TransferList = function (EstimatorId, startdate, enddate, estimateStatus, SearchText) { 
    var StartDayVal = startdate;
    var EndDayval = enddate;
    var startdates = $(".EstimateFilterStartDate").val();
    var enddates = $(".EstimateFilterEndDate").val();

    console.log(StartDayVal);
    console.log(EndDayval);
    console.log("estimateStatus 1", estimateStatus);

    //var partNumval = PartNumber;
    var EstimatorIdval = EstimatorId;
    if (IsLead == true) {
        $(".loadtransfferlist").load("/Estimator/InventoryTransferListPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + SearchText + "&StrStartDate=" + startdates + "&StrEndDate=" + enddates + "&estimateStatus=" + estimateStatus);
    }  
}
var EstimatorList = function (EstimatorId, startdate, enddate, estimateStatus, SearchText) {
    var StartDayVal = startdate;
    var EndDayval = enddate;
    var startdates = $(".EstimateFilterStartDate").val();
    var enddates = $(".EstimateFilterEndDate").val();

    console.log(StartDayVal);
    console.log(EndDayval);
    console.log("estimateStatus 1", estimateStatus);

    //var partNumval = PartNumber;
    var EstimatorIdval = EstimatorId;
    if (IsLead == true) {
        $(".loadreceiverlist").load("/Inventory/InventoryReceiveListPartial");
    } 
}
//InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Open");
var InitialEstimatorList = function (startdate, enddate, SearchText, estimateStatus) {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    console.log("call InitialEstimatorList")
    if (IsLead == true) {
        $(".loadestimatorlist").load("/Estimator/EstimatorListPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + SearchText + "&StrStartDate=" + startdate + "&StrEndDate=" + enddate + "&estimateStatus=" + estimateStatus);
    }
    else {
        $(LoadCustomerDiv + ".loadestimatorlist").load("/Estimator/EstimatorListPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + SearchText + "&StrStartDate=" + startdate + "&StrEndDate=" + enddate + "&estimateStatus=" + estimateStatus);
    } 
}
 
var EstimatePartial = {
    EstimatorClone: function (EstimatorId) {
        cancel = false;
        $.ajax({
            url: domainurl + "/Estimator/CloneEstimator",
            data: { EstimatorId: EstimatorId },
            type: "Post",
            dataType: "Json",
            success: function (data) {
                if (data.result) {
                    OpenSuccessMessageNew("Success!", data.message);
                    OpenEstimatorTab();
                    CloseTopToBottomModal();
                } else {
                    OpenErrorMessageNew("Error!", data.message);
                }

            }
        });
    }

};
var UpdatePtoCookie = function () {
    if (IsLead == true) {
        var FirstDayStr = $(".EstimateFilterStartDate").val();
        var EndDayStr = $(".EstimateFilterEndDate").val();
        var FilterWeeksStr = $(".FilterWeeks").val();
    }
    else {
        var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        var FirstDayStr = $(LoadCustomerDiv + ".EstimateFilterStartDate").val();
        var EndDayStr = $(LoadCustomerDiv + ".EstimateFilterEndDate").val();
        var FilterWeeksStr = $(LoadCustomerDiv + ".FilterWeeks").val();
    }
    var PTOFilterStr = "";
    var NewCookie = String.format("{0},{1},{2},{3}", FirstDayStr, EndDayStr, FilterWeeksStr, PTOFilterStr);

    $.cookie("_PtoFilter", NewCookie, { expires: 1 * 60 * 24, path: '/' });
}
var ConvertEstimateToOrder = function () {
    var url = "/Estimate/ConvertEstimateToWorkOrServiceOrder";
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        data: JSON.stringify({ customerid: customerid, InstallDate: insdate, detailId: detailId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {
                //$("#ConvertEstimatetoWorkOrService").addClass('disabled');
                if (data.ServiceAppointID != "00000000-0000-0000-0000-000000000000") {
                    OpenTopToBottomModal(domainurl + "/ServiceOrder/TopToBottomModalServiceOrder/?AppointmentId=" + data.ServiceAppointID + "&CustomerId=" + data.cusid);
                }
                else {
                    OpenTopToBottomModal(domainurl + "/WorkOrder/TopToBottomWorkOrder?AppointmentId=" + data.WorkAppointID + "&CustomerId=" + data.cusid);
                }
                OpenEstimateTab();
                //parent.CloseTopToBottomModal();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";

var SetLoaderText = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".openestimateTab_Load").html(TabsLoaderText);
    $(LoadCustomerDiv + ".acceptedestimatorTab_Load").html(TabsLoaderText);
    $(LoadCustomerDiv + ".pendingestimatorTab_Load").html(TabsLoaderText);
}


var OpenEstimatorTabLoad = function () {
    console.log(IsLead);
    console.log("Test 01 ");
    if (IsLead == true) {
        $(".loadestimatorlist").html(TabsLoaderText);
        InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Open");
    }
    else {
        var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        $(LoadCustomerDiv + ".loadestimatorlist").html(TabsLoaderText);
        InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Open");
    }

}
var PendingEstimatorTabLoad = function () {
    console.log("Estimator Pending");
    if (IsLead == true) {
        $(".loadestimatorlist").html(TabsLoaderText);
        InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Declined");
    }
    else {
        var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        $(LoadCustomerDiv + ".loadestimatorlist").html(TabsLoaderText);
        InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Declined");
    }

}
var AcceptedEstimatorTabLoad = function () {
    if (IsLead == true) {
        $(".loadestimatorlist").html(TabsLoaderText);
        InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Accepted");
    }
    else {
        var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        $(LoadCustomerDiv + ".loadestimatorlist").html(TabsLoaderText);
        InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Accepted");
    }

}
/*
var OpenLabourTabLoad = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".loadestimatorlist").html(TabsLoaderText);
    InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Labour");
}
var OpenMaterialsTabLoad = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".loadestimatorlist").html(TabsLoaderText);
    InitialEstimatorList(StartDatepicker, EndDatepicker, null, "Materials");
}
*/
$(".EstimatorReport").click(function () {
    var ids = "";
    var idsAll = "";
    var flag = 0;
    var StartDateVal = $("#StatDate").val();
    var EndDateVal = $("#EndDate").val();
    //var SearchText = $("#PTOFilterVal").val();
    var Status = $("#myEstimatorTab").hasClass('active');
    $(".CheckItems").each(function () {
        idsAll += $(this).attr("data-id") + ",";
        if ($(this).is(':checked')) {
            flag = 1;
            ids += $(this).attr("data-id") + ","
        }

    });
    //var ColumnName = "Estimate,Status,User,Date,Total,LastNoteAdded";

    if (flag == 0) {
        if (idsAll != "") {
            console.log(CustomerLoadId);
            window.location.href = domainurl + "/Reports/DownloadEstimatorList/?CustomerId=" + CustomerLoadGuid + "&SelectAllIds=" + idsAll + "&StrStartDate=" + StartDateVal + "&StrEndDate=" + EndDateVal + "&estimateStatus=" + Status;// + "&SearchText=" + SearchText;
        }
        else {
            OpenErrorMessageNew("", "Estimate List are empty.");
        }
    }
    else {
        console.log(CustomerLoadId);
        window.location.href = domainurl + "/Reports/DownloadEstimatorList/?CustomerId=" + CustomerLoadGuid + "&SelectAllIds=" + ids + "&StrStartDate=" + StartDateVal + "&StrEndDate=" + EndDateVal + "&estimateStatus=" + Status; // + "&SearchText=" + SearchText;
    }


});

$(document).ready(function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(".EstimateFilterStartDate").val("");
    $('.EstimateFilterEndDate').val("");
    OpenEstimatorTabLoad();

    console.log("sdfdsfdsf");
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }




    var idlist = [{ id: LoadCustomerDiv + ".InvoicePrint", type: 'iframe', width: Popupwidth, height: 600 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    $("#IsCheckValee").change(function () {
        console.log("hlw");
        if ($(this).is(':checked')) {
            $(LoadCustomerDiv + ".CheckItems").each(function () {
                $(this).prop('checked', true);
            });
        }
        else {
            $(LoadCustomerDiv + ".CheckItems").each(function () {
                $(this).prop('checked', false);
            });
        }
    })
    //$('[data-toggle="tooltip"]').tooltip();




    if (IsLead == true) {
        $(".EstimateFilterStartDate").val("");
        $('.EstimateFilterEndDate').val("");

        $('.FilterWeeks').val('-1');
        $(".PayrollFilterBtn").click(function () {
            console.log("ssa obayed");
            var estimatestatus = $("#myEstimatorTab li.active").attr('data-tab');
            var startdate = $(".EstimateFilterStartDate").val();
            var enddate = $('.EstimateFilterEndDate').val();
            var EstimatorId = $('.searchtext').val();
            //var SearchText = $('.searchtext').val();
            EstimatorList(EstimatorId, startdate, enddate, estimatestatus);
            /*UpdatePtoCookie();*/
        });
    }
    else {
        $(LoadCustomerDiv + ".EstimateFilterStartDate").val("");
        $(LoadCustomerDiv + '.EstimateFilterEndDate').val("");

        $(LoadCustomerDiv + '.FilterWeeks').val('-1');
        $(LoadCustomerDiv + ".PayrollFilterBtn").click(function () {
            console.log("ssa");
            var estimatestatus = $("#myEstimatorTab li.active").attr('data-tab');
            var startdate = $(LoadCustomerDiv + ".EstimateFilterStartDate").val();
            var enddate = $(LoadCustomerDiv + '.EstimateFilterEndDate').val();
            var EstimatorId = $('.searchtext').val();
            //var SearchText = $('.searchtext').val();
            EstimatorList(EstimatorId, startdate, enddate, estimatestatus);
            /*UpdatePtoCookie();*/
        });
    }
    $(LoadCustomerDiv + ".FilterWeeks").change(function () {
        if ($(this).val().split('/').length == 2) {
            var StartingDay = getDateOfISOWeek($(this).val().split('/')[1], $(this).val().split('/')[0]);
            StartDatepicker.setDate(StartingDay);
            var EndingDay = StartingDay.addDays(6);
            EndDatepicker.setDate(EndingDay);
        }
    });
    StartDatepicker = new Pikaday({
        field: $(LoadCustomerDiv + '.EstimateFilterStartDate')[0],
        format: 'MM/DD/YYYY'
    });
    EndDatepicker = new Pikaday({
        field: $(LoadCustomerDiv + '.EstimateFilterEndDate')[0],
        format: 'MM/DD/YYYY'
    });

    $(LoadCustomerDiv + ".EstimatorReport").click(function () {
        var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
        var ids = "";
        var idsAll = "";
        var flag = 0;
        var StartDateVal = $("#StatDate").val();
        var EndDateVal = $("#EndDate").val();
        //var SearchText = $("#PTOFilterVal").val();
        var Status = $("#myEstimatorTab").hasClass('active');
        $(LoadCustomerDiv + ".CheckItems").each(function () {
            idsAll += $(this).attr("data-id") + ",";
            if ($(this).is(':checked')) {
                flag = 1;
                ids += $(this).attr("data-id") + ","
            }

        });
        //var ColumnName = "Estimate,Status,User,Date,Total,LastNoteAdded";

        if (flag == 0) {
            if (idsAll != "") {
                console.log(CustomerLoadId);
                window.location.href = domainurl + "/Reports/DownloadEstimatorList/?CustomerId=" + CustomerLoadGuid + "&SelectAllIds=" + idsAll + "&StrStartDate=" + StartDateVal + "&StrEndDate=" + EndDateVal + "&estimateStatus=" + Status;// + "&SearchText=" + SearchText;
            }
            else {
                OpenErrorMessageNew("", "Estimate List are empty.");
            }
        }
        else {
            console.log(CustomerLoadId);
            window.location.href = domainurl + "/Reports/DownloadEstimatorList/?CustomerId=" + CustomerLoadGuid + "&SelectAllIds=" + ids + "&StrStartDate=" + StartDateVal + "&StrEndDate=" + EndDateVal + "&estimateStatus=" + Status; // + "&SearchText=" + SearchText;
        }


    });

    $(LoadCustomerDiv + ".InvPrint").click(function () {

        var ids = "";
        var idsAll = "";
        var flag = 0;
        $(LoadCustomerDiv + ".CheckItems").each(function () {
            idsAll += $(this).attr("data-id") + ",";
            if ($(this).is(':checked')) {
                flag = 1;
                ids += $(this).attr("data-id") + ","
            }

        });


        if (flag == 0) {
            if (idsAll != "") {
                $(LoadCustomerDiv + ".InvoicePrint").attr("href", domainurl + "/Invoice/GetInvoiceListPartial/?Ids=" + idsAll);
                $(LoadCustomerDiv + ".InvoicePrint").click();
            }
            else {
                OpenErrorMessageNew("", "Estimate List are empty.", function () {
                    location.reload();
                });
            }

            // window.location.href = "/Invoice/GetInvoiceListPartial/?Ids=" + idsAll ;
        }
        else {
            $(LoadCustomerDiv + ".InvoicePrint").attr("href", domainurl + "/Invoice/GetInvoiceListPartial/?Ids=" + ids);
            $(LoadCustomerDiv + ".InvoicePrint").click();
            //window.location.href = "/Invoice/GetInvoiceListPartial/?Ids=" + ids;
        }


    });


    $(".InvPrintForPhone").click(function () {

        var ids = "";
        var idsAll = "";
        var flag = 0;
        $(".CheckItems").each(function () {
            idsAll += $(this).attr("data-id") + ",";
            if ($(this).is(':checked')) {
                flag = 1;
                ids += $(this).attr("data-id") + ","
            }

        });


        if (flag == 0) {
            if (idsAll != "") {
                var IdSallSt = idsAll.toString();
                var DownloadUrl = domainurl + "/Invoice/DownLoadAllInvoicePdfList/?InvIdList=" + IdSallSt;
                console.log(DownloadUrl);
                parent.window.open(DownloadUrl, '_blank');
                parent.$.magnificPopup.close();

            }
            else {
                OpenErrorMessageNew("", "Estimate List are empty.", function () {
                    location.reload();
                });
            }

            // window.location.href = "/Invoice/GetInvoiceListPartial/?Ids=" + idsAll ;
        }
        else {
            var IdSallSt = ids.toString();
            var DownloadUrl = domainurl + "/Invoice/DownLoadAllInvoicePdfList/?InvIdList=" + IdSallSt;
            console.log(DownloadUrl);
            parent.window.open(DownloadUrl, '_blank');
            parent.$.magnificPopup.close();

            //window.location.href = "/Invoice/GetInvoiceListPartial/?Ids=" + ids;
        }


    });
 
    $(".ConvertEstimateToOrder").click(function () {
        insdate = $(this).attr('idval');
        customerid = $(this).attr('data-id');
        detailId = $(this).attr('idval1')
        if (insdate != "1/1/0001 12:00:00 AM") {
            OpenSuccessMessageNew("Success!", "Are you want to convert estimate to work or service order", ConvertEstimateToOrder);
        }
        else {
            OpenErrorMessageNew("Error!", "Please check new estimate install date", "");
        }
    });
    if ($(".PTOFilter").val() == "AllTime") {
        var Today = new Date();
        //$("#PayrollFilterStartDate").val("");
        //$("#PayrollFilterEndDate").val("");
        var FirstDayOfMonth = FirstDayOfMonthForAll;
        var LastDayOfMonth = new Date(Today.getFullYear() + 1, 11, 31);

        StartDatepicker.setDate(FirstDayOfMonth);
        EndDatepicker.setDate(LastDayOfMonth);


    }
    $(".PTOFilter").change(function () {
        if ($(this).val() == "Today") {
            var Today = new Date();
            StartDatepicker.setDate(Today);
            EndDatepicker.setDate(Today);
        }
        else if ($(this).val() == "Yesterday") {
            var Today = new Date();
            EndDatepicker.setDate(Today.addDays(-1));
            StartDatepicker.setDate(Today);
        }
        else if ($(this).val() == "ThisWeek") {
            var Today = new Date();
            var Week = Today.getWeek();
            var StartDay = getDateOfISOWeek(Week, Today.getFullYear());

            StartDatepicker.setDate(StartDay);
            EndDatepicker.setDate(StartDay.addDays(6));
        }
        else if ($(this).val() == "LastWeek") {
            var Today = new Date();
            Today = Today.addDays(-7);

            var Week = Today.getWeek();
            var StartDay = getDateOfISOWeek(Week, Today.getFullYear());

            StartDatepicker.setDate(StartDay);
            EndDatepicker.setDate(StartDay.addDays(6));
        }
        else if ($(this).val() == "ThisMonth") {
            var Today = new Date();

            var FirstDayOfMonth = new Date(Today.getFullYear(), Today.getMonth(), 1);
            var LastDayOfMonth = new Date(Today.getFullYear(), Today.getMonth() + 1, 0);

            StartDatepicker.setDate(FirstDayOfMonth);
            EndDatepicker.setDate(LastDayOfMonth);

        }
        else if ($(this).val() == "LastMonth") {
            var Today = new Date();

            var FirstDayOfMonth = new Date(Today.getFullYear(), Today.getMonth() - 1, 1);
            var LastDayOfMonth = new Date(Today.getFullYear(), Today.getMonth(), 0);

            StartDatepicker.setDate(FirstDayOfMonth);
            EndDatepicker.setDate(LastDayOfMonth);
        }
        else if ($(this).val() == "ThisYear") {
            var Today = new Date();

            var FirstDayOfMonth = new Date(Today.getFullYear(), 0, 1);
            var LastDayOfMonth = new Date(Today.getFullYear(), 11, 31);

            StartDatepicker.setDate(FirstDayOfMonth);
            EndDatepicker.setDate(LastDayOfMonth);
        }
        else if ($(this).val() == "LastYear") {
            var Today = new Date();

            var FirstDayOfMonth = new Date(Today.getFullYear() - 1, 0, 1);
            var LastDayOfMonth = new Date(Today.getFullYear() - 1, 11, 31);

            StartDatepicker.setDate(FirstDayOfMonth);
            EndDatepicker.setDate(LastDayOfMonth);
        }
        else if ($(this).val() == "AllTime") {
            var Today = new Date();
            //$("#PayrollFilterStartDate").val("");
            //$("#PayrollFilterEndDate").val("");
            var FirstDayOfMonth = FirstDayOfMonthForAll;
            var LastDayOfMonth = new Date(Today.getFullYear() + 1, 11, 31);

            StartDatepicker.setDate(FirstDayOfMonth);
            EndDatepicker.setDate(LastDayOfMonth);
        }
    });
});