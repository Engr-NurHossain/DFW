var EstimateList = function (startdate, enddate, SearchText) {

    //$(".customer-options-tabs li").removeClass('active');
    //$(".tab_Content_customer_items .tab-pane").removeClass('active');
    //$(".EstimateTab").removeClass('hidden');
    //$(".EstimateTab").addClass('active');
    //$(".EstimateTab_Load").addClass('active');
    //$(".EstimateTab_Load").html(TabsLoaderText);
    var StartDayVal = startdate;
    var EndDayval = enddate;


    $("#LeadEstimate").load(domainurl + "/Leads/LeadEstimatePartial/?CustomerId=" + customerId + "&SearchText=" + SearchText + "&StrStartDate=" + StartDayVal + "&StrEndDate=" + EndDayval);
    LeadDetailTabCount();
}
var UpdatePtoCookie = function () {

    var FirstDayStr = $(".EstimateFilterStartDate").val();
    var EndDayStr = $(".EstimateFilterEndDate").val();
    var FilterWeeksStr = $(".FilterWeeks").val();
    var PTOFilterStr = "";
    var NewCookie = String.format("{0},{1},{2},{3}", FirstDayStr, EndDayStr, FilterWeeksStr, PTOFilterStr);

    $.cookie("_PtoFilter", NewCookie, { expires: 1 * 60 * 24, path: '/' });
}
var OpenEstById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        if (typeof (customerId) == "undefined") {
            customerId = 0;
        }
        OpenTopToBottomModal(domainurl + "/Leads/AddLeadEstimate/?customerid=" + customerId + "&Id=" + invId);
    }
}
//var OpenEstimatorById = function (EstimatorId) {
//    if (typeof (EstimatorId) != "undefined" && EstimatorId > 0) {
//        if (typeof (customerId) == "undefined") {
//            customerId = 0;
//        }
//        OpenTopToBottomModal(domainurl + "/Leads/AddLeadEstimator/?customerid=" + customerId + "&Id=" + invId);
//    }
//}
var OpenInvoiceNotesById = function (invId) {
    if (typeof (invId) != "undefined" && invId > 0) {
        if (typeof (customerId) == "undefined") {
            customerId = 0;
        }
        OpenRightToLeftModal(domainurl + "/Invoice/ShowInvoiceNotes/?InvoiceId=" + invId);
    }
}
$(document).ready(function () {

    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }

    var idlist = [{ id: ".InvoicePrint", type: 'iframe', width: Popupwidth, height: 600 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    $("#IsCheckValee").change(function () {
        console.log("hlw");
        if ($(this).is(':checked')) {
            $(".CheckItems").each(function () {
                $(this).prop('checked', true);
            });
        }
        else {
            $(".CheckItems").each(function () {
                $(this).prop('checked', false);
            });
        }
    })
    //$('[data-toggle="tooltip"]').tooltip()
    var EstimateTable = $('#tblEstimate').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "language": {
            "emptyTable": "No data available"
        },
        "order": [[2, "desc"]]
    });
    StartDatepicker = new Pikaday({
        field: $('.EstimateFilterStartDate')[0],
        format: 'MM/DD/YYYY'
    });
    EndDatepicker = new Pikaday({
        field: $('.EstimateFilterEndDate')[0],
        format: 'MM/DD/YYYY'
    });
    $(".EstimateFilterStartDate").val("");
    $('.EstimateFilterEndDate').val("");

    $('.FilterWeeks').val('-1');
    $(".PayrollFilterBtn").click(function () {
        var startdate = $(".EstimateFilterStartDate").val();
        var enddate = $('.EstimateFilterEndDate').val();
        var SearchText = $('.searchtext').val();
        EstimateList(startdate, enddate, SearchText);
        /*UpdatePtoCookie();*/


    });
    EstimateTable.order([0, 'desc']).draw();
    $("#AddLeadEstimate").click(function () {
        OpenTopToBottomModal(domainurl + "/Leads/AddLeadEstimate?customerid=" + customerId);
    });
    $("#tblEstimate_wrapper").find('.row').css("margin-left", "0");
    $("#tblEstimate_wrapper").find('.row').css("margin-right", "0");
    $("#tblEstimate_wrapper").find('.col-sm-12').css("padding-left", "0");
    $("#tblEstimate_wrapper").find('.col-sm-12').css("padding-right", "0");
    $("#tblEstimate_wrapper").find('.col-sm-7').css("padding-right", "0");

    $(".InvoiceReport").click(function () {

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

    $(".InvPrint").click(function () {

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
                $(".InvoicePrint").attr("href", domainurl + "/Invoice/GetInvoiceListPartial/?Ids=" + idsAll);
                $(".InvoicePrint").click();
            }
            else {
                OpenErrorMessageNew("", "Estimate List are empty.", function () {
                    location.reload();
                });
            }

            // window.location.href = "/Invoice/GetInvoiceListPartial/?Ids=" + idsAll ;
        }
        else {
            $(".InvoicePrint").attr("href", domainurl + "/Invoice/GetInvoiceListPartial/?Ids=" + ids);
            $(".InvoicePrint").click();
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
            var FirstDayOfMonth = '@ViewBag.FilterStartDate';
            var LastDayOfMonth = new Date(Today.getFullYear() + 1, 11, 31);

            StartDatepicker.setDate(FirstDayOfMonth);
            EndDatepicker.setDate(LastDayOfMonth);
        }
    });
})