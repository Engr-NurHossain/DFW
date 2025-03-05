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
var EditLead = function () {
    var selectedID = [];
    var checkboxs = $('.checkbox-custom');
    for (var i = 0; i < checkboxs.length; i++) {
        if ($(checkboxs[i]).is(":checked")) {
            selectedID.push(parseInt($(checkboxs[i]).attr('idval')));
        }
    }
    console.log(selectedID);
    if (selectedID.length > 1) {
        OpenErrorMessageNew("Error !", "You can edit only one lead at a time,please select one lead.");
    }
    else if (selectedID.length <= 0) {
        OpenErrorMessageNew("Error !", "Please select at least one lead.")
    }
    else {
        var id = selectedID[0];
        $(".addManufacturerMagnific").attr("href", domainurl + "/Leads/AddLeads?id=" + id);
        $(".addManufacturerMagnific").click();
        parent.LoadLeadList();
    }
}

$(document).ready(function () {
    $(window).trigger('resize');
    $(".ListViewLoader").hide();
    $("#successmessageClose").click(function () {
        setTimeout(function () {
            $(".ListContents").load(domainurl + "/Leads/LeadsListPartial");
        }, 200);
    });
    $("#AddNewCustomer").click(function () {
        LoadLeadVerificationInfo(0, true);
    });
    $("#AddNewCustomerv2").click(function () {
        LoadLeadInfov2(0, true);
    });
    setTimeout(function () {
        $(".ListContents").slideDown();
    }, 500);
    $('.leads-name-anchor-style').click(function (e) {
        e.preventDefault();
        if (cntrlIsPressed) {
            var href = $(e.target).attr('href');
            window.open(href, '_blank');
        } else {
            var id = $(this).attr('id');
            LoadLeadsDetail(id);
        }
    });
    $(".Delete-Lead").click(function () {
        var selectedID = [];
        var checkboxs = $('.checkbox-custom');
        for (var i = 0; i < checkboxs.length; i++) {
            if ($(checkboxs[i]).is(":checked")) {
                selectedID.push(parseInt($(checkboxs[i]).attr('idval')));
            }
        }
        if (selectedID.length > 0) {
            OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete this item?", DeleteLead);
        }

        else {
            OpenErrorMessageNew("Error !", "Please select at least one lead.")
        }
    });
    $(".Edit-Lead").click(function () {
        EditLead();
    })
    var DeleteLead = function () {

        for (var i = 0; i < selectedID.length; i++) {
            $.ajax({
                url: domainurl + "/Leads/DeleteLeads",
                data: { id: selectedID[i] },
                type: "Post",
                dataType: "Json"
            }).done(function () {
                console.log("Deleted");
            });
        }
        parent.LoadLeadList();
    }
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
    var idlist = [{ id: ".ExportLeadReport", type: 'iframe', width: customerreportpopwinowwith, height: customerreportpopwinowheight },
        { id: ".customerlistprint", type: 'iframe', width: customerprintpopwinowwith, height: customerprintpopwinowheight }

    ];
    jQuery.each(idlist, function (i, val) {
        magnificPopupObj(val);
    });
    $("#LeadReport").click(function () {
        var selectedID = [];
        var UserList = "";
        var FilterUser = "";
        var checkboxs = $('.Export_excel_lead');
        $(".CheckItems").each(function () {
            if ($(this).is(':checked')) {
                UserList += ($(this).val()) + ",";
            }
        });
        $(".CheckItems").each(function () {
            FilterUser += ($(this).val()) + ",";
        });
        for (var i = 0; i < checkboxs.length; i++) {
            selectedID.push(parseInt($(checkboxs[i]).attr('data-id')));
        }
        var ColumnName = "";
        $('.th').each(function () {
            if ($(this).attr('data-info') != "" && $(this).attr('data-info') != undefined && $(this).attr('data-info') != null) {
                ColumnName += $(this).attr('data-info').trim() + "," + $(this).text().trim() + "-";
            }
        });
        $(".ExportLeadReport").attr('href', domainurl + "/Reports/ExportConfirm/?ColumnName=" + ColumnName + "&Ids=" + selectedID + "&ReportFor=Lead&UserList=" + UserList + "&FilterUser=" + FilterUser);
        $(".ExportLeadReport").click();
    });
});


