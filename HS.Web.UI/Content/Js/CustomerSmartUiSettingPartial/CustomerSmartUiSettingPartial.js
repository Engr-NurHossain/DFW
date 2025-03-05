var UpdateCustomerUi = function () {
    var url = domainurl + "/Setup/UpdateCustomerUiSettings";

    var GridSettings = [];
    $(".customerGridSettings tbody tr").each(function () {
        var idval = $(this).attr('data-id');
        GridSettings.push({
            Id: $(this).attr('data-id'),
            ListKeyName: "CustomerGrid",
            SelectedColumn: $(this).find('.CustKeyName').attr('data-val'),
            OrderBy: $(this).find(".orderby").text(),
            InputType: $(this).find(".InputType").text(),
            FormActive: $(this).find(".CustFormValue").is(':checked'),
            GridActive: $(this).find(".CustGridValue").is(':checked'),
            ColumnGroup: $("#CustColumnGroup_" + idval).val(),
            IsActive: $(this).find(".IsActive").text(),
            IsFilter: $(this).find(".CustFilterValue").is(':checked'),
            IsCustomerRequired: $(this).find(".CustRequiredValue").is(':checked'),
            IsCustomerLabel: $(this).find(".CustLabelChacked").is(':checked')
           
        });
    });

    console.log(GridSettings)

    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(GridSettings),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            // LoadInventory(true);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}

var UpdateLeadUi = function () {
    var url = domainurl + "/Setup/UpdateLeadUiSettings";

    var GridSettings = [];
    $(".customerGridSettings tbody tr").each(function () {
        var idval = $(this).attr('data-val');
        GridSettings.push({
            Id: $(this).attr('data-val'),
            ListKeyName: "LeadGrid",
            SelectedColumn: $(this).find('.CustKeyName').attr('data-val'),
            OrderBy: $(this).find(".orderby").text(),
            InputType: $(this).find(".InputType").text(),
            FormActive: $(this).find(".LeadFormValue").is(':checked'),
            GridActive: $(this).find(".LeadGridValue").is(':checked'),
            ColumnGroup: $("#LeadColumnGroup_" + idval).val(),
            IsActive: $(this).find(".IsActive").text(),
            IsFilter: $(this).find(".LeadFilterValue").is(':checked'),
            IsLeadRequired: $(this).find(".LeadRequiredValue").is(':checked'),
            IsLeadLabel: $(this).find(".LeadLabelChacked").is(':checked')
        });
    });

    console.log(GridSettings)

    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(GridSettings),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            // LoadInventory(true);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}

var UpdateCustomerGroupUi = function () {
    var url = domainurl + "/Setup/UpdateCustomerUiSettings";

    var GridSettings = [];
    $(".customerGroupGridSettings tbody tr").each(function () {
        GridSettings.push({
            Id: $(this).attr('data-id'),
            ListKeyName: "CustomerGridGroup",
            SelectedColumn: $(this).find('.CustGroupKeyName').attr('data-val'),
            OrderBy: $(this).find(".orderby").text(),
            InputType: $(this).find(".InputType").text(),
            FormActive: $(this).find(".CustGroupFormValue").is(':checked'),
            GridActive: $(this).find(".CustGroupGridValue").is(':checked'),
            ColumnGroup: $("#CustGroupColumnGroup" + $(this).attr('data-id')).val(),
            IsActive: $(this).find(".IsActive").text(),
            IsFilter: $(this).find(".CustGroupFilterValue").is(':checked'),
        });
    });

    console.log(GridSettings)

    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(GridSettings),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            // LoadInventory(true);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}
var UpdateLeadGroupUi = function () {
    var url = domainurl + "/Setup/UpdateLeadUiSettings";

    var GridSettings = [];
    $(".InventoryGroupGridSettings tbody tr").each(function () {
        GridSettings.push({
            Id: $(this).attr('data-id'),
            ListKeyName: "LeadGridGroup",
            SelectedColumn: $(this).find('.GroupKeyName').text(),
            OrderBy: $(this).find(".orderby").text(),
            InputType: $(this).find(".InputType").text(),
            FormActive: $(this).find(".LeadGroupFormValue").is(':checked'),
            GridActive: $(this).find(".LeadGroupGridValue").is(':checked'),
            ColumnGroup: $("#GroupColumnGroup" + $(this).attr('data-id')).val(),
            IsActive: $(this).find(".IsActive").text(),
            IsFilter: $(this).find(".LeadGroupFilterValue").is(':checked'),
        });
    });

    console.log(GridSettings)

    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(GridSettings),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            // LoadInventory(true);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}
$(document).ready(function () {
    $(".LoaderWorkingDiv").hide();
    $("#CustomerUiSettingsTab").height(window.innerHeight - 142);
    $("#UiSettingsTab").height(window.innerHeight - 142);
    $("#EquipmentUiSettingsTab").height(window.innerHeight - 142);
    $(".CustFormValue").click(function () {
        UpdateCustomerUi();
    });
    $(".CustGridValue").click(function () {
        UpdateCustomerUi();
    });
    $(".CustColumnGroup").children().change(function () {
        UpdateCustomerUi();
    })
    $(".CustGroupFormValue").click(function () {
        UpdateCustomerGroupUi();
    });
    $(".CustGroupGridValue").click(function () {
        UpdateCustomerGroupUi();
    });
    $(".LeadColumnGroup").children().change(function () {
        UpdateLeadUi();
    })
    $(".LeadGridValue").click(function () {
        UpdateLeadUi();
    })
    $(".LeadFormValue").click(function () {
        UpdateLeadUi();
    })
    $(".LeadGroupFormValue").click(function () {
        UpdateLeadGroupUi();
    });
    $(".LeadGroupGridValue").click(function () {
        UpdateLeadGroupUi();
    });
    $(".CustFilterValue").click(function () {
        UpdateCustomerUi();
    })
    $(".LeadFilterValue").click(function () {
        UpdateLeadUi();
    })

    $(".CustRequiredValue").click(function () {
        UpdateCustomerUi();
    })
    $(".LeadRequiredValue").click(function () {
        UpdateLeadUi();
    })
    $(".CustLabelChacked").click(function () {
        UpdateCustomerUi();
    })
    $(".LeadLabelChacked").click(function () {
        UpdateLeadUi();
    })
    $(".customerGridSettings tbody").sortable({
        update: function () {
            var i = 1;
            $(".customerGridSettings tbody tr td.orderby").each(function () {
                $(this).text(i);
                i += 1;
            });
            UpdateCustomerUi();
            UpdateLeadUi();
        }
    }).disableSelection();

    $(".InventoryGroupGridSettings tbody").sortable({
        update: function () {
            var i = 1;
            $(".InventoryGroupGridSettings tbody tr td.orderby").each(function () {
                $(this).text(i);
                i += 1;
            });
            UpdateLeadGroupUi();
        }
    }).disableSelection();

    $(".customerGroupGridSettings tbody").sortable({
        update: function () {
            var i = 1;
            $(".customerGroupGridSettings tbody tr td.orderby").each(function () {
                $(this).text(i);
                i += 1;
            });
            UpdateCustomerGroupUi();
        }
    }).disableSelection();

    $('#BtnCustomerGrid').click(function () {
        $.ajax({
            type: "POST",
            ajaxStart: function () { },
            url: '/template/GenerateCustsomerList', 
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                // LoadInventory(true);
                alert('File generated!');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    });
    $('#BtnLeadGrid').click(function () {
        $.ajax({
            type: "POST",
            ajaxStart: function () { },
            url: '/template/GenerateLeadList',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                // LoadInventory(true);
                alert('File generated!');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        })
    });
    
});
$(window).resize(function () {
    $("#CustomerUiSettingsTab").height(window.innerHeight - 142);
    $("#UiSettingsTab").height(window.innerHeight - 142);
    $("#EquipmentUiSettingsTab").height(window.innerHeight - 142);
});