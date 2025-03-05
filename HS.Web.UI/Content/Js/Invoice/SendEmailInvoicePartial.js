function printFrame(id) {
    var frm = document.getElementById(id).contentWindow;
    frm.focus();// focus on contentWindow is needed on some ie versions
    frm.print();
    return false;
}

var PrintInvoiceEmail = function () {
    var url = domainurl + "/Invoice/SaveInvoicePdf";
    var param = JSON.stringify({
        "InvoiceId": id
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
            console.log("for ckheck phone");
            var UrlInv = domainurl + data.filePath;
            var iframeobj = "<iframe class='pdf-styles'id='iframePdf' src=" + UrlInv + "></iframe>";
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                $("#PdfDownLoadForPhone").attr('href', data.filePath);
            }
            else {
                $(".pdf_iframe").append(iframeobj);
                $(".pdf_iframe").removeClass('hidden');
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
};

var PrintInvoiceStatementEmail = function () {
    var CustomerList = [];
    CustomerList.push(customerIntId);
    var strStatus = "RMR";
    if (StatementStatus != null || StatementStatus != '' || StatementStatus != 'undefined') {
        strStatus = StatementStatus;
    }
    var url = domainurl + "/Invoice/CreateCustomerUnpaidInvoiceStatement";
    var param = JSON.stringify({
        CustomerIntIdList: CustomerList,
        StatementFor: strStatus
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
            var UrlInv = domainurl + data.filePath;
            var iframeobj = "<iframe class='pdf-styles'id='iframePdf' src=" + UrlInv + "></iframe>";
            if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                $("#PdfDownLoadForPhone").attr('href', data.filePath);
            }
            else {
                $(".pdf_iframe").append(iframeobj);
                $(".pdf_iframe").removeClass('hidden');
            }

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
};

$(document).ready(function () {
    console.log("Stm");
    $(".btnSaveAndClose").click(function () {
        var EmailDes = tinyMCE.get('Description').getContent();
        var EmailSub = $("#EmailSubject").val();
        var ccEmail = $("#CCEmail").val();
        var toEmail = $("#ToEmail").val();
        var actionUrl = "";
        var param = "";
        if (isStatement) {
            actionUrl = domainurl + "/Invoice/SendInvoiceStatementByEmail";
            param = {
                Id: id,
                CustomerId: customerIntId,
                StatementFor: StatementStatus,
                EmailDescription: encodeURI(EmailDes),
                EmailSubject: EmailSub,
                CCEmail: ccEmail,
                ToEmail: toEmail
            };
        }
        else {
            actionUrl = domainurl + "/Invoice/AddInvoice";
            param = {
                id: id,
                SendEmail: true,
                CreatePdf: false,
                EmailDescription: EmailDes,
                EmailSubject: EmailSub,
                ccEmail: ccEmail,
                EmailAddress: toEmail
            };
        }
        $.ajax({
            type: "POST",
            ajaxStart: $(".loader-div").show(),
            url: actionUrl,
            data: param,
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.result == true && data.EmailSent == true) {
                    
                    parent.OpenSuccessMessageNew("Success!", "Successfully email sent", function () {
                        setTimeout(function () {
                            parent.ClosePopup();
                          //  parent.CloseTopToBottomModal();
                        }, 600);
                    });
                } else {
                    parent.OpenErrorMessageNew("Error!", data.message, function () {
                        setTimeout(function () {
                            parent.ClosePopup();
                        }, 600);
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    });

    if (isStatement) {
        
        PrintInvoiceStatementEmail();
    }
    else {
        PrintInvoiceEmail();
    }
    
});