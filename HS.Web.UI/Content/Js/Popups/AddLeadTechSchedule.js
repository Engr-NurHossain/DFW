var SaveLeadTechSchedule = function () {
    if (CommonUiValidation()) {

        var url = domainurl + "/Leads/AddLeadTechSchedule";
        console.log('cid' + $("#CustomerIdVal").val());
        var param = JSON.stringify({
            Id: $("#Id").val(),
            CustomerId: $("#LeadCustomerIdVal").val(),
            InstallDate: SchedulePicker.getDate(),
            CompanyId: $("#CompanyIdVal").val(),
            EmployeeId: $("#FirstName").val(),
            ArrivalTime: $("#ArrivalTime").val(),
            DepartureTime: $("#DepartureTime").val(),
            EstimatedArrival: $("#EstimatedArrival").val(),
            FirstName: $("#FirstName").val(),
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
                $('.close').trigger('click');
                setTimeout(function () {
                    
                }, 600);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })

    }
}
$(document).ready(function () {

    $("#SaveLeadScheduleTech").click(function () {
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
            SaveLeadTechSchedule();
        }
    });
});