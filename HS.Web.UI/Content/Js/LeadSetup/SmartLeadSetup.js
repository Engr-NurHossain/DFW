var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var step = parseInt($("#step").val());
var CalActive = function () {
    var lilist = $(".lilist");
    var activeCount = $(".lilist.current").length;
    for (var icoun = 0; icoun < lilist.length; icoun++) {
        var itemtarget = lilist[icoun];
        if ($(itemtarget).hasClass('current')) {
            if (step == icoun + 1) {
                //if (step == 2 && IsLead == "0")
                //{
                //    $("#btnSavandNex span").text("Save & Close");
                //}
                //if (step == 3 && IsLead == "0") {
                //    window.location.href = '/Customer/Customerdetail/?id=' + LeadId + '#TicketTab';
                //}
                if (step != 5) {
                    $("#LoadLeadDetail").html(LoaderDom);
                }
                $($(itemtarget).next().find('a').parent()).addClass("current");
                $($(itemtarget).next().find('a').parent()).addClass("activeli");
                $($(itemtarget).find('a').parent()).removeClass("activeli");
                $("#LoadLeadDetail").load($($(itemtarget).next()).attr('data-url'));
                if (step == 4) {
                    $("#btnSavandNex span").text("Save & Review");
                    $("#btnSavandClose").removeClass('hidden');
                    $(".ContractTypeDiv").removeClass('hidden');
                    $("#btnPayNow").removeClass('hidden');
                    $("#btnsendEcontract").removeClass('hidden');
                    $("#btnsendIsPccontract").removeClass('hidden');
                    $("#leadToCustomerConvert").removeClass('hidden');
                }
                return false;
            }
        }
    }
}
var ClosePopup = function () {
    $.magnificPopup.close();
}


var LeadToCustomer = function (CustomerId, IntId, ProceedWithoutPaymentMethod) {


    console.log("ConvertLeadToCustomer");
    var url = domainurl + "/SmartLeads/ConvertLeadToCustomer/";
    var param = JSON.stringify({
        CustomerIdStr: CustomerId,
        Id: IntId,
        ProceedWithoutPaymentMethod: ProceedWithoutPaymentMethod
    });
    $.ajax({
        url: url,
        data: param,
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            console.log("casc");
            $(".lead_setup_partial_loader").addClass("hidden");
            $(".lead_setup_partial_outer").removeClass("hidden");
            if (data.result == false && typeof (data.noPayment) != "undefined") {
                OpenConfirmationMessageNew("", data.message, function () {
                    LeadToCustomer(CustomerId, IntId, true);
                });
            }
            else if (data.result == true) {
                var SuccessMessage = "Converted to customer successfully.";

                if (typeof (IsLead) != "undefined" && IsLead == "0") {
                    SuccessMessage = "Contract updated successfully.";
                }
                
                OpenSuccessMessageNew("", SuccessMessage, function () {
                    window.location.href = "/Customer/Customerdetail/?id=" + data.CID;
                });

                LoadCustomerDetail(data.CID, true);
            }
        }
    });
}


$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#LoadPackage").addClass("activeli");
    $("#btnSavandNex").click(function () {
        console.log('btnSavandNex2');
        step = parseInt($("#step").val());
        if (CommonUiValidation()) {
            if ($("#TblEmergencyContactList tbody tr").length < parseInt(contactreq) && $("#step").val() == "4") {

            }
            else if ($(".SmartSetUpServiceList tbody tr").length < 1 && oneServicerequired == "True" && $("#step").val() == "2") {

            }
            else {

                CalActive();
            }
        }
    });
    $(".cd-breadcrumb li").click(function (item) {
        var SelectVal = 0;
        var liselected = $(item.target).parent();
        var liselectedspan = $(item.target).parent().parent();
        var selectedindex = -1;
        if ($(liselectedspan).hasClass('current'))
            liselected = liselectedspan;
        if ($(liselected).hasClass('current')) {
            console.log(liselected);
            $(liselected).addClass('activeli');
            var liList = $(".custom_container li");
            for (var ilicount = 0; ilicount < liList.length; ilicount++) {
                if ($(liList[ilicount]).attr('id') == $(liselected).attr('id')) {
                    selectedindex = ilicount;
                    $("#LoadLeadDetail").html(LoaderDom);
                    $("#LoadLeadDetail").load($(liselected).attr('data-url'));
                    $("#btnSavandNex span").text("Save & Next");
                }
            }
            selectedindex = selectedindex + 1;
            for (var ilicount2 = 0; ilicount2 < selectedindex; ilicount2++) {
                $(liList[ilicount2]).addClass('current');
            }
            for (var ilicount = 0; ilicount < liList.length; ilicount++) {
                if ($(liList[ilicount]).attr('id') != $(liselected).attr('id')) {
                    $(liList[ilicount]).removeClass('activeli');
                }
            }
            if (selectedindex == 5) {
                $("#btnSavandNex span").text("Save & Review");
                $("#btnSavandClose").removeClass('hidden');
                $(".ContractTypeDiv").removeClass('hidden');
                $("#btnPayNow").removeClass('hidden');
                $("#btnsendEcontract").removeClass('hidden');
                $("#btnsendIsPccontract").removeClass('hidden');
                $("#leadToCustomerConvert").removeClass('hidden');
            }
            else {
                $("#btnSavandClose").addClass('hidden');
                $(".ContractTypeDiv").addClass('hidden');
                $("#btnPayNow").addClass('hidden');
                $("#btnsendEcontract").addClass('hidden');
                $("#btnsendIsPccontract").addClass('hidden');
                $("#leadToCustomerConvert").addClass('hidden');
            }
            if (selectedindex == 3 && IsLead == "0") {
                $("#btnSavandNex span").text("Save & Close");
            }
            else if (selectedindex != 5) {
                $("#btnSavandNex span").text("Save & Next");
            }
        }
    });

    $(".leadToCustomerConvert").click(function () {
        console.log("Smart start up for convert to lead")

        var customerId = $(this).attr('idval');
        var CustomerName = $(this).attr('idval1');
        var CustomerMail = $(this).attr('idval2');
        var Id = $(this).attr('data-id');
        if (CustomerName != "" && CustomerMail != "") {
            OpenConfirmationMessageNew("Confirm?", ConvertLeadToCustomerMsg, function () {
                LeadToCustomer(customerId, Id, false);
                $(".lead_setup_partial_outer").addClass("hidden");
                $(".lead_setup_partial_loader").html(TabsLoaderText);
            });
        }
        else {
            OpenErrorMessageNew("Error!", ConvertLeadToCustomerErrorMsg, "");
        }
    });


});

