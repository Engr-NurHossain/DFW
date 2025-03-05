var mindatefilter;
var maxdatefilter;
var SortByCol = "id";
var lastdateval;
var leadestimate = false;
var leadBooking = false;
var leadthismonth = false;
var leadlastmonth = false;


var my_date_format = function (input) {
    var d = new Date(Date.parse(input.replace(/-/g, "/")));
    var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var date = d.getDate() + " " + month[d.getMonth()] + ", " + d.getFullYear();

    return (date);
};
var CustomerSearchKeyUp = function (pageNumber) {
    if (typeof (pageNumber) == "undefined") {
        return;
    }
    var UserList = $(".UserList").val();
    var SourceList = $(".SourceList").val();
    var firstdate = $(".min-date").val();
    console.log(firstdate);
    //var v = $(".max-date").val().split(/\D+/);
    //var d = new Date(v[2], v[0] - 1, (parseInt(v[1]) + 1));
    var lastdate = $(".max-date").val();
    var searchtext = encodeURI($("#srch-term").val());
    var status = encodeURI($(".Status").val());
    var branch = $("#BranchList").val();
    var FollowUpDate = $(".FollowUpDate").val();
    $('.filter-Lead-List').html(LoaderDom);
    $('.filter-Lead-List').load(domainurl + "/Leads/FilterCustomerListPartial/?User=" + UserList
        + "&Source=" + SourceList
        + "&PageNo=" + pageNumber
        + "&firstdate=" + firstdate
        + "&lastdate=" + lastdate
        + "&FollowUpDate=" + FollowUpDate
        + "&SortBy=" + SortByCol
        + "&SearchText=" + encodeURI(searchtext)
        + "&Status=" + status
        + "&Branch=" + branch + "&LeadEstimate=" + leadestimate + "&leadthismonth=" + leadthismonth + "&leadlastmonth=" + leadlastmonth + "&leadBooking=" + leadBooking);
}
var NewCustomerSearchKeyUp = function (pageNumber) {
    if (typeof (pageNumber) == "undefined") {
        return;
    }
    var UserList = $(".UserList").val();
    var SourceList = $(".SourceList").val();
    var firstdate = $(".min-date").val();
    console.log(firstdate);
    //var v = $(".max-date").val().split(/\D+/);
    //var d = new Date(v[2], v[0] - 1, (parseInt(v[1]) + 1));
    var lastdate = $(".max-date").val();
    var searchtext = $("#srch-term").val();
    var status = encodeURI("New");
    var branch = $("#BranchList").val();
    var FollowUpDate = $(".FollowUpDate").val();
    $('.filter-Lead-List').html(LoaderDom);
    $('.filter-Lead-List').load(domainurl + "/Leads/FilterCustomerListPartial/?User=" + UserList
        + "&Source=" + SourceList
        + "&PageNo=" + pageNumber
        + "&firstdate=" + firstdate
        + "&lastdate=" + lastdate
        + "&FollowUpDate=" + FollowUpDate
        + "&SortBy=" + SortByCol
        + "&SearchText=" + encodeURI(searchtext)
        + "&Status=" + status
        + "&Branch=" + branch + "&LeadEstimate=" + leadestimate + "&leadthismonth=" + leadthismonth + "&leadlastmonth=" + leadlastmonth + "&leadBooking=" + leadBooking);
}
var CustomerSearchKeyUpDashboard = function (pageNumber) {
    if (typeof (pageNumber) == "undefined") {
        return;
    }
    var UserList = $(".UserList").val();
    var SourceList = $(".SourceList").val();
    var firstdate = $(".min-date").val();

    var lastdate = $(".max-date").val();
    var searchtext = $("#srch-term").val();
    var status = encodeURI($(".Status").val());
    var branch = $("#BranchList").val();
    var FollowUpDate = $(".FollowUpDate").val();
    $('.filter-Lead-List').load(domainurl + "/Leads/FilterCustomerListPartial/?User=" + UserList
        + "&Source=" + SourceList
        + "&PageNo=" + pageNumber
        + "&firstdate=" + firstdate
        + "&lastdate=" + lastdate
        + "&FollowUpDate=" + FollowUpDate
        + "&SortBy=" + SortByCol
        + "&SearchText=" + encodeURI(searchtext)
        + "&Status=" + status
        + "&Branch=" + branch + "&LeadEstimate=" + leadestimate + "&leadthismonth=" + leadthismonth + "&leadlastmonth=" + leadlastmonth + "&leadBooking=" + leadBooking);
}
var LoadLeadEstimateList = function () {
    leadestimate = true;
    leadthismonth = false;
    leadlastmonth = false;
    leadBooking = false;
    CustomerSearchKeyUp(1);
}
var LoadLeadThisMonthList = function () {
    leadthismonth = true;
    leadestimate = false;
    leadlastmonth = false;
    leadBooking = false;
    CustomerSearchKeyUp(1);
}
var LoadLeadLastMonthList = function () {
    leadlastmonth = true;
    leadestimate = false;
    leadthismonth = false;
    leadBooking = false;
    CustomerSearchKeyUp(1);
}
var LoadListLead = function () {
    leadestimate = false;
    leadthismonth = false;
    leadlastmonth = false;
    leadBooking = false;
    CustomerSearchKeyUp(1);
}
var LoadLeadBookingList = function () {
    leadestimate = false;
    leadthismonth = false;
    leadlastmonth = false;
    leadBooking = true;
    CustomerSearchKeyUp(1);
}
var MarkAllNewLeadAsRead = function () {
    var url = domainurl + "/Leads/MarkAllNewLeadAsRead/";
    $.ajax({
        type: "POST",
        ajaxStart: $(".loader-div").show(),
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data) {

                CustomerSearchKeyUp(1);
                $(".MarkAllNewLeadAsReadDiv").addClass("hidden");


                // LoadLeadVerificationInfo(0, true);
                //var value = data.LeadId;
                //LoadLeadVerificationInfo(value);
                //OpenSuccessMessageNew("Success!", "All New lead created successfully.", function () {
                //    LoadLeadVerificationInfo(0, true);
                //})
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    FollowUpDateFilter = new Pikaday({ format: 'MM/DD/YYYY', field: $("#FollowUpDateFilter")[0] });
    if (dailyfirstdate != '') {
        $(".min-date").val(dailyfirstdate);
    }
    if (dailylastdate != '') {
        $(".max-date").val(dailylastdate);
    }
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    });
    $(document).click(function (e) {
        if ($($(e.target).parent()).hasClass('btn-filter')) {
            return;
        }
        else if ($(".fliter-list").is(":visible")) {
            $(".fliter-list").hide();
        }
    });
    $('#page-wrapper').click(function (e) {
        if ($($(e.target).parent()).hasClass('btn-filter')) {
            return;
        }
        else if ($(".fliter-list").is(":visible")) {
            $(".fliter-list").hide();
        }
    });

    //mindatefilter = new Pikaday({ format: 'MM/DD/YYYY', field: $(".min-date")[0] });
    //maxdatefilter = new Pikaday({ format: 'MM/DD/YYYY', field: $(".max-date")[0] });
    //CustomerSearchKeyUp(1);

    //if ($(".min-date").val() != "" && $(".max-date").val() != "") {
    //    CustomerSearchKeyUpDashboard(1);
    //}
    var UserList = $(".UserList").val();
    var SourceList = $(".SourceList").val();
    var firstdate = $(".min-date").val();
    var lastdate = $(".max-date").val();
    var status = encodeURI($(".Status").val());
    var branch = $("#BranchList").val();
    var FollowUpDate = $(".FollowUpDate").val();
    

    $(".filter-Lead-List").load(domainurl + "/Leads/FilterCustomerListPartial/?UserList=" + UserList
        + "&SourceList=" + SourceList
        + "&firstdate=" + firstdate
        + "&lastdate=" + lastdate
        + "&FollowUpDate=" + FollowUpDate
        + "&Status=" + status
        + "&Branch=" + branch + "&LeadEstimate=" + leadestimate + "&leadthismonth=" + leadthismonth + "&leadlastmonth=" + leadlastmonth + "&leadBooking=" + leadBooking);

    $(".btn-apply-filter").click(function () {
        CustomerSearchKeyUp(1);
    })
    $(".custom-btn").click(function () {
        CustomerSearchKeyUp(1);
    })
    $(".btn-apply-Datefilter").click(function () {
        CustomerSearchKeyUp(1);
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
    });
    $(".btn-filter").click(function () {
        $(".fliter-list").toggle();
        $(".lead_list_filter").load("/Customer/FilterCustomerLeadListGridSettingPartial?key=LeadGrid");
    });
    $(".fliter-list").click(function (e) {
        e.stopPropagation();
    });


    $(".btn-reset-filter").click(function () {
        $("#min-date").val("");
        $(".max-date").val("");
        $("#FollowUpDateFilter").val("");
        $(".UserList").val("-1");
        $(".Status").val("-1");
        $("#BranchList").val("-1");
    })
    $(".lead-grid-col").click(function () {
        OpenRightToLeftModal(domainurl + "/Leads/LeadGridSettings");
    });
    $(".Emailtextforlead").click(function () {
        OpenRightToLeftModal(domainurl + "/Leads/LeadEmailTextTemplatePartial");
    })
    $("#srch-term").keyup(function (e) {
        e.preventDefault();
        if (e.keyCode == 13) {
            CustomerSearchKeyUp(1);
        }
    })
    $('#MarkAllNewLeadAsRead').click(function () {
        MarkAllNewLeadAsRead();
    });

    $("#DisplayNewLeadsOnly").click(function () {
        NewCustomerSearchKeyUp(1);
    })
})