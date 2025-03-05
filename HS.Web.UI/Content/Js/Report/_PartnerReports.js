String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};
var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
//var empGIDList = "'ac0ce890-bc5b-4c34-aab2-017af19bedf6','fc081c84-1825-4bd0-9194-d59bdd5a1850','fe304ee6-d5b7-43dd-8a9f-f82337e4c0ff'";
var DataTablePageSize = 50;
var PayrollStartDatepicker;
var PayrollEndDatepicker;

var GetSelectedUserId = function ()
{
    var UserIdList = "";
    $('.treeview .User:checked').each(function () {
        UserIdList += ",'" + $(this).val() + "'"
    });

    UserIdList = UserIdList.slice(1);
    return UserIdList;
}


var my_date_format = function (input) {
    var d = new Date(Date.parse(input.replace(/-/g, "/")));
    var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var date = d.getDate() + " " + month[d.getMonth()] + ", " + d.getFullYear();

    return (date);
};

var FilterPartnerReport2 = function (pageno, order) {
    var datemin = $(".min-date").val();
    var datemax = $(".max-date").val();
    
    pagesize = 50;
    $(".Partner_Report").load(domainurl + "/Reports/LoadPartnerReportPartial?Start=" + encodeURI(datemin) + "&End=" + encodeURI(datemax) + "&pageno=" + pageno + "&pagesize=" + pagesize + "&searchtxt=" + encodeURI($("#sales_txt_search").val()) + "&invostatus=" + encodeURI($("#sales_inv_status").val()) + "&order=" + order + "&empGIDList=" + GetSelectedUserId());
}


var PartnerReportTab = function () {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    $(".Partner_Report").html(TabsLoaderText);
    $(".Partner_Report").load(domainurl + "/Reports/LoadPartnerReportPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&pageno=1&pagesize=50" + "&empGIDList=" + GetSelectedUserId());
}

var PartnerReportBarTab = function () {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    var UserIdList = "";
    $('.treeview .User:checked').each(function () {
        UserIdList += ",'" + $(this).val() + "'"
    });

    UserIdList = UserIdList.slice(1);
    $(".PartnerReportBarBox").load(domainurl + "/Reports/LoadPartnerReportBarPartial?Start=" + encodeURI(StartDate) + "&End=" + encodeURI(EndDate) + "&empGIDList=" + GetSelectedUserId());
}


$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    PartnerReportTab();
    PartnerReportBarTab();
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
        PartnerReportBarTab();
        FilterPartnerReport2(1, null);

    });


    $("#sales_txt_search").keyup(function (event) {
        event.preventDefault();
        if (event.keyCode == 13) {
            FilterSalesReport1(1)
        }
    })
    $(".btnPartnerFilter").click(function () {
        PartnerReportBarTab();
        PartnerReportTab();
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
