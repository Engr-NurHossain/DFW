
var SaveCompany= function () {
    if (CommonUiValidation()) {
        $.ajax(
            {
                type: "POST",
                url: "Company/AddCompany/",
                data: {
                    Id: $("#Id").val(),
                    CompanyName: $("#comName").val(),
                    UserName: $("#comUser").val(),
                    EmailAdress: $("#email").val(),
                    Phone: $("#phone").val(),
                    Address: $("#address").val(),
                    Website: $("#web").val(),
                    
                },
                success: function () {
                    //$('.inventory-popup').dialog('close');
                    $(".close").trigger("click");
                    LoadEditCompany(true);
                }
            });

    }
}
$(document).ready(function () {
    $('#comName').keypress(function (e) {
        if (e.which == 13) {
            SaveCompany();
        }
    });

    $("#savecompany").click(function () {
        SaveCompany();
    });
    //var value = $("#InHouse").prop("checked");

    //if (value == true) {
    //    $("#InHouse").prop('checked', true);
    //}
    //else {
    //    $("#InHouse").prop('checked', false);
    //}


});