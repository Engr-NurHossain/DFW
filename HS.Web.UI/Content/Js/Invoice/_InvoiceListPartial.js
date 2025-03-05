var DownLoadAllInvoicePdfList = function (idforprint) {
    console.log("aise valu");
    if (idforprint != undefined && idforprint != null && idforprint != "") {
        var IdSallSt = idforprint.toString();
        var DownloadUrl = domainurl + "/Invoice/DownLoadAllInvoicePdfList/?InvIdList=" + IdSallSt;
        console.log(DownloadUrl);
        parent.window.open(DownloadUrl, '_blank');
        parent.$.magnificPopup.close();
    }
}

$(document).ready(function () {
    //$('[data-toggle="tooltip"]').tooltip();
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }

    var idlist = [{ id: ".InvEstPreviewPartial", type: 'iframe', width: Popupwidth, height: 600 }
        //{ id: ".btn-add-invoice-Statement", type: 'iframe', width: Popupwidth, height: 600 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $("#IsCheckVal").change(function () {
        console.log("hlw");
        if ($(this).is(':checked')) {
            $(".CheckItems").each(function () {
                $(this).prop('checked', true);
            });
        }
        else {
            $(".CheckItems").each(function () {
                $(this).prop('checked', false);
            });
        }
    })

    $(".icon_sort_timeclock").click(function () {
        var orderval = $(this).attr('data-val');
        console.log(orderval)
        InvoiceSearchKeyUp(pageno, orderval);
    })
});
