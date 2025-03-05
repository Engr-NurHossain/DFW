var DataTablePageSize = 50;
var selectedID = [];
var CustomerGuidID;
var ColumnName = "";
var colarr = [];
var ClosePopup = function () {
    $.magnificPopup.close();
}
var ClosePopupGiveError = function () {
    $.magnificPopup.close();
    $("#OpenError").click();
}
var ClosePopupGiveSuccess = function () {
    $.magnificPopup.close();
    if (location.href.toLowerCase().indexOf("/recruitment") > -1) {
        LoadRecruit(true);
    }

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

$(document).ready(function () {
    
    var idlist = [{ id: ".MapManufacturerMagnific", type: 'iframe', width: 920, height: 520 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    $(".ListViewLoader").hide();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Customer/CustomersListPartial");
        }, 200);
    });
    var customerreportpopwinowwith = 600;
    var customerreportpopwinowheight = 510;
    var customerprintpopwinowwith = 920;
    var customerprintpopwinowheight = 600;

    if (Device.MobileGadget()) {
        customerreportpopwinowwith = window.innerWidth;
        customerreportpopwinowheight = window.innerHeight;
        customerprintpopwinowwith = window.innerWidth;
        customerprintpopwinowheight = window.innerHeight;
    }
    var idlist = [{ id: ".ExportCustomerReport", type: 'iframe', width: customerreportpopwinowwith, height: customerreportpopwinowheight },
        { id: ".customerlistprint", type: 'iframe', width: customerprintpopwinowwith, height: customerprintpopwinowheight }

    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    var tableCustomerList = $('#tblCustomerList').DataTable({
        "pageLength": DataTablePageSize,
        "destroy": true,
        "scrollY": "290px",
        "scrollCollapse": true,
        "language": {
            "emptyTable": "No data available"
        }
        
    });

    $('#tblCustomerList tbody').on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            tableCustomerList.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });
    $("#srch-term").keyup(function () {
        $("#tblCustomerList_filter input").val($("#srch-term").val());
        $("#tblCustomerList_filter input").trigger('keyup');
    });

    $("#AddNewCustomerList").click(function () {
        OpenTopToBottomModal(domainurl + "/Customer/AddCustomer");
        /*$(".addManufacturerMagnific").click();*/
        $(".NewCustomer-search").removeClass('hidden');
    });
    $(".btnAddNewCustomer").click(function () {
        OpenTopToBottomModal(domainurl + "/Customer/AddCustomer");
        $(".NewCustomer-search").addClass('hidden');
    })
    setTimeout(function () {
        $(".ListContents").slideDown();
    }, 500);

    
    //$('.customer-name-click-anchor-style').click(function () {
    //    var id = $(this).attr('id');
    //    var CusID = $(this).attr('id-val');
    //    CustomerViewInsert(CusID);
    //});
    //$(".dropbtn").click(function () {
        
    //})
    $(".customer-content-list").load(domainurl + "/Customer/CustomerViewList/?recent=true");
    $(".Delete-Customer").click(function () {
        var checkboxs = $('.checkbox-custom');
        for (var i = 0; i < checkboxs.length; i++) {
            if ($(checkboxs[i]).is(":checked")) {
                selectedID.push(parseInt($(checkboxs[i]).attr('idval')));
            }
        }
        if (selectedID.length > 0) {
            OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteCustomer);
        }

        else {
            OpenErrorMessageNew("Error!", "Please select at least one customer")
        }
    });

    $(".Edit-Customer").click(function () {

        var checkboxs = $('.checkbox-custom');
        for (var i = 0; i < checkboxs.length; i++) {
            if ($(checkboxs[i]).is(":checked")) {
                selectedID.push(parseInt($(checkboxs[i]).attr('idval')));
            }
        }
         
        if (selectedID.length > 1) {
            OpenErrorMessageNew("Error!", "You can edit only one customer at a time,please select one customer.");
        }

        else if (selectedID.length <= 0) {
            OpenErrorMessageNew("Error!", "Please select at least one customer")
        }
        else {
            var id = selectedID[0];
            $(".addManufacturerMagnific").attr("href", domainurl + "/Customer/AddCustomer?id=" + id);
            $(".addManufacturerMagnific").click();
            parent.LoadcustomerList();
        }
    })
    $("#CustomerReport").click(function () {
        console.log("CustomerReport");
        ColumnName = "";
        //var checkboxs = $('.Export_excel_customerList');
        //var selectid = [];
        //for (var i = 0; i < checkboxs.length; i++) {
        //    selectid.push(parseInt($(checkboxs[i]).attr('data-id')));
        //}
        var ids = "";
        var idsAll = "";
        var flag = 0;
        console.log("fff");
        $(".CheckItems").each(function () {
            idsAll += $(this).attr("idval") + ",";
            if ($(this).is(':checked')) {
                flag = 1;
                ids += $(this).attr("idval") + ","
            }

        });
        $('.th-customer').each(function () {
            if ($(this).attr('data-info') != "" && $(this).attr('data-info') != undefined && $(this).attr('data-info') != null) {
                ColumnName += $(this).attr('data-info').trim() + "," + $(this).text().trim() + "-";
            }
        });
        $(".ExportCustomerReport").attr('href', domainurl + "/Reports/ExportConfirm/?ColumnName=" + ColumnName + "&ReportFor=Customer&Ids=" + ids + "&IdsAll" + idsAll);

        $(".ExportCustomerReport").click();
    });

    var DeleteCustomer = function () {
            
        for (var i = 0; i < selectedID.length; i++) {
            $.ajax({
                url: domainurl + "/Customer/DeleteCustomer",
                data: { id: selectedID[i] },
                type: "Post",
                dataType: "Json"
            })
        }
        parent.LoadcustomerList();
    }
    
    $(".addressMapPopup").click(function () {
        CustomerGuidID = $(this).attr('data-id');
        var mapLoadUrl = domainurl + "/Customer/CustomerAddressMap?CustomerId=" + CustomerGuidID;
        $(".MapManufacturerMagnific").attr("href", mapLoadUrl);
        $(".MapManufacturerMagnific").click();
    });
});
