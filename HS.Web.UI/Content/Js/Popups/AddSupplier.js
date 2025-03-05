var SaveSupplier = function () {

    var Currenturl = window.location.href;
    var idsupplier = $("#id").val();
    console.log(Currenturl);
    console.log(idsupplier);
    var url = domainurl + "/Supplier/AddSupplier/";
    var param = JSON.stringify({
        id: $("#id").val(),
        Name: $("#Name").val(),
        EmailAddress: $("#EmailAddress").val(),
        Phone: $("#Phone").val(),
        SupplierId: $("#Supplier_SupplierId").val(),
        SalesRepName: $("#SalesRepName").val(),
        ContactPerson: '00000000-0000-0000-0000-000000000000',
        Zipcode: $("#ZipCode").val(),
        Type: $("#Type").val(),
        Country: $("#Country").val(),
        Street: $("#Street").val(),
        City: $("#City").val(),
        State: $("#State").val(),
        Note: $("#Note").val(),
        CompanyName: $("#CompanyName").val().trim(),
        TaxId: $("#TaxId").val(),
        Website: $("#Website").val()
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
            if (data.result == true) {
                $('.close').trigger('click'); 

                if (typeof (LoadSupplierDropdownAfterAdding) != 'undefined'
                    && data.SupplierId != null
                    && data.SupplierName != null) {
                    LoadSupplierDropdownAfterAdding(data.SupplierId,data.SupplierName);
                }
                else {
                    OpenSuccessMessageNew("Success!", data.message, function () {
                        setTimeout(function () {

                            if (Currenturl.includes("SupplierDetail")) {
                                console.log("working");
                                LoadSupplierDetails(idsupplier, true);
                            }
                            else {
                                $("#expense_vendor_list").load(domainurl + "/Supplier/SupplierPertial");
                            }

                        }, 600);
                    }); 
                }
                
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
function FormatCellNumber(cvalue) {
    var ValueCleanCell = "";
    if (cvalue != undefined && cvalue != "" && cvalue != null) {
        cvalue = cvalue.replace(/[-  ]/g, '');
        if (cvalue.length == 10) {
            ValueCleanCell = cvalue.replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, "$1-$2-$3");
            $("#Phone").css({ "border": "1px solid #babec5" });
        }
        else if (cvalue.length > 10) {
            ValueCleanCell = cvalue;
            $("#Phone").css({ "border": "1px solid red" });
        }
        else {
            $("#Phone").css({ "border": "1px solid red" });
            ValueCleanCell = cvalue;
        }
    }
    return ValueCleanCell;
}
$(document).ready(function () {
    var SupplierHeight = window.innerHeight - 105;
    $(".add_supplier_contents_div").height(SupplierHeight);
    $(".add_supplier_contents_div").css("overflow-y","auto");
    $("#SaveSupplier").click(function () {
        if ($("#CompanyName").val() != "") {
            SaveSupplier();
        }
        else {
            OpenErrorMessageNew("Error!", "Company Name field is required.", "");
        }
    });
    $("#Cancel").click(function () {
        parent.ClosePopup();
    });
    $("#Phone").keyup(function () {
        var cPhoneNumber = $(this).val();
        if (cPhoneNumber != undefined && cPhoneNumber != null && cPhoneNumber != "") {
            var ccleanPhoneNumber = FormatCellNumber(cPhoneNumber);
            $(this).val(ccleanPhoneNumber);
        }
    });
});