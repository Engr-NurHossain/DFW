var scheduleDate;
var todaydate = new Date();
var month = todaydate.getMonth() + 1;
var day = todaydate.getDate();
var date = (month < 10 ? '0' : '') + month + "/" + (day < 10 ? '0' : '') + day + "/" + todaydate.getFullYear();
var SaveTechSchedule = function () {
    if (CommonUiValidation()) {
        if ($("#InstallDate").val() != "") {
            var schdate = $("#InstallDate").val()
            scheduleDate = schdate + ' ' + "00:00";
        }
        var url = domainurl + "/TechSchedule/AddTechSchedule";
        console.log('cid' + $("#CustomerIdVal").val());
        var param = JSON.stringify({
            Id: $("#Id").val(),
            CustomerId: $("#CustomerIdVal").val(),
            InstallDate: scheduleDate,
            CompanyId: $("#CompanyIdVal").val(),
            EmployeeId: $("#FirstName").val(),
            ArrivalTime: $("#ArrivalTime").val(),
            DepartureTime: $("#DepartureTime").val(),
            EstimatedArrival: $("#EstimatedArrival").val(),
            FirstName: $("#FirstName").val(),
            IsSchedule: $("#IsSchedule").val()
        });

        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result == true) {
                    $('.close').trigger('click');
                    setTimeout(function () {
                        if (data.schedule != "true") {
                            OpenScheduleTab();
                        }
                    }, 600);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
        
    }
}
$(document).ready(function () {
    console.log($("#CustomerIdVal").val());
    //$('#InstallDate').keypress(function (e) {
    //    if (e.which == 13) {
    //        SaveTechSchedule();
    //    }
    //});
    
    $("#SaveScheduleTech").click(function () {
        var ArriveTime = $("#ArrivalTime").val();
        var DepartTime = $("#DepartureTime").val();
        var Dinstall = $("#InstallDate").val();
        var isValid = true;
        
        $('#InstallDate').each(function () {
                if ($.trim($(this).val()) == '') {
                    isValid = false;
                    $(this).css({
                        "border": "1px solid red",
                    });
                }
                else {
                    $(this).css({
                        "border": "",
                    });
                }
        });
        $('#ArrivalTime,#DepartureTime').each(function () {
            if ($.trim($(this).val()) == -1) {
                isValid = false;
                $(this).css({
                    "border": "1px solid red",
                });
            }
            else {
                $(this).css({
                    "border": "",
                });
            }
        });
        if (isValid == false) 
            e.preventDefault();
        else if (Dinstall == null) {
            $("#ModalErrorMessage").hide();
           
        }
        else if (ArriveTime == -1) {
            $("#ModalErrorMessage").hide();
            
        }
        else if (ArriveTime == -1) {
            $("#ModalErrorMessage").hide();
            
        }
        else if (ArriveTime == DepartTime) {
            OpenErrorMessageNew("Error!", "Arrival & Departure Time shouldn't be the same, Must be one is greater than the other.");
        }
        else if (DepartTime < ArriveTime) {
            OpenErrorMessageNew("Error!", "Arrival & Departure Time shouldn't be the same, Must be one is greater than the other.");
        }
        else {
            if ($("#InstallDate").val() >= date && $("#CustomerIdVal").val() != "") {
                SaveTechSchedule();
            }
            else if ($("#InstallDate").val() >= date && $("#CustomerIdVal").val() == "") {
                OpenErrorMessageNew("Error!", "Customer field is required", "");
            }
            else {
                OpenErrorMessageNew("Error!", "Schedule date should be greater than today's date.", "");
            }
        }
    });
});
