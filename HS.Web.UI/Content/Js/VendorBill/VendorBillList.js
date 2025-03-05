

$(document).ready(function () {
    var pagenumber = 1;
    //var table = $('#tblVendorBillList').DataTable({
    //    "pageLength": DataTablePageSize,
    //    "destroy": true,
    //    //"scrollY": "290px",
    //    //"scrollCollapse": true,
    //    "language": {
    //        "emptyTable": "No data available"
    //    }
    //});

    
    $('#btnsearchtext').click(function () {
        NavigatePageListing(pagenumber);
    })
    
    $(".icon_sort_timeclock").click(function () {
        console.log("df");
        var orderval = $(this).attr('data-val');
        console.log(orderval)
        NavigatePageListing(pageno, orderval);
    });
    var Popupwidth = 920;
    if (window.innerWidth < 920) {
        Popupwidth = window.innerWidth;
    }
    var idlist = [{ id: "#OpenCheckPreview", type: 'iframe', width: Popupwidth, height: 600 }
    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    //$('#tblVendorBillList tbody').on('click', 'tr', function () {
    //    if ($(this).hasClass('selected')) {
    //        $(this).removeClass('selected');
    //    }
    //    else {
    //        table.$('tr.selected').removeClass('selected');
    //        $(this).addClass('selected');
    //    }
    //});
    //$("#srch-term").keyup(function () {
    //    $("#tblVendorBillList_filter input").val($("#srch-term").val());
    //    $("#tblVendorBillList_filter input").trigger('keyup');
    //});

  
})