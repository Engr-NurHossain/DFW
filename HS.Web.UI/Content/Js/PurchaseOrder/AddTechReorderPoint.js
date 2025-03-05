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
var CloneSaveTechReorderPoint = function () {
    OpenConfirmationMessageNew("Confirm?", "Are you sure you want to clone this user?", function () {
        var TechnicianIdList = [];
        TechnicianIdList.push($("#CloneTechnicianId").val());
        TechnicianIdList.push(TechnicianIdr);
        var url = domainurl + "/Inventory/SaveTechReorderPointClone";
        var DetailList = [];
        var ValidattionPassed = true;
        $(".HasNewCount").each(function (e, item) {
            if ($(item).find('.txtNewCount').val() != 0 && $(item).find('.txtNewCount').val() != "") {
                DetailList.push({
                    EquipmentId: $(item).attr('data-eqpid'),
                    ReorderPoint: $(item).find('.txtNewCount').val()
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
var SaveTechReorderPoint = function () {
    var url = domainurl + "/Inventory/SaveTechReorderPoint";
    var DetailList = [];
    var ValidattionPassed = true;
    $(".HasNewCount").each(function (e, item) {
        if ($(item).find('.txtNewCount').val() != 0 && $(item).find('.txtNewCount').val() != "") {
            DetailList.push({
                EquipmentId: $(item).attr('data-eqpid'),
                TechnicianId: $(item).attr('data-techid'),
                ReorderPoint: $(item).find('.txtNewCount').val()
            });
        }
    });
    if (!ValidattionPassed) {
        OpenErrorMessageNew("Error!", "Please fill up all received quantities.");
        return 1;
    }
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

}
var InitAddPurchaseOrder = function () {
    setTimeout(function () {
        var WindowHeight = window.innerHeight;
        var divHeight = WindowHeight - ($(".avb_div_header").height() + $(".invoice-footer").height() + 16);
        $(".PoContentScroll").css("height", divHeight);
    }, 1000);
}

$(document).ready(function () {
    $("#CustomerMassRestockTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass("spnNewCount")) {
            $("#CustomerMassRestockTab tr").removeClass("focusedItem");
            $($(e.target).parent().parent()).addClass("focusedItem");
            $(e.target).parent().find('input').focus();
        } else if (e.target.tagName.toUpperCase() == 'INPUT') {
            return;
        }
        else {
            $("#CustomerMassRestockTab tr").removeClass("focusedItem");
            $($(e.target).parent()).addClass("focusedItem");
            $(e.target).find('input').focus();
        }
    });
    $(".CustomerInvoiceTab tbody").on('change', "tr td .txtNewCount", function () {
        var NewCountDom = $(this).parent().find('span.spnNewCount');
        $(NewCountDom).text($(this).val());
    });
    InitAddPurchaseOrder();
})