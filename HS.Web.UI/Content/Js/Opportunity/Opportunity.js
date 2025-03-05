var EditOpportunity = function (id) {
    OpenTopToBottomModal(domainurl + "/Opportunity/AddOpportunity?id=" + id);
}

var NavigatePageListing = function (pagenumber, order) {
    var searchText = $(".srch-term").val();
    $(".ListContents").load(domainurl + "/Opportunity/LoadOpportunityList", { PageNumber: pagenumber, SearchText: searchText, Order: order });
}

var LoadOpportunity = function () {
    var pagenumber = 1;
    var DateFrom = $(".min-date").val();
    var DateTo = $(".max-date").val();
    $(".ListContents").load(domainurl + "/Opportunity/LoadOpportunityList", { PageNumber: pagenumber,CreatedDateFrom:DateFrom,CreatedDateTo:DateTo });
}
var PrintOpportunity = function () {

    var Type = $("#OpportunityType").val();
    var status = $("#OpportunityStatus").val();
    var OpportunityProbability = $("#OpportunityProbability").val();
    var OpportunityDealReason = $("#OpportunityDealReason").val();
    var OpportunityYesNo = $("#OpportunityYesNo").val();

    var OpportunityDeliveryDays = $("#OpportunityDeliveryDays").val();
    var OpportunityCampaignSource = $("#OpportunityCampaignSource").val();
    var AccountOwner = $("#EmployeeList").val();
    var RevenueFrom = $(".RevenueFrom").val();
    var RevenueTo = $(".RevenueTo").val();
    var ProjectedGpFrom = $(".ProjectedGpFrom").val();
    var ProjectedGpTo = $(".ProjectedGpTo").val();
    var PointFrom = $(".PointFrom").val();
    var PointTo = $(".PointTo").val();
    var searchText = $(".srch-term").val();
    var PdfUrl = "/Opportunity/GetOpportunityFilterList/?Type=" + Type
          + "&OpporStatus=" + encodeURI(status)
          + "&OpportunityProbability=" + OpportunityProbability
          + "&OpportunityDealReason=" + OpportunityDealReason
          + "&OpportunityYesNo=" + OpportunityYesNo
          + "&OpportunityCampaignSource=" + OpportunityCampaignSource
          + "&OpportunityDeliveryDays=" + OpportunityDeliveryDays
          + "&AccountOwner=" + AccountOwner
          + "&RevenueFrom=" + RevenueFrom
          + "&RevenueTo=" + RevenueTo
          + "&ProjectedGpFrom=" + ProjectedGpFrom
          + "&ProjectedGpTo=" + ProjectedGpTo
          + "&PointFrom=" + PointFrom
          + "&PointTo=" + PointTo
          + "&SearchText=" + searchText
  
    window.open(PdfUrl, '_blank');

}
$(document).ready(function () {
    var pagenumber = 1;
    $("#AddNewOpportunity").click(function () {
        OpenTopToBottomModal(domainurl + "/Opportunity/AddOpportunity");
    });
    LoadOpportunity();
    $('.SearchOpportunity').click(function () {
        NavigatePageListing(pagenumber);
    })
    $(".btn-apply-filter").click(function () {
        console.log("hlw");
        if (typeof (pagenumber) == "undefined") {
            return;
        }
        var Type = $("#OpportunityType").val();
        var opportunityStatus1 = $("#OpportunityStatus").val();
        var OpportunityProbability = $("#OpportunityProbability").val();
        var OpportunityDealReason = $("#OpportunityDealReason").val();
        var OpportunityYesNo = $("#OpportunityYesNo").val();

        var OpportunityDeliveryDays = $("#OpportunityDeliveryDays").val();
        var OpportunityCampaignSource = $("#OpportunityCampaignSource").val();
        var AccountOwner = $("#EmployeeList").val();
        var RevenueFrom = $(".RevenueFrom").val();
        var RevenueTo = $(".RevenueTo").val();
        var ProjectedGpFrom = $(".ProjectedGpFrom").val();
        var ProjectedGpTo = $(".ProjectedGpTo").val();
        var PointFrom = $(".PointFrom").val();
        var PointTo = $(".PointTo").val();
        var searchText = $(".srch-term").val();
        var DateFrom = $(".min-date").val();
        var DateTo = $(".max-date").val();
        $('.ListContents').load(domainurl + "/Opportunity/LoadOpportunityList/?Type=" + Type
          + "&OpporStatus=" + encodeURI(opportunityStatus1)
          + "&OpportunityProbability=" + OpportunityProbability
          + "&OpportunityDealReason=" + OpportunityDealReason
          + "&OpportunityYesNo=" + OpportunityYesNo
          + "&OpportunityCampaignSource=" + OpportunityCampaignSource
          + "&OpportunityDeliveryDays=" + OpportunityDeliveryDays
          + "&AccountOwner=" + AccountOwner
          + "&RevenueFrom=" + RevenueFrom
          + "&RevenueTo=" + RevenueTo
          + "&ProjectedGpFrom=" + ProjectedGpFrom
          + "&ProjectedGpTo=" + ProjectedGpTo
          + "&PointFrom=" + PointFrom
          + "&PointTo=" + PointTo
          + "&SearchText=" + searchText
          + "&CreatedDateFrom=" + DateFrom
          + "&CreatedDateTo=" + DateTo
         );
    })
    $(".btn-apply-Datefilter").click(function () {
        console.log("hlw");
        if (typeof (pagenumber) == "undefined") {
            return;
        }
        var Type = $("#OpportunityType").val();
        var opportunityStatus = $("#OpportunityStatus").val();
        var OpportunityProbability = $("#OpportunityProbability").val();
        var OpportunityDealReason = $("#OpportunityDealReason").val();
        var OpportunityYesNo = $("#OpportunityYesNo").val();

        var OpportunityDeliveryDays = $("#OpportunityDeliveryDays").val();
        var OpportunityCampaignSource = $("#OpportunityCampaignSource").val();
        var AccountOwner = $("#EmployeeList").val();
        var RevenueFrom = $(".RevenueFrom").val();
        var RevenueTo = $(".RevenueTo").val();
        var ProjectedGpFrom = $(".ProjectedGpFrom").val();
        var ProjectedGpTo = $(".ProjectedGpTo").val();
        var PointFrom = $(".PointFrom").val();
        var PointTo = $(".PointTo").val();
        var searchText = $(".srch-term").val();
        var DateFrom = $(".min-date").val();
        var DateTo = $(".max-date").val();
        
        console.log(DateFrom);
        $('.ListContents').load(domainurl + "/Opportunity/LoadOpportunityList/?Type=" + Type
       + "&OpporStatus=" + encodeURI(opportunityStatus)
       + "&OpportunityProbability=" + OpportunityProbability
       + "&OpportunityDealReason=" + OpportunityDealReason
       + "&OpportunityYesNo=" + OpportunityYesNo
       + "&OpportunityCampaignSource=" + OpportunityCampaignSource
       + "&OpportunityDeliveryDays=" + OpportunityDeliveryDays
       + "&AccountOwner=" + AccountOwner
       + "&RevenueFrom=" + RevenueFrom
       + "&RevenueTo=" + RevenueTo
       + "&ProjectedGpFrom=" + ProjectedGpFrom
       + "&ProjectedGpTo=" + ProjectedGpTo
       + "&PointFrom=" + PointFrom
       + "&PointTo=" + PointTo
       + "&SearchText=" + searchText
       + "&CreatedDateFrom=" + DateFrom
       + "&CreatedDateTo="+ DateTo
      );
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
    })

    $(".btn-reset-filter").click(function () {
            $("#OpportunityType").val("-1");
            $("#OpportunityStatus").val("-1");
            $("#OpportunityProbability").val("-1");
            $("#OpportunityDealReason").val("-1");
            $("#OpportunityYesNo").val("Yes");

            $("#OpportunityDeliveryDays").val("-1");
            $("#OpportunityCampaignSource").val("-1");
            $("#EmployeeList").val("ac0ce890-bc5b-4c34-aab2-017af19bedf6");
            $(".RevenueFrom").val("");
            $(".RevenueTo").val("");
            $(".ProjectedGpFrom").val("");
            $(".ProjectedGpTo").val("");
            $(".PointFrom").val("");
            $(".PointTo").val("");
      
    })
});