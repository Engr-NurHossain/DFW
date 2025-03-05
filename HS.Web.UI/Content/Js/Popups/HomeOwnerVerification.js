var CheckHomeOwner = function () {

    url = domainurl + "/Customer/VerifyHomeOwner/";
    var param = {
        CustomerId: CustomerGuidId,
        FirstName: $("#FirstName").val(),
        LastName: $("#LastName").val(),
        Street: $("#Street").val(),
        ZipCode: $("#ZipCode").val(),
        City: $("#City").val(),
        State: $("#State").val()
    };
    // verify home owner  via AJAX call
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (json) {
            console.log(json);
            if (json.result == true) {
                OpenSuccessMessageNew("", "Owner found successfully.", function () {
                    $("#homeowner").val(json.OwnerName);
                })

            }
            else {

                OpenErrorMessageNew("", "No Owner found of this address.", function () {
                    $("#homeowner").val("");
                })
            }
        }
    });
   
}
