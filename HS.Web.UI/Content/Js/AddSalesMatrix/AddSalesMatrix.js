
var SaveSalesMatrix = function () {
    var url = domainurl + "/Matrix/AddSalesMatrix/";
    var param = JSON.stringify({
        Id: $("#Id").val(),
        Type: $("#Type").val(),
        Min: $("#Min").val(),
        Max: $("#Max").val(),
        UserX: $("#UserX").val(),
        Difference: $("#Difference").val()
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
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })

    $(".close").trigger("click");
    LoadSalesMatrix(true);
}
$(document).ready(function () {
    $("#SaveSalesMatrix").click(function () {
        if (CommonUiValidation()) {
            SaveSalesMatrix();
        }
    });
});