var StartDateDatepicker;
var EndDateDatepicker;
var enddate;
var typeval = $("#Type").val();
var AddPto = function () {
    var Param = {
        Notes: $("#PTO_Notes").val(),
        StrEndDate: enddate,
        StrStartDate: $("#PTOStartDate").val(),
        TimeFrom: $("#addPTOTimeFrom").val(),
        TimeTo: $("#addPTOTimeTO").val(),
        Type: $("#Type").val(),
        Payable: $("#Payable").is(":checked"),
        UserId:$("#Employee").val()
    };
    var url = domainurl + "/TimeClockPto/AddPto";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", data.message, function () {
                    $(".close").trigger('click');
                });
                OpenEmployeesPtoTab();
            } else {
                if (data.warning)
                {
                    OpenCautionMessageNew("Error!", data.message, function () {
                        $(".close").trigger('click');
                    });
                }
                else {
                    OpenErrorMessageNew("Error!", data.message, function () {
                        $(".close").trigger('click');
                    });
                }
             
                OpenEmployeesPtoTab();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
//var CheckPtoRemainingHour = function (empId) {
//    var Param = {
//        EmployeeId: empId
//    };
//    var url = domainurl + "/TimeClockPto/CheckPtoRemainingHour";
//    $.ajax({
//        type: "POST",
//        ajaxStart: function () { },
//        url: url,
//        data: JSON.stringify(Param),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        cache: false,
//        success: function (data) {
//            if (data.result == true) {
//                $(".payable_div").removeClass("hidden");
//            }
//            else {
//                $(".payable_div").addClass("hidden");
//            }
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            console.log(errorThrown);
//        }
//    });
//}
var TimeValidation = function () {
    var result = true;
    console.log(typeval);
    if (typeval != "FullDay" && typeval != "HalfDay" && typeval != "CustomTime") {
        if (StartDateDatepicker.getDate() > EndDateDatepicker.getDate()) {
            $('#PTOStartDate').addClass("required");
            $('#PTOEndDate').addClass("required");
            result = false;
        } else {
            $('#PTOStartDate').removeClass("required");
            $('#PTOEndDate').removeClass("required");
        }
        enddate = $('#PTOEndDate').val();
    }
    else {
        enddate = $('#PTOStartDate').val();
    }
    //if ($("#addPTOTimeFrom").val() != -1 && $("#addPTOTimeTO").val() != -1) {
    //    if (new Date("05/30/2018" + " " + $("#addPTOTimeFrom").val()) >= new Date("05/30/2018" + " " + $("#addPTOTimeTO").val())) {
    //        $('#addPTOTimeFrom').addClass("required");
    //        $('#addPTOTimeTO').addClass("required");
    //        result = false;
    //    } else {
    //        $('#addPTOTimeFrom').removeClass("required");
    //        $('#addPTOTimeTO').removeClass("required");
    //    }
    //}
    return result;
}

$(document).ready(function () {
    setTimeout(function () {
        $(".AddPtoBody").height(window.innerHeight - 103);
    }, 100);
    StartDateDatepicker = new Pikaday({
        field: $('#PTOStartDate')[0],
        trigger: $('#StartDateCustom')[0],
        format: 'MM/DD/YYYY',
        firstDay: 1,
        disableDayFn: function (date) {
            var enabled_dates = ["05/14/2017"]; // dates I want to enabled.

            if (date.getDay() === 0 && $.inArray(moment(date).format("MM/DD/YYYY"), enabled_dates) === -1) {
                return date;
            }
        },
    });
    EndDateDatepicker = new Pikaday({
        field: $('#PTOEndDate')[0],
        trigger: $('#EndDateCustom')[0],
        format: 'MM/DD/YYYY',
        firstDay: 1,
        disableDayFn: function (date) {
            var enabled_dates = ["05/14/2017"]; // dates I want to enabled.

            if (date.getDay() === 0 && $.inArray(moment(date).format("MM/DD/YYYY"), enabled_dates) === -1) {
                return date;
            }
        },
    });

    $("#Employee").change(function () {
        console.log("sdfsdf");
        if ($("#Employee").val() == "-1") {
            $(".empSelect").removeClass("hidden");
        }
        else {
            CheckPtoRemainingHour($("#Employee").val());
            $(".empSelect").addClass("hidden");
        }
    })
    $("#btnAddAndSavePto").click(function () {
        if (TimeValidation()) {
            if ($("#Employee").val() != "-1")
            {
                AddPto();
            }
            else {
                $(".empSelect").removeClass("hidden");
            }
       
        }
    });
    $("#Type").change(function () {
        typeval = $(this).val();
        if ($(this).val() == "CustomTime") {
            enddate = $('#PTOEndDate').val();
            $(".endate").removeClass('hidden');
            $(".Custom_Time_div").removeClass("hidden");
            $(".endate").addClass('hidden');
        }
        else if ($(this).val() == "FullDay") {
            $(".endate").addClass('hidden');
            enddate = $('#PTOStartDate').val();
            $(".Custom_Time_div").addClass("hidden");
            $("#addPTOTimeFrom").val("-1");
            $("#addPTOTimeTO").val("-1");
        }
        else if ($(this).val() == "HalfDay") {
            $(".endate").addClass('hidden');
            enddate = $('#PTOStartDate').val();
            $(".Custom_Time_div").addClass("hidden");
            $("#addPTOTimeFrom").val("-1");
            $("#addPTOTimeTO").val("-1");
        }
        else if ($(this).val() == "MultipleDay") {
            $(".endate").removeClass('hidden');
            $(".Custom_Time_div").addClass("hidden");
            $("#addPTOTimeFrom").val("-1");
            $("#addPTOTimeTO").val("-1");
            enddate = $('#PTOEndDate').val();
        }
        else {
            $(".endate").removeClass('hidden');
            $(".Custom_Time_div").addClass("hidden");
            $("#addPTOTimeFrom").val("-1");
            $("#addPTOTimeTO").val("-1");
            enddate = $('#PTOEndDate').val();
        }
    });
});