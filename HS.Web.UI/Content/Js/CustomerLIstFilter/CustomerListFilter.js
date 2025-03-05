var selectedID = [];
var CustomerGuidID;
var ClosePopup = function () {
    $.magnificPopup.close();
}
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    $("#OpenSuccess").click();
}

//var CustomerViewInsert = function (valueid) {
//    var url = domainurl + "/Customer/CustomerView";
//    //var valueid = $(".customer-content-list").attr('idval');
//    var param = {
//        cusid: valueid
//    }
//    $.ajax({
//        type: "POST",
//        ajaxStart: $(".loader-div").show(),
//        url: url,
//        data: JSON.stringify(param),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        cache: false,
//        success: function (data) {

//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            console.log(errorThrown);
//        }
//    })
//}

var DeleteCustomerById = function (customerid) {
    $.ajax({
        url: domainurl + "/Customer/DeleteCustomer",
        data: { Id: customerid },
        type: "Post",
        dataType: "Json",
        success: function (data) {
            if (data.result) {
                OpenSuccessMessageNew("Success!", "Customer deleted successfully!");
                LoadcustomerList();
            } else {
                OpenErrorMessageNew("Error!", data.message);
            }
        }
    });
}
var ShowCustomerDetail = function (item, e) {
    e.preventDefault();
    if (cntrlIsPressed) {
        var href = $(item).attr('href');
        window.open(href, '_blank');
    } else {
        var id = $(item).attr('id');
        var CusID = $(item).attr('id-val');
        //CustomerViewInsert(CusID);
        LoadCustomerDetail(id);
        $(".GlobalSearchInp").val("");
    }
}


$(document).ready(function () {
    $('.customer-name-click-anchor-style').click(function () {
        var id = $(this).attr('id');
        var CusID = $(this).attr('id-val');
        //CustomerViewInsert(CusID);
    });
    $(".btnSmartLeadSetup").click(function () {
        var valid = $(this).attr('data-id');
        LoadSmartLeadSetup(valid, true)
    })
    var idlist = [{ id: ".MapManufacturerMagnific", type: 'iframe', width: 920, height: 520 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $(".ListViewLoader").hide();
    //$("#successmessageClose").click(function () {
    //    setTimeout(function () {
    //        $(".ListContents").load("/Customer/CustomersListPartial");
    //    }, 200);
    //});
    
    //$('#tblCustomerList tbody').on('click', 'tr', function () {
    //    if ($(this).hasClass('selected')) {
    //        $(this).removeClass('selected');
    //    }
    //    else {
    //        tableCustomerList.$('tr.selected').removeClass('selected');
    //        $(this).addClass('selected');
    //    }
    //});
    //$("#srch-term").keyup(function () {
    //    $("#tblCustomerList_filter input").val($("#srch-term").val());
    //    $("#tblCustomerList_filter input").trigger('keyup');
    //});

    //setTimeout(function () {
    //    $(".ListContents").slideDown();
    //}, 500);


    /*$('.customer-name-click-anchor-style').click(function () {
        var id = $(this).attr('id');
        var CusID = $(this).attr('id-val');
        //CustomerViewInsert(CusID);
        LoadCustomerDetail(id);
       
    });*/

    $(".addressMapPopup").click(function () {
        CustomerGuidID = $(this).attr('data-id');
        var mapLoadUrl = domainurl + "/Customer/CustomerAddressMap?CustomerId=" + CustomerGuidID;
        $(".MapManufacturerMagnific").attr("href", mapLoadUrl);
        $(".MapManufacturerMagnific").click();
    });
    $(".CustomerDelete").click(function () {
        var CustomerDeleteid = $(this).attr('data-id');
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this customer?", function () {
            DeleteCustomerById(CustomerDeleteid)
        });
    });
});



var pageno = '@ViewBag.PageNumber';


var ShowCustomerDetail = function (item, e) {
    console.log("hlw");
    e.preventDefault();
    if (cntrlIsPressed) {
        var href = $(item).attr('href');
        window.open(href, '_blank');
    } else {
        var id = $(item).attr('id');
        var CusID = $(item).attr('id-val');
        //CustomerViewInsert(CusID);
        LoadCustomerDetail(id);
        $(".GlobalSearchInp").val("");
    }
}
$(document).ready(function () {
    $("#SelectAll").change(function () {
        if ($(this).is(':checked')) {
            $(".CheckItems").each(function () {
                $(this).prop('checked', true);
            });
        } else {
            $(".CheckItems").each(function () {
                $(this).prop('checked', false);
            });
        }
    });
    $("#SelectAllIds").change(function () {
        if ($(this).is(':checked')) {
            $(".CheckItems").each(function () {
                $(this).prop('checked', true);
            });
        } else {
            $(".CheckItems").each(function () {
                $(this).prop('checked', false);
            });
        }
    });



    $(".btnSmartLeadSetup").click(function () {
        var valid = $(this).attr('data-id');
        LoadSmartLeadSetup(valid, true)
    })
    $(".btnLeadSetup").click(function () {
        var valid = $(this).attr('data-id');
        LoadLeadSetup(valid, true)
    })
    $("#IsCheckVal1").change(function () {
        console.log("hlw");
        if ($(this).is(':checked')) {
            $(".CheckItemsCustomer").each(function () {
                $(this).prop('checked', true);
            });
            $(".CheckItems").each(function () {
                $(this).prop('checked', true);
            });
        }
        else {
            $(".CheckItemsCustomer").each(function () {
                $(this).prop('checked', false);
            });
            $(".CheckItems").each(function () {
                $(this).prop('checked', false);
            });
        }
    });
    $(".icon_sort_customer").click(function () {
        var orderval = $(this).attr('data-val');
        SortByCol = orderval;
        CustomerSearchKeyUp(pageno);
    });
    //$('[data-toggle="tooltip"]').tooltip();
    $(".unpaidamount_click").click(function () {
        var idval = $(this).attr('data-id');
        window.location.href = domainurl + '/Customer/CustomerDetail/?id=' + idval + "#InvoiceTab";
    })
    parent.$(".spnCustomerCount").html(TotalPageCount);
});
