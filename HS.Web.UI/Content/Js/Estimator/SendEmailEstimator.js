
var SendEstimatorEmail = function (EstimatorId, subject) {

        var url = "/Estimator/SendEstimatorInEmail";
        var param = JSON.stringify({
            EstimatorId: EstimatorId,
            BodyContent: tinyMCE.get('Description').getContent(),
            subject: subject
        })

        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                parent.OpenSuccessMessageNew("Success!", "Email Sent Successfully.", function () {
                    parent.ClosePopup(); 
                });
                //window.location.reload();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $(".loader-div").hide();
                console.log(errorThrown);
            }
        })
}

var SendEstimatorSMS = function (EstimatorId) {
    
    var Number = $(".ContactNumber").val()
    var url = "/SMS/SendEstimatorText";
    var  sms = $("#SMSDescription").val();
    var param = JSON.stringify({
        CustomerId: CustomerGuid,
        EstimatorId: EstimatorId,
        ContactNumber: Number,
        Message: sms
    });
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.result) {
                parent.OpenSuccessMessageNew("Success!", "SMS Sent Successfully.", function () {
                    parent.ClosePopup();
                });
            } else {
                parent.OpenErrorMessageNew("Error!", "SMS Can't Send.", function () {
                });
            }
        }
    });
}

$(document).ready(function () {
    $(".btnSaveAndClose").click(function () {
        var subject = $("#EmailSubject").val();
        SendEstimatorEmail(EstimatorId, subject);
    });

    $(".btnSendTextAndClose").click(function () {
        if ($(".ContactNumber").val().trim() == ""
        || $("#SMSDescription").val().trim() == "") {
            parent.OpenErrorMessageNew("Error!", "Contact number and SMS body can't be empty.");
        }
        else{
            SendEstimatorSMS(EstimatorId);
        }
        
    });
});