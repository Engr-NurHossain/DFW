var InitializeCustomerDropdown = function (dropdownitem, type) {
    var LoadUrl = domainurl + "/Ticket/GetCustomerList";
    var PlaceHolder = 'Account';
    if (type == "lead") {
        LoadUrl = domainurl + "/Ticket/GetLeadList";
        PlaceHolder = 'Lead';
    }
    else if (type == "opportunity") {
        LoadUrl = domainurl + "/Ticket/GetOpportunityList";
        PlaceHolder = 'Opportunity';
    }
    $(dropdownitem).select2({
        placeholder: PlaceHolder,
        allowClear: true,
        minimumInputLength: 1,
        ajax: {
            url: LoadUrl,
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
                            text: item.CustomerName + (item.BusinessName != '' ? ' (' + item.BusinessName + ')' : '') + (item.Street != "" ? " [" + item.Street + "]" : ""),
                            id: item.CustomerId
                        }
                    })
                };
            }
        }
    });
}
var AddActivity = function(FromCustomer)
{
    if (CommonUiValidation()) {
        if ($("#Type").val() != "-1") {
            SaveActivity(FromCustomer);
        }
        else {

            $("#TypeLabel").removeClass("hidden")
        }

    }
}

var SaveActivity = function (FromCustomer) {
    console.log(FromCustomer);
    var AssociatedWith = "00000000-0000-0000-0000-000000000000";
    if ($("#AssociatedType").val() == "Account") {
        AssociatedWith = $("#AccountId").val();
    } else if ($("#AssociatedType").val() == "Lead") {
        AssociatedWith = $("#LeadId").val();
    } else if ($("#AssociatedType").val() == "Opportunity") {
        AssociatedWith = $("#OpportunityId").val();
    }
    else if ($("#AssociatedType").val() == "Contact") {
        AssociatedWith = $("#ContactId").val();
    }
    else if (FromCustomer == "true")
    {
        AssociatedWith = $("#CusAssociatedWith").val();
        
    }
    var Param = {
        "Id": $("#ActivityId").val(),
        "ActivityId": $("#ActivityGuid").val(),
        "ActivityType": $("#Type").val(),
        "Description": $("#Description").val(),
        "AssignedTo": $("#AssignedTo").val(),
        "DueDate": $("#DueDate").val(),
        "Status": $("#Status").val(),
        "AssociatedWith": AssociatedWith,
        "AssociatedType": $("#AssociatedType").val(),
        "Origin": $("#Origin").val(),
        "Note": $("#Note").val(),
        "CreatedBy": "00000000-0000-0000-0000-000000000000",
        "CreatedDate": "1-1-2017",
        "FromCustomer": FromCustomer,
        "ActivityTab": $("#ActivityTab").val(),
        "NotifyBy": $("#NotifyBy").val(),
        //"Origin": 'Manual',
        "Department": $("#Department").val(),
    };
    var url = domainurl + "/Activity/AddActivity";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
          
            console.log(data.FromCustomer);
            if (data.ActivityTab == true) {
                OpenSuccessMessageNew("Success!", data.message, function () {

                    OpenActivityTab();
                    CloseTopToBottomModal();
                    if (typeof (LoadActivityBox) == "function") {
                        LoadActivityBox();
                    }
                });
            }
            else {
                if (data.FromCustomer == true) {
                    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
                    OpenSuccessMessageNew("", data.message, function () {
                        if (typeof (LoadActivityBox) == "function") {
                            LoadActivityBox();
                            UpdateCustomerTabCounter();
                        }
                        CloseTopToBottomModal();
                       
                      
                    });
                }
                else if (data.FromCustomer == false) {
                    OpenSuccessMessageNew("", data.message, function () {
                        var Id = $("#ActivityId").val();
                        LoadActivity(true);
                        CloseTopToBottomModal();
                    });
                }
                else {
                    OpenErrorMessageNew("", data.message, function () {
                        CloseTopToBottomModal();
                    });
                }
            }
            
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}

var DeleteActivity = function (Id) {

    var url = "/Activity/DeleteActivity";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify({
            Id: Id,
            ActivityTab: $("#ActivityTab").val()
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
           
            if (data.res == true) {
                var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
                OpenSuccessMessageNew("Success!", data.message, function () {

                    OpenActivityTab();
                    CloseTopToBottomModal();
                    if (typeof (LoadActivityBox) == "function") {
                        LoadActivityBox();
                      
                    }
                });
            }
            else {
                if (data.result) {
                    OpenSuccessMessageNew("Success!", data.message, function () {

                        LoadActivity(true);
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
$(document).ready(function () {
    //setTimeout(function () {
    //    $(".AddActivityContainer").height(window.innerHeight);
    //}, 100);
    //new Pikaday({
    //    field: $('#DueDate')[0],
    //    trigger: $('#DueDateCustom')[0],
    //    format: 'MM/DD/YYYY',
    //    firstDay: 1
    //});
    var picker = new Pikaday(
      {
          showTime: true,
          showMinutes: true,
          field: $('#DueDate')[0],
          trigger: $('#DueDateCustom')[0],
          firstDay: 1,
          //minDate: new Date(),
          maxDate: new Date('2030-12-31'),
          format: 'MM/DD/YYYY LT',/*MM/DD/YYYY hh:mm:ss*/
          yearRange: [2017, 2030],
          bound: true,
       
      });

    InitializeCustomerDropdown($('.dropdown_opportunity'), "opportunity");
    InitializeCustomerDropdown($('.dropdown_customar'), "customer");
    InitializeCustomerDropdown($('.dropdown_lead'), "lead");


    $("#AssociatedType").change(function () {
        if ($("#AssociatedType").val() == "Account") {
            $(".OpportunityDiv").addClass("hidden");
            $(".AccountDiv").removeClass("hidden");
            $(".LeadDiv").addClass("hidden");
        } else if ($("#AssociatedType").val() == "Lead") {
            $(".OpportunityDiv").addClass("hidden");
            $(".AccountDiv").addClass("hidden");
            $(".LeadDiv").removeClass("hidden");
        } else if ($("#AssociatedType").val() == "Opportunity") {
            $(".OpportunityDiv").removeClass("hidden");
            $(".AccountDiv").addClass("hidden");
            $(".LeadDiv").addClass("hidden");
        } else {
            $(".OpportunityDiv").addClass("hidden");
            $(".AccountDiv").addClass("hidden");
            $(".LeadDiv").addClass("hidden");
        }
    });


});