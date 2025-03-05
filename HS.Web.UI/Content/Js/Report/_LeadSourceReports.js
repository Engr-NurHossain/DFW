String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var DataTablePageSize = 50;
var PayrollStartDatepicker;
var PayrollEndDatepicker;
 

var GetSelectedUserId = function () {
    var UserIdList = ""; 
    $('.treeview .user:checked').each(function () {
        UserIdList += ",'" + $(this).val() + "'"
    });

    UserIdList = UserIdList.slice(1);
    return encodeURI(UserIdList);
}
 
var my_date_format = function (input) {
    var d = new Date(Date.parse(input.replace(/-/g, "/")));
    var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var date = d.getDate() + " " + month[d.getMonth()] + ", " + d.getFullYear();

    return (date);
};

var FilterLeadSourceReport2 = function (pageno, order) {
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    
    pagesize = 50;
    $(".LeadSource_Report").load(domainurl + "/Reports/LoadLeadSourceReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order + "&empGIDList=" + GetSelectedUserId());
}


var LeadSourceReportTab = function () {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    $(".LeadSource_Report").html(TabsLoaderText);
    $(".LeadSource_Report").load(domainurl + "/Reports/LoadLeadSourceReportPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&empGIDList=" + GetSelectedUserId());
}

var LeadSourceReportBarTab = function () {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    var UserIdList = "";
    $('.treeview .User:checked').each(function () {
        UserIdList += ",'" + $(this).val() + "'"
    });

    UserIdList = UserIdList.slice(1);
    $(".LeadSourceReportBarBox").load(domainurl + "/Reports/LoadLeadSourceReportBarPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&empGIDList=" + GetSelectedUserId());
}


$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    LeadSourceReportTab();
    LeadSourceReportBarTab();
    $(".partner_report_emp_list").click(function () {
        if ($(".treeview_cont").is(":visible")) {
            $(".treeview_cont").slideUp();

        } else {
            $(".treeview_cont").slideDown();

        }
    });

    $(".DateFilterContents .btn-apply-Datefilter").click(function () {
        var DateFrom = $(".min-date").val();
        var DateTo = $(".max-date").val();
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
        LeadSourceReportBarTab();
        FilterLeadSourceReport2(1, null);

    });


    $("#sales_txt_search").keyup(function (event) {
        event.preventDefault();
        if (event.keyCode == 13) {
            FilterSalesReport1(1)
        }
    })
    $(".btnLeadSourceFilter").click(function () {
        LeadSourceReportBarTab();
        LeadSourceReportTab();
       // $(".treeview_cont").slideUp();
    });
    if (window.innerWidth < 769)
    {
       
    }
    else {
        $('.treeview').height(window.innerHeight - 237);
    }
   
});
$(window).resize(function () {
    if (window.innerWidth < 769) {

    }
    else {
        $('.treeview').height(window.innerHeight - 237);
    }
   
});
