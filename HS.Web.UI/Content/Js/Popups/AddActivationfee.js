var SaveActivationfee = function () {
    $.ajax(
        {
            type: "POST",
            url: "ActivationFee/AddActivationFee/",
            data: {
                Id: $("#Id").val(),
                //Name: $("#actName").val(),
                Fee: $("#Fee").val(),
                Name: $("#Name").val()

            },
            success: function () {
                //$('.inventory-popup').dialog('close');
                $("#Right-To-Left-Modal-Body .close").trigger("click");
                OpenSuccessMessageNew("Success", "Fee added successfully");
                $(".LoadActivation").load(domainurl + "/ActivationFee/ActivationFeePartial/");
            }
        });
}
$(document).ready(function () {
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    //$('#actName').keypress(function (e) {
    //    if (e.which == 13) {
    //        SaveActivationfee();
    //    }
    //});

    $("#SaveAccFee").click(function () {
        if (CommonUiValidation()) {
            var value = $("#Fee").val();
            if (!$.isNumeric(value)) {
                OpenRightToLeftModal();
                OpenErrorMessageNew("Error!", "Please Input Valid Number");
            }
            else {
                SaveActivationfee();
            }
        }
    });
});