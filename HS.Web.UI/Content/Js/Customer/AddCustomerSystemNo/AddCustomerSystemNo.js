var GenerateNumber = function () {

    var prefixVal = $("#Prefix").val();
    var StartingNumberVal = parseInt($("#StartingNumber").val());
    var TotalNumberVal = parseInt($("#TotalNumber").val());


    var url = domainurl + "/Customer/AddCustomerSystemNo";
    var param = JSON.stringify({
        Prefix: prefixVal,
        StartingNumber: StartingNumberVal,
        TotalNumber: TotalNumberVal
    });
    $(".LoaderWorkingDiv1").show();
    $.ajax({
        type: "POST",
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $(".LoaderWorkingDiv1").hide();
            if (data != '') {
                $(".resultDiv").html(data);
                parent.OpenSuccessMessageNew("Success!", "")
            }
            else
            {
                parent.OpenErrorMessageNew("Success!", "Required all fields!")
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
            parent.OpenErrorMessageNew("Error!", data.erroResult);
        }
    })
}


$(document).ready(function () {
    $(".generate_system_no_inner_height").height(window.innerHeight - 107);
    $(".LoaderWorkingDiv1").hide();
    $(".btn-genrate-number").click(function () {
        GenerateNumber();
    });
    $(window).resize(function () {
        $(".generate_system_no_inner_height").height(window.innerHeight - 107);
    })
});