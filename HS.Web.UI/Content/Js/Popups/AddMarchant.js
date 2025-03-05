var SaveMarchant = function () {
    var url = domainurl + "/App/AddMarchant/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        Name: $("#Name").val(),
        OrderBy: $("#OrderBy").val()
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
            if (data == true) {
                parent.ClosePopupGiveSuccess();
            } else {
                parent.ClosePopupGiveError();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
$(document).ready(function () {
    $("#SaveManufacturer").click(function () {
        if (CommonUiValidation()) {
            SaveMarchant();
        } 
    });
    $("#Cancel").click(function () {
        parent.ClosePopup();
    });
});