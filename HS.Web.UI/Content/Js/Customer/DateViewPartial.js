var DropDownDateTime = function () {
    if ($('.dropdown-filter').is(":visible")) {
        $(".dropdown-filter").hide();
    }
    else {
        $(".dropdown-filter").show();
    }
}
var UpdatePtoCookie = function () {
    var FirstDayStr = $("#PayrollFilterStartDate").val();
    var EndDayStr = $("#PayrollFilterEndDate").val();

    var PTOFilterStr = $("#DateRange").val();
    var NewCookie = String.format("{0},{1},{2}", FirstDayStr, EndDayStr, PTOFilterStr);
    console.log(NewCookie);
    $.cookie("_DateViewFilter", NewCookie, { expires: 1 * 60 * 24, path: '/' });
}

$(document).ready(function () {
    StartDateDatepicker = new Pikaday({
        field: $('#PayrollFilterStartDate')[0],
        format: 'MM/DD/YYYY',
        yearRange: [2016, 2030],
        defaultDate: '01/01/01'
        //minDate: new Date('1-1-2020'),
    });
    EndDateDatepicker = new Pikaday({
        field: $('#PayrollFilterEndDate')[0],
        format: 'MM/DD/YYYY',
        yearRange: [2016, 2030],
        defaultDate: '01/01/01'
        //minDate: new Date('1-1-2020'),
    });
    $("body").on('click', function (evt) {
        console.log('datetime body click');
        var clickClass = $(evt.target || evt.srcElement).attr("class");
        var clickId = $(evt.target || evt.srcElement).attr("id");
        if (typeof clickClass === "undefined" && evt.target != null && evt.target.offsetParent != null && evt.target.offsetParent.className != null && evt.target.offsetParent.className == "dropdown-filter") {
            clickClass = evt.target.offsetParent.className;
        }

        //if (clickClass != "dateTable" && clickClass != "date-start _GAmC" && clickClass != "_GArn" && clickClass != "clearfix fixed_header custom_top" && clickClass != "fa fa-caret-down" && clickClass != "_GAjo" && clickClass != "caret_style") {
        //        //if ($('.dropdown-filter').is(":visible")) {
        //        //    $(".dropdown-filter").hide();
        //        //}
        //}
        if (clickClass != "dateTable" && clickClass != "date-start _GAmC" && clickClass != "_GArn" && clickClass != "clearfix fixed_header custom_top" && clickClass != "fa fa-caret-down" && clickClass != "_GAjo" && clickClass != "caret_style" && clickClass != "date-start _GAmC" && clickClass != "date-end _GAaC" && clickClass != "_GArn" && clickClass != "clearfix fixed_header custom_top" && clickClass != "fa fa-caret-down" && clickClass != "_GAjo" && clickClass != "caret_style"
            && clickClass != "dropdown-filter" && clickClass != "input-group-addon" && clickClass != "filter-by-equipmentClass sub-list" && clickClass != "form-control select_leaduser daterangeList" && clickClass != "form-control date-range-filter min-date" && clickClass != "form-control date-range-filter max-date" && clickClass != "undefined") {
            if ($('.dropdown-filter').is(":visible")) {
                $(".dropdown-filter").hide();
            }
        }

    });
    $(".dropdown-filter").hide();
    $(".dateTable").click(function () {
        if ($('.dropdown-filter').is(":visible")) {
            $(".dropdown-filter").hide();
        }
        else {
            $(".dropdown-filter").show();
        }

    })

    $("#PayrollFilterStartDate").blur(function () {
        $("#DateRange").val("Custom");
    })
    $("#PayrollFilterEndDate").blur(function () {
        $("#DateRange").val("Custom");
    })
    $("#DateRange").change(function () {
        console.log("hlw");
        if ($(this).val() == "Today") {
            var Today = new Date();
            StartDateDatepicker.setDate(Today);
            EndDateDatepicker.setDate(Today);
        }
        else if ($(this).val() == "Yesterday") {
            var Today = new Date();
            EndDateDatepicker.setDate(Today.addDays(-1));
            StartDateDatepicker.setDate(Today);
        }
        else if ($(this).val() == "ThisWeek") {
            var Today = new Date();
            var Week = Today.getWeek();
            var StartDay = getDateOfISOWeek(Week, Today.getFullYear());

            StartDateDatepicker.setDate(StartDay);
            EndDateDatepicker.setDate(StartDay.addDays(6));
        }
        else if ($(this).val() == "LastWeek") {
            var Today = new Date();
            Today = Today.addDays(-7);

            var Week = Today.getWeek();
            var StartDay = getDateOfISOWeek(Week, Today.getFullYear());

            StartDateDatepicker.setDate(StartDay);
            EndDateDatepicker.setDate(StartDay.addDays(6));
        }
        else if ($(this).val() == "ThisMonth") {
            var Today = new Date();

            var FirstDayOfMonth = new Date(Today.getFullYear(), Today.getMonth(), 1);
            var LastDayOfMonth = new Date(Today.getFullYear(), Today.getMonth() + 1, 0);

            StartDateDatepicker.setDate(FirstDayOfMonth);
            EndDateDatepicker.setDate(LastDayOfMonth);

        }
        else if ($(this).val() == "LastMonth") {
            var Today = new Date();

            var FirstDayOfMonth = new Date(Today.getFullYear(), Today.getMonth() - 1, 1);
            var LastDayOfMonth = new Date(Today.getFullYear(), Today.getMonth(), 0);

            StartDateDatepicker.setDate(FirstDayOfMonth);
            EndDateDatepicker.setDate(LastDayOfMonth);
        }
        else if ($(this).val() == "ThisYear") {
            var Today = new Date();

            var FirstDayOfMonth = new Date(Today.getFullYear(), 0, 1);
            var LastDayOfMonth = new Date(Today.getFullYear(), 11, 31);

            StartDateDatepicker.setDate(FirstDayOfMonth);
            EndDateDatepicker.setDate(LastDayOfMonth);
        }
        else if ($(this).val() == "LastYear") {
            var Today = new Date();

            var FirstDayOfMonth = new Date(Today.getFullYear() - 1, 0, 1);
            var LastDayOfMonth = new Date(Today.getFullYear() - 1, 11, 31);

            StartDateDatepicker.setDate(FirstDayOfMonth);
            EndDateDatepicker.setDate(LastDayOfMonth);
        }
        else if ($(this).val() == "AllTime") {
            $("#PayrollFilterStartDate").val("");
            $("#PayrollFilterEndDate").val("");
        }
        else if ($(this).val() == "Last90Days") {
            var date = new Date();
            var last90 = new Date(new Date().setDate(date.getDate() - 90));
            StartDateDatepicker.setDate(last90);
            EndDateDatepicker.setDate(date);
        }
        else if ($(this).val() == "Custom") {
            $("#PayrollFilterStartDate").val("");
            $("#PayrollFilterEndDate").val("");
        }
    });

    if (StartDateFilter == "NaN undefined, NaN") {
        StartDateFilter = "All Time";
        EndDateFilter = "";
    }
    console.log(StartDateFilter);
    if (StartDateFilter != "01/01/0001") {
        StartDateFilter = my_date_format(StartDateFilter);


    }
    else {
        StartDateFilter = "All Time"
        $("#PayrollFilterStartDate").val("");
    }
    if (EndDateFilter != "01/01/0001") {
        EndDateFilter = my_date_format(EndDateFilter);


    }
    else {
        EndDateFilter = "";
        $("#PayrollFilterEndDate").val("");
    }
    $(".DateFilterContents .date-start").html("");
    $(".DateFilterContents .date-end").html("");
    $(".DateFilterContents .date-start").html(StartDateFilter);
    $(".DateFilterContents .date-end").html(EndDateFilter);

})