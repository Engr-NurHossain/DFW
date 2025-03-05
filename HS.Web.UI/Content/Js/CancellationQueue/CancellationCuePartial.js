var my_date_format = function (input) {
    console.log(input + " r");
    var d = new Date(Date.parse(input.replace(/-/g, "/")));
    var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var date = d.getDate() + " " + month[d.getMonth()] + ", " + d.getFullYear();
    return (date);
};
var CustomerSearchKeyUp = function (pageno) {
    var CancellationReason = encodeURI($("#CancellationReason").val());
    var DFWCancellationReason = encodeURI($("#CancellationReasonDFW").val());
    var effectivemindate = $("#effective_min_date").val();
    var effectivemaxdate = $("#effective_max_date").val();
    pagesize = parseInt(cn) + 50;
    var customer = encodeURI($("#cus_name").val());

    var ContractSigned = $("#ContractSigned").val();
  //  $("#CustomerCancellationTab").html(TabsLoaderText);
    $(".CustomerCancellation_report").load(domainurl + "/Reports/CancellationCuePartial?pageno=" + pageno + "&pagesize=" + pagesize + "&reason=" + CancellationReason + "&employeereason=" + DFWCancellationReason + "&contractSigned=" + ContractSigned + "&effectivemindate=" + effectivemindate + "&effectivemaxdate=" + effectivemaxdate + "&name=" + customer);
}

$(document).ready(function () {
    var StartDate = $(".min-date").val();
    var EndDate = $(".max-date").val();
    $(".LoaderWorkingDiv").hide();
    $("#LeadReportTab .invoice-loader").hide();
    if (StartDate != "" && EndDate != "") {
        StartDate = my_date_format(StartDate);
        EndDate = my_date_format(EndDate);
        console.log(StartDate + " " + EndDate);
        if (StartDate == "NaN undefined, NaN") {
            StartDate = "All Time";
            EndDate = "";
        }

        $(".DateFilterContents .date-start").html("");
        $(".DateFilterContents .date-end").html("");
        $(".DateFilterContents .date-start").html(StartDate);
        $(".DateFilterContents .date-end").html(EndDate);
        $(".DateFilterContents .dropdown-filter").hide();
    }
    else {
        $(".DateFilterContents .date-start").html("All Time");
        $(".DateFilterContents .date-end").html("");
        $(".DateFilterContents .dropdown-filter").hide();
    }
    $(".DateFilterContents .btn-apply-Datefilter").unbind().click(function () {
        var StartDateVal = $(".min-date").val();
        var EndDateVal = $(".max-date").val();
        UpdatePtoCookie();
        CustomerSearchKeyUp(1);
    });
    $(".SearchCacncelledCustomer").click(function () {
        var StartDateVal = $(".min-date").val();
        var pageno = 1;
        var EndDateVal = $(".max-date").val();
        var CancellationReason = encodeURI($("#CancellationReason").val());
        var DFWCancellationReason = encodeURI($("#CancellationReasonDFW").val());
        var effectivemindate = $("#effective_min_date").val();
        var effectivemaxdate = $("#effective_max_date").val();
        var ContractSigned = $("#ContractSigned").val();
        var customer = encodeURI($("#cus_name").val());
        customer = customer.replace(/\s+/g, ' ').trim(); 

        $(".CustomerCancellation_report").html(TabsLoaderText);
        $(".CustomerCancellation_report").load(domainurl + "/Reports/CancellationCuePartial/?Start=" + StartDateVal + "&End=" + EndDateVal + "&pageno=" + pageno + "&pagesize=" + DataTablePageSize + "&reason=" + CancellationReason + "&employeereason=" + DFWCancellationReason + "&contractSigned=" + ContractSigned + "&effectivemindate=" + effectivemindate + "&effectivemaxdate=" + effectivemaxdate + "&name=" + customer);
    })
});