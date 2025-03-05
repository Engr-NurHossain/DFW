var geocoder;
var currentformatedAddress;
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";

function successFunction(position) {
    var lat = position.coords.latitude;
    var lng = position.coords.longitude;
    codeLatLng(lat, lng)
}

function errorFunction() {
    alert("Geocoder failed");
}

function initialize() {
    $('.tt-menu').hide();
    geocoder = new google.maps.Geocoder();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successFunction, errorFunction);
    }
}
function codeLatLng(lat, lng) {

    var latlng = new google.maps.LatLng(lat, lng);
    geocoder.geocode({ 'latLng': latlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            console.log(results)
            if (results[1]) {
                for (var i = 0; i < results[0].address_components.length; i++) {
                    for (var b = 0; b < results[0].address_components[i].types.length; b++) {

                        //there are different types that might hold a city admin_area_lvl_1 usually does in come cases looking for sublocality type will be more appropriate
                        if (results[0].address_components[i].types[b] == "locality") {
                            //this is the object you are looking for
                            currentformatedAddress = results[0].formatted_address;
                            window.open("https://www.google.com/maps?saddr=" + currentformatedAddress.replace(",", "").replace(" ", "+") + "&daddr=" + DestinationCusAddress.replace(",", "").replace(" ", "+"), "_blank");
                            //console.log(results[0]);
                            break;
                        }
                    }
                }
                //city data
                //console.log(city.short_name + " " + city.long_name)
                //alert(city.short_name + " " + city.long_name)
                //$("#city_search_text_index").val(city.short_name);

            } else {
                alert("No results found");
            }
        } else {
            alert("Geocoder failed due to: " + status);
        }
    });
}
var GetDirection = function () {
    initialize();
};

