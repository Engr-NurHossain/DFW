var ClosePopup = function () {
    $.magnificPopup.close();
}

$(document).ready(function () {
    $(".add_vendor_bill_container").height(window.innerHeight - 95);
    vendorid = $('#SupplierId').val();
    PaymentDate = new Pikaday({ format: 'MM/DD/YYYY', field: $('#PaymentDate')[0] });
    PaymentDueDate = new Pikaday({
        field: $('.PaymentDueDate')[0],
        trigger: $('#Bill_PaymentDueDateCustom')[0],
        format: 'MM/DD/YYYY',
        firstDay: 1
    });

    $(".LoadBillFiles").load("/Expense/LoadBillFiles?BillNo=" + BillNo)

    $("#SaveBillFile").click(function () {
        if (CommonUiValidation() && $("#UploadedPath").val() != "") {
            SaveBillFile();
            $(".fileborder").removeClass('red-border');
        }
        if ($("#UploadedPath").val() == "") {
            $("#uploadfileerror").removeClass("hidden");
            $(".fileborder").addClass('red-border');
        }
    });
    if (PaymentStatus == "Undefined" || PaymentStatus == "Init") {
        $(".invoice-make-payment-div").hide();
        $(".balance-info-due").show();
        $(".balance-info-paid").hide();
    }
    else if (PaymentStatus == "Open") {
        $(".invoice-make-payment-div").show();
        $(".balance-info-paid").hide();
        $(".balance-info-due").show();
    }
    else if (PaymentStatus == "Partial") {
        $(".InvoiceSaveButton").hide();
        $(".balance-info-paid").hide();
    }
    else if (PaymentStatus == "Paid") {
        $(".invoice-make-payment-div").hide();
        $(".balance-info-due").hide();
        $(".balance-info-paid").show();
        $(".InvoiceSaveButton").hide();
    }




    //$("#SaveCustomerFiles").click(function () {
    //    if (CommonUiValidation() && $("#UploadedPath").val() != "") {
    //        SaveCustomerFile();
    //        $(".fileborder").removeClass('red-border');
    //    }
    //    if ($("#UploadedPath").val() == "") {
    //        $("#uploadfileerror").removeClass("hidden");
    //        $(".fileborder").addClass('red-border');
    //    }
    //});



    $("#UploadCustomerFileBtn").click(function () {
        console.log("sdfdsf");
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $(".change-picture-logo").click(function () {
        $("#UploadedFile").click();
        $("#UploadSuccessMessage").addClass('hidden');
    });
    $('#UploadedFile').fileupload({
        pasteZone: null,
        url: domainurl + '/Expense/UploadBillFile', /* CustomerId*/
        dataType: 'json',
        add: function (e, data) {
            var ext = data.files[0].name.split(".");
            if ($("#description").val() == "") {
                var filename = data.fileInput[0].value.split('\\').pop();
                $("#description").val(filename);
            }
            if (ext[1].toLowerCase() == 'doc' || ext[1].toLowerCase() == 'docx' || ext[1].toLowerCase() == 'xls' || ext[1].toLowerCase() == 'xlsx' || ext[1].toLowerCase() == 'jpeg' || ext[1].toLowerCase() == 'jpg' || ext[1].toLowerCase() == 'gif' || ext[1].toLowerCase() == 'png' || ext[1].toLowerCase() == 'rtf' || ext[1].toLowerCase() == 'pdf' || ext[1].toLowerCase() == 'txt' || ext[1].toLowerCase() == 'mp4' || ext[1].toLowerCase() == 'mov') {

                if (data.files[0].size <= 50000000) {
                    UserFileUploadjqXHRData = data;
                }
                else {
                    OpenErrorMessageNew("Error!", "File size is more then 50 mb.", function () {
                        $(".close").click();
                    })
                }

            }
            else {
                OpenErrorMessageNew("Error!", "File format not valid.", function () {
                    $(".close").click();
                })
            }

        },
        progress: function (e, data) {
            var percentVal = parseInt(data.loaded / data.total * 100, 10);
            $(".file-progress").show();
            $(".file-progress .progress-bar").animate({
                width: percentVal + "%"
            }, 40);
            $(".file-progress .progress-bar span").text(percentVal + '%');
        },
        done: function (event, data) {
            console.log("dfdf");
            setTimeout(function () {
                $(".file-progress").hide();
                $(".file-progress .progress-bar").animate({
                    width: 0 + "%"
                }, 0);
                $(".file-progress .progress-bar span").text(0 + '%');
            }, 500);

            if ((typeof (data.result.isUploaded) != "undefined") && data.result.isUploaded == true) {
                $("#UploadSuccessMessage").removeClass('hidden');
                //$("#UploadCustomerFileBtn").addClass('hidden');
                $("#UploadedPath").val(data.result.filePath);
                var spfile = data.result.FullFilePath.split('.');
                //if (spfile[1] == "png" || spfile[1] == "jpg" || spfile[1] == "jpeg") {
                //    $(".Upload_Doc").addClass('hidden');

                //    $(".LoadPreviewDocument").removeClass('hidden');
                //    $("#Preview_Doc").attr('src', data.result.FullFilePath);
                //}
                $(".fileborder").removeClass('red-border');
                $("#uploadfileerror").addClass("hidden");
                $("#Bill_file_description").val(data.result.filedes);
                $(".add_vendor_bill_image_add .fileborder").css("border", "1px solid #ccc");
                var index = spfile.length - 1;
                if (spfile[index] == "png" || spfile[index] == "PNG" || spfile[index] == "jpg" || spfile[index] == "JPG" || spfile[index] == "jpeg" || spfile[index] == "JPEG") {
                    //$(".Upload_Doc").addClass('hidden');
                    //$(".LoadPreviewDocument").removeClass('hidden');
                    //$("#Preview_Doc").attr('src', data.result.FullFilePath);
                    $("#UploadCustomerFileBtn").attr('src', data.result.FullFilePath)
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").addClass('custom-file');
                    $("#UploadCustomerFileBtn").removeClass('otherfileposition');
                    $(".fileborder").addClass('border_none');
                }
                else if (spfile[index] == "pdf") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', domainurl + '/Content/Icons/pdf.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else if (spfile[index] == "doc" || spfile[index] == "docx") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else if (spfile[index] == "mp4" || spfile[index] == "mov") {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/mp4.png');
                    //$("#UploadCustomerFileBtn").addClass('otherfileposition');
                    //$("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                else {
                    $(".chooseFilebtn").addClass("hidden");
                    $(".changeFilebtn").removeClass("hidden");
                    $(".deleteDoc").removeClass("hidden");
                    $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/docx.png');
                    $("#UploadCustomerFileBtn").addClass('otherfileposition');
                    $("#UploadCustomerFileBtn").removeClass('custom-file');
                    $(".fileborder").removeClass('border_none');
                }
                //else {
                //    $(".Upload_Doc").addClass('hidden');
                //    $(".LoadPreviewDocument").addClass('hidden');
                //    $(".LoadPreviewDocument1").removeClass('hidden');
                //    $("#Frame_Doc").attr('src', data.result.FullFilePath);
                //}
            }
        },
        fail: function (event, data) {
            //if (data.files[0].error) {
            //    //alert(data.files[0].error);
            //}
        }
    });
    $("#UploadedFile").on("change", function () {
        if (UserFileUploadjqXHRData) {
            UserFileUploadjqXHRData.submit();
        }
        return false;
    });

    $(".deleteDoc").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this file?", function () {
            $(".Upload_Doc").removeClass('hidden');
            //$(".LoadPreviewDocument").addClass('hidden');
            //$(".LoadPreviewDocument1").addClass('hidden');
            $("#UploadCustomerFileBtn").attr('src', '/Content/Icons/blank_thumb_file.png');
            $(".chooseFilebtn").removeClass("hidden");
            $(".changeFilebtn").addClass("hidden");
            $(".deleteDoc").addClass("hidden");
            $("#Preview_Doc").attr('src', "");
            $("#Frame_Doc").attr('src', "");
            $("#UploadSuccessMessage").hide();
            $("#Bill_file_description").val("");
            $("#UploadedPath").val('');
            $(".fileborder").addClass('border_none');
            $("#UploadCustomerFileBtn").removeClass('otherfileposition');
            $(".add_vendor_bill_image_add .fileborder").css("border", "none");
        });
    });
});




