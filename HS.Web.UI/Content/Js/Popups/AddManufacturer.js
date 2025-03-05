var SaveManufacturer = function () {
    var url = domainurl + "/App/AddManufacturer/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        Name: $("#Name").val(),
        ManufacturerId: $("#ManufacturerId").val()
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
            if (typeof (LoadManufacturerDropdownAfterAdding) != "undefined" && data.result) {
                LoadManufacturerDropdownAfterAdding(data.ManufacturerId, data.ManufacturerName)
            }
            else {
                if (data.result == true) {
                    OpenSuccessMessageNew("Success!", "Manufacturer Saved Successfully!", ReloadManufacturersList);
                } else {
                    OpenErrorMessageNew("Error!", "Something wrong Manufacturer not Saved!", ReloadManufacturersList);
                }
            }            
        },
        error: function (jqXHR, textStatus, errorThrown) { 
            console.log(errorThrown);
        }
    });
    $("#Right-To-Left-Modal-Body .close").trigger("click");
}
var ReloadManufacturersList = function () {
    $(".ListContents").load(domainurl + "/App/ManufaturersListPartial");
}
$(document).ready(function () {
    $("#SaveManufacturer").click(function () {
        if (CommonUiValidation()) {
            SaveManufacturer();
        } 
    });
     
});