var ClosePopup = function () {
    $.magnificPopup.close();
}
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var AddCorrespondenceEmail = function (CusId) {
    OpenRightToLeftModal(domainurl + "/Leads/MailToSalesPerson/?id=" + CusId + "&Cid=0");
    history.pushState({ urlpath: window.location.pathname }, window.location.hash, "#addCorrespondence");
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    $("#OpenSuccess").click();
}
var OpenDefaultTab = function () {
    $(".customer-options-tabs li").removeClass('active');
    $(".tab_Content_customer_items .tab-pane").removeClass('active');
}
var Ckeckfirsttime = 1;
var UpdateCustomerTabCounter = function () {
    var Param = {
        CustomerId: CustomerLoadGuid
    };
    var url = domainurl + "/Customer/CustomerTabCounts";
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                //console.log(data)
                var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
                /*Invoice*/
                if (data.data.InvoiceCount > 0) {
                    $(LoadCustomerDiv + ".InvoiceCounter").text(String.format("({0})", data.data.InvoiceCount))
                } else {
                    $(LoadCustomerDiv + ".InvoiceCounter").text("")
                }
                /*Estimate*/
                if (data.data.EstimateCount > 0) {
                    $(LoadCustomerDiv + ".EstimateCounter").text(String.format("({0})", data.data.EstimateCount))
                } else {
                    $(LoadCustomerDiv + ".EstimateCounter").text("")
                }
                /*Estimator*/
                if (data.data.EstimatorCount > 0) {
                    $(LoadCustomerDiv + ".EstimatorCounter").text(String.format("({0})", data.data.EstimatorCount))
                } else {
                    $(LoadCustomerDiv + ".EstimatorCounter").text("")
                }
                /*RecurringBilling*/
                if (data.data.RecurringBillingCount > 0) {
                    $(LoadCustomerDiv + ".RecurringBillingCounter").text(String.format("({0})", data.data.RecurringBillingCount))
                } else {
                    $(LoadCustomerDiv + ".RecurringBillingCounter").text("")
                }
                /*Ticket*/
                if (data.data.TicketsCount > 0) {
                    $(LoadCustomerDiv + ".TicketCounter").text(String.format("({0})", data.data.TicketsCount))
                } else {
                    $(LoadCustomerDiv + ".TicketCounter").text("")
                }
                /*Orders*/
                if (data.data.OrderCount > 0) {
                    $(LoadCustomerDiv + ".OrderCounter").text(String.format("({0})", data.data.OrderCount))
                } else {
                    $(LoadCustomerDiv + ".OrderCounter").text("")
                }
                /*Funding*/
                if (data.data.TotalFundingCount > 0) {
                    $(LoadCustomerDiv + ".FundingCounter").text(String.format("({0})", data.data.TotalFundingCount))
                } else {
                    $(LoadCustomerDiv + ".FundingCounter").text("")
                }
                /*Files*/
                if (data.data.FilesCount > 0) {
                    $(LoadCustomerDiv + ".FilesCounter").text(String.format("({0})", data.data.FilesCount))
                } else {
                    $(LoadCustomerDiv + ".FilesCounter").text("")
                }
                /*Notes*/
                if (data.data.NotesCount > 0) {
                    Ckeckfirsttime = 3;
                    $(LoadCustomerDiv + ".NotesCounter").text(String.format("({0})", data.data.NotesCount))
                } else {
                    $(LoadCustomerDiv + ".NotesCounter").text("")
                }
                /*Correspondance*/
                if (data.data.CorrespondenceCount > 0) {
                    $(LoadCustomerDiv + ".CorrespondanceCounter").text(String.format("({0})", data.data.CorrespondenceCount))
                } else {
                    $(LoadCustomerDiv + ".CorrespondanceCounter").text("")
                }
                /*CustomerCredit*/
                $(LoadCustomerDiv + ".Customer_credit_balance .credit_amount").text(parseFloat(data.data.CustomerCredit).toFixed(2))

                /*Activity*/
                if (data.data.ActivityCustomer > 0) {
                    $(LoadCustomerDiv + ".ActivityCounter").text(String.format("({0})", data.data.ActivityCustomer))
                } else {
                    $(LoadCustomerDiv + ".ActivityCounter").text("")
                }

                /*Opportunity*/
                if (data.data.OpportunityCustomer > 0) {
                    $(LoadCustomerDiv + ".OpportunityCounter").text(String.format("({0})", data.data.OpportunityCustomer))
                } else {
                    $(LoadCustomerDiv + ".OpportunityCounter").text("")
                }

                /*Contact*/
                if (data.data.ContactCustomer > 0) {
                    $(LoadCustomerDiv + ".ContactCounter").text(String.format("({0})", data.data.ContactCustomer))
                } else {
                    $(LoadCustomerDiv + ".ContactCounter").text("")
                }
                /*Booking*/
                if (data.data.BookingCount > 0) {
                    $(LoadCustomerDiv + ".BookingCounter").text(String.format("({0})", data.data.BookingCount))
                } else {
                    $(LoadCustomerDiv + ".BookingCounter").text("")
                }
                /*Log*/
                if (data.data.LogCount > 0) {
                    $(LoadCustomerDiv + ".LogCounter").text(String.format("({0})", data.data.LogCount))
                } else {
                    $(LoadCustomerDiv + ".LogCounter").text("")
                }

                /*Active File Status Count*/
                if (data.data.ActiveFileStatusCount > 0) {
                    console.log(data.data.ActiveFileStatusCount);
                    $(LoadCustomerDiv + ".ActiveFilesCounter").text(`(${data.data.ActiveFileStatusCount})`);
                    $(LoadCustomerDiv + ".InActiveFilesCounter").text(`(${data.data.InActiveFileStatusCount})`);
                } else {
                    $(LoadCustomerDiv + ".ActiveFilesCounter").text("");
                    
                }



                /*InActive File Status Count*/
                if (data.data.InActiveFileStatusCount > 0) {
                  
                    $(LoadCustomerDiv + ".InActiveFilesCounter").text(`(${data.data.InActiveFileStatusCount})`);
                    $(LoadCustomerDiv + ".ActiveFilesCounter").text(`(${data.data.ActiveFileStatusCount})`);

                } else {
                  
                    $(LoadCustomerDiv + ".InActiveFilesCounter").text("");
                    
                }
                
            }
            else {
                console.log(data);
            }
            console.log('done', 'UpdateCustomerTabCounter');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
    
}

var ClearUnusedDom = function (CustomerId) {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".tab-pane").each(function () {
        if (!$(this).hasClass('CustomerDetailTab_Load')) {
            if (!$(this).hasClass('active')) {
                $(this).html(TabsLoaderText)
            }
        }

    });
}
//var ShareCustomerInfo = function (CustomerId) {

//    var SendEmailUrl = domainurl + "/Customer/ShareCustomerInfo?Id=" + CustomerId;
//    console.log("Email link created.:" + CustomerId);
//    //OpenTopToBottomModal(domainurl + "/Customer/ShareCustomerInfo?Id=" + CustomerId);

