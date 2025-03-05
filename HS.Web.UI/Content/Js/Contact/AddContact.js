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
                        return {
                            text: (item.BusinessName != '' ? item.BusinessName : item.CustomerName) + (item.Street != "" ? " [" + item.Street + "]" : ""),
                            id: item.CustomerId
                        }
                    })
                };
            }
        }
    });
}
var InitializeLeadDropdown = function (dropdownitem) {
    $(dropdownitem).select2({
        placeholder: 'Lead',
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/Ticket/GetLeadList",
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

                        return {
                            text: (item.BusinessName != '' ? item.BusinessName : item.CustomerName) + (item.Street != "" ? " [" + item.Street + "]" : ""),
                            id: item.CustomerId
                        }
                    })
                };
            }
        }
    });
}
var InitializeContactDropdown = function (dropdownitem) {
    $(dropdownitem).select2({
        placeholder: 'Tags',
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/Contact/GetContactList",
            dataType: 'json',
            type: "GET",
            quietMillis: 50,
            data: function (term) {
                console.log(term);
                return {
                    q: term,
                    data: $("#Contact_tag").val()
                };
            },
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (item) {

                        return {
                            text: item.TagName,
                            id: item.TagIdentifier
                        }
                    })
                };
            }
        }
    });
}
var InitializeOpportunityDropdown = function (dropdownitem) {
    $(dropdownitem).select2({
        placeholder: 'Opportunity',
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: domainurl + "/Ticket/GetOpportunityList",
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

                        return {
                            text: item.OpportunityName,
                            id: item.OpportunityId
                        }
                    })
                };
            }
        }
    });
}
function FormatePhoneNumber(Value) {
    console.log("dfdsf");
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
        if (Value.length == 10) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "($1) $2-$3");
            $("#MobileNumber").css({ "border": "1px solid #babec5" });
            $("#MobileNumberVN").addClass("hidden");
            $("#MobileNumber").removeClass("required");
        }
        else if (Value.length > 10) {
            ValueClean = Value;
            $("#MobileNumber").css({ "border": "1px solid red" });
            $("#MobileNumberVN").removeClass("hidden");
            $("#MobileNumber").addClass("required");
        }
        else {
            $("#MobileNumber").css({ "border": "1px solid red" });
            $("#MobileNumberVN").removeClass("hidden");
            $("#MobileNumber").addClass("required");
            ValueClean = Value;
        }
    }
    return ValueClean;
}
function FormateWorkNumber(Value) {
    var ValueClean = "";
    if (Value != undefined && Value != "" && Value != null) {
        Value = Value.replace(/[-()  ]/g, '');
        if (Value.length == 10) {
            ValueClean = Value.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "($1) $2-$3");
            $("#WorkNumeber").css({ "border": "1px solid #babec5" });
            $("#WorkNumeberVN").addClass("hidden");
            $("#WorkNumeber").removeClass("required");
        }
        else if (Value.length > 10) {
            ValueClean = Value;
            $("#WorkNumeber").css({ "border": "1px solid red" });
            $("#WorkNumeberVN").removeClass("hidden");
            $("#WorkNumeber").addClass("required");
        }
        else {
            $("#WorkNumeber").css({ "border": "1px solid red" });
            $("#WorkNumeberVN").removeClass("hidden");
            $("#WorkNumeber").addClass("required");
            ValueClean = Value;
        }
    }
    return ValueClean;
}
var SaveContact = function (FromCustomer) {
    console.log("dsfd");
    var WorkNumeberckeck = false;
    var MobileNumberckeck = false;
    var Comonvckeck = false;

    


    var WorkNumeberV = $("#WorkNumeber").val().replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1$2$3");
    var MobileNumberV = $("#MobileNumber").val().replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1$2$3");

   

    //var MobileNumberV = $(".MobileNumber").val().replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1$2$3");
    if (WorkNumeberV != undefined && WorkNumeberV != "" && WorkNumeberV != null && FormateWorkNumber(WorkNumeberV).length == 12 && phone_validate(WorkNumeberV))
    {
        $("#WorkNumeberVN").addClass("hidden");
        $("#WorkNumeber").removeClass("required");
        WorkNumeberckeck = true;

    }
    else {
        $("#WorkNumeberVN").removeClass("hidden");
        $("#WorkNumeber").addClass("required");
        WorkNumeberckeck = false;

    }
    if (MobileNumberV.length == 0 ) {
        
            $("#MobileNumberVN").addClass("hidden");
            $("#MobileNumber").removeClass("required");
            $("#MobileNumber").css({ "border": "1px solid #babec5" });
            MobileNumberckeck = true;
        
    }
    
    else if (FormatePhoneNumber(MobileNumberV).length == 12) {
        $("#MobileNumberVN").addClass("hidden");
        $("#MobileNumber").removeClass("required");
        $("#MobileNumber").css({ "border": "1px solid #babec5" });
        MobileNumberckeck = true;

    }
    else{
        $("#MobileNumberVN").removeClass("hidden");
        $("#MobileNumber").addClass("required");
        MobileNumberckeck = false;

    }


    if (WorkNumeberV.length == 0) {

        //$("#WorkNumeber").addClass("hidden");
        $("#WorkNumeber").removeClass("required");
        $("#WorkNumeber").css({ "border": "1px solid #babec5" });
        WorkNumeberckeck = true;

    }

    else if (FormatePhoneNumber(WorkNumeberV).length == 12) {
        $("#WorkNumeberVN").addClass("hidden");
        $("#WorkNumeber").removeClass("required");
        $("#WorkNumeber").css({ "border": "1px solid #babec5" });
        WorkNumeberckeck = true;

    }
    else {
        $("#WorkNumeber").removeClass("hidden");
        $("#WorkNumeber").addClass("required");
        WorkNumeberckeck = false;

    }
    if (CommonUiValidation()) {
        Comonvckeck = true;
    }
    if (Comonvckeck && MobileNumberckeck && WorkNumeberckeck)
    {
        var Param = {
            "Id": $("#Id").val(),
            "ContactId": $("#ContactId").val(),
            "FirstName": $("#FirstName").val(),
            "LastName": $("#LastName").val(),
            "Work": $("#WorkNumeber").val().replace(/[-  ]/g, '').replace(/[()]/g, ''),
            "Ext": $("#Ext").val(),
            "Mobile": $("#MobileNumber").val().replace(/[-  ]/g, '').replace(/[()]/g, ''),
            "Email": $("#Email").val(),
            "Title": $("#Title").val(),
            "Role": $("#Role").val(),
            "Suffix": $("#Suffix").val(),
            "Facebook": $("#Facebook").val(),
            "Twitter": $("#Twitter").val(),
            "Instagram": $("#Instagram").val(),
            "LinkedIN": $("#LinkedIN").val(),
            "Notes": $("#Notes").val(),
            "ContactOwner": $("#ContactOwner").val(),
            "CustomerId": $("#CustomerId").val(),
            "LeadId": $("#LeadId").val(),
            "OpportunityId": $("#OpportunityId").val(),
            "FromCustomer": FromCustomer,
            "ContactTab": $("#contactTab").val()
        };
        var fparam = { 'contact': Param, 'ListTag': $("#Contact_tag").val() }
        var url = domainurl + "/Contact/AddContact";
        $.ajax({
            type: "POST",
            ajaxStart: function () { },
            url: url,
            data: JSON.stringify(fparam),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.ContactTab == true) {
                    console.log("xxx")
                    OpenSuccessMessageNew("Success !", "", function () {
                        var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
                        OpenContactTab();
                        CloseTopToBottomModal();
                        if (typeof (LoadContactBox) == "function") {
                            LoadContactBox();
                        }
                    });
                }
                else {
                    if (data.FromCustomer == "true") {
                        console.log("ccc")
                        OpenSuccessMessageNew("Success !", ContactSaveMessage, function () {
                            if (typeof (LoadContactBox) == "function") {
                                LoadContactBox();
                                UpdateCustomerTabCounter();
                            }
                            CloseTopToBottomModal();
                        });
                    }
                    else if (data.FromCustomer == "false") {
                        console.log("fff")
                        OpenSuccessMessageNew("Success !", ContactSaveMessage, function () {
                            var pagenumber = 1;
                            console.log(data.ContactId);
                            LoadContactDetail(data.ContactId,true);
                
                            CloseTopToBottomModal();
                        });
                    }
                    else {
                        OpenErrorMessageNew("Error!", "Please check your given input.");
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    }

  
}

var DeleteContact = function (Id) {

    var url = "/Contact/DeleteContact";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify({
            Id: Id,
            ContactTab: $("#contactTab").val()
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            console.log(data);
            if (data.ContactTab == true) {
                OpenSuccessMessageNew("Success !", ContactSaveMessage, function () {
                    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
                    OpenContactTab();
                    CloseTopToBottomModal();
                    if (typeof (LoadContactBox) == "function") {
                        LoadContactBox();
                    }
                });
                
            }
            else
            {
                if (data.result) {
                    OpenSuccessMessageNew("Success!", ContactDeleteSuccessMessage, function () {
                        var pagenumber = 0;
                        //$("#LoadContactList").load(domainurl + "/Contact/LoadContactList", { PageNumber: pagenumber });
                        LoadContacts(1);
                        CloseTopToBottomModal();
                    });

                }
                else {
                    OpenErrorMessageNew("Error!", data.message);
                }
            }
        
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var DeleteContactConfirm = function(Id)
{
    OpenConfirmationMessageNew("", ContactDeleteMessage, function () {

        DeleteContact(Id);
    });
}
var CustomerDetails = function(id)
{
    window.open("/Customer/Customerdetail/?id="+id, "_blank")
}
var Opportunity = function(id)
{
    window.open("/Opportunities?opportunityAdd=opportunityAdd&ADDid=" + id, "_blank");
  
}
var Lead = function (id) {
    window.open("/Lead/Leadsdetail/?id=" + id, "_blank")
}
$(document).ready(function () {

    CloseDatepicker = new Pikaday({ format: 'MM/DD/YYYY', field: $('#CloseDate')[0] });
    InitializeCustomerDropdown($('.dropdown_customar'));
    InitializeLeadDropdown($('.dropdown_lead'));
    InitializeContactDropdown($('.dropdown_contact'));
    InitializeOpportunityDropdown($('.dropdown_opportunity'));
    $(".add_contact_inner").height(window.innerHeight - $(".add_contact_header").height() - $(".add_contact_footer").height() - 48);

    $("#FirstName").keyup(function () {
        if ($("#FirstName").val() != "") {
            $("#Cname").addClass("hidden");
            $("#FirstName").removeClass("required");
        }
        else {
            $("#Cname").removeClass("hidden");
            $("#FirstName").addClass("required");
        }
    })

    $("#MobileNumber").keyup(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var cleanPhoneNumber = FormatePhoneNumber(PhoneNumber);
            $(this).val(cleanPhoneNumber);
        }
    });
    $("#WorkNumeber").keyup(function () {
        var PhoneNumber = $(this).val();
        if (PhoneNumber != undefined && PhoneNumber != null && PhoneNumber != "") {
            var cleanPhoneNumber = FormateWorkNumber(PhoneNumber);
            $(this).val(cleanPhoneNumber);
        }
    });
    var MobileNumber = $("#MobileNumber").val();
    if (MobileNumber.length == 10) {
        $("#MobileNumber").val(MobileNumber.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
    var WorkNumeber = $("#WorkNumeber").val();
    if (WorkNumeber.length == 10) {
        $("#WorkNumeber").val(WorkNumeber.replace(/^(\d{3})(\d{3})(\d{4}).*/, '($1) $2-$3'));
    }
});
$(window).resize(function () {
    $(".add_contact_inner").height(window.innerHeight - $(".add_contact_header").height() - $(".add_contact_footer").height() - 48);
})