var SaveAndClose = function () {
    SaveInvoice(false, false, "others");
    CloseTopToBottomModal();
    OpenSuccessMessageNew("Success!", "Bill Successfully Saved", function () { OpenInvoiceTab() });
}
var PrintOrPreview = function () {
    SaveBill(true, true, "PreviewOnly");
}

var InvoiceEqSuggestionclickbind = function (item) {
    $('.SupplierBillTab .tt-suggestion').click(function () {
        console.log('SupplierBillTab');
        var clickitem = this;
        $('.SupplierBillTab .tt-menu').hide();
        $(item).val($(clickitem).attr('data-name'));
        //$(item).attr('data-id', $(clickitem).attr('data-id'));
        itemName = $(item).parent().find('span');
        $(itemName).text($(item).val());
        $(item).val($(clickitem).attr('data-select'));
        AccountId = $(clickitem).attr('data-id').trim();
        $(item).parent().parent().attr('data-id', $(clickitem).attr('data-id'));
        $(item).parent().parent().addClass('HasItem');
        $($($(item).parent()).find('.hdnaccounttypeId')).val($(clickitem).attr('data-id'));

        CalculateNewAmount();

    });
    $('.SupplierBillTab .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}
var SearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/Expense/GetEquipmentTypeListByKey",
        data: {
            key: $(item).val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);

            if (resultparse.length > 0) {
                console.log('NewProjectSuggestion');
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    searchresultstring = searchresultstring + String.format(PropertyUserSuggestiontemplate,
                        resultparse[i].Name.replaceAll('"', '\'\''),
                        resultparse[i].Type,
                        resultparse[i].Id);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                InvoiceEqSuggestionclickbind(item);
                if (resultparse.length > 4) {
                    $(".NewProjectSuggestion").height(352);
                    $(".NewProjectSuggestion").css('position', 'relative');
                    /*$(".NewProjectSuggestion").perfectScrollbar()*/
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
var SearchKeyDown = function (item, event) {
    if (event.keyCode == 13) {
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        $(selectedTTMenu).click();
        $('.tt-menu').hide();
    }
    if (event.keyCode == 40) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugstionDom).is(':visible')) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }

            event.preventDefault();
        }
        else {

            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).next('tr')).addClass('focusedItem');
            if ($(event.target).hasClass('ProductName')) {
                $($(trselected).next('tr')).find('input.ProductName').focus();
            }
        }
    }
    if (event.keyCode == 38) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0 && $(ttSugstionDom).is(':visible')) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
            event.preventDefault();
        }

        else {
            var trselected = $($(event.target).parent()).parent();
            $(trselected).removeClass('focusedItem');
            $($(trselected).prev('tr')).addClass('focusedItem');
            $($(trselected).prev('tr')).find('input.ProductName').focus();
        }
    }
}
var OthersKeyDown = function (item, event) {
    if (event.keyCode == 40) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).next('tr')).addClass('focusedItem');
        if ($(event.target).hasClass('txtProductDesc')) {
            $($(trselected).next('tr')).find('input.txtProductDesc').focus();
            //} else if ($(event.target).hasClass('txtProductQuantity')) {
            //    $($(trselected).next('tr')).find('input.txtProductQuantity').focus();
            //} else if ($(event.target).hasClass('txtProductRate')) {
            //    $($(trselected).next('tr')).find('input.txtProductRate').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).next('tr')).find('input.txtProductAmount').focus();
        }
    } else if (event.keyCode == 38) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        $($(trselected).prev('tr')).addClass('focusedItem');
        if ($(event.target).hasClass('txtProductDesc')) {
            $($(trselected).prev('tr')).find('input.txtProductDesc').focus();
            //} else if ($(event.target).hasClass('txtProductQuantity')) {
            //    $($(trselected).prev('tr')).find('input.txtProductQuantity').focus();
            //} else if ($(event.target).hasClass('txtProductRate')) {
            //    $($(trselected).prev('tr')).find('input.txtProductRate').focus();
        } else if ($(event.target).hasClass('txtProductAmount')) {
            $($(trselected).prev('tr')).find('input.txtProductAmount').focus();
        }
    }
    else if (event.keyCode == 9 && $(event.target).hasClass('txtProductAmount')) {
        var trselected = $($(event.target).parent()).parent();
        $(trselected).removeClass('focusedItem');
        var trfocuseditem = $(trselected).next('tr');
        $(trfocuseditem).addClass('focusedItem');
        $($(trfocuseditem).find('input.ProductName')).focus();
        event.preventDefault();
    }

}
var CalculateNewAmount = function () {
    var amount = 0;
    $(".txtProductAmount").each(function () {
        var currAmount = parseFloat($(this).val().trim());
        if (!isNaN(currAmount)) {
            amount += currAmount;
        }
    });
    amount = parseFloat(amount).toFixed(2);
    amount1 = amount.replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    $(".amount").text(TransCurrency + amount1);
    $(".FinalTotalTxt").text(TransCurrency + amount1);
    $(".balanceDueAmount").text(TransCurrency + amount1);


    TotalAmount = amount;
    FinalTotal = amount;
    BalanceDue = amount;

    TotalAmount = FinalTotal;
}
var InitRowIndex = function () {
    var i = 1;
    $(".SupplierBillTab tbody tr td:first-child").each(function () {
        $(this).text(i);
        i += 1;
    });
}

