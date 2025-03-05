var CustomerLeadImportFile = function () {
    OpenRightToLeftModal(domainurl + "/File/AddCustomerLeadImportFile/");
}
var AssignToUser = function () {
    OpenRightToLeftModal("/Leads/AddLeadAssignedToUser");
}

var LeadSearchKeyUp = function (pageNumber, DateFileter, OrderValue = '') {
    $('#lead-paging').html('');
    $('#boxshiner').show();
    $(".filter-Lead-List").hide();
    if (typeof (pageNumber) == "undefined") {
        return;
    }
    var UserList = $(".UserList").val();
    var SourceList = $(".SourceList").val();
    var firstdate = $(".min-date").val();
    var lastdate = $(".max-date").val();


    var TechnicianId = $("#TechUserList").val();
    var SalesPersonId = $("#SalesUserList").val();
    var PaymentMethod = encodeURIComponent($("#PaymentMethodLsit").val());
    var SourceList = $(".SourceList").val();
    var SalesDate = $("#SalesDateFilter").val();
    var FollowUpDate = $("#FollowUpDateFilter").val();
    var InstallationDate = $("#InstallationDate").val();
    var FundingCompany = $("#FundingCompanyList").val();
    var searchtext = encodeURI($("#srch-term").val());
    $('#boxshiner').show();
    $('#dvHeaderContent').hide();
    $('.filter-list-customer').html(LoaderDom);
    var paramlite = { 
        PageNo: pageNumber,
        isLead: 1, SearchText: searchtext, 
        FirstDate: firstdate,
        LastDate: lastdate,
        IdText: $("#txt_id").length > 0 && $("#txt_id").val().trim() != '' ? $("#txt_id").attr('datakey') + '#' + $("#txt_id").val(): '',
        FirstNameText: $("#txt_firstname").length > 0 && $("#txt_firstname").val() != null && $("#txt_firstname").val().trim() != '' ? $("#txt_firstname").attr('datakey') + '#' + $("#txt_firstname").val() : '',
        LastNameText: $("#txt_lastname").length > 0 && $("#txt_lastname").val().trim() != '' ? $("#txt_lastname").attr('datakey') + '#' + $("#txt_lastname").val() : '',
        BusinessNameText: $("#txt_businessname").length > 0 && $("#txt_businessname").val().trim() != '' ? $("#txt_businessname").attr('datakey') + '#' + $("#txt_businessname").val() : '',
        EmailText: $("#txt_emailaddress").length > 0 && $("#txt_emailaddress").val().trim() != '' ? $("#txt_emailaddress").attr('datakey') + '#' + $("#txt_emailaddress").val() : '',
       
        //SalesPersonText: $("#txt_soldby").length > 0 && $("#txt_soldby").val().trim() != '-1' && $("#txt_soldby").val().trim() != '' ? $("#txt_soldby").attr('datakey') + '#' + $("#txt_soldby").val() : '',
        
        JoinDateText: $("#txt_joindate").length > 0 && $("#txt_joindate").val().trim() != '' ? $("#txt_joindate").attr('datakey') + '#' + $("#txt_joindate").val() : '',
        ActiveStatusText: $("#txt_isactive").length > 0 && $("#txt_isactive").val().trim() != '-1' ? $("#txt_isactive").attr('datakey') + '#' + $("#txt_isactive").val() : '',
        DisplayNameText: $("#txt_displayname").length > 0 && $("#txt_displayname").val().trim() != '' ? $("#txt_displayname").attr('datakey') + '#' + $("#txt_displayname").val() : '',
        FollowupDateText: $("#txt_followupdate").length > 0 && $("#txt_followupdate").val().trim() != '' ? $("#txt_followupdate").attr('datakey') + '#' + $("#txt_followupdate").val() : '',
        CustomerNoText: $("#txt_customerno").length > 0 && $("#txt_customerno").val().trim() != '' ? $("#txt_customerno").attr('datakey') + '#' + $("#txt_customerno").val() : '',
        //LeadStatusText: $("#txt_leadstatus").length > 0 && $("#txt_leadstatus").val().trim() != '-1' ? $("#txt_leadstatus").attr('datakey') + '#' + $("#txt_leadstatus").val() : '',
        
        CustomerTypeText: $("#txt_customertype").length > 0 && $("#txt_customertype").val().trim() != '-1' ? $("#txt_customertype").attr('datakey') + '#' + $("#txt_customertype").val() : '',
        PrimaryPhoneText: $("#txt_primaryphone").length > 0 && $("#txt_primaryphone").val().trim() != '' ? $("#txt_primaryphone").attr('datakey') + '#' + $("#txt_primaryphone").val() : '',
        SecondaryPhoneText: $("#txt_secondaryphone").length > 0 && $("#txt_secondaryphone").val().trim() != '' ? $("#txt_secondaryphone").attr('datakey') + '#' + $("#txt_secondaryphone").val() : '',
        CellNoText: $("#txt_cellno").length > 0 && $("#txt_cellno").val().trim() != '' ? $("#txt_cellno").attr('datakey') + '#' + $("#txt_cellno").val() : '',
        AccountNoText: $("#txt_accountno").length > 0 && $("#txt_accountno").val().trim() != '' ? $("#txt_accountno").attr('datakey') + '#' + $("#txt_accountno").val() : '',
        DbaText: $("#txt_dba").length > 0 && $("#txt_dba").val().trim() != '' ? $("#txt_dba").attr('datakey') + '#' + $("#txt_dba").val() : '',
        //BranchidText: $("#txt_branchid").length > 0 && $("#txt_branchid").val().trim() != '-1' ? $("#txt_branchid").attr('datakey') + '#' + $("#txt_branchid").val() : '',
        
        //SalesLocationText: $("#txt_saleslocation").length > 0 && $("#txt_saleslocation").val().trim() != '-1' ? $("#txt_saleslocation").attr('datakey') + '#' + $("#txt_saleslocation").val() : '',
        PlatformIdText: $("#txt_platformid").length > 0 && $("#txt_platformid").val().trim() != '' ? $("#txt_platformid").attr('datakey') + '#' + $("#txt_platformid").val() : '',
        SettingOrderBy: OrderValue.trim() != '' && OrderValue != null ? OrderValue : '',
        BusinessAccountTypeText: $("#txt_businessaccounttype").length > 0 && $("#txt_businessaccounttype").val().trim() != '-1' ? $("#txt_businessaccounttype").attr('datakey') + '#' + $("#txt_businessaccounttype").val() : '',


        StreetText: $("#txt_street").length > 0 && $("#txt_street").val() != null && $("#txt_street").val().trim() != '' ? $("#txt_street").attr('datakey') + '#' + encodeURIComponent($("#txt_street").val()) : '',
        SalesPersonText: ($("#txt_soldby").length > 0 && $("#txt_soldby").val() != null) ? encodeURI($("#txt_soldby").val()) : '',
        StatusText: ($("#txt_customerstatus").length > 0 && $("#txt_customerstatus").val() != null ) ? encodeURI($("#txt_customerstatus").val()) : '',
        BranchidText: ($("#txt_branchid").length > 0 && $("#txt_branchid").val() != null)  ? encodeURI($("#txt_branchid").val()) : '',
        SalesLocationText: ($("#txt_saleslocation").length > 0 && $("#txt_saleslocation").val() != null) ? encodeURI($("#txt_saleslocation").val()) : '',
        LeadSourceText: ($("#txt_leadsource").length > 0 && $("#txt_leadsource").val() != null) ? encodeURI($("#txt_leadsource").val()) : '',
        LeadStatusText: ($("#txt_leadstatus").length > 0 && $("#txt_leadstatus").val() != null) ? encodeURI($("#txt_leadstatus").val()) : '',


    };

    console.log('LeadFilter')
    $.ajax({
        type: "POST",
        url: '/Customer/CustomerListFilteredLite',
        data: JSON.stringify(paramlite),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            data.result.CustomerList = JSON.parse(data.result.CustomerListString);
            console.log(data);
            //$('#boxshiner').hide();
            //$('#dvHeaderContent').show();
            //$('#dvHeaderContent').html(data.CustomerTopper);
            var empTemplate = $("#hbLeadTemplate").html();
            if (Device.All())
                empTemplate = empTemplate.replace(/style=/g, "data-style=");
            var sourceHtml = Handlebars.compile(empTemplate);
            $("#lead-paging").html(data.paged); 
            $(".filter-Lead-List").html(sourceHtml(data.result));
            $(".filter-Lead-List").show();
            $('#boxshiner').hide();
            LoadEvents();

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
            OpenErrorMessageNew("Error!", "Sorry, but this page didn't load properly. Please try again.")
        }
    });
    Handlebars.registerHelper("ifBill", function (a) {
        console.log("ifBill fired");
        var c;
        var d = 0;

        if (a.length !== d) {
            return 'Billing: ' + a;
        } else {
            c = null;
        }
        return c;
    });
    Handlebars.registerHelper("date2", function (c) {
        console.log("date fired");
        var numb = c.match(/\d/g);
        var numb1 = numb.join("");
        var numb2 = numb1.substring(0, 10);
        //var a =new Date();
        //var formattedDate  = a.getDate(numb2) +
        //    "-" +
        //    (a.getMonth(numb2) + 1) +
        //    "-" +
        //    a.getFullYear(numb2);
        console.log(numb2);
        return new Date(numb2).toDateString();
    });
    Handlebars.registerHelper("signAgreement", function (a) {
        console.log("signAgreement fired");

        var c;
        if (a !== true) {
            return null;
        } else {
            c = 'Yes';
        }
        return 'Sign Agreement: ' + c;
    });
}
var CustomerSearchKeyUp = function (pagenumber) {
    LeadSearchKeyUp(pagenumber);
}
var LoadLeadList = function () {
    setTimeout(function () {
        $(".ListContents").hide();
        $(".ListViewLoader").show();
       // $(".ListContents").load(domainurl + "/Leads/LeadsListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
    }, 5);
}
var LoadEvents = function () {
    setTimeout(function () {
        $("#IsCheckVal1").click(function () {
            if ($(this).is(':checked')) {
                $(".CheckItemsCustomer").each(function () {
                    $(this).prop('checked', true);
                });
            }
            else {
                $(".CheckItemsCustomer").each(function () {
                    $(this).prop('checked', false);
                });
            }
        });
    }, 1000);
}
 