//    $(".CustomerInfosharespace").attr('href', SendEmailUrl);
//    $(".CustomerInfosharespace").click();
//}
var OpenCustomerDetailTab = function () {
    console.log("ff");
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".CustomerDetailTab").removeClass('hidden');
    $(LoadCustomerDiv + ".CustomerDetailTab").addClass('active');
    $(LoadCustomerDiv + ".CustomerDetailTab_Load").addClass('active');

    if (Ckeckfirsttime == 3) {
        $(LoadCustomerDiv + ".CustomerDetailTab_Load").html(TabsLoaderText);
        $("#customer_tab_" + CustomerLoadId).load(domainurl + String.format("/Customer/CustomerDetails/?id={0}", CustomerLoadId));
        Ckeckfirsttime = Ckeckfirsttime + 1;
    }
    //$("#customer_tab_" + CustomerLoadId).load(domainurl + String.format("/Customer/CustomerDetails/?id={0}", CustomerLoadId));
    //$(LoadCustomerDiv + ".CustomerDetailTab_Load").html(TabsLoaderText);
    //ClearUnusedDom();
    //$(LoadCustomerDiv + ".CustomerDetailTab_Load").load("/Customer/Customerdetail/?id=" + CustomerLoadId);
}

var OpenWorkOrderTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".WorkOrderTab").removeClass('hidden');
    $(LoadCustomerDiv + ".WorkOrderTab").addClass('active');
    $(LoadCustomerDiv + ".WorkOrderTab_Load").addClass('active');
    $(LoadCustomerDiv + ".WorkOrderTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".WorkOrderTab_Load").load(domainurl + "/WorkOrder/WorkOrderPartial?customerid=" + CustomerLoadGuid);
}

var OpenCorrespondenceTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".CorrespondenceTab").removeClass('hidden');
    $(LoadCustomerDiv + ".CorrespondenceTab").addClass('active');
    $(LoadCustomerDiv + ".CorrespondenceTab_Load").addClass('active');
    $(LoadCustomerDiv + ".CorrespondenceTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".CorrespondenceTab_Load").load(domainurl + "/Leads/CorrespondenceList?CustomerId=" + CustomerLoadGuid);
    UpdateCustomerTabCounter();
}

var OpenThirdPartyApiTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".ThirdPartyApiTab").removeClass('hidden');
    $(LoadCustomerDiv + ".ThirdPartyApiTab").addClass('active');
    $(LoadCustomerDiv + ".ThirdPartyApiTab_Load").addClass('active');
    $(LoadCustomerDiv + ".ThirdPartyApiTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".ThirdPartyApiTab_Load").load("/API/CustomerApiTabs?customerid=" + CustomerLoadGuid);
}

var OpenContactTab = function (soldby) {
    CustomerGuidID = $(this).attr('data-id');
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".ContactTab_Load").removeClass('hidden');
    $(LoadCustomerDiv + ".ContactTab_Load").addClass('active');
    $(LoadCustomerDiv + ".ContactTab_Load").addClass('active');
    $(LoadCustomerDiv + ".ContactTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".ContactTab_Load").load(domainurl + "/Customer/CustomerContactList/?FromCustomer=" + CustomerLoadGuid + "&soldby=" + soldby);
    UpdateCustomerTabCounter();
}
var OpenServiceOrderTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".ServiceOrderTab").removeClass('hidden');
    $(LoadCustomerDiv + ".ServiceOrderTab").addClass('active');
    $(LoadCustomerDiv + ".ServiceOrderTab_Load").addClass('active');
    $(LoadCustomerDiv + ".ServiceOrderTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".ServiceOrderTab_Load").load(domainurl + "/ServiceOrder/ServiceOrderPartial?customerid=" + CustomerLoadGuid);
}

var OpenNotesTab = function (pageno) {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".NotesTab").removeClass('hidden');
    $(LoadCustomerDiv + ".NotesTab").addClass('active');
    $(LoadCustomerDiv + ".NotesTab_Load").addClass('active');
    $(LoadCustomerDiv + ".NotesTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".NotesTab_Load").load(domainurl + "/Notes/NotesPartial?customerid=" + CustomerLoadGuid + "&pageno=" + pageno + "&pagesize=50");
    UpdateCustomerTabCounter();
}

var OpenInvoiceTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".InvoiceTab").removeClass('hidden');
    $(LoadCustomerDiv + ".InvoiceTab").addClass('active');
    $(LoadCustomerDiv + ".InvoiceTab_Load").addClass('active');
    $(LoadCustomerDiv + ".InvoiceTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".InvoiceTab_Load").load(domainurl + "/Invoice/InvoicePartial/?CustomerId=" + CustomerLoadGuid);
    UpdateCustomerTabCounter();
}
var OpenBookingTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".BookingTab").removeClass('hidden');
    $(LoadCustomerDiv + ".BookingTab").addClass('active');
    $(LoadCustomerDiv + ".BookingTab_Load").addClass('active');
    $(LoadCustomerDiv + ".BookingTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".BookingTab_Load").load(domainurl + "/Booking/LeadBookingPartial?CustomerId=" + CustomerLoadId);
    UpdateCustomerTabCounter();
}
var OpenDocumentFileManagementTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".DocumentFileManagementTab").removeClass('hidden');
    $(LoadCustomerDiv + ".DocumentFileManagementTab").addClass('active');
    $(LoadCustomerDiv + ".DocumentFileManagement_Load").addClass('active');
    $(LoadCustomerDiv + ".DocumentFileManagement_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".DocumentFileManagement_Load").load(domainurl + "/File/LeadDocumentFileManagementPartial?CustomerId=" + CustomerLoadId);
    UpdateCustomerTabCounter();
}
var OpenLogTab = function () {
    console.log("LogTab_Load");

    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".LogTab").removeClass('hidden');
    $(LoadCustomerDiv + ".LogTab").addClass('active');
    $(LoadCustomerDiv + ".LogTab_Load").addClass('active');
    $(LoadCustomerDiv + ".LogTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".LogTab_Load").load(domainurl + "/Booking/LoadLogPartial?pageno=" + 1 + "&pagesize=" + 50 + "&searchtxt=" + "" + "&CustomerId=" + CustomerLoadId + "&order=" + "");

    //$(LoadCustomerDiv + ".LogTab_Load").load(domainurl + "/Booking/LoadLogPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + 1 + "&pagesize=" + 50 + "&searchtxt=" + "" + "&CustomerId=" + CustomerLoadId + "&order=" + "");


    //$(LoadCustomerDiv + ".LogTab_Load").load(domainurl + "/Booking/LeadBookingPartial?CustomerId=" + CustomerLoadId);
    UpdateCustomerTabCounter();
}

var OpenInspectionTab = function () {

    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    OpenTopToBottomModal(domainurl + "/Customer/LoadCustomerInspection?CustomerId=" + CustomerLoadId);

}
var OpenTransactionTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".TransactionsTab").removeClass('hidden');
    $(LoadCustomerDiv + ".TransactionsTab").addClass('active');
    $(LoadCustomerDiv + ".TransactionsTab_Load").addClass('active');
    $(LoadCustomerDiv + ".TransactionsTab_Load").html(TabsLoaderText);
    //ClearUnusedDom();
    $(LoadCustomerDiv + ".TransactionsTab_Load").load(domainurl + "/Transaction/TransactionPartial/?CustomerId=" + CustomerLoadId);
    UpdateCustomerTabCounter();
}

var OpenEstimateTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".EstimateTab").removeClass('hidden');
    $(LoadCustomerDiv + ".EstimateTab").addClass('active');
    $(LoadCustomerDiv + ".EstimateTab_Load").addClass('active');
    $(LoadCustomerDiv + ".EstimateTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".EstimateTab_Load").load("/Estimate/EstimatePartial/?CustomerId=" + CustomerLoadId);
    UpdateCustomerTabCounter();
}
var OpenEstimatorTab = function () { //IsLead  $(LoadCustomerDiv + ".loadestimatorlist").load("/Estimator/EstimatorListPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + SearchText + "&StrStartDate=" + startdates + "&StrEndDate=" + enddates + "&estimateStatus=" + estimateStatus);
    var SearchText = "";
    var startdates = "";
    var enddates = "";
    var estimateStatus = "Open";
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".EstimatorTab").removeClass('hidden');
    $(LoadCustomerDiv + ".EstimatorTab").addClass('active');
    $(LoadCustomerDiv + ".EstimatorTab_Load").addClass('active');
    $(LoadCustomerDiv + ".EstimatorTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".EstimatorTab_Load").load("/Estimator/EstimatorPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + SearchText + "&StrStartDate=" + startdates + "&StrEndDate=" + enddates + "&estimateStatus=" + estimateStatus); //+ "&IsLead=" + IsLead
    UpdateCustomerTabCounter();
}
var OpenCustomerRecurringBillingTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".CustomerRecurringBillingTab").removeClass('hidden');
    $(LoadCustomerDiv + ".CustomerRecurringBillingTab").addClass('active');
    $(LoadCustomerDiv + ".CustomerRecurringBillingTab_Load").addClass('active');
    $(LoadCustomerDiv + ".CustomerRecurringBillingTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".CustomerRecurringBillingTab_Load").load("/RecurringBilling/RecurringBillingPartial/?CustomerId=" + CustomerLoadId);
    UpdateCustomerTabCounter();
}
var OpenOrderTab = function (pageno) {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".OrderTab").removeClass('hidden');
    $(LoadCustomerDiv + ".OrderTab").addClass('active');
    $(LoadCustomerDiv + ".OrderTab_Load").addClass('active');
    $(LoadCustomerDiv + ".OrderTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    //$(LoadCustomerDiv + ".OrderTab_Load").load("/Order/CustomerOrderPartial/?CustomerId=" + CustomerLoadId);
    $(LoadCustomerDiv + ".OrderTab_Load").load("/Order/OrderPartial?PageNo=" + pageno + "&PageSize=10" + "&SearchText=&order=&startdate=&enddate=&customerid=" + CustomerLoadGuid);
    UpdateCustomerTabCounter();
}

var OpenScheduleTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".ScheduleTab").removeClass('hidden');
    $(LoadCustomerDiv + ".ScheduleTab").addClass('active');
    $(LoadCustomerDiv + ".ScheduleTab_Load").addClass('active');
    $(LoadCustomerDiv + ".ScheduleTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".ScheduleTab_Load").load(domainurl + "/TechSchedule/TechSchedulePartial?customerid=" + CustomerLoadGuid);
}

var OpenFundingTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".FundingTab").removeClass('hidden');
    $(LoadCustomerDiv + ".FundingTab").addClass('active');
    $(LoadCustomerDiv + ".FundingTab_Load").addClass('active');
    $(LoadCustomerDiv + ".FundingTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".FundingTab_Load").load(domainurl + "/Funding/FundingPartial?customerid=" + CustomerLoadGuid);
}

var OpenFilesTab = function (soldby) {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".FilesTab").removeClass('hidden');
    $(LoadCustomerDiv + ".FilesTab").addClass('active');
    $(LoadCustomerDiv + ".FilesTab_Load").addClass('active');
    $(LoadCustomerDiv + ".FilesTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".FilesTab_Load").load(domainurl + "/File/CustomerFilesAndDocument/?id=" + CustomerLoadId + "&soldby=" + soldby);
    UpdateCustomerTabCounter();
}
var OpenTicketTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".TicketTab").removeClass('hidden');
    $(LoadCustomerDiv + ".TicketTab").addClass('active');
    $(LoadCustomerDiv + ".TicketTab_Load").addClass('active');
    $(LoadCustomerDiv + ".TicketTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".TicketTab_Load").load(domainurl + "/Ticket/TicketListPartial/?CustomerId=" + CustomerLoadGuid);
    UpdateCustomerTabCounter();
}
var OpenActivityTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".ActivityTab").removeClass('hidden');
    $(LoadCustomerDiv + ".ActivityTab").addClass('active');
    $(LoadCustomerDiv + ".ActivityTab_Load").addClass('active');
    $(LoadCustomerDiv + ".ActivityTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".ActivityTab_Load").load(domainurl + "/Activity/ActivityListPartial/?CustomerId=" + CustomerLoadGuid);
    UpdateCustomerTabCounter();
}
var OpenOpportunityTab = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer-options-tabs li").removeClass('active');
    $(LoadCustomerDiv + ".tab_Content_customer_items .tab-pane").removeClass('active');
    $(LoadCustomerDiv + ".OpportunityTab").removeClass('hidden');
    $(LoadCustomerDiv + ".OpportunityTab").addClass('active');
    $(LoadCustomerDiv + ".OpportunityTab_Load").addClass('active');
    $(LoadCustomerDiv + ".OpportunityTab_Load").html(TabsLoaderText);
    ClearUnusedDom();
    $(LoadCustomerDiv + ".OpportunityTab_Load").load(domainurl + "/Opportunity/OpportunityListPartial/?CustomerId=" + CustomerLoadGuid);
    UpdateCustomerTabCounter();
}

