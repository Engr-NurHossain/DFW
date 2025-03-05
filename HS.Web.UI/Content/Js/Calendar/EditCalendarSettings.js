var SaveSettings = function () {
    $.ajax(
        {
            type: "POST",
            url: "Calendar/EditSettings/",
            data: {
                Id: $("#Id").val(),
                SearchKey: $("#SearchKey").val(),
                Value: value,
                CompanyId: $("#CompanyId").val(),
                IsActive: IsActive,
                OptionalValue: OptionalValue,
                Description: Description
            },
            success: function () {
                $("#savesign").show();
                $("#savesign").fadeOut(3000);
            }
        });
}
$(document).ready(function () {
    $("#savesign").hide();
    $("#SaveSettings").click(function () {
        if (CheckBox == InputType) {
            var v = $("#IsCheckVal").prop('checked');
            value = v.toString();
            SaveSettings();
        }
        else if (NumberValue == InputType) {
            IsActive = $("#IsCheckVal").prop('checked');
            value = $("#Value").val();
            SaveSettings();
        }
        else if (TextBoxValue == InputType) {
            value = $("#ColorDisplay").val();
            SaveSettings();
        }
        else {

            if (searchkey == "ScheduleCalendarMinTimeRange") {
                var value1 = $("#Value").val();
                var splitvalue = value1.split(':');
                var fvalue = splitvalue[0] + splitvalue[1];
                if (parseInt(fvalue) > parseInt(maxtime)) {
                    OpenErrorMessageNew("Error!", "Min time should not be greater than max time", "");
                }
                else {
                    value = $("#Value").val();
                    SaveSettings();
                }
            }
            else if (searchkey == "ScheduleCalendarMaxTimeRange") {
                var value1 = $("#Value").val();
                var splitvalue = value1.split(':');
                var fvalue = splitvalue[0] + splitvalue[1];
                if (parseInt(mintime) > parseInt(fvalue)) {
                    OpenErrorMessageNew("Error!", "Max time should not be less than min time", "");
                }
                else {
                    value = $("#Value").val();
                    SaveSettings();
                }
            }
            else if (searchkey == "CustomCalendarTopRowEmployee") {
                var isBottome = $("#IsBottomVal").prop('checked');
                if (isBottome) { Description = "Yes"; }
                else { Description = "No";}
                IsActive = $("#IsCheckVal").prop('checked');
                OptionalValue = $("#OptionalValue").val();
                value = $("#Value").val();
                SaveSettings();
            }
            else if (searchkey == "CalendarEventTicketResize") {
                OptionalValue = String($("#IsCheckVal").prop('checked'));
                IsActive = true;
                value = $("#Value").val();
                SaveSettings();
            }

            else {
                value = $("#Value").val();
                SaveSettings();
            }
        }
    });
    $(".close").click(function () {
        window.location = domainurl + "/calendarsetup";
    });
});

window.addEventListener("load", startup, false);
var colorWell, defaultColor = $("#ColorDisplay").val();
function startup() {
    colorWell = document.querySelector("#ColorDisplay");
    colorWell.value = defaultColor;
    colorWell.addEventListener("input", updateFirst, false);
    colorWell.addEventListener("change", updateAll, false);
    colorWell.select();
}