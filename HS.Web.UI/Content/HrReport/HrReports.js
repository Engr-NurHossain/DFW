var TabsLoaderText = "<div class='invoice-loader'><div><div class='lds-css ng-scope'><div style='margin:auto; z-index:99;' class='lds-double-ring'><div></div><div></div></div></div></div></div>";
var date = new Date();
var OpenEmployeeTab = function () {
    $(".sales_matrix_report_nav ul li").removeClass("active");
    $(".sales_matrix_report_nav .tab-content .tab-pane").removeClass("active");
    $(".EmployeeNav").addClass("active");
    $("#EmployeeTab").addClass("active");
    $("#EmployeeTab").html(TabsLoaderText);
    $(".default_date_filter").show();
    $("#EmployeeTab").load(domainurl + "/Reports/Employee");

    window.history.pushState({ urlPath: window.location.pathname }, "", $(".EmployeeNav a").attr('href'))
}
var OpenInsuranceTab= function () {
    $(".sales_matrix_report_nav ul li").removeClass("active");
    $(".sales_matrix_report_nav .tab-content .tab-pane").removeClass("active");
    $(".InsuranceNav").addClass("active");
    $("#InsuranceTab").addClass("active");
    $("#InsuranceTab").html(TabsLoaderText);
    $(".default_date_filter").show();

    $("#InsuranceTab").load(domainurl + "/Reports/Insurance");

    window.history.pushState({ urlPath: window.location.pathname }, "", $(".InsuranceNav a").attr('href'))
}

$(document).ready(function () {
   
    $("#DepartmentFilter").selectpicker('val', '');
    $("#EmpStatusFilter").selectpicker('val', ["1"]);
    $(".LoaderWorkingDiv").hide();
    if (top.location.hash != "") {
        if (top.location.hash == "#tabEmployee") {
            OpenEmployeeTab();
        }
        else if (top.location.hash == "#tabInsurance") {
            OpenInsuranceTab();
        }
    }
    else {
        OpenEmployeeTab();
    }


    $(".EmployeeNav").click(function () {
        OpenEmployeeTab();
    });

    $(".InsuranceNav").click(function () {
        OpenInsuranceTab();

    });

   

    $(".DateFilterContents .btn-apply-Datefilter").unbind().click(function () {
        var DateFrom = $(".min-date").val();
        var DateTo = $(".max-date").val();
        UpdatePtoCookie();

        if (top.location.hash != "") {
            if (top.location.hash == "#tabEmployee") {
                OpenEmployeeTab();
            }
            else if (top.location.hash == "#tabInsurance") {
                OpenInsuranceTab();
            }
        }
        else {
            OpenEmployeeTab();
        }
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

    });
});