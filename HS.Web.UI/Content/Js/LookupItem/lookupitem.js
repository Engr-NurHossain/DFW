
var NewLkTemplate = "<tr data-id='{0}' class='LookupItem'>"
                        +"<td class='DataKey hidden'>{1}</td>"
                        +"<td class='DataValue'>{2}</td>"
                        +"<td class='DisplayText'><input type='text' value='{3}' /></td>"
                        +"<td><input class='IsActive' type='checkbox' {4} /></td>"
                        + "<td class='DataOrder'>{5}</td>"
                        + "<td class='DisplayColor'><input type='hidden' value='{6}'/>{6}</td>"
                        +"<td>"
                        + "<a data-toggle='tooltip' title='Delete' onclick='DeleteLookup({0},\"{1}\",this)'><i class='fa fa-trash-o'></i></a>"
                        + "<a class='icon_green margin-left-5' data-toggle='tooltip' title='Update' onclick='UpdateLookup(this)'><i class='fa fa-floppy-o'></i></a>"
                        +"</td>"
                    + "</tr>";
var UpdateLookup = function (LookupDomE) {
    var LookupDom = $(LookupDomE).parent().parent();
    if (!$(LookupDom).hasClass('LookupItem')) {
        LookupDom = $(e.target).parent().parent().parent();
    }
    var Param = {
        Id: $(LookupDom).attr('data-id'),
        DataKey: SelectedDataKey,
        DataValue: $(LookupDom).find('.DataValue').text().trim(),
        DisplayText: $(LookupDom).find('.DisplayText input').val().trim(),
        DataOrder: $(LookupDom).find(".DataOrder").text().trim(),
        AlterDisplayText: $(LookupDom).find('.DisplayColor input').val(),
        AlterDisplayText1: $(LookupDom).find(".AlterIsActive").is(':checked'),
        IsActive: $(LookupDom).find(".IsActive").is(':checked'),
        IsDefaultItem: $(LookupDom).find(".IsDefaultItem").is(':checked')
    };
    console.log(Param);
    var url = domainurl + "/Setup/UpdateLookup";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) { 
                OpenSuccessMessageNew("Success!", data.message);
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var DeleteLookupConfirm = function (Lkid, DataKey, selecteddom) {
    console.log("he");
    var url = domainurl + "/Setup/DeleteLookup";
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify({
            LookupId: Lkid,
            Datakey: DataKey
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (data.result) {
                $($(selecteddom).parent().parent()).remove();
               
                OpenSuccessMessageNew("Success!", data.message);
            }
            else {
                OpenErrorMessageNew("Error!", data.message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var DeleteLookup = function (Lkid, DataKey, selecteddom) {
    //console.log("hlwwww");
    //var LookupDom = $(e.target).parent().parent();
    //var Lkid = $(LookupDom).attr('data-id');
    //var DataKey = $(LookupDom).find(".DataKey").text();
    OpenConfirmationMessageNew("Confirm?", "Are you really want to delete this item?", function () {
          
        DeleteLookupConfirm(Lkid, DataKey, selecteddom);
    }); 
}
var UpdateLookupGrid = function () {
    var url = domainurl + "/Setup/UpdateLookupList";

    var GridSettings = [];
    $("tbody tr").each(function () {
        GridSettings.push({
            Id: $(this).attr('data-id'),
            DataKey: SelectedDataKey,
            DataValue: $(this).find('.DataValue').text().trim(),
            DisplayText: $(this).find('.DisplayText input').val().trim(),
            DataOrder: $(this).find(".DataOrder").text().trim(),
            AlterDisplayText: $(this).find('.DisplayColor input').val().trim(),
            IsActive: $(this).find(".IsActive").is(':checked'),
            AlterDisplayText1: $(this).find(".AlterIsActive").is(':checked'),
            IsDefaultItem: $(this).find(".IsDefaultItem").is(':checked')
        });
    });
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(GridSettings),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
    
var AddLookup = function () {
    var url = domainurl + "/Setup/AddLookup";
    var DataOrder=0;
    if($(".DataOrder:last").text()!="" && $(".DataOrder:last").text()!=undefined)
    {
        DataOrder=parseInt($(".DataOrder:last").text()) + 1
    }
    var GridSettings =({
        Id: 0,
        DataKey: SelectedDataKey,
        DataValue: $("#NewDataValue").val(),
        DisplayText: $('.NewDisplayText').val(),
        DataOrder: DataOrder,
        AlterDisplayText: $("#ColorDisplay").val(),
        IsActive: $("#NewIsActive").is(':checked')
    });
    $.ajax({
        type: "POST",
        ajaxStart: function () { },
        url: url,
        data: JSON.stringify(GridSettings),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        success: function (data) {
            if (!data.result) {
                OpenErrorMessageNew("Error", data.message);
            }
            $('.NewDisplayText').val("");
            $("#NewDataValue").val("");
            var CheckedValue = "";
            if (data.data.IsActive) {
                CheckedValue= "checked";
            }
            var DisplayColorVal = "";
            if (data.data.AlterDisplayText != null && data.data.AlterDisplayText != "null") {
                DisplayColorVal = data.data.AlterDisplayText;
            }
            var newRow = String.format(NewLkTemplate
                , data.data.Id /*0*/
                , data.data.DataKey /*1*/
                , data.data.DataValue /*2*/
                , data.data.DisplayText /*3*/
                , CheckedValue /*4*/
                , data.data.DataOrder /*5*/
                , DisplayColorVal /*6*/);
            console.log(newRow);
            $(".LookupItemsTable tbody").append(newRow);
            //$('[data-toggle="tooltip"]').tooltip();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });
}
var ApplySort = function () {
    $(".LookupItemsTable tbody").sortable({
        update: function () {
            var i = 0;
            $(".LookupItemsTable tbody tr td.DataOrder").each(function () {
                $(this).text(i);
                i += 1;
            });
            console.log("sort");
            UpdateLookupGrid();
        }
    }).disableSelection();
};
$(document).ready(function () {
    ApplySort();
    $("#AddNewLookup").click(function () {
        if (CommonUiValidation()) {
            AddLookup();
            $(".EditModule").load(domainurl + "/Setup/LookupItems/?Key=" + SelectedDataKey);

        }
    });
    //$('[data-toggle="tooltip"]').tooltip();
});
