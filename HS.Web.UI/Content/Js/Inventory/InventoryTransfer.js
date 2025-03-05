//blockstart:: Common 
var target = "";

var SaveTabState = function () {
    target = sessionStorage.getItem("CurrentTab");
}

var BindTabState = function () {
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        target = $(e.target).attr("data-target"); // activated tab
        sessionStorage.setItem("CurrentTab", target);
        //console.log(target);
    });
}

var RestoreTabState = function () {
    if (target == '#TechReceivedTab') { $("a[data-target='#TechReceivedTab']").click(); }
}


$("#btnDownloadTTReport").click(function () {
    DataListDownload(1, 1, null, $("#TechTrfList").val(), $("#TechRcvList").val(), true);
});


//blockend:: Common 

//Tech Transfer 
var InventoryTechReceiveConfirm = function (id, eqpid, techid, qty, ReqSrc, page='TT') {
    var url = '/Inventory/TechReceiveConfirm';
    $('.rcv').hide();
    $('.dec').hide();
    $(".LoaderWorkingDiv").show()
    var param = JSON.stringify({
        id: id,
        eqpid: eqpid,
        techid: techid,
        qty: qty,
        reqSrc: ReqSrc
    });
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: param,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                //OpenSuccessMessageNew("Success", data.message);
                OpenSuccessToast("Success", data.message)
            }
            else {
                OpenErrorMessageNew("Error", data.message);
            }
            if (page == 'TL')
                TrfLogListLoad(1, 1, null);
            else
                TrfListLoad(1, 1, null);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        },
        complete: function (data) {
            $(".LoaderWorkingDiv").hide()
            $('.rcv').show();
            $('.dec').show();
        }
    });
}
var RecevierTabLoad = function (pageNoTrf, pageNoRcv, order, techTrfFrom, techTrfTo, techRCVFrom, techRCVTo, Status) {
    
    if (typeof (pageNoTrf) != "undefined") {
        var setectSts = encodeURI($("#poStatus").val());
        EstimatorId = $("#EstimatorId").val();
        StartDate = $(".min-date").val();
        EndDate = $(".max-date").val();
        //console.log(techRCVFrom);
        //var url = '/Inventory/TechTransferLogListPartialPost';
        var url = '/Inventory/InventoryReceiveListPartial';
        $(".LoaderWorkingDiv").show()
        var param = JSON.stringify({
            SearchText: encodeURI($(".PoSearchText").val()),
            selectsts: setectSts,
            PageNoTrf: pageNoTrf,
            PageNoRcv: pageNoRcv,
            order: order,
            Start: encodeURI(StartDate),
            End: encodeURI(EndDate),
            TFEmployeeIds: techTrfFrom,
            TTEmployeeIds: techTrfTo,
            RFEmployeeIds: techRCVFrom,
            RTEmployeeIds: techRCVTo,
            EstimatorId: EstimatorId,
            status: Status
        });
        //console.log(param);
        $.ajax({
            type: "POST",
            ajaxStart: function () { },
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            cache: false,
            success: function (data) {
                /*$(".PurchaseOrderTable").html(data); */
                $(".recieverTab_Load").html(data);
                $(".tranferTab_Load").html('');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            },
            complete: function (data) {
                $(".LoaderWorkingDiv").hide()
                $("#TechRcvFromList").selectpicker('val', selectedTechsRcvFromList);
                $("#TechRcvToList").selectpicker('val', selectedTechsRcvToList); 
            }
        });
    }
}
var InventoryTechReceiveDecline = function (id, eqpid, techid, qty, page = 'TT') {
    OpenConfirmationMessageNew("Confirmation", "Are you sure, you want to decline this item?", function () {
        var url = '/Inventory/InventoryTechReceiveDecline';
        var param = JSON.stringify({
            id: id
        });
        $.ajax({
            type: "POST",
            ajaxStart: function () { },
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result) {
                    OpenSuccessMessageNew("Success", data.message);
                    if (page == 'TL')
                        TrfLogListLoad(1, 1, null);
                    else
                        TrfListLoad(1, 1, null);
                }
                else {
                    OpenErrorMessageNew("Error", data.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    });
}

var InventoryTechApproveConfirm = function (eqpid, techid, qty, tqty) {
    OpenRightToLeftModal("/Inventory/AssignInventoryTechApprove?eqpid=" + eqpid + "&techid=" + techid + "&qty=" + qty + "&tqty=" + tqty);
}

var RenderTechFilterDDTT = function () {
    $("#TechTrfList").attr("data-live-search", "true");
    $("#TechTrfList").attr("data-selected-text-format", "count");
    $("#TechTrfList").attr("data-actions-box", "true");
    $("#TechTrfList").selectpicker('val', selectedTechsTrfList);

    $("#TechRcvList").attr("data-live-search", "true");
    $("#TechRcvList").attr("data-selected-text-format", "count");
    $("#TechRcvList").attr("data-actions-box", "true");
    $("#TechRcvList").selectpicker('val', selectedTechsRcvList);

    var techTrf = $('span.filter-option.pull-left').eq(0).text();
    var techRcv = $('span.filter-option.pull-left').eq(1).text();
    /*console.log(techTrf);*/
    techTrf = techTrf.replace('items', 'Technicians(Transfer To)');
    $('span.filter-option.pull-left').eq(0).text(techTrf);
    //console.log(techTrf);
    techRcv = techRcv.replace('items', 'Technicians(Transfer From)');
    $('span.filter-option.pull-left').eq(1).text(techRcv);
}

var BindTechFilterDDTT = function () {
    $("#TechTrfList").change(function () {

        if ($("#TechTrfList").is(":visible")) {
            //console.log($("#EqList").val());

        }
        console.log('Trf');
        //console.log($("#TechTrfList").val(), $("#TechRcvList").val());
        if ($("#TechTrfList").val() != null) {
            TrfListLoad(1, 1, null, $("#TechTrfList").val(), $("#TechRcvList").val());
        }

    });

    $("#TechRcvList").change(function () {

        if ($("#TechRcvList").is(":visible")) {
            //console.log($("#EqList").val());

        }
        //console.log($("#TechTrfList").val(), $("#TechRcvList").val());
        if ($("#TechRcvList").val() != null) {
            TrfListLoad(1, 1, null, $("#TechTrfList").val(), $("#TechRcvList").val());
        }

    });
}



//Transfer Log

var RenderTechFilterDDTL = function () {
    $("#TechTrfFromList").attr("data-live-search", "true");
    $("#TechTrfFromList").attr("data-selected-text-format", "count");
    $("#TechTrfFromList").attr("data-actions-box", "true");
    $("#TechTrfFromList").selectpicker('val', selectedTechsTrfFromList);

    $("#TechTrfToList").attr("data-live-search", "true");
    $("#TechTrfToList").attr("data-selected-text-format", "count");
    $("#TechTrfToList").attr("data-actions-box", "true");
    $("#TechTrfToList").selectpicker('val', selectedTechsTrfToList);

    $("#TechRcvFromList").attr("data-live-search", "true");
    $("#TechRcvFromList").attr("data-selected-text-format", "count");
    $("#TechRcvFromList").attr("data-actions-box", "true");
    $("#TechRcvFromList").selectpicker('val', selectedTechsRcvFromList);

    $("#TechRcvToList").attr("data-live-search", "true");
    $("#TechRcvToList").attr("data-selected-text-format", "count");
    $("#TechRcvToList").attr("data-actions-box", "true");
    $("#TechRcvToList").selectpicker('val', selectedTechsRcvToList);


    var techTrfFrom = $('span.filter-option.pull-left').eq(0).text();
    var techTrfTo = $('span.filter-option.pull-left').eq(1).text();
    var techRcvFrom = $('span.filter-option.pull-left').eq(2).text();
    var techRcvTo = $('span.filter-option.pull-left').eq(3).text();
    /*console.log(techTrf);*/
    techTrfFrom = techTrfFrom.replace('items', 'Technicians(Transfer From)');
    $('span.filter-option.pull-left').eq(0).text(techTrfFrom);
    techTrfTo = techTrfTo.replace('items', 'Technicians(Transfer To)');
    $('span.filter-option.pull-left').eq(1).text(techTrfTo);
    //console.log(techTrf);
    techRcvFrom = techRcvFrom.replace('items', 'Technicians(Receive From)');
    $('span.filter-option.pull-left').eq(2).text(techRcvFrom);
    techRcvTo = techRcvTo.replace('items', 'Technicians(Receive To)');
    $('span.filter-option.pull-left').eq(3).text(techRcvTo);
}

var BindTechFilterDDTL = function () {
    $("#TechTrfFromList").change(function () {

        if ($("#TechTrfFromList").is(":visible")) {
            //console.log($("#EqList").val());

        }
        if ($("#TechTrfFromList").val() != null) {
            TrfLogListLoadPost(1, 1, null, $("#TechTrfFromList").val(), $("#TechTrfToList").val(), $("#TechRcvFromList").val(), $("#TechRcvToList").val());
        }

    });

    $("#TechTrfToList").change(function () {

        if ($("#TechTrfToList").is(":visible")) {
            //console.log($("#EqList").val());

        }
        console.log('Trf');
        //console.log($("#TechTrfToList").val(), $("#TechRcvFromList").val());
        if ($("#TechTrfToList").val() != null) {
            TrfLogListLoadPost(1, 1, null, $("#TechTrfFromList").val(), $("#TechTrfToList").val(), $("#TechRcvFromList").val(), $("#TechRcvToList").val());
        }

    });

    $("#TechRcvFromList").change(function () {
        console.log("Test 03");
        if ($("#TechRcvFromList").is(":visible")) {
            //console.log($("#EqList").val());

        }
        //console.log($("#TechTrfFromList").val(), $("#TechRcvFromList").val());
        if ($("#TechRcvFromList").val() != null) {
            TrfLogListLoad(1, 1, null, $("#TechTrfFromList").val(), $("#TechTrfToList").val(), $("#TechRcvFromList").val(), $("#TechRcvToList").val());
        }

    });

    $("#TechRcvToList").change(function () {
        console.log("Test 01");
        if ($("#TechRcvToList").is(":visible")) {
            //console.log($("#EqList").val());

        }
        //console.log($("#TechTrfFromList").val(), $("#TechRcvToList").val());
        if ($("#TechRcvToList").val() != null) {
            TrfLogListLoad(1, 1, null, $("#TechTrfFromList").val(), $("#TechTrfToList").val(), $("#TechRcvFromList").val(), $("#TechRcvToList").val());
        }

    });
    
}


var checkedItem = [];
/*var bulkpage = "";*/
var BulkInventoryTechReceiveConfirm = function (type, page ='TT') {

    if (checkedItem.length == 0) {
        toastr.warning("Select atleast 1 transfer item.");
        return false;
    }

    if (type == 'R') {
        var url = '/Inventory/BulkTechReceiveConfirm';
        var param = JSON.stringify(checkedItem);
        $('.AddNewEquipment').prop('disabled', true);
        $(".LoaderWorkingDiv").show()
        $.ajax({
            type: "POST",
            ajaxStart: function () { },
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result) {
                    //OpenSuccessMessageNew("Success", data.message);
                    OpenSuccessToast("Success", data.message)
                }
                else {
                    OpenErrorMessageNew("Error", data.message);
                }
                if (page == 'TL')
                    TrfLogListLoad(1, 1, null);
                else
                    TrfListLoad(1, 1, null);
                checkedItem = [];
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                checkedItem = [];
            },
            complete: function (data) {
                $(".LoaderWorkingDiv").hide()
                $("a[tabname='#TransferLogTab']").click();
                $('.AddNewEquipment').prop('disabled', false);
            }
        });
    }
    else {
        OpenConfirmationMessageNew("Confirmation", "Are you sure, you want to decline these item/items?", function () {
            var url = '/Inventory/BulkInventoryTechReceiveDecline';
            var param = JSON.stringify(checkedItem);
            $(".LoaderWorkingDiv").show()
            $.ajax({
                type: "POST",
                ajaxStart: function () { },
                url: url,
                data: param,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                success: function (data) {
                    if (data.result) {
                        OpenSuccessMessageNew("Success", data.message);
                       
                        TrfLogListLoadPost(1, 1, null,null,null,null,null,'decline');
                       
                    }
                    else {
                        OpenErrorMessageNew("Error", data.message);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                },
                complete: function (data) {
                    $(".LoaderWorkingDiv").hide()
                    $("a[tabname='#TransferLogTab']").click();
                }
            });
        });
    }
}

var togglecheckboxitem = function (_this) {
    checkedItem = [];
    var totalCheckboxes = $('input[class=chkitem]').length;
    if (totalCheckboxes == 0) {
        $('#chkbulk').prop('checked', false);
        return false;
    }
    $('.chktd input[class=chkitem]').each(function () {
        this.checked = $(_this).prop('checked');
        addcheckeditem(this,0);
    });
    
}

var addcheckeditem = function (_this, type) {
    var tlId = $(_this).attr('tlId');
    var tlEquipmentId = $(_this).attr('tlEquipmentId');
    var tlTechnicianId = $(_this).attr('tlTechnicianId');
    var tlQuantity = $(_this).attr('tlQuantity');
    var tlReqsrc = $(_this).attr('tlReqsrc');
    bulkpage = $(_this).attr('tlpage');

    if ($(_this).prop('checked') == true) {
        checkedItem.push({
            id: tlId,
            eqpid: tlEquipmentId,
            techid: tlTechnicianId,
            qty: tlQuantity,
            reqSrc: tlReqsrc
        });
    }
    else {
        console.log(tlId);
        $.each(checkedItem, function (index, value) {
            if (value != undefined) {
                if (value.id == tlId) {
                    checkedItem.splice(index, 1);
                }
            }
        });
    }
    console.log(checkedItem);

    if (type == 1) {
        uncheckbulkchk();
    }
}

var uncheckbulkchk = function () {

    var totalchkitemtick = 0;
    var totalchkitem = $('input[class=chkitem]').length;
   
    $('.chktd input[class=chkitem]').each(function () {
        if (this.checked == true) {
            totalchkitemtick = totalchkitemtick + 1;
        }
    });

    if (totalchkitem == totalchkitemtick) {
        $('#chkbulk').prop('checked', true);
    }
    else {
        $('#chkbulk').prop('checked', false);
    }

}


var TrfLogListLoadPost = function (pageNoTrf, pageNoRcv, order, techTrfFrom, techTrfTo, techRCVFrom, techRCVTo,Status) {
    console.log('new post data');
    if (typeof (pageNoTrf) != "undefined") {
        var setectSts = encodeURI($("#poStatus").val());
        EstimatorId = $("#EstimatorId").val();
        StartDate = $(".min-date").val();
        EndDate = $(".max-date").val();
        //console.log(techRCVFrom);
        //var url = '/Inventory/TechTransferLogListPartialPost';
        var url = '/Inventory/InventoryTransferListPartial';
        $(".LoaderWorkingDiv").show()
        var param = JSON.stringify({
            SearchText: encodeURI($(".PoSearchText").val()),
            selectsts: setectSts,
            PageNoTrf: pageNoTrf,
            PageNoRcv: pageNoRcv,
            order: order,
            Start: encodeURI(StartDate),
            End: encodeURI(EndDate),
            TFEmployeeIds: techTrfFrom,
            TTEmployeeIds: techTrfTo,
            RFEmployeeIds: techRCVFrom,
            RTEmployeeIds: techRCVTo,
            EstimatorId: EstimatorId,
            status: Status
        });
        //console.log(param);
        $.ajax({
            type: "POST",
            ajaxStart: function () { },
            url: url,
            data: param,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            cache: false,
            success: function (data) { 
                /*$(".PurchaseOrderTable").html(data); */
                $(".tranferTab_Load").html(data); 
                $(".recieverTab_Load").html(''); 
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            },
            complete: function (data) {
                $(".LoaderWorkingDiv").hide()
            }
        });
    }
}

var addnewpo = function () {
    OpenTopToBottomModal(domainurl + "/Inventory/AddTechTransfer?id=0&OpenTab=Transferlog");
}
 
var ReceiveDownload = function () {
    console.log("Test doenload 1");
    DataListDownload(1, 1, null, $("#TechRcvFromList").val(), $("#TechRcvToList").val(), true);
}
 