var OpenReceivePayment = function () {
    OpenTopToBottomModal(domainurl + "/Transaction/ReceivePayment/?CustomerId=" + CustomerLoadId);
}
var EditCustomer = function (CustomerId) {

    OpenTopToBottomModal(domainurl + "/Customer/AddCustomer?id=" + CustomerId);
}
//[Shariful-25-9-19]
var EditCustomerInspection = function (CustomerId) {

    OpenTopToBottomModal(domainurl + "/Customer/LoadCustomerInspection?CustomerId=" + CustomerId);
}
//[~Shariful-25-9-19]
var EditCredential = function (CustomerId) {
    OpenRightToLeftModal(domainurl + "/Customer/EditCredential?CustomerId=" + CustomerId);
}
var CredentialSave = function () {
    var userlogin = {};
    userlogin.UserName = $("#username").val();
    userlogin.Password = $("#password").val();
    userlogin.Id = $("#Userlogin").val();
    userlogin.UserId = $("#CustomerId").val();
    userlogin.EmailAddress = $("#Email").val();
    if ($('#SendMailAddress').is(':checked') == true) {
        userlogin.SendMail = "true";
    }
    else {
        userlogin.SendMail = "false";
    }

    $.ajax({
        type: "POST",
        url: domainurl + "/Customer/CredentialSave",
        data: '{userlogin: ' + JSON.stringify(userlogin) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response.result);
            if (response.result) {
                if (response.result == true) {

                    OpenSuccessMessageNew("Success!", response.message, function () {


                        $("#Right-To-Left-Modal-Body .close").click();
                    });


                }

                else {
                    OpenErrorMessageNew("Error!", response.message);
                }

            }
            else {
                OpenErrorMessageNew("Error!", response.message);
            }
        }
    });
}

var CheckTokenForCredential = function () {


    var userlogin = {};
    userlogin.UserName = $("#username").val();
    userlogin.Password = $("#password").val();
    userlogin.Id = $("#Userlogin").val();
    userlogin.UserId = $("#CustomerId").val();
    userlogin.EmailAddress = $("#Email").val();
    if ($('#SendMailAddress').is(':checked') == true) {
        userlogin.SendMail = "true";
    }
    else {
        userlogin.SendMail = "false";
    }

    $.ajax({
        type: "POST",
        url: domainurl + "/Customer/CheckRoleAndSaveCredential",
        data: '{userlogin: ' + JSON.stringify(userlogin) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response.result);
            if (response.result) {
                OpenSuccessMessageNew("Success!", response.message, function () {


                    $("#Right-To-Left-Modal-Body .close").click();
                });
            }
            else {
                OpenErrorMessageNew("Error!", response.message);
            }
        }
    });

}

var PasswordGenerate = function () {
    $.ajax({

        type: "POST",
        url: domainurl + "/Customer/GeneratePassword",
        data: '{length: ' + JSON.stringify("8") + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#password").val("");
            $("#password").val(response);
        }
    });
}
var EditCustomerBill = function (CustomerId) {
    OpenTopToBottomModal(domainurl + "/Customer/AddCustomer?id=" + CustomerId + "&IsGotoBill=true");
}
var SaveCredential = function () {
    console.log("hihello");
    if (CommonUiValidation()) {

        CheckTokenForCredential();
    }
    else {

    }
}
var OpenCustomerEmergency = function (CustomerId) {
    OpenTopToBottomModal(domainurl + "/Customer/AddCustomer?id=" + CustomerId + "&IsEmergency=true");
}
var OpenCustomerContact = function (CustomerId) {
    OpenTopToBottomModal(domainurl + "/Contact/AddContact?CustomerId=" + CustomerId);
}
var OpenCustomerActivity = function (CustomerId) {
    OpenTopToBottomModal("/Activity/AddActivity?CustomerId=" + CustomerId);
}

var OpenCustomerEditActivity = function (CustomerId) {
    console.log(CustomerId);
    OpenTopToBottomModal(domainurl + "/Customer/AddCustomer?id=" + CustomerId + "&IsAccountActivity=true");
}
var GetDate = function (day, month, year) {
    if ($("li.SalesTab").hasClass('active')) {
        NewSalesLoad(day, month, year);
    } else if ($("li.WorkOrderTab").hasClass('active')) {
        NewWorkOrderLoad(day, month, year);
    } else if ($("li.ServiceOrderTab").hasClass('active')) {
        //$(".Left-Modal-Open").click()
        ServiceCalendarLoad(day, month, year);
    }
}
var GetEcontract = function () {

    var DownloadUrl = domainurl + "/API/GetEcontract/?EcontractId=" + EnvelopeID + "&&CustomerId=" + CustomerLoadGuid;
    window.open(DownloadUrl, '_blank');

}
var LoadActivityBox = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".CustomerActivity").load(domainurl + "/Customer/CustomerActivities?customerId=" + CustomerLoadGuid);
}
var LoadOpportunityBox = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer_opportunitylist").load(domainurl + "/Customer/CustomerOpportunityList?CustomerId=" + CustomerLoadGuid);
}
var LoadContactBox = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".contacts").load(domainurl + "/Customer/CustomerContacts?customerId=" + CustomerLoadGuid + "&soldby=" + $(".contacts").attr('soldby'));
}

