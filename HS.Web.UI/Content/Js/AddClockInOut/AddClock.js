var ClockInOutSave = function () { 
    var ClockAdd = {};
    ClockAdd.Id = $("#TimeClockId").val();
    ClockAdd.UserId = $("#UserId").val();
    ClockAdd.ClockInTime = $("#TimeClockInDatepicker").val();
    ClockAdd.ClockOutTime = $("#TimeClockOutDatepicker").val();
    ClockAdd.ClockInNote = $("#TimeClockInNote").val();
    ClockAdd.ClockOutNote = $("#TimeClockOutNote").val();
    ClockAdd.Intime = $(".timepicker1").val(),
    ClockAdd.Outtime = $(".timepicker2").val(),

    $.ajax({
        type: "POST",
        url: domainurl + "/TimeClockPto/AddEmployeesClock",
        data: '{ClockAdd: ' + JSON.stringify(ClockAdd) + '}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.result) {
                $("#vehicleId").val(response.Id);
                OpenSuccessMessageNew("Success!", response.message, function () {
                    var EmpLoadGuid = $("#UserId").val();
                    $(".employeetimeclock-load").load(domainurl + String.format("/TimeClockPto/EmployeeTimeClock/?UserId={0}", EmpLoadGuid));
                    //var EmpLoadId = $(".EmployeesTimeClockDiv .tab-list.active").attr('data-id');
                    //var EmpLoadGuid = $(".EmployeesTimeClockDiv .tab-list.active").attr('data-Guid');
                    //$("#customer_tab_" + EmpLoadId).html(LoaderDom);
                    //$("#customer_tab_" + EmpLoadId).load(String.format("/TimeClockPto/EmployeeTimeClock/?UserId={0}", EmpLoadGuid));
                    $(".close").click();
                });


            }
            else {
                OpenErrorMessageNew("Error!", response.message);
            }
            //window.location.reload();
        }
    });
}

$(document).ready(function () {
 
    //$('#timeSpend').timepicker({
    //    minuteStep: 1,
    //    template: 'modal',
    //    appendWidgetTo: 'body',
    //    showSeconds: true,
    //    showMeridian: false,
    //    defaultTime: false
    //});
    var pickerOut = new Pikaday(
        {
        
            field: $('#TimeClockOutDatepicker')[0],
            firstDay: 1,
            trigger: $('#TimeClockOutDatepickerCustom')[0],
            maxDate: new Date('2030-12-31'),
            format: 'MM/DD/YYYY',
            yearRange: [2017, 2030],
            bound: true,
            container: document.getElementById('container2')
           
        });
    var pickerIn = new Pikaday(
    {
        field: $('#TimeClockInDatepicker')[0],
        firstDay: 1,
        trigger: $('#TimeClockInDatepickerCustom')[0],
        maxDate: new Date('2030-12-31'),
        format: 'MM/DD/YYYY',
        yearRange: [2017, 2030],
        bound: true,
        container: document.getElementById('container1')
      
    });
   
  
    $("#Type").change(function () {
        if ($('#Type :selected').text() == "Clock Out") {
            $(".timeLabel").removeClass("hidden");
            $(".input-group.bootstrap-timepicker.timepicker").removeClass("hidden");
        }
        else {
            $(".timeLabel").addClass("hidden");
            $(".input-group.bootstrap-timepicker.timepicker").addClass("hidden");
        }
    });

    $('#btnSaveClock').click(function () {
        if (CommonUiValidation() && $("#UserId").val() != "00000000-0000-0000-0000-000000000000") {
            ClockInOutSave();
        }
        else {
            $("#UserId").attr("style", "border-color: red !important");
        }
    });
    $("#UserId").change(function () {
        if ($("#UserId").val() != "00000000-0000-0000-0000-000000000000") {
            $("#UserId").attr("style", "border-color: #ccc !important");
        }
        else {
            $("#UserId").attr("style", "border-color: red !important");
        }
    })
    var windowsHeight = window.innerHeight - 102;
    $(".container_add_clock").height(windowsHeight);
    //if (typeof (validtimeclock) != "undefined" && validtimeclock != null && validtimeclock != "" && parseInt(validtimeclock) > 0) {
    //    console.log(validtimeclock);
    //    pickerOut.setMinDate(pickerOut.getDate());
    //    pickerIn.setMinDate(pickerIn.getDate());
    //}
});
