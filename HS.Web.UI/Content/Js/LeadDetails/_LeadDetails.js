var geocoder;
var currentformatedAddress;

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

var OpenLeadEstimateTab = function () {
    $("#LeadEstimate").load(domainurl + "/Leads/LeadEstimatePartial?CustomerId=" + customerId);
    LeadDetailTabCount();
}
var AddAdditionalContact = function () {
    OpenRightToLeftModal(domainurl + "/Customer/CustomerAdditionalInfo?CustomerId=" + CustomerGuid);
}
var AddSystemInformation = function () {
    
}
var AddExistingItem = function () {
    OpenRightToLeftModal(domainurl + "/Customer/CustomerExistingItem?CustomerId=" + CustomerGuid);
}

var LoadLeadCorrespondenceTab = function () {
    $(".Lead_Send_Email_Tab").html(LoaderDom);
    $(".Lead_Send_Email_Tab").load(domainurl + "/Leads/CorrespondenceList/?CustomerId=" + CustomerGuid);
    LeadDetailTabCount();
}
var ShowTag = function (item) {
    $("#isVerifyEmail").attr("data-original-title", "");
    $("#isVerifyEmail").attr("data-original-title", item);
}
var OpenAddNote = function () {
    OpenRightToLeftModal(domainurl + "/Leads/AddLeadNotes?CustomerId=" + customerId + "&From=customerId")
}
var targetUrl1;
var LeadDetailTabCount = function () {
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: "/Leads/LeadDetailsTabCount",
        data: JSON.stringify({ customerid: CustomerGuid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result == true) {
                if (data.model.LeadEstimate.LeadEstimateCount > 0) {
                    $(".lead_estimate").text(String.format("({0})", data.model.LeadEstimate.LeadEstimateCount));
                }
                else {
                    $(".lead_estimate").text("");
                }
                if (data.model.LeadNote.LeadNoteCount > 0) {
                    $(".lead_note").text(String.format("({0})", data.model.LeadNote.LeadNoteCount));
                }
                else {
                    $(".lead_note").text("");
                }
                if (data.model.LeadCorrespondence.LeadCorresCount > 0) {
                    $(".lead_corres").text(String.format("({0})", data.model.LeadCorrespondence.LeadCorresCount));
                }
                else {
                    $(".lead_corres").text("");
                }
                if (data.model.LeadFile.LeadFileCount > 0) {
                    $(".lead_file").text(String.format("({0})", data.model.LeadFile.LeadFileCount));
                }
                else {
                    $(".lead_file").text("");
                }
                if (data.model.Booking.BookingCount > 0) {
                    $(".lead_booking").text(String.format("({0})", data.model.Booking.BookingCount));
                }
                else {
                    $(".lead_booking").text("");
                }
                if (data.model.TicketCount > 0) {
                    $(".lead_ticket").text(String.format("({0})", data.model.TicketCount));
                }
                else {
                    $(".lead_ticket").text("");
                }
                if (data.model.EstimatorCount > 0) {
                    $(".lead_estimator").text(String.format("({0})", data.model.EstimatorCount));
                }
                else {
                    $(".lead_estimator").text("");
                }
                if (data.model.LogCount > 0) {
                    $(".lead_log").text(String.format("({0})", data.model.LogCount));
                }
                else {
                    $(".lead_log").text("");
                } 
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var OpenLeadBookTab = function () {
    $("#BookingTab").load(domainurl + "/Booking/LeadBookingPartial?CustomerId=" + customerId);
}
var OpenLeadDocumentCenterTab = function () {
    $("#DocumentCenterTab").load(domainurl + "/Leads/LeadFilesAndDocument?id=" + LeadId + "&SoldBy=" + $("#DocumentCenterTab").attr('soldby'));
}
var OpenTicketTab = function () {
    $("#TicketTab").html(TabsLoaderText);
    $("#TicketTab").load(domainurl + "/Ticket/TicketListPartial/?CustomerId=" + CustomerGuid);
}

var OpenEstimatorTab = function () { //IsLead
    var SearchText = "";
    var startdates = "";
    var enddates = "";
    var estimateStatus = "Open";
    LeadDetailTabCount();
    $("#EstimatorTab").html(TabsLoaderText);
    //$(LoadCustomerDiv + ".EstimatorTab_Load").load("/Estimator/EstimatorPartial/?CustomerId=" + CustomerLoadId + "&SearchText=" + SearchText + "&StrStartDate=" + startdates + "&StrEndDate=" + enddates + "&estimateStatus=" + estimateStatus); //+ "&IsLead=" + IsLead
    $("#EstimatorTab").load(domainurl + "/Estimator/EstimatorPartial/?CustomerId=" + customerId + "&SearchText=" + SearchText + "&StrStartDate=" + startdates + "&StrEndDate=" + enddates + "&estimateStatus=" + estimateStatus); // + "&IsLead="+ IsLead
}
var OpenISPCTab = function () {
    console.log("dd")//IsLead

    $("#ISPCTab").html(TabsLoaderText);
    $("#ISPCTab").load(domainurl + "/API/ISPC/?CustomerId=" + CustomerGuid);
}
var OpenLogTab = function () { //IsLead
    LeadDetailTabCount();
    $("#LogTab").html(TabsLoaderText);
    $("#LogTab").load(domainurl + "/Booking/LoadLogPartial/?CustomerId=" + customerId + "&pageno=1" + "&pagesize=50"); 
}
var openLeadNoteTab = function (pageno) {
    console.log("leadnote");
    LeadDetailTabCount();
    $(".FollowUpTabContent").html(TabsLoaderText);
    var FollowUpTavContentCustomerid = $(".FollowUpTabContent").attr('id-val');
    $(".FollowUpTabContent").load(domainurl + "/Leads/LoadLeadFollowUpTabPartial/?CustomerId=" + FollowUpTavContentCustomerid + "&pageno=" + pageno + "&pagesize=50");
}
var OpenLeadEstimateTab = function () {
    LeadDetailTabCount();
    $("#LeadEstimate").html(TabsLoaderText);
    $("#LeadEstimate").load(domainurl + "/Leads/LeadEstimatePartial?CustomerId=" + customerId);
}


var GetEcontract = function () {

    var DownloadUrl = domainurl + "/API/GetEcontract/?EcontractId=" + EnvelopeID + "&&CustomerId=" + CustomerLoadGuid;
    window.open(DownloadUrl, '_blank');
    //var url = domainurl + "/API/GetEcontract";
    //var Param = JSON.stringify({
    //    EContractId: EnvelopeID
    //});
    //$.ajax({
    //    type: "POST",
    //    url: url,
    //    data: Param,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    cache: false,
    //    success: function (data) {

    //    },
    //    error: function (jqXHR, textStatus, errorThrown) {
    //        console.log(errorThrown);

    //    }
    //});
}
$(document).ready(function () {
    $("#ACHTab").load(domainurl + "/Customer/LoadCustomerACHPaymentInfo?customerid=" + CustomerLoadGuid);
    $("#CCTab").load(domainurl + "/Customer/LoadCustomerCCPaymentInfo?customerid=" + CustomerLoadGuid);
    OpenLeadEstimateTab();
    if (customform == "true") {
        $(".editCustomerCustomerDetailsHeader2").removeClass('hidden');
    }
    $("#additionalInfo").load(domainurl + "/Leads/LeadAdditionalInfo/?CustomerId=" + CustomerGuid);
    $("#SystemInfo").load(domainurl + "/Leads/LeadSystemInfo/?CustomerId=" + CustomerGuid);
    $("#CustomerContactTrackList").load(domainurl + "/Leads/LeadCustomerContactTrackList/?CustomerId=" + CustomerGuid);
    $("#creditscorecheck").load(domainurl + "/Leads/CreditScoreList/?CustomerId=" + CustomerGuid);
    $(".Notes-Box").load(domainurl + "/Leads/LeadNoteBoxes?customerId=" + CustomerGuid + "&SoldBy=" + $(".Notes-Box").attr('soldby'));
    $(".Lead_Sales_Person_Info_History").load(domainurl + "/Leads/LeadSalesPersonBox?SoldBy=" + $(".Lead_Sales_Person_Info_History").attr('soldby'));
    $('ul.lead_detail_tab_list li a').click(function (e) {
        if (!TabPopStateCheck) {
            window.history.pushState({ urlPath: window.location.pathname }, "", $(e.target).attr('tabname'));
        }
        TabPopStateCheck = false;
    });
    if (top.location.hash != '') {
        if ($("[tabname='" + top.location.hash + "']").length > 0) {
            TabPopStateCheck = true;
            $("[tabname='" + top.location.hash + "']").click();
        }
    }
    $("#btnLeadSetup").click(function () {
        var valid = $(this).attr('data-id');
        LoadLeadSetup(valid, true)
    });
    $("#btnSmartLeadSetup").click(function () {
        var valid = $(this).attr('data-id');
        LoadSmartLeadSetup(valid, true)
    });
    $("#btnEcontract").click(function () {
        GetEcontract();
    })
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".LoadAgreementPopUp", type: 'iframe', width: Popupwidth, height: 650 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    var LeadPopupwidth = 720;
    if (window.innerWidth < 720) {
        LeadPopupwidth = window.innerWidth;
    }
    var idlist1 = [{ id: ".MapManufacturerMagnific", type: 'iframe', width: 920, height: 520 },
        { id: ".MapManufacturerMagnificPrevious", type: 'iframe', width: 920, height: 520 },
        { id: ".LeadInfosharespace", type: 'iframe', width: LeadPopupwidth, height: 520 }

    ];
    jQuery.each(idlist1, function (i, val) {
        magnificPopupObj(val);
    });
    $("#SignIn_Aggrement").click(function () {
        var url = domainurl + "/Leads/FinalCustomerSetupData";
        var param = JSON.stringify({
            setupid: customerId
        })
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result == true) {
                    if (Device.iPad()) {
                        $(".LoadAgreementPopUp1")[0].click();
                    }
                    else {
                        $("#InstallationAgreement")[0].click();
                    }
                }
                else {
                    OpenErrorMessageNew("Error!", data.message, "");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    })
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $(".LoadLeadNotes").load(domainurl + "/Leads/LeadNotesPartial/?customerid=" + customerid);
    $(".lead-history-join").load(domainurl + "/Leads/LeadJoiningHistoryPartial/?CustomerId=" + customerid);
    $(".lead-history-notes").load(domainurl + "/Leads/LeadNotesHistoryPartial/?CustomerId=" + customerid);
    $(".lead-history-followUps").load(domainurl + "/Leads/LeadFollowUpHistoryPartial/?CustomerId=" + customerid);
    $(".lead-history-email").load(domainurl + "/Leads/LeadEmailHistoryPartial/?CustomerId=" + customerid);
    //$(".ClickSetup").click(function () {
    //    LoadLeadSetup(LeadId);
    //});
    $("#CorrespondanceTabHead").click(function () {
        LoadLeadCorrespondenceTab();
    });
    $("#btnPackage").click(function () {
        LoadSmartLeadSetup(LeadId, true);
    });
    $(".ClickVerify").click(function () {
        LoadVerifyLeads(LeadId);
    });
    $(".QA1Call").click(function () {
        OpenTopToBottomModal(domainurl + "/Leads/QA1QuestionariePartial?id=" + LeadId);
    });
    $(".QA2Call").click(function () {
        OpenTopToBottomModal(domainurl + "/Leads/QA2QuestionariePartial?id=" + LeadId);
    });
    $(".Techcall").click(function () {
        OpenTopToBottomModal(domainurl + "/Leads/LeadTechCallPartial");
    });
    //$(".TechScheduleTab").click(function () {
    //    $(".TechSchedule").load("/Leads/LeadTechScheduleCalendar/");
    //});
    //$("#AddSchedule").click(function () {
    //    OpenRightToLeftModal("/Leads/AddLeadTechSchedule/?id=0&Leadid=" + LeadId);
    //});
    //$("#DocumentCenterTab").load(domainurl + "/Leads/LeadFilesAndDocument?id=" + LeadId + "&SoldBy=" + $("#DocumentCenterTab").attr('soldby'));
    if (LeadTrackingTab == "true") {
        console.log("LeadTrackingTab")
        $("#TrackingTab").load(domainurl + "/Leads/LoadLeadTrackingPartial?leadId=" + LeadId);
    }
    $(".btn-envelope").click(function () {
        $("#CorrespondanceTabHead").click();
    })
    $("#LoadCreditCheck").click(function () {
        OpenRightToLeftModal(domainurl + "/Leads/AddLeadCreditCheckPartial?id=" + customerId);
    })
    if (tab == "Reminder") {
        $(".followUpTab").addClass('active');
        $("#followUpTab").addClass('active');
        $(".LoadLeadDetail").removeClass('active');
        OpenRightToLeftModal(domainurl + "/Leads/AddNewReminderNote?id=" + NoteId + "&CustomerId=" + CustomerGuid + "&Timeval=" + ViewBagtime + "&IsComplete=" + ViewBagcomplete);
    }
    if (tab == "Note") {
        $(".followUpTab").addClass('active');
        $("#followUpTab").addClass('active');
        $(".LoadLeadDetail").removeClass('active');
        OpenRightToLeftModal(domainurl + "/Leads/AddLeadNotes?id=" + NoteId);
    }

    $(".leadMapPopup").click(function () {
        var mapLoadUrl = domainurl + "/Customer/CustomerAddressMap?CustomerId=" + CustomerGuid;
        $(".MapManufacturerMagnific").attr("href", mapLoadUrl);
        $(".MapManufacturerMagnific").click();
    });
    $(".LeadInfoShare").click(function () {
        CustomerId = $(this).attr('data-id');
        var SendEmailUrl = domainurl + "/Customer/ShareCustomerInfo?Id=" + CustomerId + "&from=lead";
        console.log("Email link created.:" + CustomerId);
        //OpenTopToBottomModal(domainurl + "/Customer/ShareCustomerInfo?Id=" + CustomerId);

        $(".LeadInfosharespace").attr("href", SendEmailUrl);
        $(".LeadInfosharespace").click();
    });
    $("#btn_addccpaymentcustomer").click(function () {
        OpenRightToLeftModal("/SmartLeads/CCAddViewPaymentMethod?customerid=" + CustomerLoadGuid + "&type=Customer");
    });
    $("#btn_addachpaymentcustomer").click(function () {
        OpenRightToLeftModal("/SmartLeads/ACHAddViewPaymentMethod?customerid=" + CustomerLoadGuid + "&type=Customer");
    });
    //$('[data-toggle="tooltip"]').tooltip();
})