var LoadTicketBox = function () {
    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";
    $(LoadCustomerDiv + ".customer_ticket_list").load(domainurl + "/Ticket/CustomerOverviewTicketList?customerid=" + CustomerLoadId);
}

var LoadCustomerDetailBoxes = function () {
    LoadContactBox();
    LoadOpportunityBox();
    LoadActivityBox();
    LoadTicketBox();
}

$(document).ready(function () {
    var CustPopupwidth = 720;
    if (window.innerWidth < 720) {
        CustPopupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".MapManufacturerMagnific", type: 'iframe', width: 920, height: 520 },
    { id: ".MapManufacturerMagnificPrevious", type: 'iframe', width: 920, height: 520 },
    { id: ".CustomerInfosharespace", type: 'iframe', width: CustPopupwidth, height: 520 },

    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    LoadCustomerDetailBoxes();

    var LoadCustomerDiv = "#customer_tab_" + CustomerLoadId + " ";

    var fileGUID = $(LoadCustomerDiv + ".customer-file-content").attr('idval');
    $(LoadCustomerDiv + ".customer-file-content").load(domainurl + "/Customer/CustomerFileDetails?customerid=" + fileGUID);

    var hisGUID = $(LoadCustomerDiv + ".history-JoinDate").attr('idval');
    $(LoadCustomerDiv + ".history-JoinDate").load(domainurl + "/Customer/CustomerCreatedHistory?customerId=" + hisGUID);

    var invGUID = $(LoadCustomerDiv + ".lnvoice-Histories").attr('idval');
    $(LoadCustomerDiv + ".lnvoice-Histories").load(domainurl + "/Customer/CustomerInvoiceHistories?customerId=" + invGUID);

    var activeGUID = $(LoadCustomerDiv + ".CustomerCancelAndActiveHistory").attr('idval');
    $(LoadCustomerDiv + ".CustomerCancelAndActiveHistory").load(domainurl + "/Customer/CustomerActiveInactiveHistory?customerId=" + invGUID);

    var estGUID = $(LoadCustomerDiv + ".Estimate-Histories").attr('idval');
    $(LoadCustomerDiv + ".Estimate-Histories").load(domainurl + "/Customer/CreateEstimateHistoryPartial?CustomerId=" + estGUID);

    var payGUID = $(LoadCustomerDiv + ".Payment-Histories").attr('idval');
    $(LoadCustomerDiv + ".Payment-Histories").load(domainurl + "/Customer/InvoicePaymentHistoryPartial?CustomerId=" + payGUID);

    var serviceGUID = $(LoadCustomerDiv + ".ServiceOrder-Histories").attr('idval');
    $(LoadCustomerDiv + ".ServiceOrder-Histories").load(domainurl + "/Customer/CustomerServiceOrderHistories?customerId=" + serviceGUID);

    var workGUID = $(LoadCustomerDiv + ".WorkOrder-Histories").attr('idval');
    $(LoadCustomerDiv + ".WorkOrder-Histories").load(domainurl + "/Customer/CustomerWorkOrderHistories?customerId=" + workGUID);

    var billGUID = $(LoadCustomerDiv + ".Billing-Histories").attr('idval');
    $(LoadCustomerDiv + ".Billing-Histories").load(domainurl + "/Customer/CustomerBillingHistories?customerId=" + billGUID);

    var notesBoxGUID = $(LoadCustomerDiv + ".Notes-Box").attr('idval');
    $(LoadCustomerDiv + ".Notes-Box").load(domainurl + "/Customer/CustomerNoteBoxes?customerId=" + notesBoxGUID);

    $(".MigrationData").click(function () {
        var notesBoxGUID = $(LoadCustomerDiv + ".Notes-Box").attr('idval');
        $(LoadCustomerDiv + ".Migration-Notes-Box").load(domainurl + "/Customer/CustomerPreviousData?customerId=" + notesBoxGUID);
    })

    var ordersBoxGUID = $(LoadCustomerDiv + ".Orders-Box").attr('idval');
    $(LoadCustomerDiv + ".Orders-Box").load(domainurl + "/Customer/CustomerOrderBoxes?customerId=" + ordersBoxGUID);

    var SalesPersonBoxGUID = $(LoadCustomerDiv + ".Sales_Person_Info_History").attr('soldby');
    $(LoadCustomerDiv + ".Sales_Person_Info_History").load(domainurl + "/Customer/CustomerSalesPersonBox?soldby=" + SalesPersonBoxGUID);

    var InfoGUID = $(LoadCustomerDiv + ".System-Info-History").attr('idval');
    $(LoadCustomerDiv + ".System-Info-History").load(domainurl + "/Customer/CustomerSystemInfoDetails?customerId=" + InfoGUID);

    var ActivityGUID = $(LoadCustomerDiv + ".Account-Activity-History").attr('idval');

    $(LoadCustomerDiv + ".Account-Activity-History").load(domainurl + "/Customer/CustomerAccountActivityDetails?customerId=" + ActivityGUID);

    var ResponsibleGUID = $(LoadCustomerDiv + ".Responsible-Person-Histories").attr('idval');
    $(LoadCustomerDiv + ".Responsible-Person-Histories").load(domainurl + "/Customer/CustomerResponsiblePersonDetail?customerId=" + ResponsibleGUID);


    var ApiGUID = $(LoadCustomerDiv + ".Api-Setting-Histories").attr('idval');
    $(LoadCustomerDiv + ".Api-Setting-Histories").load(domainurl + "/Customer/CustomerApiSettingDetail?customerId=" + ApiGUID);


    var CustomersendEmailGUID = $(LoadCustomerDiv + ".CustomerSendEmailHistory").attr('idval');
    $(LoadCustomerDiv + ".CustomerSendEmailHistory").load(domainurl + "/Customer/CustomerSendEmailHistory?CustomerId=" + CustomersendEmailGUID);

    $('.back-to-customerlist').click(function () {
        LoadCustomer(true);
    })
    $(".LoaderWorkingDiv").hide();
    $("#LoadManufacturers").addClass("active");
    var newsupplierpopwinowwith = 920;
    var newsupplierpopwinowheight = 600;
    if (Device.MobileGadget()) {
        newsupplierpopwinowwith = window.innerWidth;
        newsupplierpopwinowheight = window.innerHeight;
    }
    $(".addressMapPopup").click(function () {
        CustomerGuidID = $(this).attr('data-id');
        var mapLoadUrl = domainurl + "/Customer/CustomerAddressMap?CustomerId=" + CustomerGuidID;
        $(".MapManufacturerMagnific").attr("href", mapLoadUrl);
        $(".MapManufacturerMagnific").click();
    });
    $(".addressMapPopupPrevious").click(function () {
        CustomerGuidID = $(this).attr('data-id');
        var mapLoadUrlPrevious = domainurl + "/Customer/CustomerAddressMapPrevious?CustomerId=" + CustomerGuidID;
        $(".MapManufacturerMagnificPrevious").attr("href", mapLoadUrlPrevious);
        $(".MapManufacturerMagnificPrevious").click();
    });
    $(".customerInfoShare").click(function () {
        CustomerId = $(this).attr('data-id');
        var SendEmailUrl = domainurl + "/Customer/ShareCustomerInfo?Id=" + CustomerId + "&from=customer";
        console.log("Email link created.:" + CustomerId);
        //OpenTopToBottomModal(domainurl + "/Customer/ShareCustomerInfo?Id=" + CustomerId);

        $(".CustomerInfosharespace").attr("href", SendEmailUrl);
        $(".CustomerInfosharespace").click();
    });
    $("#btnEcontract").click(function () {
        GetEcontract();
    })
    $(".btn_addccpaymentcustomer").click(function () {
        OpenRightToLeftModal("/SmartLeads/CCAddViewPaymentMethod?customerid=" + CustomerLoadGuid + "&type=Customer");
    });
    $(".btn_addachpaymentcustomer").click(function () {
        OpenRightToLeftModal("/SmartLeads/ACHAddViewPaymentMethod?customerid=" + CustomerLoadGuid + "&type=Customer");
    });
    $("#IsTestAccount").change(function () {
        var Param = {
            CustomerId: CustomerLoadGuid,
            IsTestAccount: $("#IsTestAccount").prop('checked') ? true : false
        };
        $.ajax({
            type: "POST",
            url: domainurl + "/Customer/UpdateTestAccountChecked",
            data: JSON.stringify(Param),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    OpenSuccessMessageNew("Success!", "");
                    location.reload();
                }
                else {
                    OpenErrorMessageNew("Error!","");
                }
            }
        });
    });
});
