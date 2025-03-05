var SaveServicefee = function () {
    $.ajax(
        {
            type: "POST",
            url: "ServiceFee/AddServiceFee/",
            data: {
                Id: $("#Id").val(),
                //Name: $("#actName").val(),
                Fee: parseFloat($("#Fee").val()).toFixed(2),
                Name: $("#Name").val()
            },
            success: function () {
                //$('.inventory-popup').dialog('close');
                $(".close").trigger("click");
                OpenSuccessMessageNew("Success", "Service fee added successfully");
                $(".LoadService").load(domainurl + "/ServiceFee/ServiceFeePartial/");
            }
        });
}
$(document).ready(function () {
    //$("#serFee").keyup(function () {

    //    $("#txtName").val($(this).val());
    //});
    //$('#serFee').keypress(function (e) {
    //    if (e.which == 13) {
    //        SaveServicefee();
    //    }
    //});
    parent.$('.close').click(function () {
        parent.$(".modal-body").html('');
    })
    $("#SaveserFee").click(function () {


        //if (CommonUiValidation()) {
        //    var value = $("#Fee").val();
        //    if (!$.isNumeric(value)) {
        //        OpenRightToLeftModal();
        //        OpenConfirmationMessage("Warning", "Please Input Valid Number");
        //    }
        //    else {
        //        SaveActivationfee();
        //    }
        //}


        if (CommonUiValidation()) {
            var value = parseFloat($("#Fee").val());
            console.log("Parse float fee : " + value);
            if (!$.isNumeric(value)) {
                OpenRightToLeftModal();
                OpenErrorMessageNew("Error!", "Please Input Valid Number");
            }
            else {
                SaveServicefee();
            }
        }
    });
});