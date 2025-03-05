var OthersKeyDown = function (item, event) {
    if (event.keyCode == 40) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).next('tr')).addClass('focusedItem');
    } else if (event.keyCode == 38) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).prev('tr')).addClass('focusedItem');
    }
}
var CloneSaveMassRestock = function () {
    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to clone this user?", function () {
        var TechnicianIdList = $("#CloneTechnicianId").val();
        TechnicianIdList.push(TechnicianIdr);
        var url = domainurl + "/Inventory/SaveMassRestockClone";
        var DetailList = [];
        var ValidattionPassed = true;
        $(".HasNewCount").each(function (e, item) {
            if ($(item).find('.txtNewCount').val() != 0 && $(item).find('.txtNewCount').val() != "") {
                DetailList.push({
                    EquipmentId: $(item).attr('data-eqpid'),
                    New: $(item).find('.txtNewCount').val(),
                    Quantity: $(item).attr('data-quantity'),
                });
            }
        });
        if (!ValidattionPassed) {
            OpenErrorMessageNew("Error!", "Please fill up all received quantities.");
            return 1;
        }
        var param = JSON.stringify({
            MassRestockList: DetailList,
            TechnicianIdList: TechnicianIdList
        });
        $.ajax({
            type: "POST",
            ajaxStart: $(".AddInvoiceLoader").removeClass('hidden'),
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                $(".AddInvoiceLoader").addClass('hidden');
                if (data.result) {
                    OpenSuccessMessageNew("Success", "", function () {
                        RestockDataLoad(TechnicianIdr, Idr);
                    });
                } else {
                    OpenErrorMessageNew("Error!", "");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $(".AddInvoiceLoader").addClass('hidden');
                OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
            }
        });
    });
}

function CheckMassRestock() {
    var url = domainurl + "/Inventory/CheckMassRestock";
    var DetailList = [];
    $(".HasNewCount").each(function (e, item) {
        if ($(item).find('.txtNewCount').val() != 0 && $(item).find('.txtNewCount').val() != "") {
            DetailList.push({
                EquipmentId: $(item).attr('data-eqpid'),
                TechnicianId: $(item).attr('data-techid'),
                New: $(item).find('.txtNewCount').val(),
                Quantity: $(item).attr('data-quantity'),
            });
        }
    });

    var param = JSON.stringify({
        MassRestockList: DetailList,
        TechnicianId: TechnicianIdr
    });

    $.ajax({
        type: "POST",
        ajaxStart: $(".AddInvoiceLoader").removeClass('hidden'),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $(".AddInvoiceLoader").addClass('hidden');
            if (data.result) {
                var array = data.MassRestockName.split(",");
                var dataitem = "<ol style='text-align: left;'>";
                $.each(array, function (i) {
                    dataitem += "<li>" + array[i]+"</li>"
                });
                dataitem += "</ol>"
                //OpenCautionMessageNew("Confirm?", "Below are the items already in the Transfer queue. To place a new request, either Accept or Decline the existing request. If you're adding request for items other than the equipment below, change the quantity for the below equipment to zero and then click Save." + dataitem, function () {


                //    SaveMassRestock(param)
                //});
                OpenCautionMessageNew('Duplicate Request', "Below are the items already in the Transfer queue. To place a new request, either Accept or Decline the existing request. If you're adding request for items other than the equipment below, change the quantity for the below equipment to zero and then click Save." + dataitem)
            } else {
                SaveMassRestock(param)
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".AddInvoiceLoader").addClass('hidden');
            OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
        }
    });
}

function checkalert()
{
    alert("Hello")
}

var SaveMassRestock = function (param) {
    var url = domainurl + "/Inventory/SaveMassRestock";

    var ValidattionPassed = true;
   
    if (!ValidattionPassed) {
        OpenErrorMessageNew("Error!", "Please fill up all received quantities.");
        return 1;
    }
   
    $.ajax({
        type: "POST",
        ajaxStart: $(".AddInvoiceLoader").removeClass('hidden'),
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            $(".AddInvoiceLoader").addClass('hidden');
            if (data.result) {
                OpenSuccessMessageNew("Success", data.message, function () {
                    RestockDataLoad(TechnicianIdr, Idr);
                });
            } else {
                OpenErrorMessageNew("Error!",data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".AddInvoiceLoader").addClass('hidden');
            OpenErrorMessageNew("Error!", "Internal error occured. Please try again later or contact system admin.");
        }
    });

}
var InitAddPurchaseOrder = function () {
    setTimeout(function () {
        var WindowHeight = window.innerHeight;
        var divHeight = WindowHeight - ($(".avb_div_header").height() + $(".invoice-footer").height() + 16);
        $(".PoContentScroll").css("height", divHeight);
    }, 1000);
}

$(document).ready(function () {
    //$("#CustomerMassRestockTab tbody").on('blur', 'tr', function (item) {
    //    if (typeof ($(item.target).parent().parent().attr('data-id')) == 'undefined') {
    //        var trdom = $(item.target).parent().parent();
    //        $(trdom).find("input.txtNewCount").val('');
    //        $(trdom).find("span.spnNewCount").text('');
    //    }
    //});
    var subtotal = 0;
    var sum = 0;

    $('.btnAddRequest').click(function () {
        this.disabled = true;
        setTimeout(function () {
            $('.btnAddRequest').attr('disabled', false);
        }, 10000);
    });

    $("#CustomerMassRestockTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass("spnNewCount")) {
            var quantityOnHand = $(e.target).parent().attr("data-Quantity");
            if (quantityOnHand != "0") {
                $("#CustomerMassRestockTab tr").removeClass("focusedItem");
                $($(e.target).parent().parent()).addClass("focusedItem");
                $(e.target).parent().find('input').focus();
            }
        } else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            var quantityOnHand = $(e.target).parent().attr("data-Quantity");
            if (quantityOnHand != "0") {
                $("#CustomerMassRestockTab tr").removeClass("focusedItem");
                $($(e.target).parent()).addClass("focusedItem");
                $(e.target).find('input').focus();
            }
        }
    });
    //$(".CustomerInvoiceTab tbody").on('change', "tr td .txtNewCount", function () {
    //    console.log("aise;");
    //    var NewCountDom = $(this).parent().find('span.spnNewCount');

    //    $('span.spnNewCount').each(function () {
    //        subtotal += Number($(this).val());
    //    });

    //    $(NewCountDom).text($(this).val());
    //    subtotal += Number($(this).val());
        
    //    $('.totalnew').text(subtotal);
    //});

    InitAddPurchaseOrder();


    $('.txtNewCount').change(function () {
        console.log("aise;");
        var subtotal = 0;
        var NewCountDom = $(this).parent().find('span.spnNewCount');


        $(NewCountDom).text($(this).val());
        $('.txtNewCount').each(function () {
            subtotal += Number($(this).val());
        });
        $('.totalnew').text(subtotal);

    })


})