var showallr = false;
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var RestockDataLoad = function (techId, Id, order, showall, isService) {
    var fDate = null;
    var LDate = null;
    if (isService == "All") {
        FDate = null;
        LDate = null;
    }
    else if (isService == "Daily") {
        FDate = firstdate;
        LDate = lastdate;
    }
    else if (isService == "Weekly") {
        FDate = thisfirstdate;
        LDate = thislastdate;
    }
    else if (isService == "Monthly") {
        FDate = thisfirstmonthdate;
        LDate = thislastmonthdate;
    }
    else if (isService == "Yearly") {
        FDate = thisfirstyeardate;
        LDate = thislastyeardate;
    }

    console.log("RestockDataLoad working");
    var LoadUrl = domainurl + "/App/LoadTechUpsoldPartialList?TechnicianId=" + techId + "&Id=" + Id + "&Order=" + order + "&ShowAll=" + showall + "&Searchtext=" + isService + "&FDate=" + FDate + "&LDate=" + LDate;
    //var LoadUrl = domainurl + "/App/LoadTechEstimatePartialList";
    $(".RestockDataLoad_" + Id).html(TabsLoaderText);
    $(".RestockDataLoad_" + Id).load(LoadUrl);
}
var date = new Date();
var dailydate = date.getMonth() + "/" + date.getDate() + "/" + date.getFullYear();
var firstdate = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
var max = dailydate.split(/\D+/);
var maxdate = new Date(max[2], max[0], (parseInt(max[1]) + 1));
var lastdate = maxdate.getMonth() + 1 + "/" + maxdate.getDate() + "/" + maxdate.getFullYear();
var first = date.getDate() - date.getDay();
var last = first + 6;
var firstday = new Date(date.setDate(first));
var lastday = new Date(date.setDate(last));
var thisfirstdate = firstday.getMonth() + 1 + "/" + firstday.getDate() + "/" + firstday.getFullYear();
var thislastdate = lastday.getMonth() + 1 + "/" + lastday.getDate() + "/" + lastday.getFullYear();
var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);
var thisfirstmonthdate = firstDay.getMonth() + 1 + "/" + firstDay.getDate() + "/" + firstDay.getFullYear();
var thislastmonthdate = lastDay.getMonth() + 1 + "/" + lastDay.getDate() + "/" + lastDay.getFullYear();
var firstyearday = new Date("1/1/" + date.getFullYear());
var lastyearday = new Date("12/31/" + date.getFullYear());
var thisfirstyeardate = firstyearday.getMonth() + 1 + "/" + firstyearday.getDate() + "/" + firstyearday.getFullYear();
var thislastyeardate = lastyearday.getMonth() + 1 + "/" + lastyearday.getDate() + "/" + lastyearday.getFullYear();
$(document).ready(function () {
    RestockDataLoad(TechnicianIdr, Idr, "", showallr, isService);
    //$(".RestockDataLoadPanelHeading").find("a").click(function () {
    //    $(".ChkShowAllMassRestock").prop("checked", false);
    //    showallr = false;
    //    $(".RestockDataClear").html("");
    //    TechnicianIdr = $(this).attr("data-techid");
    //    Idr = $(this).attr("data-id");
    //    RestockDataLoad(TechnicianIdr, Idr, "", showallr);
    //});
    //$(".RestockDataLoadPanelHeading").find("a").first().trigger("click");
    //$('#accordion').find('.accordion_header').click(function () {
    //    var $this = $(this);
    //    $this.toggleClass("open");
    //});
    //$(".RestockDataLoadPanelHeading").find(".ChkShowAllMassRestock").change(function () {
    //    if ($(this).prop("checked")) {
    //        showallr = true;
    //    }
    //    else {
    //        showallr = false;
    //    }
    //    $(".RestockDataClear").html("");
    //    TechnicianIdr = $(this).attr("data-techid");
    //    Idr = $(this).attr("data-id");
    //    RestockDataLoad(TechnicianIdr, Idr, "", showallr);
    //});
});