var SaveBillFile = function () {
    var url = domainurl + "/Expense/AddVendorBilFile/";
    var param = JSON.stringify({
        File: $("#UploadedPath").val(),
        BillNo: BillNo,
        FileDes: $('#Bill_file_description').val()
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
            OpenSuccessMessageNew("Success!", "File Saved Successfully.", function () {
                $(".LoadBillFiles").load("/Expense/LoadBillFiles?BillNo=" + BillNo)
                $("#Right-To-Left-Modal-Body .close").click();
            });
            $(".customer-files-modal-head .close").click();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

}

var SaveBill = function (SendEmail, CreatePdf, CameFrom) {
    if (typeof (SendEmail) == "undefined") {
        SendEmail = false;
    }
    if (typeof (CreatePdf) == "undefined") {
        CreatePdf = false;
    }
    var url = domainurl + "/Expense/AddVendorBill";

    if (typeof (CameFrom) != "undefined" && CameFrom == "PreviewOnly") {
        url = domainurl + "/Expense/CreateBillPreview";
    }
    var DetailList = [];
    var hashItem = $(".HasItem");
    for (var icount = 0 ; icount < hashItem.length; icount++) {
        DetailList.push({
            CustomerBillId: Bill_Id,
            EquipmentId: '00000000-0000-0000-0000-000000000000',
            AccoutTypeId: $(hashItem[icount]).find('.hdnaccounttypeId').val(),
            Dscription: $(hashItem[icount]).find('.txtProductDesc').val(),
            Quantity: 0,
            Rate: 0.0,
            Amount: $(hashItem[icount]).find('.txtProductAmount').val(),
            EquipmentName: $(hashItem[icount]).find('.ProductName').val(),
        });
    }

    var param = JSON.stringify({
        "Bill.Id": Bill_Id,
        "Bill.BillNo": BillNo,
        "Bill.JobName": $("#Bill_JobName").val(),
        "Bill.EmployeeId": $("#Bill_EmployeeId").val(),
        "Bill.CompanyId": '00000000-0000-0000-0000-000000000000',
        "Bill.SupplierId": vendorid,
        "Bill.Type": BillType,
        "Bill.Amount": TotalAmount,
        "Bill.PaymentMethod": $("#PaymentMethod").val(),
        "Bill.PaymentStatus": PaymentStatus,
        "Bill.PaymentDate": $("#Bill_PaymentDate").val(),
        "Bill.PaymentDueDate": $("#Bill_PaymentDueDate").val(),
        "Bill.PaymentDue": BalanceDue,
        "Bill.BillCycle": "",
        "Bill.Notes": $(".InvoiceMessage").val(),
        "Bill.RefNo": $("#BillRefNo").val(),
        "Bill.SupplierAddress": VendorAddress,
        "Bill.PurchaseOrderId": $("#Bill_PurchaseOrderId").val(),
        "Bill.InvoiceId": $("#Bill_InvoiceId").val(),
        "Bill.PaymentTerm": $("#Bill_PaymentTerm").val(),
        BillDetailList: DetailList,


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
            if (location.href.toLowerCase().indexOf("/supplier/supplierdetail/?id=") > -1) {
                console.log("supplierdetail");
                /*This block will be used if the bill opens from vendor profile*/
                if (CameFrom == "PreviewOnly") {
                    $(".BillPreviewPopup").click();
                } else if (CameFrom == "SaveAndClose") {
                    OpenSuccessMessageNew("Success!", "Bill Successfully Saved", function () { LoadSupplierDetails(CurrentSupplierId, true); CloseTopToBottomModal(); });
                } else if (CameFrom == "SaveAndNew") {
                    OpenSuccessMessageNew("Success!", "Bill Successfully Saved", function () { LoadExpense(true); OpenTopToBottomModal("@(AppConfig.DomainSitePath)/Expense/AddVendorBill"); });
                } else if (CameFrom == "SaveOnly") {
                    OpenSuccessMessageNew("Success!", "Bill Successfully Saved", function () { LoadSupplierDetails(CurrentSupplierId, true); CloseTopToBottomModal(); });
                }
                /*This block will be used if the bill opens from vendor profile*/
            } else {
                if (CameFrom == "PreviewOnly") {
                    $(".BillPreviewPopup").click();
                } else if (CameFrom == "SaveAndClose") {
                    OpenSuccessMessageNew("Success!", "Bill Successfully Saved", function () { LoadExpense(true); CloseTopToBottomModal(); });
                } else if (CameFrom == "SaveAndNew") {
                    OpenSuccessMessageNew("Success!", "Bill Successfully Saved", function () { LoadExpense(true); OpenTopToBottomModal("@(AppConfig.DomainSitePath)/Expense/AddVendorBill"); });
                } else if (CameFrom == "SaveOnly") {
                    OpenSuccessMessageNew("Success!", "Bill Successfully Saved", function () { LoadExpense(true); CloseTopToBottomModal(); });
                }
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })

}
var SaveAndClose = function () {
    if ($(".HasItem").length == 0) {
        OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", "");
    }
    else if (TotalAmount <= 0) {
        OpenErrorMessageNew("Error!", "Balance Cannot be 0", "");
    }
    else if (PaymentStatus == "Partial" || PaymentStatus == "Paid") {
        LoadExpense(true);
        CloseTopToBottomModal();
    }
    else {
        SaveBill(false, false, "SaveAndClose");
    }
}
var SaveAndNew = function () {
    if ($(".HasItem").length == 0) {
        OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", "");
    }
    else if (TotalAmount <= 0) {
        OpenErrorMessageNew("Error!", "Balance Cannot be 0", "");
    }
    else if (PaymentStatus == "Partial" || PaymentStatus == "Paid") {
        LoadExpense(true);
        OpenTopToBottomModal(domainurl + "/Expense/AddVendorBill");
    } else {
        SaveBill(false, false, "SaveAndNew");

    }
}
var SaveOnly = function () {
    console.log("list");



    if ($(".HasItem").length == 0) {
        OpenErrorMessageNew("Error!", "You have to select at least one equipment to proceed", "");


    }
    else if (TotalAmount <= 0) {
        OpenErrorMessageNew("Error!", "Balance Cannot be 0", "");
    }
    else if (PaymentStatus == "Partial" || PaymentStatus == "Paid") {
        LoadExpense(true);
        CloseTopToBottomModal();
    }

    else {
        if (parent.$('.txtProductAmount').val() == "" && parent.$("#VendorList").val() == "") {
            OpenErrorMessageNew("Error!", "Please select vendor and input amount", "");
        }
        else {
            SaveBill(false, false, "SaveOnly");


        }
    }
}
$(document).ready(function () {
    console.log('aaa')

    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: ".BillPreviewPopup", type: 'iframe', width: Popupwidth, height: 600 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });

    setTimeout(function () {
        //$(".avp_table_info_container").height(window.innerHeight - 253);
        if ($("#Bill_BillFor").val() == "Employee") {
            $(".Vendor_name_insert_div").addClass("hidden");
        } else if ($("#Bill_BillFor").val() == "InHouse") {
            $(".Vendor_name_insert_div").addClass("hidden");
            $(".Emplopyee_dropdown").addClass("hidden");
        } else {
            $(".Emplopyee_dropdown").removeClass("hidden");
        }
    }, 200);
    $("#SendEmail").val($("#CustomerList").find(":selected").attr("data-EmailAddress"));
    $(".SupplierBillTab tbody").sortable({
        update: function () {
            var i = 1;
            $(".SupplierBillTab tbody tr td:first-child").each(function () {
                $(this).text(i);
                i += 1;
            });
        }
    }).disableSelection();

    DueDatepicker = new Pikaday({
        field: $('#Invoice_DueDate')[0],
        format: 'MM/DD/YYYY'
    });
    PaymentDate = new Pikaday({
        field: $('#Bill_PaymentDate')[0],
        trigger: $('#Bill_PaymentDateCustom')[0],
        format: 'MM/DD/YYYY',
        firstDay: 1
    });
    InitRowIndex();
    if ($("#PaymentMethod").val() == "Cash" || $("#PaymentMethod").val() == "-1") {
        $(".payment-method-div").hide();
    } else {
        $(".payment-method-div").show();
    }
    $("#Bill_BillFor").change(function () {
        if ($("#Bill_BillFor").val() == "Employee") {
            $(".Vendor_name_insert_div").addClass("hidden");
            $(".Emplopyee_dropdown").removeClass("hidden");
        }
        else if ($("#Bill_BillFor").val() == "InHouse") {
            $(".Vendor_name_insert_div").addClass("hidden");
            $(".Emplopyee_dropdown").addClass("hidden");
            $("#VendorList").val("");
            $("#SupplierId").val("0");
            $("#Bill_EmployeeId").val("00000000-0000-0000-0000-000000000000");
        }
        else {
            $(".Vendor_name_insert_div").removeClass("hidden");
            $(".Emplopyee_dropdown").removeClass("hidden");
        }
    });
    $("#Credittest1").change(function () {
        var Creditval = $("#Credittest1").val();
        $("#CardType").val(Creditval);
    });
    $("#Credittest2").change(function () {
        var Creditval1 = $("#Credittest2").val();
        $("#CardType").val(Creditval1);
    });
    $("#Credittest3").change(function () {
        var Creditval2 = $("#Credittest3").val();
        $("#CardType").val(Creditval2);
    });
    $("#Debittest1").change(function () {
        var Debitval = $("#Debittest1").val();
        $(".DebitType").val(Debitval);
    });
    $("#Debittest2").change(function () {
        var Debitval1 = $("#Debittest2").val();
        $(".DebitType").val(Debitval1);
    });
    $("#Debittest3").change(function () {
        var Debitval2 = $("#Debittest3").val();
        $(".DebitType").val(Debitval2);
    });
    $("#PaymentMethod").change(function () {
        if ($(this).val() == "Cash" || $(this).val() == "-1") {
            $(".payment-method-div").hide();
        } else {
            if (PaymentMethodClick) {
                $(".PaymentMethodForms").hide();
                if ($("#PaymentMethod").val() == "ACH") {
                    $("#ACHForm").show();
                } else if ($("#PaymentMethod").val() == "Cash") {
                    $(".PaymentMethodForms").hide();
                } else if ($("#PaymentMethod").val() == "Check") {
                    $("#CheckForm").show();
                } else if ($("#PaymentMethod").val() == "Credit Card") {
                    $("#CreditForm").show();
                } else if ($("#PaymentMethod").val() == "Debit Card") {
                    $("#DebitForm").show();
                }
                else if ($("#PaymentMethod").val() == "Invoice") {
                    $("#InvoiceForm").show();
                }
            }
            $(".payment-method-div").show();
        }
    });
    $(".Edit_Payment_Method").click(function () {
        if (PaymentMethodClick) {
            PaymentMethodClick = false;
            $(".PaymentMethodForms").hide();
            $(this).html("Edit Payment Method");
        } else {
            PaymentMethodClick = true;
            $(this).html("Close");
            $(".PaymentMethodForms").hide();
            if ($("#PaymentMethod").val() == "ACH") {
                $("#ACHForm").show();
            } else if ($("#PaymentMethod").val() == "Cash") {
                $(".PaymentMethodForms").hide();
            } else if ($("#PaymentMethod").val() == "Check") {
                $("#CheckForm").show();
            } else if ($("#PaymentMethod").val() == "Credit Card") {
                $("#CreditForm").show();
            } else if ($("#PaymentMethod").val() == "Debit Card") {
                $("#DebitForm").show();
            }
            else if ($("#PaymentMethod").val() == "Invoice") {
                $("#InvoiceForm").show();
            }
        }
    });
    $("#CustomerList").change(function () {
        var selectedEmail = $(this).find(":selected").attr("data-EmailAddress");
        $("#SendEmail").val(selectedEmail);

        $("#EmailAddress").val(selectedEmail);
        var Address = $(this).find(":selected").attr("data-Address");


        var selectedAddress = "";
        if (Address != ",")
            selectedAddress += Address;

        $("#Invoice_BillingAddress").val(selectedAddress);

    });
    $(".btnAddLines").click(function () {
        for (var i = 0; i < 4; i++) {
            $("#SupplierBillTab tbody tr:last").after(NewEquipmentRow);
        }
        var i = 1;
        $(".SupplierBillTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $(".btnClearLines").click(function () {
        $(".SupplierBillTab tbody").html(NewEquipmentRow + NewEquipmentRow);
        var i = 1;
        $(".SupplierBillTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
        CalculateNewAmount();
    });
    $("#SupplierBillTab tbody").on('focusout', 'input.ProductName', function () {
        $(".tt-menu").hide();
        var ProductNameDom = $(this).parent().find('span.spnProductName');
        $(ProductNameDom).text($(this).val());
    });
    $("#SupplierBillTab tbody").on('click', 'tr', function (e) {
        if ($(e.target).hasClass('fa') || $(e.target).hasClass('tt-sug-text') || $(e.target).hasClass('tt-suggestion')) {
            return;
        }
        if (PaymentStatus == "Partial" || PaymentStatus == "Paid") {
            return;
        }
        else {
            if ($(e.target).hasClass("spnProductName") || $(e.target).hasClass("spnProductDesc")
              || $(e.target).hasClass("spnProductQuantity") || $(e.target).hasClass("spnProductRate")
              || $(e.target).hasClass("spnProductAmount")) {

                $("#SupplierBillTab tr").removeClass("focusedItem");
                $($(e.target).parent().parent()).addClass("focusedItem");
                $(e.target).parent().find('input').focus();
            } else if (e.target.tagName.toUpperCase() == 'INPUT') {
                return;
            }
            else {
                $("#SupplierBillTab tr").removeClass("focusedItem");
                $($(e.target).parent()).addClass("focusedItem");
                $(e.target).find('input').focus();
            }
        }
    });
    $("#SupplierBillTab tbody").on('click', 'tr:last', function (e) {
        if ($(e.target).hasClass('fa')) {
            return;
        }
        if (PaymentStatus == "Partial" || PaymentStatus == "Paid") {
            return;
        }
        $("#SupplierBillTab tbody tr:last").after(NewEquipmentRow);
        var i = 1;
        $(".SupplierBillTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
    });
    $(".SupplierBillTab tbody").on('click', 'tr td i.fa', function (e) {
        if (PaymentStatus == "Partial" || PaymentStatus == "Paid") {
            return;
        }
        $(this).parent().parent().remove();
        if ($(".SupplierBillTab tbody tr").length < 2) {
            $("#SupplierBillTab tbody tr:last").after(NewEquipmentRow);
        }
        var i = 1;
        $(".SupplierBillTab tbody tr td:first-child").each(function () {
            $(this).text(i);
            i += 1;
        });
        CalculateNewAmount();
    });
    $(".SupplierBillTab tbody").on('change', "tr td .txtProductAmount", function () {
        var ProductAmountDom = $(this).parent().parent().find('span.spnProductAmount');
        $(ProductAmountDom).text(Currency + parseFloat($(this).val()).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,'));
        CalculateNewAmount();
    });
    $(".SupplierBillTab tbody").on('change', "tr td .txtProductDesc", function () {
        var ProductQuantityDom = $(this).parent().find('span.spnProductDesc');
        $(ProductQuantityDom).text($(this).val());
    });
    $("#discountType").change(function () {
        CalculateNewAmount();
    });
    $('#Invoice_ShippingCost').focusout(function () {
        CalculateNewAmount();
    });
    CalculateNewAmount();
    if (PaymentStatus == "Partial") {
        var invoicedom = $(".invoice-informations");
        invoicedom.find("input").prop("disabled", true);
        invoicedom.find("select").prop("disabled", true);
        invoicedom.find("textarea").prop("disabled", true);
        $(".balanceDueAmount").text("$" + BillBalanceDue);
        $(".big-amount-top.amount").text("$" + BillBalanceDue);
        BalanceDue = BillBalanceDue;
    } else if (PaymentStatus == "Paid") {
        var invoicedom = $(".invoice-informations");
        invoicedom.find("input").prop("disabled", true);
        invoicedom.find("select").prop("disabled", true);
        invoicedom.find("textarea").prop("disabled", true);
        $(".balanceDueAmount").text("$" + BillBalanceDue);
        BalanceDue = BillBalanceDue;
    }
});


var Vendorclickbind = function (item) {
    $('.Vendor_name_insert_div .tt-suggestion').click(function () {
        var clickitem = this;
        $('.Vendor_name_insert_div .tt-menu').hide();
        var VendorNum = $(clickitem).attr("data-companyname").trim();
        $("#VendorList").val(VendorNum);
        var VendorCity = $(clickitem).attr("data-city").trim();
        var VendorState = $(clickitem).attr("data-state").trim();
        VendorAddress = $(clickitem).attr("data-street").trim() + "\n" + $(clickitem).attr("data-city").trim() + ", " + $(clickitem).attr("data-state").trim() + " " + $(clickitem).attr("data-zip").trim();

        $("#SupplierAddress").val(VendorAddress);
        vendorid = $(clickitem).attr("data-vendorid").trim();
    });
    $('.Vendor_name_insert_div .tt-suggestion').hover(function () {
        var clickitem = this;
        $('.tt-suggestion').removeClass("active");
        $(clickitem).addClass('active');
    });
}
var VendorSearchKeyDown = function (item, event) {
    if (event.keyCode == 13) {
        var selectedTTMenu = $(event.target).parent().find('.tt-suggestion.active');
        if (selectedTTMenu.length > 0) {
            setTimeout(function () { $(selectedTTMenu).click(); }, 10)
            $('.tt-menu').hide();
        }
    }
    if (event.keyCode == 40) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0) {
            if ($(ttSugActive).length == 0) {
                $($(ttSugstionDom).get(0)).addClass('active');
                $(item).val($($(ttSugstionDom).get(0)).attr('data-select'))
            }
            else {
                var suggestionlist = $(ttSugstionDom);
                var activesuggestion = $(ttSugActive);
                var indexactive = -1;
                for (var id = 0; id < suggestionlist.length; id++) {
                    if ($(suggestionlist[id]).hasClass('active'))
                        indexactive = id;
                }
                if (indexactive < suggestionlist.length - 1) {
                    $(ttSugstionDom).removeClass('active');
                    var possibleactive = $(ttSugstionDom).get(indexactive + 1);
                    $($(ttSugstionDom).get(indexactive + 1)).addClass('active');
                    $(possibleactive).addClass('active');
                    $(item).val($(possibleactive).attr('data-select'));
                }
            }
            event.preventDefault();
        }
    }
    if (event.keyCode == 38) {
        var ttSugstionDom = $(event.target).parent().find('.tt-suggestion');
        var ttSugActive = $(event.target).parent().find('.tt-suggestion.active');
        if ($(ttSugstionDom).length > 0 && $(ttSugActive).length > 0) {
            var suggestionlist = $(ttSugstionDom);
            var activesuggestion = $(ttSugActive);
            var indexactive = -1;
            for (var id = 0; id < suggestionlist.length; id++) {
                if ($(suggestionlist[id]).hasClass('active'))
                    indexactive = id;
            }
            if (indexactive > 0) {
                $(ttSugstionDom).removeClass('active');
                var possibleactive = $(ttSugstionDom).get(indexactive - 1);
                $(possibleactive).addClass('active');
                $(item).val($(possibleactive).attr('data-select'))
            }
            event.preventDefault();
        }
    }
}
var VendorSearchKeyUp = function (item, event) {
    if (event.keyCode == 40 || event.keyCode == 38 || event.keyCode == 13)
        return false;
    $.ajax({
        url: domainurl + "/Expense/GetVendorListByKey",
        data: {
            key: $(item).val()
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var resultparse = JSON.parse(data.result);
            if (resultparse.length > 0) {
                console.log('NewProjectSuggestion2');
                var searchresultstring = "<div class='NewProjectSuggestion'>";
                for (var i = 0; i < resultparse.length; i++) {
                    var name = resultparse[i].Name;

                    searchresultstring = searchresultstring + String.format(VendorSuggestiontemplate,
                        resultparse[i].Street,
                        resultparse[i].City == "-1" ? "" : resultparse[i].City,
                        resultparse[i].State == "-1" ? "" : resultparse[i].State,
                        resultparse[i].Zipcode,
                        resultparse[i].Name,
                        resultparse[i].Id,
                        resultparse[i].CompanyName);
                }
                searchresultstring += "</div>";
                var ttdom = $($(item).parent()).find('.tt-menu');
                var ttdomComplete = $($(item).parent()).find('.tt-dataset-autocomplete');
                $(ttdomComplete).html(searchresultstring);
                $(ttdom).show();

                Vendorclickbind(item);
                if (resultparse.length > 0) {
                    $(".Vendor_name_insert_div .NewProjectSuggestion").height(200);
                }
            }
            if (resultparse.length == 0)
                $('.tt-menu').hide();
        }
    });
}
$(document).ready(function () {
    var NewDueDate = new Date($("#Bill_PaymentDate").val());
    if (NewDueDate == "Invalid Date") {
        NewDueDate = new Date();
        $("#Bill_PaymentDate").val(NewDueDate.getMonth() + 1 + "/" + NewDueDate.getDate() + "/" + NewDueDate.getFullYear());
    }
    NewDueDate = NewDueDate.addDays(parseInt($("#Bill_PaymentTerm").val()));
    NewDueDate = NewDueDate.getMonth() + 1 + "/" + NewDueDate.getDate() + "/" + NewDueDate.getFullYear();
    $("#Bill_PaymentDueDate").val(NewDueDate);

    $("#Bill_PaymentTerm").change(function () {
        var NewDueDate = new Date($("#Bill_PaymentDate").val());
        if (NewDueDate == "Invalid Date") {
            NewDueDate = new Date();
            $("#Bill_PaymentDate").val(NewDueDate.getMonth() + 1 + "/" + NewDueDate.getDate() + "/" + NewDueDate.getFullYear());
        }
        NewDueDate = NewDueDate.addDays(parseInt($("#Bill_PaymentTerm").val()));
        NewDueDate = NewDueDate.getMonth() + 1 + "/" + NewDueDate.getDate() + "/" + NewDueDate.getFullYear();
        $("#Bill_PaymentDueDate").val(NewDueDate);
    });
    $("#VendorList").focusout(function () {
        setTimeout(function () {
            $(".Vendor_name_insert_div .tt-menu").hide();
        }, 200);
    });
})

$(window).resize(function () {
    $(".add_vendor_bill_container").height(window.innerHeight - 95);
})