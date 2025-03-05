var SaveCityTax = function () {
    if (CommonUiValidation()) {
        $.ajax(
            {
                type: "POST",
                url: "CityTax/AddCityTax/",
                data: {
                    Id: $("#Id").val(),
                    City: $("#City").val(),
                    Country: $("#Country").val(),
                    State: $("#State").val(),
                    ZipCode: $("#ZipCode").val(),
                    Rate: $("#Rate").val(),
                    IsMonitoring: $("#Monitoring").prop("checked"),
                    IsEquipment: $("#Equipment").prop("checked"),
                    IsOther: $("#Other").prop("checked")
                },
                success: function () {
                    $(".close").trigger("click");
                    LoadCityTax(true);
                    //$('.inventory-popup').dialog('close');

                }
            }); 
    }
}
$(document).ready(function () {
    var value = $("#Monitoring").prop("checked");
    console.log("monitoring value", value);
    $("#SaveCityTax").click(function () {
        SaveCityTax();
    });

});