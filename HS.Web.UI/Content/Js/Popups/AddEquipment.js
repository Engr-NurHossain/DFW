var SaveCustomer = function () {
    var url = domainurl + "/Customer/AddCustomer/";
    var param = JSON.stringify({
        Note: $("#Note").val()
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
    $("#SaveCustomer").click(function () {
        SaveCustomer();
        parent.LoadcustomerList();
       
    });
    $("#Cancel").click(function () {
        parent.ClosePopup();
    });
});