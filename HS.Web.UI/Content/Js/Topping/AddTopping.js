
var saveTopping = function () {
    console.log("test1");
    var ToppingList = [];
    $(".HasItem").each(function () {
        if (typeof ($(this).find('#ToppingName').val()) != "undefined" && $(this).find('#ToppingName').val() != null && $(this).find('#ToppingName').val() != "" &&
            typeof ($(this).find('#ToppingPrice').val()) != "undefined" && $(this).find('#ToppingPrice').val() != null && $(this).find('#ToppingPrice').val() != "") {
            ToppingList.push({
                Id: $(this).attr('data-id'),
                ToppingName: $(this).find('#ToppingName').val(),
                Description: $(this).find('#Description').val(),
                Price: $(this).find('#ToppingPrice').val(),
                IsAvailable: true,
                IsDefault: ($("#RequiredItem").val() != "0" && $("#RequiredItem").val() != "") ? $($(this).find("#chk_item_topping_default")).prop("checked") : false
            });
        }
    });
    var ToppingCategoryParam = {
        Id: $("#ToppingCategoryId").val(),
        ToppingCategory: $("#ToppingCategory").val(),
        RequiredItem: $("#RequiredItem").val()
    };
    var url = domainurl + "/MenuManagement/AddTopping/";
    var param = JSON.stringify({
        ToppingCategoryModel: ToppingCategoryParam,
        Toppings: ToppingList
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

            console.log("test2");
            if (data.result == true) {

                OpenIeateryPopupModal("Success", "Topping saved successfully");
                OpenTopToBottomModal(domainurl + "/MenuManagement/AddTopping/?ToppingCategoryId=" + data.id);
            ToppingListLoad(1);
            }
            else
            {
                parent.OpenErrorMessageNew("Error!", "Topping saved failed");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var saveToppingAndNew = function () {
    console.log("test1");
    var ToppingList = [];
    $(".HasItem").each(function () {
        if (typeof ($(this).find('#ToppingName').val()) != "undefined" && $(this).find('#ToppingName').val() != null && $(this).find('#ToppingName').val() != "" &&
            typeof ($(this).find('#ToppingPrice').val()) != "undefined" && $(this).find('#ToppingPrice').val() != null && $(this).find('#ToppingPrice').val() != "") {
            ToppingList.push({
                Id: $(this).attr('data-id'),
                ToppingName: $(this).find('#ToppingName').val(),
                Description: $(this).find('#Description').val(),
                Price: $(this).find('#ToppingPrice').val(),
                IsAvailable: true,
                IsDefault: ($("#RequiredItem").val() != "0" && $("#RequiredItem").val() != "") ? $($(this).find("#chk_item_topping_default")).prop("checked") : false
            });
        }
    });
    var ToppingCategoryParam = {
        Id: $("#ToppingCategoryId").val(),
        ToppingCategory: $("#ToppingCategory").val(),
        RequiredItem: $("#RequiredItem").val()
    };
    var url = domainurl + "/MenuManagement/AddTopping/";
    var param = JSON.stringify({
        ToppingCategoryModel: ToppingCategoryParam,
        Toppings: ToppingList
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

            console.log("test2");
            if (data.result == true) {

                OpenIeateryPopupModal("Success", "Topping saved successfully");
                OpenTopToBottomModal(domainurl + "/MenuManagement/AddTopping/?ToppingCategoryId=" + 0);
                ToppingListLoad(1);
            }
            else {
                parent.OpenErrorMessageNew("Error!", "Topping saved failed");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var saveToppingAndClose = function () {
    console.log("test1");
    var ToppingList = [];
    $(".HasItem").each(function () {
        if (typeof ($(this).find('#ToppingName').val()) != "undefined" && $(this).find('#ToppingName').val() != null && $(this).find('#ToppingName').val() != "" &&
            typeof ($(this).find('#ToppingPrice').val()) != "undefined" && $(this).find('#ToppingPrice').val() != null && $(this).find('#ToppingPrice').val() != "") {
            ToppingList.push({
                Id: $(this).attr('data-id'),
                ToppingName: $(this).find('#ToppingName').val(),
                Description: $(this).find('#Description').val(),
                Price: $(this).find('#ToppingPrice').val(),
                IsAvailable: true,
                IsDefault: ($("#RequiredItem").val() != "0" && $("#RequiredItem").val() != "") ? $($(this).find("#chk_item_topping_default")).prop("checked") : false
            });
        }
    });
    var ToppingCategoryParam = {
        Id: $("#ToppingCategoryId").val(),
        ToppingCategory: $("#ToppingCategory").val(),
        RequiredItem: $("#RequiredItem").val()
    };
    var url = domainurl + "/MenuManagement/AddTopping/";
    var param = JSON.stringify({
        ToppingCategoryModel: ToppingCategoryParam,
        Toppings: ToppingList
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

            console.log("test2");
            if (data.result == true) {

                OpenIeateryPopupModal("Success", "Topping saved successfully");
                CloseTopToBottomModal();
                ToppingListLoad(1);
            }
            else {
                parent.OpenErrorMessageNew("Error!", "Topping saved failed");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(".loader-div").hide();
            console.log(errorThrown);
        }
    });
}
var counter = 1;
if (typeof ($("#add").val()) != "undefined" && $("#add").val() != null && $("#add").val() != "") {  
    counter = $("#add").val();
}
//$(function () {
$('#add').click(function () {
    if (parseInt(itemreqcount) > 0) {
        $('<tr id="tablerow' + counter + '" class="HasItem"><td>' +
            '<input type="text" class="text-box single-line" placeholder="Topping Name" name="ToppingName" id="ToppingName" />' +
            '</td>' +
            '<td>' +
            '<div class="editor-field currency_style">' +
            '<div class="input-group">' +
            '<div class="input-group-prepend">' +
            '<div class="input-group-text">' + Currency + '</div></div>' +
            '<input type="text" class="text-box single-line form-control" placeholder="Topping Price" name="ToppingPrice" id="ToppingPrice" /></div></div>' +
            '</td>' +
            '<td>' +
                '<div class="editor-field">' +
                    '<input class="text-box single-line" placeholder="Description" name="Description" id="Description" type="text" />' +
                '</div>' +
            '</td>' +
            '<td><input type="checkbox" class="chk_item_topping_default" id="chk_item_topping_default" /></td>' +
            '<td class="act_head">' +
            '<button type="button" class="btn btn-primary" onclick="removeTr(' + counter + ');" title="Delete this row"><i class="fa fa-trash-o" aria-hidden="true"></i></button>' +
            '</td>' +
            '</tr>').appendTo('#submissionTable');
    }
    else {
        $('<tr id="tablerow' + counter + '" class="HasItem"><td>' +
            '<input type="text" class="text-box single-line" placeholder="Topping Name" name="ToppingName" id="ToppingName" />' +
            '</td>' +
            '<td>' +
            '<div class="editor-field currency_style">' +
            '<div class="input-group">' +
            '<div class="input-group-prepend">' +
            '<div class="input-group-text">' + Currency + '</div></div>' +
            '<input type="text" class="text-box single-line form-control" placeholder="Topping Price" name="ToppingPrice" id="ToppingPrice" /></div></div>' +
            '</td>' +
            '<td>' +
            '<div class="editor-field">' +
            '<input class="text-box single-line" placeholder="Description" name="Description" id="Description" type="text" />' +
            '</div>' +
            '</td>' +
            '<td class="act_head">' +
            '<button type="button" class="btn btn-primary" onclick="removeTr(' + counter + ');" title="Delete this row"><i class="fa fa-trash-o" aria-hidden="true"></i></button>' +
            '</td>' +
            '</tr>').appendTo('#submissionTable');
    }
        
        counter++;
        return false;
    });
//});
    var removeTr = function (index) {
    if (index > 0) {
        $('#tablerow' + index).remove();
        counter--;
    }
    return false;
    }

    
    var DeleteToppingById = function (tid) {
        $.ajax({
            url: domainurl + "/MenuManagement/DeleteTopping",
            data: { Id: tid },
            type: "Post",
            dataType: "Json",
            success: function (data) {
                if (data.result) {
                    OpenIeateryPopupModal("Success", data.message)
                    CloseTopToBottomModal();
                    ToppingListLoad(1);
                } else {
                    OpenErrorMessageNew("Error!", data.message)
                }
            }
        });
    }

    $(document).ready(function () {
        var toppingcateoryid = $(".Loadtoppingitem").attr('data-id');
        $(".Loadtoppingitem").load("/MenuManagement/ToppingListPartial?toppingcateoryid=" + toppingcateoryid);
    $(".add_topping_height").height(window.innerHeight - 125);
    
    $("#DeleteThisTopping").click(function () {
        OpenConfirmationMessageNew("Confirm?", "Are you sure you want to delete?", function () {
            DeleteToppingById($("#ToppingCategoryId").val())
        });
    });
    $("#saveTopping").click(function () {
        if (CommonUiValidation()) {
                    saveTopping();
        }
        else {
            ScrollToError();
        }

    });
        $("#saveToppingAndClose").click(function () {
            if (CommonUiValidation()) {
                saveToppingAndClose();
            }
            else {
                ScrollToError();
            }

        });
        $("#saveToppingAndNew").click(function () {
            if (CommonUiValidation()) {
                saveToppingAndNew();
            }
            else {
                ScrollToError();
            }

        });
        $(".chk_item_topping_default").change(function () {
            var totalreqitem = parseInt($("#RequiredItem").val());
            var selecteditem = $(".chk_item_topping_default:checked").length;
            if (totalreqitem > 0 && selecteditem > totalreqitem) {
                OpenErrorMessageNew("Error", "Default selected items should be equal topping required");
                $(this).prop("checked", false);
            }
        })
});
$(window).resize(function () {
    $(".add_topping_height").height(window.innerHeight - 125);

});