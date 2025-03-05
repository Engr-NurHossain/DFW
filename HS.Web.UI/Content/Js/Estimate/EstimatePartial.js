var EstimateList = function (startdate, enddate, SearchText, estimateStatus) {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    var StartDayVal = startdate;
    var EndDayval = enddate;



    $(LoadCustomerDiv + ".loadestimatelist").load("/Estimate/EstimateListPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + SearchText + "&StrStartDate=" + StartDayVal + "&StrEndDate=" + EndDayval + "&estimateStatus=" + estimateStatus);
}
var InitialEstimateList = function (startdate, enddate, SearchText, estimateStatus) {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".loadestimatelist").load("/Estimate/EstimateListPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + SearchText + "&StrStartDate=" + startdate + "&StrEndDate=" + enddate + "&estimateStatus=" + estimateStatus);
}


var UpdatePtoCookie = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    var FirstDayStr = $(LoadCustomerDiv + ".EstimateFilterStartDate").val();
    var EndDayStr = $(LoadCustomerDiv + ".EstimateFilterEndDate").val();
    var FilterWeeksStr = $(LoadCustomerDiv + ".FilterWeeks").val();
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
    $(LoadCustomerDiv + ".completeestimateTab_Load").html(TabsLoaderText);
  
}
var OpenEstimateTabLoad = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".loadestimatelist").html(TabsLoaderText);
    InitialEstimateList(StartDatepicker, EndDatepicker, null, "Open");
}

var CompletedEstimateTabLoad = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".loadestimatelist").html(TabsLoaderText);
    InitialEstimateList(StartDatepicker, EndDatepicker, null, "Completed");
}
$(document).ready(function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    InitialEstimateList(StartDatepicker, EndDatepicker, null, "Open");
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


    $(LoadCustomerDiv + ".EstimateFilterStartDate").val("");
    $(LoadCustomerDiv + '.EstimateFilterEndDate').val("");

    $(LoadCustomerDiv + '.FilterWeeks').val('-1');
    $(LoadCustomerDiv + ".PayrollFilterBtn").click(function () {
        var estimatestatus = $("#myEstimateTab li.active").attr('data-tab');
        var startdate = $(LoadCustomerDiv + ".EstimateFilterStartDate").val();
        var enddate = $(LoadCustomerDiv + '.EstimateFilterEndDate').val();
        var SearchText = $('.searchtext').val();
        EstimateList(startdate, enddate, SearchText, estimatestatus);
        /*UpdatePtoCookie();*/


    });
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
    $(LoadCustomerDiv + ".InvoiceReport").click(function () {

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
        var ColumnName = "Estimate,Status,User,Date,Total,LastNoteAdded";

        if (flag == 0) {
            if (idsAll != "") {
                window.location.href = domainurl + "/Reports/NewReport/?ColumnNames=" + ColumnName + "&ReportFor=Estimate&SelectAllIds=" + idsAll;
            }
            else {
                OpenErrorMessageNew("", "Estimate List are empty.");
            }
        }
        else {
            window.location.href = domainurl + "/Reports/NewReport/?ColumnNames=" + ColumnName + "&ReportFor=Estimate&SelectAllIds=" + ids;
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
    if ($(LoadCustomerDiv + ".PTOFilter").val() == "AllTime") {
        var Today = new Date();
        //$("#PayrollFilterStartDate").val("");
        //$("#PayrollFilterEndDate").val("");
        var FirstDayOfMonth = FirstDayOfMonthForAll;
        var LastDayOfMonth = new Date(Today.getFullYear() + 1, 11, 31);

        StartDatepicker.setDate(FirstDayOfMonth);
        EndDatepicker.setDate(LastDayOfMonth);


    }
    $(LoadCustomerDiv + ".PTOFilter").change(function () {
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