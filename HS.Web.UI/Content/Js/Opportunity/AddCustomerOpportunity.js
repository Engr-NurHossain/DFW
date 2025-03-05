var CloseDatepicker;
var InitializeCustomerDropdown = function (dropdownitem) {
    $(dropdownitem).select2({
        placeholder: 'Customer',
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/Ticket/GetCustomerList",
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (term) {
                return {
                    q: term
                };
            },
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (item) {
                        if (typeof (item.Street) != "undefined" && item.Street != null && item.Street != "") {
                            if (typeof (item.BusinessName) != "undefined" && item.BusinessName != null && item.BusinessName != "") {
                                return {
                                    text: item.CustomerName + " (" + item.BusinessName + ")" + " [" + item.Street + "]",
                                    id: item.CustomerId
                                }
                            }
                            else {
                                return {
                                    text: item.CustomerName + " [" + item.Street + "]",
                                    id: item.CustomerId
                                }
                            }
                        }
                        else {
                            if (typeof (item.BusinessName) != "undefined" && item.BusinessName != null && item.BusinessName != "") {
                                return {
                                    text: item.CustomerName + " (" + item.BusinessName + ")",
                                    id: item.CustomerId
                                }
                            }
                            else {
                                return {
                                    text: item.CustomerName,
                                    id: item.CustomerId
                                }
                            }
                        }
                    })
                };
            }
        }
    });
    $(dropdownitem).on("select2:closing", function (e) {

    });


}
var SaveOpportunity = function (fromCustomer) {

    if (CommonUiValidation()) {
        var IsForeCast = false;
        if ($("#IsForecast").val() == "Yes") {
            IsForeCast = true;
        }
        var Param = {
            "Id": $("#Id").val(),
            "OpportunityId": $("#OpportunityId").val(),
            "CustomerId": $("#CustomerId").val(),
            "OpportunityName": $("#OpportunityName").val(),
            "Type": $("#Type").val(),
            "LeadSource": $("#LeadSource").val(),
            "Revenue": ($(".Revenue").val()) != "NaN" ? $(".Revenue").val().replaceAll(",", "") : 0,
            "ProjectedGP": ($(".ProjectedGP").val()) != "NaN" ? $(".ProjectedGP").val().replaceAll(",", "") : 0,
            "Points": ($(".Points").val()) != "NaN" ? $(".Points").val().replaceAll(",", "") : 0,
            "TotalProjectedGP": ($(".TotalProjectedGP").val()) != "NaN" ? $(".TotalProjectedGP").val().replaceAll(",", "") : 0,
            "CloseDate": CloseDatepicker.getDate(),
            "Status": $("#Status").val(),
            "Probability": $("#Probability").val(),
            "DealReason": $("#DealReason").val(),
            "IsForecast": IsForeCast,
            "DeliveryDays": $("#DeliveryDays").val(),
            "Competitors": $("#Competitors").val(),
            "CampaignSource": $("#CampaignSource").val(),
            "AccountOwner": $("#AccountOwner").val(),
            "FromCustomer": fromCustomer
        };
        console.log(Param)
        var url = domainurl + "/Opportunity/SaveOpportunity";
        $.ajax({
            type: "POST",
            ajaxStart: function () { },
            url: url,
            data: JSON.stringify(Param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.fromCustomer == 'false') {
                    OpenSuccessMessageNew("Success !", "Opportunity saved successfully", function () {
                        OpenOpportunityTab();
                        CloseTopToBottomModal();
                    });
                }
                else if (data.fromCustomer == 'true') {
                    OpenSuccessMessageNew("Success !", "Opportunity saved successfully", function () {
                        OpenOpportunityTab();
                        CloseTopToBottomModal();
                    });
                }
                else {
                    OpenErrorMessageNew("Error!", "Please check your given input.");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }
  
}

var DeleteOpportunity = function (Id) {

    var url = "/Opportunity/DeleteOpportunity";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify({
            Id: Id,

        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                  
                    OpenOpportunityTab();
                    CloseTopToBottomModal();
                });

            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    CloseDatepicker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#CloseDate')[0] });
    InitializeCustomerDropdown($('.dropdown_customar'));
    //$("#btnSaveOpportunity").click(function () {
    //    SaveOpportunity();
    //});
    $(".add_opportunity_inner").height(window.innerHeight - 95);
    $(".Revenue").blur(function () {
        var formattedAmount = parseFloat($(".Revenue").val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (formattedAmount == "NaN") {
            formattedAmount = 0;
        }
        $(".Revenue").val(formattedAmount);
    })
    $(".TotalProjectedGP").blur(function () {
        var formattedAmount = parseFloat($(".TotalProjectedGP").val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (formattedAmount == "NaN") {
            formattedAmount = 0;
        }
        $(".TotalProjectedGP").val(formattedAmount);
    })
    $("#OpportunityName").keyup(function ()
    {
        if($("#OpportunityName").val() != "")
        {
            $("#Oname").addClass("hidden");
            $("#OpportunityName").removeClass("required");
        }
        else {
            $("#Oname").removeClass("hidden");
            $("#OpportunityName").addClass("required");
        }
    })
    $(".ProjectedGP").blur(function () {
        var formattedAmount = parseFloat($(".ProjectedGP").val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (formattedAmount == "NaN") {
            formattedAmount = 0;
        }
        $(".ProjectedGP").val(formattedAmount);
        var ProjectedGp = $(".ProjectedGP").val().replaceAll(",", "");
        var Point;
        if ($(".Points").val() == "")
        {
            Point = 0;
        }
        else {
            Point = $(".Points").val().replaceAll(",", "");
        }
  
        ProjectedGp = parseFloat(ProjectedGp);
        Point = parseFloat(Point);
        var Total = (ProjectedGp + Point).toFixed(2);
        $(".TotalProjectedGP").val(Total);
    })
    $(".Points").blur(function () {
        var formattedAmount = parseFloat($(".Points").val()).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        if (formattedAmount == "NaN")
        {
            formattedAmount = 0;
        }
        $(".Points").val(formattedAmount);
        var ProjectedGp;
        if ($(".ProjectedGP").val() == "")
        {
            ProjectedGp = 0;
        }
        else
        {
            ProjectedGp = $(".ProjectedGP").val().replaceAll(",", "");
        }
        var Point = $(".Points").val().replaceAll(",", "");

       
        ProjectedGp = parseFloat(ProjectedGp);
        Point = parseFloat(Point);
        var Total = (ProjectedGp + Point).toFixed(2);
        $(".TotalProjectedGP").val(Total);
    })
});
$(window).resize(function () {
    $(".add_opportunity_inner").height(window.innerHeight - 115);
})