$(document).ready(function () {
      
    $(".LoaderWorkingDiv").hide();
    $("#LoadManufacturers").addClass("active");
    //var idlist = [{ id: ".addManufacturerMagnific", type: 'iframe', width: 920, height: 520 }
    //];
    //jQuery.each(idlist, function (i, val) {
    //    magnificPopupObj(val);
    //});
    $(".ListViewLoader").hide();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Leads/LeadsListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
            //$(".ListContents").slideDown();
        }, 5);
    });
    setTimeout(function () {
        //$(".ListContents").load(domainurl + "/Leads/LeadsListPartial?firstdate=" + firstdate + "&lastdate=" + lastdate);
        //$(".ListContents").slideDown();
        LeadSearchKeyUp(1);
    }, 5);
    $(".btn-filter").click(function () {
        
        if ($('.fliter-list').is(':visible')) {
            $(".fliter-list").hide(300);
        }
        else {
            if ($(".lead_list_filter").html().trim() == '') {
                $('#boxshiner').show();
                $(".lead_list_filter").load("/Customer/FilterCustomerLeadListGridSettingPartial?key=LeadGrid", function () {
                    $(".fliter-list").show();
                    $('#boxshiner').hide();
                });
            }
            else {

                $(".fliter-list").show();
            }

        }
    });
    $("#srch-term").keyup(function (e) {
        e.preventDefault();
        if (e.keyCode == 13) {
            LeadSearchKeyUp(1);
        }
    });
    $('#btnLeadSearch').click(function () { 
        LeadSearchKeyUp(1);
    });
    $(".btn-apply-Datefilter").click(function () {

        var StartDate = my_date_format($(".DateFilterContents .min-date").val());
        var EndDate = my_date_format($(".DateFilterContents .max-date").val())
        if (StartDate == "NaN undefined, NaN") {
            StartDate = "All Time";
            EndDate = "";
        }

        $(".DateFilterContents .date-start").html("");
        $(".DateFilterContents .date-end").html("");
        $(".DateFilterContents .date-start").html(StartDate);
        $(".DateFilterContents .date-end").html(EndDate);
        $(".DateFilterContents .dropdown-filter").hide();
        UpdatePtoCookie();
        LeadSearchKeyUp(1);
    });
    $("#CustomerSearchButton").click(function () {
        LeadSearchKeyUp(1);
    });
    var customerreportpopwinowwith = 600;
    var customerreportpopwinowheight = 510;
    var customerprintpopwinowwith = 920;
    var customerprintpopwinowheight = 600;

    if (Device.MobileGadget()) {
        customerreportpopwinowwith = window.innerWidth;
        customerreportpopwinowheight = window.innerHeight;
        customerprintpopwinowwith = window.innerWidth;
        customerprintpopwinowheight = window.innerHeight;
    }
    var idlist = [{ id: ".ExportLeadReport", type: 'iframe', width: customerreportpopwinowwith, height: customerreportpopwinowheight },
    { id: ".customerlistprint", type: 'iframe', width: customerprintpopwinowwith, height: customerprintpopwinowheight }

    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $("#LeadReport").click(function () {
        var selectedID = [];
        var UserList = "";
        var FilterUser = "";
        var checkboxs = $('.Export_excel_lead');
        $(".CheckItems").each(function () {
            if ($(this).is(':checked')) {
                UserList += ($(this).val()) + ",";
            }
        });
        $(".CheckItems").each(function () {
            FilterUser += ($(this).val()) + ",";
        });
        for (var i = 0; i < checkboxs.length; i++) {
            selectedID.push(parseInt($(checkboxs[i]).attr('data-id')));
        }
        var ColumnName = "";
        $('.th').each(function () {
            if ($(this).attr('data-info') != "" && $(this).attr('data-info') != undefined && $(this).attr('data-info') != null) {
                ColumnName += $(this).attr('data-info').trim() + "," + $(this).text().trim() + "-";
            }
        });
        $(".ExportLeadReport").attr('href', domainurl + "/Reports/ExportConfirm/?ColumnName=" + ColumnName + "&Ids=" + selectedID + "&ReportFor=Lead&UserList=" + UserList + "&FilterUser=" + FilterUser);
        $(".ExportLeadReport").click();
    });

    $("#AddNewCustomer").click(function () {
        LoadLeadVerificationInfo(0, true);
    });
    $("#AddNewCustomerv2").click(function () {
        LoadLeadInfov2(0, true);
    });
})
